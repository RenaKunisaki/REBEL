using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Reflection;

//WARNING! Madness lies below.
//This could probably be better but I don't know C# well enough and
//have already spent a few hours wading through an ocean of Microsoft(R)
//Red Tape(tm) to get it just plain working.
//This should maybe dump hopefully all the textures, assuming your system
//has the directory to dump them to...
//It will take a minute or two. Maybe more if your system is slow.
//Note if you have any resource packs or mods enabled, it'll dump those
//too, which may or may not be desirable.

namespace REBEL {
    public class TextureDumper {
        REBEL Mod;
        public TextureDumper(REBEL mod) {
            this.Mod = mod;
        }

        public void dumpAllLoadedTextures() {
            //var fields = typeof(ReLogic.Content.Asset<Texture2D>).GetProperties(); //.Select(f => f.Name).ToList();
			//foreach(var field in fields) {
			//	Logger.Info($"field: {field}");
			//}

            String cwd = System.IO.Directory.GetCurrentDirectory();
			String basePath = cwd + "/TerrariaTextureDump/";
            Mod.Logger.Info($"Dumping to {basePath}...");
            System.IO.Directory.CreateDirectory(basePath);
            var fields = typeof(TextureAssets).GetFields(BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.GetField |
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.FlattenHierarchy);
			foreach(var field in fields) {
				//Mod.Logger.Info($"field: {field}");
                FieldInfo prop = typeof(TextureAssets).GetField(field.Name);
                if(prop is null) {
                    PropertyInfo lol = typeof(TextureAssets).GetProperty(field.Name);
                    if(lol is not null) {
                        _dumpTexturesProp(field, lol, basePath);
                    }
                }
                else _dumpTexturesField(field, prop, basePath);
			}
			Mod.Logger.Info("Done.");
        }

        //you'd think, given these two methods have identical bodies,
        //there'd be some simple way to condense them into one, probably
        //using a template. and, apparently, you'd be wrong.
        void _dumpTexturesField(FieldInfo field, FieldInfo prop, String basePath) {
            if(field.FieldType == typeof(Texture2D)) {
                Texture2D tex = prop.GetValue(typeof(TextureAssets)) as Texture2D;
                dumpTexture(tex, $"{basePath}{field.Name}.png");
            }
            else if(field.FieldType == typeof(Texture2D[])) {
                Texture2D[] tex = prop.GetValue(typeof(TextureAssets)) as Texture2D[];
                dumpTextureArray(tex, $"{basePath}{field.Name}_");
            }
            else if(field.FieldType == typeof(ReLogic.Content.Asset<Texture2D>)) {
                ReLogic.Content.Asset<Texture2D> tex =
                    prop.GetValue(typeof(TextureAssets)) as ReLogic.Content.Asset<Texture2D>;
                if(tex is not null && tex.Value is not null) {
                    dumpTexture(tex.Value, $"{basePath}{field.Name}.png");
                }
            }
            else if(field.FieldType == typeof(ReLogic.Content.Asset<Texture2D>[])) {
                ReLogic.Content.Asset<Texture2D>[] tex =
                    prop.GetValue(typeof(TextureAssets)) as ReLogic.Content.Asset<Texture2D>[];
                dumpTextureArray(tex, $"{basePath}{field.Name}_");
            }
            else if(field.FieldType == typeof(ReLogic.Content.Asset<Texture2D>[,])) {
                ReLogic.Content.Asset<Texture2D>[,] tex =
                    prop.GetValue(typeof(TextureAssets)) as ReLogic.Content.Asset<Texture2D>[,];
                dumpTextureArray(tex, $"{basePath}{field.Name}_");
            }
            else {
                Mod.Logger.Info($"Don't know how to dump: {field.FieldType}");
            }
        }
        void _dumpTexturesProp(FieldInfo field, PropertyInfo prop, String basePath) {
            if(field.FieldType == typeof(Texture2D)) {
                Texture2D tex = prop.GetValue(typeof(TextureAssets)) as Texture2D;
                dumpTexture(tex, $"{basePath}{field.Name}.png");
            }
            else if(field.FieldType == typeof(Texture2D[])) {
                Texture2D[] tex = prop.GetValue(typeof(TextureAssets)) as Texture2D[];
                dumpTextureArray(tex, $"{basePath}{field.Name}_");
            }
            else if(field.FieldType == typeof(ReLogic.Content.Asset<Texture2D>)) {
                ReLogic.Content.Asset<Texture2D> tex =
                    prop.GetValue(typeof(TextureAssets)) as ReLogic.Content.Asset<Texture2D>;
                if(tex is not null && tex.Value is not null) {
                    dumpTexture(tex.Value, $"{basePath}{field.Name}.png");
                }
            }
            else if(field.FieldType == typeof(ReLogic.Content.Asset<Texture2D>[])) {
                ReLogic.Content.Asset<Texture2D>[] tex =
                    prop.GetValue(typeof(TextureAssets)) as ReLogic.Content.Asset<Texture2D>[];
                dumpTextureArray(tex, $"{basePath}{field.Name}_");
            }
            else if(field.FieldType == typeof(ReLogic.Content.Asset<Texture2D>[,])) {
                ReLogic.Content.Asset<Texture2D>[,] tex =
                    prop.GetValue(typeof(TextureAssets)) as ReLogic.Content.Asset<Texture2D>[,];
                dumpTextureArray(tex, $"{basePath}{field.Name}_");
            }
            else {
                Mod.Logger.Info($"Don't know how to dump: {field.FieldType}");
            }
        }

		void dumpTextureArray(ReLogic.Content.Asset<Texture2D>[,] textures,
        String path) {
            if(textures is null) {
                Mod.Logger.Info($"{path} is null");
            }
            else {
                for(int i=0; i<textures.GetLength(0); i++) {
                    for(int j=0; j<textures.GetLength(1); j++) {
                        if(textures[i,j] is not null) {
                            dumpTexture(textures[i,j].Value, $"{path}{i}_{j}.png");
                        }
                    }
                }
            }
        }
		void dumpTextureArray(ReLogic.Content.Asset<Texture2D>[] textures,
        String path) {
            if(textures is null) {
                Mod.Logger.Info($"{path} is null");
            }
            else {
                for(int i=0; i<textures.Length; i++) {
                    if(textures[i] is not null) {
                        dumpTexture(textures[i].Value, $"{path}{i}.png");
                    }
                }
            }
        }
		void dumpTextureArray(Texture2D[] textures, String path) {
            if(textures is null) {
                Mod.Logger.Info($"{path} is null");
            }
            else {
                for(int i=0; i<textures.Length; i++) {
                    if(textures[i] is not null) {
                        dumpTexture(textures[i], $"{path}{i}.png");
                    }
                }
            }
		}
		void dumpTexture(Texture2D tex, String path) {
            if(tex is not null && tex.Width > 1 || tex.Height > 1) {
                Stream stream = File.Create(path);
                //Mod.Logger.Info($"Writing: {path}");
                tex.SaveAsPng(stream, tex.Width, tex.Height);
                stream.Dispose();
            }
		}
    }
}
