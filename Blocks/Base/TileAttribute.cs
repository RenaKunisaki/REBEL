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
        public int minValue, maxValue, defaultValue, step, bigStep;
        public string format;
        public TileIntAttribute(String name, String description,
        int minValue = Int32.MinValue,
        int maxValue = Int32.MaxValue,
        int defaultValue = 0,
        int step = 1, int bigStep = 10,
        string format="G"): base(name, description, defaultValue) {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.step     = step;
            this.bigStep  = bigStep;
            this.format   = format;
        }
        public override int receive(BinaryReader reader) => reader.ReadInt32();
        public override void send(BinaryWriter writer, int value) {
            writer.Write(value);
        }
    }
    public class TileFloatAttribute: TileAttribute<float> {
        //A float attribute, which has a minimum and maximum value.
        public float minValue, maxValue, defaultValue, step, bigStep;
        public string format;
        public TileFloatAttribute(String name, String description,
        float minValue = Single.NegativeInfinity,
        float maxValue = Single.PositiveInfinity,
        float defaultValue = 0,
        float step = 1f, float bigStep = 10f,
        string format="G"): base(name, description, defaultValue) {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.step     = step;
            this.bigStep  = bigStep;
            this.format   = format;
        }
        //ReadFloat would be too obvious
        public override float receive(BinaryReader reader) => reader.ReadSingle();
        public override void send(BinaryWriter writer, float value) {
            writer.Write(value);
        }
    }

    public class TileEnumAttribute: TileAttribute<int> {
        //An attribute which can be one of a given list of values.
        public int defaultValue;
        public Dictionary<int, String> values; //ID => display name
        public List<int> sort; //order to display in UI

        public TileEnumAttribute(String name, String description,
        String values, int defaultValue = 0, String sort=""):
        base(name, description, defaultValue) {
            //horrible hack because of C# limitations
            this.values = new Dictionary<int, String>();
            int i = 0;
            foreach(String line in values.Split('\n', StringSplitOptions.TrimEntries)) {
                String val = line;
                if(line.Contains('\t')) {
                    var fields = line.Split('\t', 2);
                    if(!Int32.TryParse(fields[0], out i)) {
                        ModContent.GetInstance<REBEL>().Logger.Error(
                            $"Init: Invalid key \"{fields[0]}\" for value "+
                            $"\"{fields[1]}\" for attribute {this.name}");
                    }
                    val = fields[1];
                }
                this.values[i++] = val;
            }

            this.sort = new List<int>();
            if(sort != "") {
                foreach(String line in sort.Split('\n', StringSplitOptions.TrimEntries)) {
                    foreach(String item in line.Split(',', StringSplitOptions.TrimEntries)) {
                        int val;
                        if(!Int32.TryParse(item, out val)) {
                            ModContent.GetInstance<REBEL>().Logger.Error(
                                $"Init: Invalid sort ID \"{item}\" "+
                                $"for attribute {this.name}");
                        }
                        else this.sort.Append(val);
                    }
                    this.sort.Append(-1); //end of line
                }
            }
        }
        public override int receive(BinaryReader reader) {
            int val = reader.ReadInt32();
            if(!this.values.ContainsKey(val)) {
                ModContent.GetInstance<REBEL>().Logger.Error(
                    $"Receive: Invalid value {val} for attribute {this.name}");
                //default to an arbitrary valid value
                val = this.values.Keys.First();
            }
            return val;
        }
        public override void send(BinaryWriter writer, int value) {
            if(!this.values.ContainsKey(value)) {
                ModContent.GetInstance<REBEL>().Logger.Error(
                    $"Send: Invalid value {value} for attribute {this.name}");
                return;
            }
            writer.Write(value);
        }
    }
} //namespace
