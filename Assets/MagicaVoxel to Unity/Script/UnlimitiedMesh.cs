namespace VoxeltoUnity {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;


	public class UnlimitiedMesh {


		// Global
		private const int MAX_VERTEX_COUNT = 65532;


		// API
		public int Count {
			get {
				return Meshs.Count;
			}
		}

		public Mesh this[int index] {
			get {
				return GetMeshAt(index);
			}
		}


		// Data
		private List<Mesh> Meshs = new List<Mesh>();




		public UnlimitiedMesh (List<Vector3> vertices, List<Vector2> uvs, List<BoneWeight> boneWeights = null) {

			int vCount = vertices.Count;
			int meshNum = vCount / MAX_VERTEX_COUNT + 1;

			Meshs = new List<Mesh>();

			for (int index = 0; index < meshNum; index++) {

				var mesh = new Mesh();

				// Vertices
				int vertCount = Mathf.Min(MAX_VERTEX_COUNT, vertices.Count - index * MAX_VERTEX_COUNT);
				mesh.SetVertices(vertices.GetRange(index * MAX_VERTEX_COUNT, vertCount));

				// UV
				mesh.SetUVs(0, uvs.GetRange(
					index * MAX_VERTEX_COUNT,
					Mathf.Min(MAX_VERTEX_COUNT, uvs.Count - index * MAX_VERTEX_COUNT)
				));

				// Tri
				mesh.SetTriangles(GetDefaultTriangleData(vertCount), 0);
				mesh.UploadMeshData(false);

				// Color
				mesh.colors = GetWhiteColors(vertCount);

				// BoneWeights
				if (boneWeights != null && boneWeights.Count > 0) {
					mesh.boneWeights = boneWeights.GetRange(index * MAX_VERTEX_COUNT, vertCount).ToArray();
				}

				mesh.RecalculateNormals();
				mesh.UploadMeshData(false);

				Meshs.Add(mesh);

			}

		}



		public Mesh GetMeshAt (int index) {
			return Meshs[index];
		}



		private int[] GetDefaultTriangleData (int verCount) {
			int quadCount = verCount / 4;
			int[] result = new int[quadCount * 6];
			for (int i = 0; i < quadCount; i++) {
				result[i * 6] = i * 4;
				result[i * 6 + 1] = i * 4 + 1;
				result[i * 6 + 2] = i * 4 + 2;
				result[i * 6 + 3] = i * 4;
				result[i * 6 + 4] = i * 4 + 2;
				result[i * 6 + 5] = i * 4 + 3;
			}
			return result;
		}



		private Color[] GetWhiteColors (int verCount) {
			var colors = new Color[verCount];
			Color c = Color.white;
			for (int i = 0; i < verCount; i++) {
				colors[i] = c;
			}
			return colors;
		}



	}
}