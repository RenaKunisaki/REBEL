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

namespace REBEL.Blocks {
    //add to a field/property to have it be saved/restored
    [System.AttributeUsage(System.AttributeTargets.Field |
     System.AttributeTargets.Property)]
    public abstract class TileAttributeBase: System.Attribute {
    }

    public abstract class TileAttribute<T>: TileAttributeBase {
        public String name, description;
        public TileAttribute(String name, String description) {
            this.name = name;
            this.description = description;
        }

        public virtual void save(TagCompound tag, T value) {
            tag[this.name] = value;
        }

        public virtual T load(TagCompound tag) {
            return tag.Get<T>(this.name); //XXX default value
        }

        public abstract T receive(BinaryReader reader);
        public abstract void send(BinaryWriter writer, T value);
    }

    public class TileIntAttribute: TileAttribute<int> {
        protected int minValue, maxValue, defaultValue;
        public TileIntAttribute(String name, String description,
        int minValue=Int32.MinValue,
        int maxValue=Int32.MaxValue,
        int defaultValue=0): base(name, description) {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.defaultValue = defaultValue;
        }
        public override int receive(BinaryReader reader) => reader.ReadInt32();
        public override void send(BinaryWriter writer, int value) {
            writer.Write(value);
        }
    }
    public class TileFloatAttribute: TileAttribute<float> {
        protected float minValue, maxValue, defaultValue;
        public TileFloatAttribute(String name, String description,
        float minValue=Single.MinValue,
        float maxValue=Single.MaxValue,
        float defaultValue=0): base(name, description) {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.defaultValue = defaultValue;
        }
        //ReadFloat would be too obvious
        public override float receive(BinaryReader reader) => reader.ReadSingle();
        public override void send(BinaryWriter writer, float value) {
            writer.Write(value);
        }
    }

    public abstract class RebelModTileEntity: ModTileEntity {
        public override void Update() {
            // Sending 86 aka, TileEntitySharing, triggers NetSend.
            //Think of it like manually calling sync.
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null,
                ID, Position.X, Position.Y);
		}

        public override void NetReceive(BinaryReader reader) {
			//lightIntensity = reader.ReadSingle();
			//animSpeed      = reader.ReadInt32();
		}
		public override void NetSend(BinaryWriter writer) {
			//writer.Write(lightIntensity);
			//writer.Write(animSpeed);
		}

        public override void SaveData(TagCompound tag) {
			_saveAttrs(tag);
		}
		public override void LoadData(TagCompound tag) {
            //lightIntensity = tag.Get<float>("lightIntensity");
            //animSpeed = tag.Get<int>("animSpeed");
            //_iterAttrs();
            _loadAttrs(tag);
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

        //XXX find a way to avoid duplicating all this...
        public void _saveAttrs(TagCompound tag) {
            Mod.Logger.Info("Start saveAttrs");
            Type type = this.GetType();
            foreach(var field in type.GetMembers()
            .Where(f => f is not null)) {
                var attr = field.GetCustomAttributes(false)
                .DefaultIfEmpty(null)
                .FirstOrDefault(attr => (attr is TileAttributeBase));
                if(attr is null) continue;
                if(field.MemberType == MemberTypes.Field) {
                    FieldInfo prop = type.GetField(field.Name);
                    Mod.Logger.Info($"Field {field.Name} type {prop.FieldType} value {prop.GetValue(this)} attr {attr}");
                    //prop.SetValue(this, attr.load(tag));
                }
                else if(field.MemberType == MemberTypes.Property) {
                    PropertyInfo prop = type.GetProperty(field.Name);
                    Mod.Logger.Info($"Property {field.Name} type {prop.PropertyType} value {prop.GetValue(this)} attr {attr}");
                    //prop.SetProperty(this, attr.load(tag));
                }
            }
            Mod.Logger.Info("done saveAttrs");
        }

        public void _loadAttrs(TagCompound tag) {
            Mod.Logger.Info("Start loadAttrs");
            Type type = this.GetType();
            foreach(var field in type.GetMembers()
            .Where(f => f is not null)) {
                var attr = field.GetCustomAttributes(false)
                .DefaultIfEmpty(null)
                .FirstOrDefault(attr => (attr is TileAttributeBase));
                if(attr is null) continue;
                if(field.MemberType == MemberTypes.Field) {
                    FieldInfo prop = type.GetField(field.Name);
                    Mod.Logger.Info($"Field {field.Name} type {prop.FieldType} value {prop.GetValue(this)} attr {attr}");
                    //prop.SetValue(this, attr.load(tag));
                }
                else if(field.MemberType == MemberTypes.Property) {
                    PropertyInfo prop = type.GetProperty(field.Name);
                    Mod.Logger.Info($"Property {field.Name} type {prop.PropertyType} value {prop.GetValue(this)} attr {attr}");
                    //prop.SetProperty(this, attr.load(tag));
                }
            }
            Mod.Logger.Info("done loadAttrs");
        }
    }
} //namespace
