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
    public abstract class RebelModTileEntityBase: ModTileEntity {
        public abstract String displayName {get;}
        public abstract List<MemberInfo> getAttrs();
        public virtual void refresh(int i, int j) {}
        public abstract T _getField<T>(MemberInfo member);
        public abstract void _setField<T>(MemberInfo member, T value);
    }

    public abstract class RebelModTileEntity<TileType>: RebelModTileEntityBase
    where TileType: ModTile {
        //public abstract String displayName {get;}

        public override bool IsTileValidForEntity(int i, int j) {
			Tile tile = Framing.GetTileSafely(i, j);
			return tile.HasTile
                && tile.TileType == ModContent.TileType<TileType>();
                //&& tile.TileFrameX == 0 && tile.TileFrameY == 0;
		}

        public override void Update() {
            // Sending 86 aka, TileEntitySharing, triggers NetSend.
            //Think of it like manually calling sync.
            //XXX only if some attribute has changed
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
			//Mod.Logger.Info("Start SaveData");
            foreach(var field in getAttrs()) {
                var attr = field.GetCustomAttribute<TileAttributeBase>(false);
                switch(attr) {
                    //sadly doesn't seem to be any way to just automatically
                    //cast attr to whatever type it actually is, so we need
                    //to repeat the list of types...
                    //XXX find a way to avoid duplicating all this.
                    case null: continue;
                    case TileEnumAttribute E:
                        E.save(tag, _getField<int>(field));
                        break;
                    case TileFloatAttribute F:
                        F.save(tag, _getField<float>(field));
                        break;
                    case TileIntAttribute I:
                        I.save(tag, _getField<int>(field));
                        break;
                    default:
                        Mod.Logger.Error($"BUG: No entry in SaveData for type {attr}");
                        break;
                }
            }
            //Mod.Logger.Info("Done SaveData");
		}

		public override void LoadData(TagCompound tag) {
            //Mod.Logger.Info("Start LoadData");
            foreach(var field in getAttrs()) {
                var attr = field.GetCustomAttribute<TileAttributeBase>(false);
                switch(attr) {
                    case null: continue;
                    case TileEnumAttribute E:
                        _setField<int>(field, E.load(tag));
                        break;
                    case TileFloatAttribute F:
                        _setField<float>(field, F.load(tag));
                        break;
                    case TileIntAttribute I:
                        _setField<int>(field, I.load(tag));
                        break;
                    default:
                        Mod.Logger.Error($"BUG: No entry in LoadData for type {attr}");
                        break;
                }
            }
            //Mod.Logger.Info("Done LoadData");
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

        public override List<MemberInfo> getAttrs() {
            //XXX can probably be simplified
            var result = new List<MemberInfo>();
            foreach(var field in this.GetType().GetMembers()
            .Where(f => f is not null)) {
                result.Add(field);
            }
            return result;
        }

        //some extra stupid reflection nonsense for getting/setting
        //the value of an arbitrary member whether it's a field
        //or a property, because of course those would require
        //almost-but-not-quite-identical logic.
        //XXX move to some kind of helper class.
        public override T _getField<T>(MemberInfo member) {
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
        public override void _setField<T>(MemberInfo member, T value) {
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
    } //class
} //namespace
