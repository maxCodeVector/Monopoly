namespace VoxeltoUnity {
	using Moenen.Saving;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;


	// Scene Part
	public partial class VoxelEditorWindow {




		#region --- VAR ---



		// Global
		private const string ROOT_NAME = "Voxel_Editor_Root";
		private const int MAX_QUAD_COUNT = 42000;
		private static int LAYER_ID;
		private static int LAYER_ID_ALPHA;
		private static int LAYER_MASK;
		private static int LAYER_MASK_ALPHA;
		private static int COLOR_SHADER_ID;
		private static Shader QUAD_SHADER;
		private static Shader GROUND_SHADER;
		private static Shader MESH_SHADER;
		private static readonly Vector3[] NORMAL_DIRECTION_OFFSET_ALT = new Vector3[6] {
			new Vector3(0, 0.5f, 0),// u
			new Vector3(0, -0.5f, 0),// d
			new Vector3(-0.5f, 0, 0),// l 
			new Vector3(0.5f, 0, 0),// r 
			new Vector3(0, 0, 0.5f),// f
			new Vector3(0, 0, -0.5f),// b
		};
		private static readonly RaycastHit[] HITS = new RaycastHit[12];


		// Short
		private static Mesh ConeMesh {
			get {
				if (!_ConeMesh) {
					_ConeMesh = Util.CreateConeMesh(1f, 1f);
				}
				return _ConeMesh;
			}
		}



		// Data
		private static Mesh _ConeMesh = null;
		private MeshRenderer HightlightMR = null;
		private Transform HighlightTF = null;
		private Transform Root = null;
		private Transform CameraRoot = null;
		private Transform BoxRoot = null;
		private Transform RigEditingRoot = null;
		private Transform SpriteEditingRoot = null;
		private Transform GeneratorEditingRoot = null;
		private Transform BoneRoot = null;
		private Transform CubeTF = null;
		private Transform ContainerTF = null;
		private Transform MoveBoneAsixRootTF = null;
		private Transform DirectionSignRoot = null;
		private Camera Camera = null;
		private Camera AlphaCamera = null;
		private Int3 HoveringVoxelPosition = null;
		private Int3 HoveredVoxelPosition = null;
		private float CameraSizeMin;
		private float CameraSizeMax;


		// Saving
		private EditorSavingBool ShowBackgroundBox = new EditorSavingBool("VEditor.ShowBackgroundBox", true);
		private EditorSavingColor VoxelFaceColorF = new EditorSavingColor("VEditor.VoxelFaceColorF", new Color(1f, 1f, 1f));
		private EditorSavingColor VoxelFaceColorB = new EditorSavingColor("VEditor.VoxelFaceColorB", new Color(1f, 1f, 1f));
		private EditorSavingColor VoxelFaceColorU = new EditorSavingColor("VEditor.VoxelFaceColorU", new Color(0.9f, 0.9f, 0.9f));
		private EditorSavingColor VoxelFaceColorD = new EditorSavingColor("VEditor.VoxelFaceColorD", new Color(0.5f, 0.5f, 0.5f));
		private EditorSavingColor VoxelFaceColorL = new EditorSavingColor("VEditor.VoxelFaceColorL", new Color(0.8f, 0.8f, 0.8f));
		private EditorSavingColor VoxelFaceColorR = new EditorSavingColor("VEditor.VoxelFaceColorR", new Color(0.8f, 0.8f, 0.8f));


		#endregion




		#region --- GUI ---



		private void CubeGUI () {
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
				ViewCastHit((hit) => {
					if (hit.transform.name == "Cube") {
						Quaternion lookRot = Quaternion.LookRotation(-hit.normal);
						Vector3 euler = lookRot.eulerAngles;
						euler.z = 0f;
						CameraRoot.rotation = Quaternion.Euler(euler);
						CameraRoot.localPosition = Vector3.zero;
						Event.current.Use();
						Repaint();
						RefreshCubeTransform();
					}
				});
			}
		}



		private void HighlightGUI () {

			if (HighlightTF && (
				Event.current.type == EventType.MouseMove ||
				(Event.current.type == EventType.MouseDrag && Event.current.button == 0) ||
				Event.current.type == EventType.MouseUp ||
				Event.current.type == EventType.ScrollWheel ||
				Event.current.type == EventType.MouseDown
			)) {

				HoveringVoxelPosition = null;

				if (!ViewRect.Contains(Event.current.mousePosition)) {
					if (HighlightTF.gameObject.activeSelf) {
						HighlightTF.gameObject.SetActive(false);
						Repaint();
					}
					return;
				}

				ViewCastHit((hit) => {
					string hitName = hit.transform.name;

					if (hitName == "Cube") {
						// Cube
						HighlightTF.gameObject.SetActive(true);
						HighlightTF.position = hit.point;
						HighlightTF.localScale = hit.transform.localScale * 0.5f;
						HighlightTF.rotation = Quaternion.LookRotation(-hit.normal);
						Repaint();
					} else if (IsRigging) {
						if (PaintingBoneIndex == -1 || CurrentBrushType == BrushType.Rect) {
							if (HighlightTF.gameObject.activeSelf) {
								HighlightTF.gameObject.SetActive(false);
								Repaint();
							}
						} else {
							// Editing Bone
							if (hitName == "Voxel") {
								HighlightTF.gameObject.SetActive(true);
								HighlightTF.position = hit.transform.position;
								HighlightTF.localScale = hit.transform.localScale;
								HighlightTF.rotation = Quaternion.LookRotation(-hit.normal);
								HoveringVoxelPosition = hit.transform.parent.localPosition;
								HoveredVoxelPosition = HoveringVoxelPosition;
								Repaint();
							} else if (hitName == "Box") {
								HighlightTF.gameObject.SetActive(true);
								int maxAxis = Util.MaxAxis(hit.normal);
								Vector3 offset = ContainerTF.position;
								var newPos = new Vector3(
									Mathf.Round(hit.point.x - offset.x) + offset.x,
									Mathf.Round(hit.point.y - offset.y) + offset.y,
									Mathf.Round(hit.point.z - offset.z) + offset.z
								);
								newPos[maxAxis] = hit.point[maxAxis];
								HighlightTF.position = newPos;
								HighlightTF.localScale = Vector3.one;
								HighlightTF.rotation = Quaternion.LookRotation(-hit.normal);
								HoveringVoxelPosition = HighlightTF.localPosition - ContainerTF.localPosition;
								HoveredVoxelPosition = HoveringVoxelPosition;
								Repaint();
							} else {
								if (HighlightTF.gameObject.activeSelf) {
									HighlightTF.gameObject.SetActive(false);
									Repaint();
								}
							}
						}
					} else {
						if (HighlightTF.gameObject.activeSelf) {
							HighlightTF.gameObject.SetActive(false);
							Repaint();
						}
					}

				}, () => {
					if (HighlightTF.gameObject.activeSelf) {
						HighlightTF.gameObject.SetActive(false);
						Repaint();
					}
				});

			}
		}



		#endregion




		#region --- LGC ---



		// Spawn Roots
		private bool SpawnRoot () {

			if (!Data) { return false; }

			try {

				// Root
				Root = new GameObject(ROOT_NAME).transform;
				Root.gameObject.hideFlags = HideFlags.HideAndDontSave;
				Root.position = Random.insideUnitSphere * Random.Range(10240f, 20480f);
				Root.localScale = Vector3.one;

				// Camera
				SpawnCamera();

				// Environment
				SpawnContainer();
				SpawnBox();
				SpawnDirectionSign();

				// UI
				SpawnHightlight();
				SpawnCube();

				// Edit Mode
				SpawnRigRoot();
				SpawnSpriteRoot();
				SpawnGeneratorRoot();
				SpawnMoveBoneAsixRoot();

				// Faces (Must be Last)
				bool success = SpawnVoxelFaces();
				Util.ClearProgressBar();
				return success;

			} catch (System.Exception ex) {
				Util.ClearProgressBar();
				Debug.LogError(ex.Message);
				return false;
			}
		}



		private void RemoveRoot () {
			var oldRoot = GameObject.Find(ROOT_NAME);
			if (oldRoot) {
				DestroyImmediate(oldRoot, false);
			} else if (Root) {
				DestroyImmediate(Root.gameObject, false);
			}
			Root = null;
			Camera = null;
			ContainerTF = null;
			Resources.UnloadUnusedAssets();
		}



		private void SpawnCamera () {
			// Camera Root
			CameraRoot = new GameObject("Camera Root").transform;
			CameraRoot.SetParent(Root, false);
			CameraRoot.localPosition = Vector3.zero;
			CameraRoot.localRotation = Quaternion.Euler(33.5f, 33.5f, 0f);

			// Camera
			Camera = GetNewCamera();
			AlphaCamera = GetNewCamera();
			Camera.cullingMask = LAYER_MASK;
			AlphaCamera.cullingMask = LAYER_MASK_ALPHA;

			CameraSizeMin = 5f;
			CameraSizeMax = 20f;


		}



		private void SpawnContainer () {

			// Container
			var voxels = Data.Voxels[CurrentModelIndex];
			int sizeX = voxels.GetLength(0);
			int sizeY = voxels.GetLength(1);
			int sizeZ = voxels.GetLength(2);
			ContainerTF = new GameObject("Container").transform;
			ContainerTF.SetParent(Root, false);
			ContainerTF.localPosition = -new Vector3(sizeX / 2f, sizeY / 2f, sizeZ / 2f);

		}



		private void SpawnBox () {

			// Box
			BoxRoot = new GameObject("Box Root").transform;
			BoxRoot.SetParent(Root, false);
			BoxRoot.localPosition = new Vector3(-0.5f, -0.5f, -0.5f);
			Vector3 size = Data.GetModelSize(CurrentModelIndex);
			Vector3 fixedSize = size + new Vector3(8, 4, 8);

			CreateBoxGroundQuad( // D
				new Vector3(0, -size.y * 0.5f, 0),
				new Vector3(90, 0, 0),
				new Vector3(fixedSize.x, fixedSize.z, 1f),
				new Color(0.2f, 0.2f, 0.21f)
			);

			CreateBoxGroundQuad( // U
				new Vector3(0, fixedSize.y * 0.5f + (fixedSize.y - size.y) / 2f, 0),
				new Vector3(-90, 0, 0),
				new Vector3(fixedSize.x, fixedSize.z, 1f),
				new Color(0.2f, 0.2f, 0.21f)
			);

			CreateBoxGroundQuad( // F
				new Vector3(0, (fixedSize.y - size.y) / 2f, fixedSize.z * 0.5f),
				new Vector3(0, 0, 0),
				new Vector3(fixedSize.x, fixedSize.y, 1f),
				new Color(0.24f, 0.24f, 0.25f)
			);

			CreateBoxGroundQuad( // B
				new Vector3(0, (fixedSize.y - size.y) / 2f, -fixedSize.z * 0.5f),
				new Vector3(180, 0, 0),
				new Vector3(fixedSize.x, fixedSize.y, 1f),
				new Color(0.24f, 0.24f, 0.25f)
			);

			CreateBoxGroundQuad( // L
				new Vector3(-fixedSize.x * 0.5f, (fixedSize.y - size.y) / 2f, 0),
				new Vector3(0, -90, 0),
				new Vector3(fixedSize.z, fixedSize.y, 1f),
				new Color(0.22f, 0.22f, 0.225f)
			);

			CreateBoxGroundQuad( // R
				new Vector3(fixedSize.x * 0.5f, (fixedSize.y - size.y) / 2f, 0),
				new Vector3(0, 90, 0),
				new Vector3(fixedSize.z, fixedSize.y, 1f),
				new Color(0.22f, 0.22f, 0.225f)
			);

			SetBoxBackgroundActive(ShowBackgroundBox);

		}



		private void SpawnDirectionSign () {

			var dRoot = new GameObject("Direction Sign").transform;
			dRoot.SetParent(Root, false);
			dRoot.localPosition = Vector3.zero;
			dRoot.localRotation = Quaternion.identity;
			dRoot.localScale = Vector3.one;

			// Arrow
			DirectionSignRoot = new GameObject("Arrow F", typeof(MeshRenderer), typeof(MeshFilter)).transform;
			DirectionSignRoot.SetParent(dRoot, false);
			DirectionSignRoot.gameObject.layer = LAYER_ID;
			DirectionSignRoot.gameObject.SetActive(ShowDirectionSign);
			DirectionSignRoot.localScale = new Vector3(0.618f, 2.4f, 0.618f);
			DirectionSignRoot.localRotation = Quaternion.Euler(90, 0, 0);

			var mf = DirectionSignRoot.GetComponent<MeshFilter>();
			mf.mesh = ConeMesh;

			var color = new Color(66f / 255f, 140f / 255f, 242f / 255f);
			var mr = DirectionSignRoot.GetComponent<MeshRenderer>();
			var mat = new Material(QUAD_SHADER);
			mat.SetColor(COLOR_SHADER_ID, color);
			mr.material = mat;

			FixDirectionSignArrowPosition();

		}



		private void SpawnHightlight () {

			// Highlight
			HighlightTF = new GameObject("Highlight Root").transform;
			HighlightTF.SetParent(Root, false);
			HighlightTF.localPosition = Vector3.zero;
			HighlightTF.localRotation = Quaternion.identity;
			HighlightTF.localScale = Vector3.one;

			var highlightQuad = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
			highlightQuad.SetParent(HighlightTF, false);
			highlightQuad.localPosition = new Vector3(0f, 0f, -0.01f);
			highlightQuad.localRotation = Quaternion.identity;
			highlightQuad.localScale = Vector3.one;
			highlightQuad.gameObject.layer = LAYER_ID;

			var mr = highlightQuad.GetComponent<MeshRenderer>();
			HightlightMR = mr;
			HightlightMR.material = new Material(QUAD_SHADER);
			SetHightlightColor(Color.red);

			var col = highlightQuad.GetComponent<Collider>();
			if (col) {
				DestroyImmediate(col, false);
			}

			HighlightTF.gameObject.SetActive(false);

		}



		private void SpawnCube () {
			// Cube
			CubeTF = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
			CubeTF.gameObject.name = "Cube";
			CubeTF.SetParent(Camera.transform, false);
			CubeTF.localRotation = Quaternion.identity;
			CubeTF.gameObject.layer = LAYER_ID;
			var cubeMR = CubeTF.GetComponent<MeshRenderer>();
			if (cubeMR) {
				DestroyImmediate(cubeMR, false);
			}
			RefreshCubeTransform();

			// Quad in Cube
			CreateAxisCubeQuad( // B
				new Vector3(0, 0, -0.5f),
				new Vector3(0, 0, 0),
				new Color(0.5f, 0.5f, 0.5f)
			);
			CreateAxisCubeQuad( // F
				new Vector3(0, 0, 0.5f),
				new Vector3(180, 0, 0),
				new Color(66f / 255f, 140f / 255f, 242f / 255f)
			);

			CreateAxisCubeQuad( // L
				new Vector3(-0.5f, 0, 0),
				new Vector3(0, 90, 0),
				new Color(0.4f, 0.4f, 0.4f)
			);
			CreateAxisCubeQuad( // R
				new Vector3(0.5f, 0, 0),
				new Vector3(0, -90, 0),
				new Color(242f / 255f, 74f / 255f, 37f / 255f)
			);

			CreateAxisCubeQuad( // D
				new Vector3(0, -0.5f, 0),
				new Vector3(-90, 0, 0),
				new Color(0.3f, 0.3f, 0.3f)
			);

			CreateAxisCubeQuad( // U
				new Vector3(0, 0.5f, 0),
				new Vector3(90, 0, 0),
				new Color(115f / 255f, 191f / 255f, 53f / 255f)
			);
		}



		private bool SpawnVoxelFaces () {

			// Faces
			if (!Data) { return false; }

			int[,,] voxels = Data.Voxels[CurrentModelIndex];
			bool tooLargeAsked = false;
			int sizeX = voxels.GetLength(0);
			int sizeY = voxels.GetLength(1);
			int sizeZ = voxels.GetLength(2);
			float maxSize = Mathf.Max(sizeX, sizeY, sizeZ, 12);
			CameraSizeMin = 5f;
			CameraSizeMax = maxSize * 2f;
			SetCameraSize((CameraSizeMin + CameraSizeMax) * 0.5f);
			SetCameraFarClip(maxSize * 4f);
			SetCameraPosition(new Vector3(0f, 0f, -maxSize * 2f));
			ContainerTF.localPosition = -new Vector3(sizeX / 2f, sizeY / 2f, sizeZ / 2f);

			switch (CurrentEditorMode) {
				case EditorMode.Rigging:
					// Quads
					for (int x = 0; x < sizeX; x++) {
						Util.ProgressBar("", "Importing...", ((float)x / (sizeX - 1)) * 0.5f + 0.5f);
						for (int y = 0; y < sizeY; y++) {
							for (int z = 0; z < sizeZ; z++) {

								if (voxels[x, y, z] == 0) { continue; }
								AddQuad(x, y, z, voxels[x, y, z], sizeX, sizeY, sizeZ);

								if (!tooLargeAsked && ContainerTF.childCount > MAX_QUAD_COUNT) {
									Util.ClearProgressBar();
									bool go = Util.Dialog("", "This model is too large. Still want to edit it ?", "Yes", "Cancel");
									tooLargeAsked = true;
									if (!go) {
										return false;
									}
								}

							}
						}
					}
					break;
				case EditorMode.Sprite:
				case EditorMode.MapGenerator:
				case EditorMode.CharacterGenerator:
					bool allInOne = CurrentEditorMode == EditorMode.CharacterGenerator;
					// Mesh
					var result = Core_Voxel.CreateModel(Data, 1f, Core_Voxel.LightMapSupportType.SmallTextureButNoLightmap, false, Vector3.one * 0.5f);
					var vModel = result.VoxelModels != null && result.VoxelModels.Length > 0 ? result.VoxelModels[0] : null;
					for (int modelIndex = allInOne ? 0 : CurrentModelIndex; modelIndex < (allInOne ? vModel.Meshs.Length : CurrentModelIndex + 1); modelIndex++) {
						var vMesh = vModel != null && vModel.Meshs != null && vModel.Meshs.Length > modelIndex ? vModel.Meshs[modelIndex] : null;
						if (vMesh == null) { break; }
						for (int i = 0; i < vMesh.Count; i++) {
							var mesh = vMesh.GetMeshAt(i);
							Texture2D texture;
							if (IsSpriting) {
								texture = vModel != null && vModel.Textures != null && vModel.Textures.Length > CurrentModelIndex ? vModel.Textures[CurrentModelIndex] : null;
							} else {
								texture = CurrentModelIndex == 0 && vModel != null && vModel.Textures != null && vModel.Textures.Length > 0 ? vModel.Textures[0] : null;
							}
							if (!mesh) { break; }

							FixMeshColorByNormal(mesh);

							var meshTF = new GameObject("_mesh", typeof(MeshRenderer), typeof(MeshFilter)).transform;
							meshTF.SetParent(ContainerTF);
							meshTF.localPosition = new Vector3(sizeX / 2f, sizeY / 2f, sizeZ / 2f);
							meshTF.localScale = Vector3.one;
							meshTF.localRotation = Quaternion.identity;
							meshTF.gameObject.layer = LAYER_ID;

							var mf = meshTF.GetComponent<MeshFilter>();
							mf.mesh = mesh;

							var mr = meshTF.GetComponent<MeshRenderer>();
							var mat = new Material(MESH_SHADER);
							mat.SetColor(COLOR_SHADER_ID, Color.white);
							mat.mainTexture = texture ? texture : Texture2D.blackTexture;
							mr.material = mat;
							mr.receiveShadows = false;
							mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
						}
					}
					break;
			}
			return true;
		}



		private void SpawnRigRoot () {

			// Root
			RigEditingRoot = new GameObject("Rig Root").transform;
			RigEditingRoot.SetParent(Root, false);
			RigEditingRoot.localPosition = ContainerTF.localPosition;
			RigEditingRoot.localRotation = Quaternion.identity;
			RigEditingRoot.localScale = Vector3.one;

		}



		private void SpawnSpriteRoot () {

			// Root
			SpriteEditingRoot = new GameObject("Sprite Root").transform;
			SpriteEditingRoot.SetParent(Root, false);
			SpriteEditingRoot.localPosition = Vector3.zero;
			SpriteEditingRoot.localRotation = Quaternion.identity;
			SpriteEditingRoot.localScale = Vector3.one;




		}



		private void SpawnGeneratorRoot () {

			// Root
			GeneratorEditingRoot = new GameObject("Generator Root").transform;
			GeneratorEditingRoot.SetParent(Root, false);
			GeneratorEditingRoot.localPosition = Vector3.zero;
			GeneratorEditingRoot.localRotation = Quaternion.identity;
			GeneratorEditingRoot.localScale = Vector3.one;

		}


		private void SpawnMoveBoneAsixRoot () {

			if (CurrentEditorMode != EditorMode.Rigging) { return; }

			MoveBoneAsixRootTF = new GameObject("Move Bone Axis").transform;
			MoveBoneAsixRootTF.SetParent(RigEditingRoot);
			MoveBoneAsixRootTF.localPosition = Vector3.zero;
			MoveBoneAsixRootTF.localRotation = Quaternion.identity;
			MoveBoneAsixRootTF.localScale = Vector3.one;
			MoveBoneAsixRootTF.gameObject.SetActive(false);

			// X
			var tfX = SpawnAxis(new Color(242f / 255f, 74f / 255f, 37f / 255f));
			tfX.name = "X";
			tfX.SetParent(MoveBoneAsixRootTF);
			tfX.SetAsLastSibling();
			tfX.localPosition = Vector3.zero;
			tfX.localRotation = Quaternion.Euler(0, 0, -90);
			tfX.localScale = Vector3.one;

			// Y
			var tfY = SpawnAxis(new Color(115f / 255f, 191f / 255f, 53f / 255f));
			tfY.name = "Y";
			tfY.SetParent(MoveBoneAsixRootTF);
			tfY.SetAsLastSibling();
			tfY.localPosition = Vector3.zero;
			tfY.localRotation = Quaternion.Euler(0, 0, 0);
			tfY.localScale = Vector3.one;

			// Z
			var tfZ = SpawnAxis(new Color(66f / 255f, 140f / 255f, 242f / 255f));
			tfZ.name = "Z";
			tfZ.SetParent(MoveBoneAsixRootTF);
			tfZ.SetAsLastSibling();
			tfZ.localPosition = Vector3.zero;
			tfZ.localRotation = Quaternion.Euler(90, 0, 0);
			tfZ.localScale = Vector3.one;



		}




		// Misc
		private Vector2 GetGUIScreenPosition (Vector2 guiPosition) {
			var pos = guiPosition - ViewRect.position;
			pos.y = ViewRect.height - pos.y;
			return pos;
		}



		private Vector2 GetGUIPosition (Vector2 guiScreenPosition) {
			guiScreenPosition.y = ViewRect.height - guiScreenPosition.y;
			return guiScreenPosition + ViewRect.position;
		}



		private void ViewCastHit (System.Action<RaycastHit> action, System.Action noneAction = null) {
			if (!Camera) { return; }
			var ray = Camera.ScreenPointToRay(GetGUIScreenPosition(Event.current.mousePosition));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				action(hit);
			} else if (noneAction != null) {
				noneAction();
			}
		}



		private void ViewCastHitAll (System.Action<RaycastHit[], int> action) {
			var pos = Event.current.mousePosition - ViewRect.position;
			pos.y = ViewRect.height - pos.y;
			int len = Physics.RaycastNonAlloc(Camera.ScreenPointToRay(pos), HITS);
			action(HITS, len);
		}



		private Camera GetNewCamera () {
			var camera = new GameObject("Camera", typeof(Camera)).GetComponent<Camera>();
			camera.transform.SetParent(CameraRoot, false);
			SetCameraPosition(new Vector3(0f, 0f, -10f));
			camera.targetTexture = new RenderTexture(1, RENDER_TEXTURE_HEIGHT, 24) { antiAliasing = 2, };
			camera.forceIntoRenderTexture = true;
			camera.allowHDR = false;
			camera.allowMSAA = false;
#if UNITY_2018
			camera.allowDynamicResolution = false;
#endif
			camera.useOcclusionCulling = false;
			camera.orthographic = true;
			camera.nearClipPlane = 0f;
			camera.farClipPlane = 100f;
			camera.clearFlags = CameraClearFlags.Color;
			camera.backgroundColor = Color.clear;
			camera.orthographicSize = 10f;
			return camera;
		}



		private void AddQuad (int x, int y, int z, int colorIndex, int sizeX, int sizeY, int sizeZ) {

			var quadRoot = new GameObject("Voxel Cube").transform;
			quadRoot.SetParent(ContainerTF, false);
			quadRoot.localPosition = new Vector3(x, y, z);

			for (int i = 0; i < 6; i++) {

				Direction dir = (Direction)i;
				if (!Data.IsExposed(CurrentModelIndex, x, y, z, sizeX, sizeY, sizeZ, dir)) {
					var empty = new GameObject("_").transform;
					empty.SetParent(quadRoot, false);
					empty.SetAsLastSibling();
					empty.gameObject.SetActive(false);
					continue;
				}

				// Quad
				var quad = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
				quad.gameObject.name = "Voxel";
				quad.SetParent(quadRoot, false);
				quad.SetAsLastSibling();
				quad.localPosition = NORMAL_DIRECTION_OFFSET_ALT[i];
				quad.localRotation = GetQuadRotation(dir);
				quad.gameObject.layer = LAYER_ID;
				var mr = quad.GetComponent<MeshRenderer>();
				if (mr) {
					var mat = new Material(QUAD_SHADER);
					var color = Data.GetColorFromPalette(colorIndex);
					mat.SetColor(COLOR_SHADER_ID, GetVoxelTintColor(color, dir));
					mr.material = mat;
				}
			}

		}



		private Quaternion GetQuadRotation (Direction dir) {
			switch (dir) {
				default:
				case Direction.Up:
					return Quaternion.Euler(90, 0, 0);
				case Direction.Down:
					return Quaternion.Euler(-90, 0, 0);
				case Direction.Left:
					return Quaternion.Euler(0, 90, 0);
				case Direction.Right:
					return Quaternion.Euler(0, -90, 0);
				case Direction.Front:
					return Quaternion.Euler(0, 180, 0);
				case Direction.Back:
					return Quaternion.Euler(0, 0, 0);
			}
		}



		private Direction GetQuadDirection (Transform qaudTF) {
			return (Direction)qaudTF.GetSiblingIndex();
		}



		private void SetQuadToAlpha (Transform renderTF, bool alpha) {
			renderTF.gameObject.layer = alpha ? LAYER_ID_ALPHA : LAYER_ID;
		}



		private void SetAllQuadsColor (System.Func<int, int, int, Color, Color> func) {

			var voxels = Data.Voxels[CurrentModelIndex];

			int len = ContainerTF.childCount;
			for (int i = 0; i < len; i++) {

				var voxelTF = ContainerTF.GetChild(i);
				int vLen = voxelTF.childCount;
				var pos = voxelTF.localPosition;

				int x = Mathf.Clamp(Mathf.RoundToInt(pos.x), 0, ModelSize.x - 1);
				int y = Mathf.Clamp(Mathf.RoundToInt(pos.y), 0, ModelSize.y - 1);
				int z = Mathf.Clamp(Mathf.RoundToInt(pos.z), 0, ModelSize.z - 1);

				var color = func.Invoke(x, y, z, Data.GetColorFromPalette(voxels[x, y, z]));

				// Color
				bool alpha = color.a < 0.5f;
				color.a = 1f;
				for (int j = 0; j < vLen; j++) {
					var quadTF = voxelTF.GetChild(j);
					if (!quadTF.gameObject.activeSelf) { continue; }
					var dir = GetQuadDirection(quadTF);
					SetQuadToAlpha(quadTF, alpha);
					var mat = quadTF.GetComponent<MeshRenderer>().sharedMaterial;
					mat.SetColor(COLOR_SHADER_ID, GetVoxelTintColor(color, dir));
				}

			}

		}



		private void SetAllQuadsColorToNormal (bool alpha = false) {
			SetAllQuadsColor((x, y, z, color) => {
				color.a = alpha ? 0f : 1f;
				return color;
			});
		}



		private void RefreshCubeTransform () {
			if (CubeTF && Camera && CameraRoot) {
				float scaleMuti = Camera.orthographicSize * 0.1f;
				CubeTF.localPosition = new Vector3(
					Camera.aspect * Camera.orthographicSize - scaleMuti * 3f,
					Camera.orthographicSize - scaleMuti * 1.2f,
					scaleMuti
				);
				CubeTF.rotation = Quaternion.identity;
				CubeTF.localScale = Vector3.one * scaleMuti;
			}
		}



		private void CreateAxisCubeQuad (Vector3 pos, Vector3 rot, Color color) {
			var q = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
			q.gameObject.layer = LAYER_ID;
			q.gameObject.name = "";
			q.SetParent(CubeTF, false);
			q.localPosition = pos;
			q.rotation = Quaternion.Euler(rot);
			var mr = q.GetComponent<MeshRenderer>();
			if (mr) {
				var mat = new Material(GROUND_SHADER);
				mat.SetColor("_Color", color);
				mr.material = mat;
			}
			var col = q.GetComponent<Collider>();
			if (col) {
				DestroyImmediate(col, false);
			}
		}



		private void CreateBoxGroundQuad (Vector3 pos, Vector3 rot, Vector3 size, Color color) {
			var groundD = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
			groundD.gameObject.layer = LAYER_ID;
			groundD.gameObject.name = "Box";
			groundD.SetParent(BoxRoot, false);
			groundD.localPosition = pos;
			groundD.rotation = Quaternion.Euler(rot);
			groundD.localScale = size;
			var mr = groundD.GetComponent<MeshRenderer>();
			if (mr) {
				var mat = new Material(GROUND_SHADER);
				mat.SetColor("_Color", color);
				mr.material = mat;
			}
		}



		private void SetBoxBackgroundActive (bool show) {
			ShowBackgroundBox.Value = show;
			BoxRoot.gameObject.SetActive(show);
		}



		private void SetAllVoxelCollidersEnable (bool enable) {
			var cols = ContainerTF.GetComponentsInChildren<Collider>();
			for (int i = 0; i < cols.Length; i++) {
				cols[i].enabled = enable;
			}
		}



		private void SetCameraSize (float size) {
			Camera.orthographicSize = size;
			AlphaCamera.orthographicSize = size;
		}



		private void SetCameraFarClip (float far) {
			Camera.farClipPlane = far;
			AlphaCamera.farClipPlane = far;
		}



		private void SetCameraPosition (Vector3 pos) {
			if (Camera) {
				Camera.transform.localPosition = pos;
			}
			if (AlphaCamera) {
				AlphaCamera.transform.localPosition = pos;
			}
		}



		private Transform SpawnAxis (Color coneColor) {

			var tf = new GameObject("", typeof(CapsuleCollider)).transform;
			var col = tf.GetComponent<CapsuleCollider>();
			col.radius = 0.3f;
			col.center = new Vector3(0, 1f, 0);
			col.height = 2f;

			// Cone
			coneColor.a = 0.618f;
			var coneMat = new Material(QUAD_SHADER);
			coneMat.SetColor(COLOR_SHADER_ID, coneColor);
			var cone = new GameObject("Cone", typeof(MeshRenderer), typeof(MeshFilter)).transform;
			cone.SetParent(tf);
			cone.localPosition = new Vector3(0, 0.618f * 2f, 0);
			cone.localRotation = Quaternion.identity;
			cone.localScale = new Vector3(0.15f, 0.8f, 0.15f);
			cone.GetComponent<MeshRenderer>().material = coneMat;
			cone.GetComponent<MeshFilter>().mesh = ConeMesh;
			cone.gameObject.layer = LAYER_ID;

			// Line
			var line = GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform;
			line.name = "Line";
			line.SetParent(tf);
			line.localPosition = new Vector3(0, 0.618f, 0);
			line.localRotation = Quaternion.identity;
			line.localScale = new Vector3(0.024f, 0.618f, 0.024f);
			line.gameObject.layer = LAYER_ID;
			line.GetComponent<MeshRenderer>().material = coneMat;
			DestroyImmediate(line.GetComponent<Collider>(), false);

			return tf;
		}



		private void ForAllTransformsIn (Transform root, System.Action<Transform> action) {
			int childCount = root.childCount;
			for (int i = 0; i < childCount; i++) {
				action(root.GetChild(i));
			}
		}



		private void ForAllTransformsIn (Transform root, System.Action<Transform, int> action) {
			int childCount = root.childCount;
			for (int i = 0; i < childCount; i++) {
				action(root.GetChild(i), i);
			}
		}



		private void ForAllTransformsIn (Transform root, System.Func<Transform, bool> action) {
			int childCount = root.childCount;
			for (int i = 0; i < childCount; i++) {
				if (action(root.GetChild(i))) {
					break;
				}
			}
		}



		private Color GetVoxelTintColor (Color color, Direction dir) {
			switch (dir) {
				default:
				case Direction.Up:
					return color * VoxelFaceColorU;
				case Direction.Down:
					return color * VoxelFaceColorD;
				case Direction.Left:
					return color * VoxelFaceColorL;
				case Direction.Right:
					return color * VoxelFaceColorR;
				case Direction.Front:
					return color * VoxelFaceColorF;
				case Direction.Back:
					return color * VoxelFaceColorB;
			}
		}



		private void SetHightlightColor (Color color) {
			if (HightlightMR) {
				HightlightMR.sharedMaterial.SetColor(COLOR_SHADER_ID, color);
			}
		}



		private void FixMeshColorByNormal (Mesh mesh) {
			if (!mesh) { return; }
			var normals = mesh.normals;
			var colors = mesh.colors;
			int len = colors.Length;
			int normalLen = normals.Length;
			var BlackColor = Color.black;
			for (int i = 0; i < len; i++) {
				if (i < normalLen) {
					var normal = normals[i];
					var dir =
						normal.x < -0.1f ? Direction.Left :
						normal.x > 0.1f ? Direction.Right :
						normal.y < -0.1f ? Direction.Down :
						normal.y > 0.1f ? Direction.Up :
						normal.z > 0.1f ? Direction.Front :
						Direction.Back;
					colors[i] = GetVoxelTintColor(colors[i], dir);
				}
			}
			mesh.SetColors(new List<Color>(colors));
			mesh.UploadMeshData(false);
		}



		#endregion



	}
}