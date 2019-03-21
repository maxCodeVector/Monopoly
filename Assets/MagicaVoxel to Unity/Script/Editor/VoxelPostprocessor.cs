namespace VoxeltoUnity {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	public class VoxelPostprocessor : AssetPostprocessor {


		public struct SpriteConfig {
			public int width;
			public int height;
			public string[] Names;
			public Vector2[] Pivots;
			public Rect[] spriteRects;
		}


		private static Dictionary<string, string> AssetMap = new Dictionary<string, string>();
		private static Dictionary<string, SpriteConfig> SpriteMap = new Dictionary<string, SpriteConfig>();
		private static List<string> TextureList = new List<string>();
		public static Shader TheShader;


		private static bool Screenshot = false;



		// API
		public static void AddObj (string objPath, string texturePath) {
			objPath = Util.GetFullPath(objPath);
			if (!AssetMap.ContainsKey(objPath)) {
				AssetMap.Add(objPath, texturePath);
			}
		}


		public static void AddTexture (string texturePath) {
			Screenshot = false;
			texturePath = Util.GetFullPath(texturePath);
			if (!TextureList.Contains(texturePath)) {
				TextureList.Add(texturePath);
			}
		}


		public static void AddSprite (string path, SpriteConfig config) {
			path = Util.GetFullPath(path);
			if (!SpriteMap.ContainsKey(path)) {
				SpriteMap.Add(path, config);
			}
		}


		public static void AddScreenshot (string texturePath) {
			AddTexture(texturePath);
			Screenshot = true;
		}


		public static void ClearAsset () {
			AssetMap.Clear();
			TextureList.Clear();
			SpriteMap.Clear();
		}





		// LGC
		private void OnPreprocessModel () {

			string fullPath = Util.GetFullPath(assetImporter.assetPath);
			ModelImporter mi = assetImporter as ModelImporter;

			if (AssetMap.ContainsKey(fullPath)) {

				string texturePath = AssetMap[fullPath];

				mi.materialSearch = ModelImporterMaterialSearch.Local;
				mi.materialName = ModelImporterMaterialName.BasedOnTextureName;
				mi.normalCalculationMode = ModelImporterNormalCalculationMode.Unweighted_Legacy;
				mi.importNormals = ModelImporterNormals.Import;
				mi.importMaterials = true;
				mi.importAnimation = false;
				mi.importBlendShapes = false;

#if UNITY_2018
				mi.materialLocation = ModelImporterMaterialLocation.InPrefab;
				EditorApplication.delayCall += () => {
					Material mat = null;
					Object[] things = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetImporter.assetPath);
					foreach (Object o in things) {
						if (o is Material) {
							mat = o as Material;
							mat.shader = TheShader;
						}
					}
					if (mat) {
						EditorApplication.delayCall += () => {
							mat.mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(Util.FixedRelativePath(texturePath));
						};
					}
				};
#endif

			}
		}





		private void OnPreprocessTexture () {
			string fullPath = Util.GetFullPath(assetImporter.assetPath);
			TextureImporter ti = assetImporter as TextureImporter;
			if (TextureList.Contains(fullPath)) {
				ti.alphaIsTransparency = Screenshot;
				ti.isReadable = true;
				ti.mipmapEnabled = false;
				ti.npotScale = TextureImporterNPOTScale.None;
				ti.filterMode = Screenshot ? FilterMode.Bilinear : FilterMode.Point;
				ti.textureCompression = Screenshot ? TextureImporterCompression.Uncompressed : TextureImporterCompression.CompressedHQ;
				ti.textureShape = TextureImporterShape.Texture2D;
				ti.textureType = TextureImporterType.Default;
				ti.wrapMode = TextureWrapMode.Clamp;
				ti.maxTextureSize = 8192;
			} else if (SpriteMap.ContainsKey(fullPath)) {

				var config = SpriteMap[fullPath];

				// Impoert
				ti.isReadable = true;
				ti.alphaIsTransparency = true;
				ti.filterMode = FilterMode.Point;
				ti.mipmapEnabled = false;
				ti.textureType = TextureImporterType.Sprite;
				ti.spriteImportMode = SpriteImportMode.Multiple;
				ti.maxTextureSize = 8192;
				ti.textureCompression = TextureImporterCompression.Uncompressed;


				// Sprites
				Rect[] rects = config.spriteRects;
				List<SpriteMetaData> newData = new List<SpriteMetaData>();
				for (int i = 0; i < rects.Length; i++) {
					SpriteMetaData smd = new SpriteMetaData() {
						pivot = config.Pivots[i],
						alignment = 9,
						name = Util.GetNameWithoutExtension(fullPath) + "_" + config.Names[i],
						rect = rects[i]
					};
					newData.Add(smd);
				}
				ti.spritesheet = newData.ToArray();

			}
		}





	}
}