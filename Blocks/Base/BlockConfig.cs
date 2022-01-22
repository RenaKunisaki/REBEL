using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

/** This is a whole ridiculous custom ModTileEntity class
 *  whose purpose is to let you define individual attributes
 *  by just giving the parameters (min, max, name, etc)
 *  and take care of all the saving/loading/network shit.
 *
 *  It's completely stupid, using a bunch of reflection
 *  nonsense and a lot of seemingly unavoidable repetition,
 *  but it gets the job done.
 *
 *  Eventually it should also take care of setting up some
 *  kind of UI for the player to configure these attributes
 *  on any particular tile.
 *
 *  Beware: Microsoft stank ahead.
 */

namespace REBEL.Blocks {
    //add to a field/property to have it be saved/restored
    [System.AttributeUsage(System.AttributeTargets.Field |
     System.AttributeTargets.Property)]
    public abstract class TileAttributeBase: System.Attribute {
        //this exists just so we can test if an attribute
        //is one of the TileAttribute types.
    }

    public abstract class TileAttribute<T>: TileAttributeBase {
        public String name, description;
        protected T defaultValue;
        public TileAttribute(String name, String description, T defaultValue) {
            this.name = name;
            this.description = description;
            this.defaultValue = defaultValue;
        }

        public virtual void save(TagCompound tag, T value) {
            //Store this attribute's value to the save file.
            ModContent.GetInstance<REBEL>().Logger.Info(
                $"Field {this.name} type {this.GetType()} save value {value}");
            tag[this.name] = value;
        }

        public virtual T load(TagCompound tag) {
            //Retrieve this attribute's value from the save file.
            T result = tag.ContainsKey(this.name) ?
                tag.Get<T>(this.name) : this.defaultValue;
            ModContent.GetInstance<REBEL>().Logger.Info(
                $"Field {this.name} type {this.GetType()} load value {result}");
            return result;
        }

        //Receive this attribute's value over network.
        public abstract T receive(BinaryReader reader);

        //Send this attribute's value over network.
        //XXX there's probably some way to template this
        //since the body looks identical for all of them.
        public abstract void send(BinaryWriter writer, T value);
    }

    public class TileIntAttribute: TileAttribute<int> {
        //An integer attribute, which has a minimum and maximum value.
        protected int minValue, maxValue, defaultValue;
        public TileIntAttribute(String name, String description,
        int minValue=Int32.MinValue,
        int maxValue=Int32.MaxValue,
        int defaultValue=0): base(name, description, defaultValue) {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
        public override int receive(BinaryReader reader) => reader.ReadInt32();
        public override void send(BinaryWriter writer, int value) {
            writer.Write(value);
        }
    }
    public class TileFloatAttribute: TileAttribute<float> {
        //A float attribute, which has a minimum and maximum value.
        protected float minValue, maxValue, defaultValue;
        public TileFloatAttribute(String name, String description,
        float minValue=Single.MinValue,
        float maxValue=Single.MaxValue,
        float defaultValue=0): base(name, description, defaultValue) {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
        //ReadFloat would be too obvious
        public override float receive(BinaryReader reader) => reader.ReadSingle();
        public override void send(BinaryWriter writer, float value) {
            writer.Write(value);
        }
    }


    public abstract class RebelModTileEntity<TileType>: ModTileEntity
    where TileType: ModTile {
        public override bool IsTileValidForEntity(int i, int j) {
			Tile tile = Main.tile[i, j];
			return tile.IsActive
                && tile.type == ModContent.TileType<TileType>();
                //&& tile.frameX == 0 && tile.frameY == 0;
		}

        public override void Update() {
            // Sending 86 aka, TileEntitySharing, triggers NetSend.
            //Think of it like manually calling sync.
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null,
                ID, Position.X, Position.Y);
		}

        public override void NetReceive(BinaryReader reader) {
			//lightIntensity = reader.ReadSingle();
			//animSpeed      = reader.ReadInt32();
            //XXX
		}
		public override void NetSend(BinaryWriter writer) {
			//writer.Write(lightIntensity);
			//writer.Write(animSpeed);
            //XXX
		}

        public override void SaveData(TagCompound tag) {
			Mod.Logger.Info("Start SaveData");
            foreach(var field in this.GetType().GetMembers()
            .Where(f => f is not null)) {
                var attr = field.GetCustomAttribute<TileAttributeBase>(false);
                switch(attr) {
                    //sadly doesn't seem to be any way to just automatically
                    //cast attr to whatever type it actually is, so we need
                    //to repeat the list of types...
                    //XXX find a way to avoid duplicating all this.
                    case null: continue;
                    case TileIntAttribute I:
                        I.save(tag, _getField<int>(field));
                        break;
                    case TileFloatAttribute F:
                        F.save(tag, _getField<float>(field));
                        break;
                    default:
                        Mod.Logger.Error($"BUG: No entry in SaveData for type {attr}");
                        break;
                }
            }
            Mod.Logger.Info("Done SaveData");
		}

		public override void LoadData(TagCompound tag) {
            Mod.Logger.Info("Start LoadData");
            foreach(var field in this.GetType().GetMembers()
            .Where(f => f is not null)) {
                var attr = field.GetCustomAttribute<TileAttributeBase>(false);
                switch(attr) {
                    case null: continue;
                    case TileIntAttribute I:
                        _setField<int>(field, I.load(tag));
                        break;
                    case TileFloatAttribute F:
                        _setField<float>(field, F.load(tag));
                        break;
                    default:
                        Mod.Logger.Error($"BUG: No entry in LoadData for type {attr}");
                        break;
                }
            }
            Mod.Logger.Info("Done LoadData");
		}

        public override int Hook_AfterPlacement(int i, int j, int type,
        int style, int direction, int alternate) {
			if(Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1,
                    null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}

        //some extra stupid reflection nonsense for getting/setting
        //the value of an arbitrary member whether it's a field
        //or a property, because of course those would require
        //almost-but-not-quite-identical logic.
        //XXX move to some kind of helper class.
        protected T _getField<T>(MemberInfo member) {
            //LOL C# repetition
            Type type = this.GetType();
            if(member.MemberType == MemberTypes.Field) {
                FieldInfo prop = type.GetField(member.Name);
                //Mod.Logger.Info($"Field {member.Name} type {prop.FieldType} value {prop.GetValue(this)} attr {attr}");
                return (T)prop.GetValue(this);
            }
            else if(member.MemberType == MemberTypes.Property) {
                PropertyInfo prop = type.GetProperty(member.Name);
                //Mod.Logger.Info($"Property {member.Name} type {prop.PropertyType} value {prop.GetValue(this)} attr {attr}");
                return (T)prop.GetValue(this);
            }
            else return default(T);
        }
        protected void _setField<T>(MemberInfo member, T value) {
            //LOL C# repetition
            Type type = this.GetType();
            if(member.MemberType == MemberTypes.Field) {
                FieldInfo prop = type.GetField(member.Name);
                //Mod.Logger.Info($"Field {member.Name} type {prop.FieldType} value {prop.GetValue(this)} attr {attr}");
                prop.SetValue(this, value);
            }
            else if(member.MemberType == MemberTypes.Property) {
                PropertyInfo prop = type.GetProperty(member.Name);
                //Mod.Logger.Info($"Property {member.Name} type {prop.PropertyType} value {prop.GetValue(this)} attr {attr}");
                prop.SetValue(this, value);
            }
        }
    }
} //namespace
