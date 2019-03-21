namespace VoxeltoUnity {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.IO;
	using System.Text;


	public struct Util {



		#region --- File ---



		public static string Read (string path) {
			StreamReader sr = File.OpenText(path);
			string data = sr.ReadToEnd();
			sr.Close();
			return data;
		}



		public static void Write (string data, string path) {
			FileStream fs = new FileStream(path, FileMode.Create);
			StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
			sw.Write(data);
			sw.Close();
			fs.Close();
		}



		public static byte[] FileToByte (string path) {
			byte[] bytes = null;
			if (File.Exists(path)) {
				bytes = File.ReadAllBytes(path);
			}
			return bytes;
		}



		public static void ByteToFile (byte[] bytes, string path) {
			string parentPath = new FileInfo(path).Directory.FullName;
			CreateFolder(parentPath);
			FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
			fs.Write(bytes, 0, bytes.Length);
			fs.Close();
			fs.Dispose();
		}



		public static bool CreateFolder (string _path) {
			try {
				_path = GetFullPath(_path);
				if (Directory.Exists(_path))
					return true;
				string _parentPath = new FileInfo(_path).Directory.FullName;
				if (Directory.Exists(_parentPath)) {
					Directory.CreateDirectory(_path);
				} else {
					CreateFolder(_parentPath);
					Directory.CreateDirectory(_path);
				}
				return true;
			} catch { }
			return false;
		}



		public static bool HasFileIn (string path, params string[] searchPattern) {
			if (PathIsDirectory(path)) {
				for (int i = 0; i < searchPattern.Length; i++) {
					if (new DirectoryInfo(path).GetFiles(searchPattern[i], SearchOption.AllDirectories).Length > 0) {
						return true;
					}
				}
			}
			return false;
		}



		public static FileInfo[] GetFilesIn (string path, params string[] searchPattern) {
			List<FileInfo> allFiles = new List<FileInfo>();
			if (PathIsDirectory(path)) {
				if (searchPattern.Length > 0) {
					allFiles.AddRange(new DirectoryInfo(path).GetFiles("*.*", SearchOption.AllDirectories));
				} else {
					for (int i = 0; i < searchPattern.Length; i++) {
						allFiles.AddRange(new DirectoryInfo(path).GetFiles(searchPattern[i], SearchOption.AllDirectories));
					}
				}
			}
			return allFiles.ToArray();
		}



		public static void DeleteFile (string path) {
			if (FileExists(path)) {
				File.Delete(path);
			}
		}



		#endregion



		#region --- Path ---



		public static string FixPath (string _path) {
			char dsChar = '/';
			char adsChar = '\\';
			_path = _path.Replace(adsChar, dsChar);
			_path = _path.Replace(new string(dsChar, 2), dsChar.ToString());
			while (_path.Length > 0 && _path[0] == dsChar) {
				_path = _path.Remove(0, 1);
			}
			while (_path.Length > 0 && _path[_path.Length - 1] == dsChar) {
				_path = _path.Remove(_path.Length - 1, 1);
			}
			return _path;
		}




		public static string GetFullPath (string path) {
			return new FileInfo(path).FullName;
		}



		public static string CreateParent (string path) {
			string parentPath = new FileInfo(path).Directory.FullName;
			if (CreateFolder(parentPath)) {
				return parentPath;
			} else {
				return "";
			}
		}



		public static string CombinePaths (params string[] paths) {
			string path = "";
			for (int i = 0; i < paths.Length; i++) {
				path = Path.Combine(path, FixPath(paths[i]));
			}
			return FixPath(path);
		}



		public static string GetExtension (string path) {
			return Path.GetExtension(path);//.txt
		}



		public static string GetNameWithoutExtension (string path) {
			return Path.GetFileNameWithoutExtension(path);
		}


		public static string GetNameWithExtension (string path) {
			return Path.GetFileName(path);
		}


		public static string ChangeExtension (string path, string newEx) {
			return Path.ChangeExtension(path, newEx);
		}



		public static bool DirectoryExists (string path) {
			return Directory.Exists(path);
		}



		public static bool FileExists (string path) {
			return File.Exists(path);
		}



		public static bool PathIsDirectory (string path) {
			if (!DirectoryExists(path)) { return false; }
			FileAttributes attr = File.GetAttributes(path);
			if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
				return true;
			else
				return false;
		}



		public static bool IsChildPath (string pathA, string pathB) {
			if (pathA.Length == pathB.Length) {
				return pathA == pathB;
			} else if (pathA.Length > pathB.Length) {
				return IsChildPathCompair(pathA, pathB);
			} else {
				return IsChildPathCompair(pathB, pathA);
			}
		}



		public static bool IsChildPathCompair (string longPath, string path) {
			if (longPath.Length <= path.Length || !PathIsDirectory(path) || !longPath.StartsWith(path)) {
				return false;
			}
			char c = longPath[path.Length];
			if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar) {
				return false;
			}
			return true;
		}



		public static string GetParentPath (string path) {
			return FixPath(path.Substring(0, path.Length - Path.GetFileName(path).Length));
		}




		#endregion



		#region --- MSC ---



		public static Vector2 Vector2Abs (Vector2 v) {
			v.x = Mathf.Abs(v.x);
			v.y = Mathf.Abs(v.y);
			return v;
		}


		public static Vector3 SwipYZ (Vector3 v) {
			float tempZ = v.z;
			v.z = v.y;
			v.y = tempZ;
			return v;
		}


		public static float Remap (float l, float r, float newL, float newR, float t) {
			return l == r ? 0 : Mathf.LerpUnclamped(
				newL, newR,
				(t - l) / (r - l)
			);
		}


		public static int MaxAxis (Vector3 v) {
			if (Mathf.Abs(v.x) >= Mathf.Abs(v.y)) {
				return Mathf.Abs(v.x) >= Mathf.Abs(v.z) ? 0 : 2;
			} else {
				return Mathf.Abs(v.y) >= Mathf.Abs(v.z) ? 1 : 2;
			}
		}


		public static void CopyToClipboard (string containt) {
			TextEditor te = new TextEditor { text = containt };
			te.SelectAll();
			te.Copy();
		}



		public static string GetObj (Mesh m) {

			StringBuilder sb = new StringBuilder();

			sb.Append("g ").Append(m.name).Append("\n");
			foreach (Vector3 v in m.vertices) {
				sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
			}
			sb.Append("\n");
			foreach (Vector3 v in m.normals) {
				sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
			}
			sb.Append("\n");
			foreach (Vector3 v in m.uv) {
				sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
			}

			sb.Append("\n");
			sb.Append("usemtl ").Append(m.name).Append("\n");
			sb.Append("usemap ").Append(m.name).Append("\n");

			int[] triangles = m.triangles;
			for (int i = 0; i < triangles.Length; i += 3) {
				sb.Append(
					string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
					triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1)
				);
			}

			return sb.ToString();
		}



		public static Texture2D RenderTextureToTexture2D (Camera renderCamera) {
			var rTex = renderCamera.targetTexture;
			if (!rTex) { return null; }
			RenderTexture.active = rTex;
			Texture2D texture = new Texture2D(rTex.width, rTex.height, TextureFormat.ARGB32, false, false) {
				filterMode = FilterMode.Bilinear
			};
			var oldColor = renderCamera.backgroundColor;
			renderCamera.backgroundColor = Color.clear;
			renderCamera.Render();
			texture.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0, false);
			texture.Apply();
			renderCamera.backgroundColor = oldColor;
			RenderTexture.active = null;
			return texture;
		}



		public static Texture2D TrimTexture (Texture2D texture, float alpha = 0.01f, int gap = 0) {
			int width = texture.width;
			int height = texture.height;
			var colors = texture.GetPixels();
			int minX = int.MaxValue;
			int minY = int.MaxValue;
			int maxX = int.MinValue;
			int maxY = int.MinValue;

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					var c = colors[y * width + x];
					if (c.a > alpha) {
						minX = Mathf.Min(minX, x);
						minY = Mathf.Min(minY, y);
						maxX = Mathf.Max(maxX, x);
						maxY = Mathf.Max(maxY, y);
					}
				}
			}

			// Gap
			minX = Mathf.Clamp(minX - gap, 0, width - 1);
			minY = Mathf.Clamp(minY - gap, 0, height - 1);
			maxX = Mathf.Clamp(maxX + gap, 0, width - 1);
			maxY = Mathf.Clamp(maxY + gap, 0, height - 1);

			int newWidth = maxX - minX + 1;
			int newHeight = maxY - minY + 1;
			if (newWidth != width || newHeight != height) {
				texture.Resize(newWidth, newHeight);
				var newColors = new Color[newWidth * newHeight];
				for (int y = 0; y < newHeight; y++) {
					for (int x = 0; x < newWidth; x++) {
						newColors[y * newWidth + x] = colors[(y + minY) * width + (x + minX)];
					}
				}
				texture.SetPixels(newColors);
				texture.Apply();
			}
			return texture;
		}



		#endregion



		#region --- MagicaVoxel Byte ---




		private static readonly Dictionary<byte, Vector4> MAGIC_BYTE_TO_TRANSFORM_MAP = new Dictionary<byte, Vector4>() {

			{ 40 , new Vector4(3,0,0,0)},
			{ 2  , new Vector4(3,3,0,0)},
			{ 24 , new Vector4(3,2,0,0)},
			{ 50 , new Vector4(3,1,0,0)},
			{ 120, new Vector4(1,0,2,0)},
			{ 98 , new Vector4(1,0,3,0)},
			{ 72 , new Vector4(1,0,0,0)},
			{ 82 , new Vector4(1,0,1,0)},
			{ 4  , new Vector4(0,0,0,0)},
			{ 22 , new Vector4(0,0,1,0)},
			{ 84 , new Vector4(0,0,2,0)},
			{ 70 , new Vector4(0,0,3,0)},
			{ 52 , new Vector4(0,2,0,0)},
			{ 118, new Vector4(0,2,3,0)},
			{ 100, new Vector4(0,2,2,0)},
			{ 38 , new Vector4(0,2,1,0)},
			{ 17 , new Vector4(0,3,0,0)},
			{ 89 , new Vector4(0,3,3,0)},
			{ 113, new Vector4(0,3,2,0)},
			{ 57 , new Vector4(0,3,1,0)},
			{ 33 , new Vector4(0,1,0,0)},
			{ 9  , new Vector4(0,1,1,0)},
			{ 65 , new Vector4(0,1,2,0)},
			{ 105, new Vector4(0,1,3,0)},

			{ 56 , new Vector4(3,0,0,1)},
			{ 34 , new Vector4(3,3,0,1)},
			{ 8  , new Vector4(3,2,0,1)},
			{ 18 , new Vector4(3,1,0,1)},
			{ 104, new Vector4(1,0,2,1)},
			{ 66 , new Vector4(1,0,3,1)},
			{ 88 , new Vector4(1,0,0,1)},
			{ 114, new Vector4(1,0,1,1)},
			{ 20 , new Vector4(0,0,0,1)},
			{ 86 , new Vector4(0,0,1,1)},
			{ 68 , new Vector4(0,0,2,1)},
			{ 6  , new Vector4(0,0,3,1)},
			{ 36 , new Vector4(0,2,0,1)},
			{ 54 , new Vector4(0,2,3,1)},
			{ 116, new Vector4(0,2,2,1)},
			{ 102, new Vector4(0,2,1,1)},
			{ 49 , new Vector4(0,3,0,1)},
			{ 25 , new Vector4(0,3,3,1)},
			{ 81 , new Vector4(0,3,2,1)},
			{ 121, new Vector4(0,3,1,1)},
			{ 1  , new Vector4(0,1,0,1)},
			{ 73 , new Vector4(0,1,1,1)},
			{ 97 , new Vector4(0,1,2,1)},
			{ 41 , new Vector4(0,1,3,1)},


		};

		private static readonly Dictionary<Vector4, byte> TRANSFORM_TO_MAGIC_BYTE_MAP = new Dictionary<Vector4, byte>() {

			{new Vector4(3,0,0,0), 40 },
			{new Vector4(3,3,0,0), 2  },
			{new Vector4(3,2,0,0), 24 },
			{new Vector4(3,1,0,0), 50 },
			{new Vector4(1,0,2,0), 120},
			{new Vector4(1,0,3,0), 98 },
			{new Vector4(1,0,0,0), 72 },
			{new Vector4(1,0,1,0), 82 },
			{new Vector4(0,0,0,0), 4  },
			{new Vector4(0,0,1,0), 22 },
			{new Vector4(0,0,2,0), 84 },
			{new Vector4(0,0,3,0), 70 },
			{new Vector4(0,2,0,0), 52 },
			{new Vector4(0,2,3,0), 118},
			{new Vector4(0,2,2,0), 100},
			{new Vector4(0,2,1,0), 38 },
			{new Vector4(0,3,0,0), 17 },
			{new Vector4(0,3,3,0), 89 },
			{new Vector4(0,3,2,0), 113},
			{new Vector4(0,3,1,0), 57 },
			{new Vector4(0,1,0,0), 33 },
			{new Vector4(0,1,1,0), 9  },
			{new Vector4(0,1,2,0), 65 },
			{new Vector4(0,1,3,0), 105},

			{new Vector4(3,0,0,1), 56 },
			{new Vector4(3,3,0,1), 34 },
			{new Vector4(3,2,0,1), 8  },
			{new Vector4(3,1,0,1), 18 },
			{new Vector4(1,0,2,1), 104},
			{new Vector4(1,0,3,1), 66 },
			{new Vector4(1,0,0,1), 88 },
			{new Vector4(1,0,1,1), 114},
			{new Vector4(0,0,0,1), 20 },
			{new Vector4(0,0,1,1), 86 },
			{new Vector4(0,0,2,1), 68 },
			{new Vector4(0,0,3,1), 6  },
			{new Vector4(0,2,0,1), 36 },
			{new Vector4(0,2,3,1), 54 },
			{new Vector4(0,2,2,1), 116},
			{new Vector4(0,2,1,1), 102},
			{new Vector4(0,3,0,1), 49 },
			{new Vector4(0,3,3,1), 25 },
			{new Vector4(0,3,2,1), 81 },
			{new Vector4(0,3,1,1), 121},
			{new Vector4(0,1,0,1), 1  },
			{new Vector4(0,1,1,1), 73 },
			{new Vector4(0,1,2,1), 97 },
			{new Vector4(0,1,3,1), 41 },

		};


		public static void VoxMatrixByteToTransform (byte the_Byte_Which_Wasted_My_While_Afternoon, out Vector3 rotation, out Vector3 scale) {
			if (MAGIC_BYTE_TO_TRANSFORM_MAP.ContainsKey(the_Byte_Which_Wasted_My_While_Afternoon)) {
				var v4 = MAGIC_BYTE_TO_TRANSFORM_MAP[the_Byte_Which_Wasted_My_While_Afternoon];
				rotation = new Vector3(v4.x * 90f, v4.y * 90f, v4.z * 90f);
				scale = v4.w < 0.5f ? Vector3.one : new Vector3(-1, 1, 1);
			} else {
				rotation = Vector3.zero;
				scale = Vector3.one;
			}
		}


		public static byte TransformToVoxMatrixByte (Vector3 rot, Vector3 scale) {
			var v4 = new Vector4(
				 (byte)(Mathf.RoundToInt((Mathf.Repeat(rot.x, 360f) / 90f) % 4)),
				 (byte)(Mathf.RoundToInt((Mathf.Repeat(rot.y, 360f) / 90f) % 4)),
				 (byte)(Mathf.RoundToInt((Mathf.Repeat(rot.z, 360f) / 90f) % 4)),
				 (byte)(scale.x > 0f ? 0 : 1)
			);
			if (TRANSFORM_TO_MAGIC_BYTE_MAP.ContainsKey(v4)) {
				return TRANSFORM_TO_MAGIC_BYTE_MAP[v4];
			} else {
				return 4;
			}
		}



		#endregion



		public static bool InRange (int x, int y, int z, int sizeX, int sizeY, int sizeZ) {
			return x >= 0 && x < sizeX && y >= 0 && y < sizeY && z >= 0 && z < sizeZ;
		}


	}



}