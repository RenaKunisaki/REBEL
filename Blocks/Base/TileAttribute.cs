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
        //this exists just so we can test if an attribute
        //is one of the TileAttribute types.
    }

    public abstract class TileAttribute<T>: TileAttributeBase {
        public String name, description;
        public T defaultValue;
        public TileAttribute(String name, String description, T defaultValue) {
            this.name = name;
            this.description = description;
            this.defaultValue = defaultValue;
        }

        public virtual void save(TagCompound tag, T value) {
            //Store this attribute's value to the save file.
            //ModContent.GetInstance<REBEL>().Logger.Info(
            //    $"Field {this.name} type {this.GetType()} save value {value}");
            tag[this.name] = value;
        }

        public virtual T load(TagCompound tag) {
            //Retrieve this attribute's value from the save file.
            T result = tag.ContainsKey(this.name) ?
                tag.Get<T>(this.name) : this.defaultValue;
            //ModContent.GetInstance<REBEL>().Logger.Info(
            //    $"Field {this.name} type {this.GetType()} load value {result}");
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
        public int minValue, maxValue, defaultValue;
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
        public float minValue, maxValue, defaultValue;
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
} //namespace
