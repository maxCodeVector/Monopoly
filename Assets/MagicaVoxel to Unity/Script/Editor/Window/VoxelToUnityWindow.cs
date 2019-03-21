namespace VoxeltoUnity {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using Moenen;
	using Moenen.Saving;


	public class VoxelToUnityWindow : MoenenEditorWindow {




		#region --- SUB ---



		public struct PathData {
			public string Path;
			public string Extension;
			public string Root;
		}




		public enum Task {
			Prefab = 0,
			Lod = 1,
			Obj = 2,
			ToJson = 3,
			ToVox = 4,
			ToQb = 5,
		}




		public enum ExportMod {
			Specified = 0,
			OriginalPath = 1,
			AskEverytime = 2,
		}



		public enum PivotMod {
			Specified = 0,
			MagicaVoxelWorldMod = 1,
		}



		public enum _25DSpriteNum {
			_1 = 1,
			_4 = 4,
			_8 = 8,
			_16 = 16,
		}



		#endregion




		#region --- VAR ---



		// Short
		private static ExportMod TheExportMod {
			get {
				return (ExportMod)ExportModIndex.Value;
			}
			set {
				ExportModIndex.Value = (int)value;
			}
		}

		private static Core_Voxel.LightMapSupportType LightMapSupportMode {
			get {
				return (Core_Voxel.LightMapSupportType)LightMapSupportTypeIndex.Value;
			}
			set {
				LightMapSupportTypeIndex.Value = (int)value;
			}
		}

		public static Shader TheShader {
			get {
				return Shader.Find(ShaderPath);
			}
			set {
				ShaderPath.Value = value ? value.name : "Mobile/Diffuse";
			}
		}


		// Data
		private Vector2 MasterScrollPosition;


		// Selection
		private static Dictionary<Object, PathData> TaskMap = new Dictionary<Object, PathData>();
		private static int VoxNum = 0;
		private static int QbNum = 0;
		private static int FolderNum = 0;
		private static int ObjNum = 0;
		private static int JsonNum = 0;
		private static Texture2D VoxFileIcon = null;
		private static Texture2D QbFileIcon = null;
		private static Texture2D JsonFileIcon = null;


		// Saving
		private static EditorSavingBool ViewPanelOpen = new EditorSavingBool("V2U.ViewPanelOpen", true);
		private static EditorSavingBool CreatePanelOpen = new EditorSavingBool("V2U.CreatePanelOpen", true);
		private static EditorSavingBool SettingPanelOpen = new EditorSavingBool("V2U.SettingPanelOpen", false);
		private static EditorSavingBool AboutPanelOpen = new EditorSavingBool("V2U.AboutPanelOpen", false);
		private static EditorSavingBool ModelGenerationSettingPanelOpen = new EditorSavingBool("V2U.ModelGenerationSettingPanelOpen", false);
		private static EditorSavingBool OptimizationSettingPanelOpen = new EditorSavingBool("V2U.OptimizationSettingPanelOpen", false);
		private static EditorSavingBool SpriteGenerationSettingPanelOpen = new EditorSavingBool("V2U.SpriteGenerationSettingPanelOpen", false);
		private static EditorSavingBool SystemSettingPanelOpen = new EditorSavingBool("V2U.SystemSettingPanelOpen", false);
		private static EditorSavingBool ToolPanelOpen = new EditorSavingBool("V2U.ToolPanelOpen", true);
		private static EditorSavingBool ColorfulTitle = new EditorSavingBool("V2U.ColorfulTitle", true);
		private static EditorSavingString ExportPath = new EditorSavingString("V2U.ExportPath", "Assets");
		private static EditorSavingInt LightMapSupportTypeIndex = new EditorSavingInt("V2U.LightMapSupportTypeIndex", 0);
		private static EditorSavingInt ExportModIndex = new EditorSavingInt("V2U.ExportModIndex", 2);
		private static EditorSavingFloat ModelScale = new EditorSavingFloat("V2U.ModelScale", 0.1f);
		private static EditorSavingInt LodNum = new EditorSavingInt("V2U.LodNum", 2);
		private static EditorSavingString ShaderPath = new EditorSavingString("V2U.ShaderPath", "Mobile/Diffuse");
		private static EditorSavingBool LogMessage = new EditorSavingBool("V2U.LogMessage", true);
		private static EditorSavingBool ShowDialog = new EditorSavingBool("V2U.ShowDialog", true);
		private static EditorSavingBool EditorDockToScene = new EditorSavingBool("V2U.EditorDockToScene", true);
		private static EditorSavingVector3 ModelPivot = new EditorSavingVector3("V2U.ModelPivot", Vector3.one * 0.5f);
		private static EditorSavingBool OptimizeFront = new EditorSavingBool("V2U.OptimizeFront", true);
		private static EditorSavingBool OptimizeBack = new EditorSavingBool("V2U.OptimizeBack", true);
		private static EditorSavingBool OptimizeUp = new EditorSavingBool("V2U.OptimizeUp", true);
		private static EditorSavingBool OptimizeDown = new EditorSavingBool("V2U.OptimizeDown", true);
		private static EditorSavingBool OptimizeLeft = new EditorSavingBool("V2U.OptimizeLeft", true);
		private static EditorSavingBool OptimizeRight = new EditorSavingBool("V2U.OptimizeRight", true);



		#endregion




		#region --- MSG ---



		[MenuItem("Tools/MagicaVoxel to Unity/Voxel to Unity")]
		public static void OpenWindow () {
			var inspector = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
			VoxelToUnityWindow window = inspector != null ?
				GetWindow<VoxelToUnityWindow>("Voxel To Unity", true, inspector) :
				GetWindow<VoxelToUnityWindow>("Voxel To Unity", true);
			window.minSize = new Vector2(275, 400);
			window.maxSize = new Vector2(600, 1000);
		}



		private void OnEnable () {
			Window_Enable();
		}



		private void OnFocus () {
			RefreshSelection();
			Repaint();
		}



		private void OnSelectionChange () {
			RefreshSelection();
			Repaint();
		}



		private void OnGUI () {

			MasterScrollPosition = GUILayout.BeginScrollView(MasterScrollPosition, GUI.skin.scrollView);

			TitleGUI();

			Window_Main();

			GUILayout.EndScrollView();

			if (Event.current.type == EventType.MouseDown) {
				GUI.FocusControl(null);
				Repaint();
			}

		}



		private void TitleGUI () {

			const string MAIN_TITLE = "Voxel to Unity";
			const string MAIN_TITLE_RICH = "<color=#ff3333>V</color><color=#ffcc00>o</color><color=#ffff33>x</color><color=#33ff33>e</color><color=#33ccff>l</color><color=#eeeeee> to Unity</color>";

			Space(6);
			LayoutV(() => {
				GUIStyle style = new GUIStyle() {
					alignment = TextAnchor.LowerCenter,
					fontSize = 12,
					fontStyle = FontStyle.Bold
				};
				style.normal.textColor = Color.white;
				style.richText = true;
				Rect rect = GUIRect(0, 18);

				GUIStyle shadowStyle = new GUIStyle(style) {
					richText = false
				};

				EditorGUI.DropShadowLabel(rect, MAIN_TITLE, shadowStyle);
				GUI.Label(rect, ColorfulTitle ? MAIN_TITLE_RICH : MAIN_TITLE, style);

			});
			Space(6);
		}



		#endregion




		#region --- API ---



		public static void Window_Enable () {
			LoadSetting();
			RefreshSelection();
			RefreshMergeSetting();
		}



		public static void Window_Main () {

			ViewGUI();
			CreateGUI();
			ToolGUI();
			SettingGUI();
			AboutGUI();

			if (GUI.changed) {
				SaveSetting();
			}

		}




		#endregion




		#region --- GUI ---



		private static void ViewGUI () {

			LayoutF(() => {
				bool addSpaceFlag = true;
				Space(2);
				int iconSize = 26;

				LayoutH(() => {

					// Init
					GUIStyle labelStyle = new GUIStyle(GUI.skin.label) {
						alignment = TextAnchor.MiddleLeft,
						fontSize = 10
					};

					// Icons
					if (FolderNum > 0) {
						if (VoxNum + QbNum + JsonNum <= 0) {
							// None With Folder
							EditorGUI.HelpBox(GUIRect(0, iconSize + 14), "There are NO .vox or .qb file in selecting folder.", MessageType.Warning);
							addSpaceFlag = false;
						}
					} else if (VoxNum + QbNum + JsonNum <= 0) {
						if (ObjNum > 0) {
							// Selecting Not Voxel File
							EditorGUI.HelpBox(GUIRect(0, iconSize + 14), "The file selecting is NOT .vox or .qb file.", MessageType.Warning);
							addSpaceFlag = false;
						} else {
							// None
							EditorGUI.HelpBox(GUIRect(0, iconSize + 14), "Select *.vox, *.qb, *.json or folder in Project View.", MessageType.Info);
							addSpaceFlag = false;
						}
					}

					Space(4);

					if (VoxNum > 0) {
						// Vox
						LayoutH(() => {
							if (VoxFileIcon) {
								GUI.DrawTexture(GUIRect(iconSize, iconSize), VoxFileIcon);
							}
						}, true);
						GUI.Label(GUIRect(0, iconSize), ".vox\n× " + VoxNum.ToString(), labelStyle);
						Space(4);
					}

					if (QbNum > 0) {
						// Qb
						LayoutH(() => {
							if (QbFileIcon) {
								GUI.DrawTexture(GUIRect(iconSize, iconSize), QbFileIcon);
							}
						}, true);
						GUI.Label(GUIRect(0, iconSize), ".qb\n× " + QbNum.ToString(), labelStyle);
					}

					if (JsonNum > 0) {
						// Json
						LayoutH(() => {
							if (JsonFileIcon) {
								GUI.DrawTexture(GUIRect(iconSize, iconSize), JsonFileIcon);
							}
						}, true);
						GUI.Label(GUIRect(0, iconSize), ".json\n× " + JsonNum.ToString(), labelStyle);
					}

				});

				Space(addSpaceFlag ? 16 : 6);

				// Scale Too Small Warning
				if (ModelScale == 0) {
					EditorGUI.HelpBox(GUIRect(0, iconSize + 14), "Model scale has been set to 0. Your model will be invisible.", MessageType.Error);
					Space(6);
				} else if (ModelScale <= 0.0001f) {
					EditorGUI.HelpBox(GUIRect(0, iconSize + 14), "Model scale is too small. Your may not able to see them.", MessageType.Warning);
					Space(6);
				}

				// Combine Warning
				if (!OptimizeFront || !OptimizeBack || !OptimizeLeft || !OptimizeRight || !OptimizeUp || !OptimizeDown) {
					EditorGUI.HelpBox(GUIRect(0, iconSize + 14), "Faces in some direction will NOT be combine.\nSee \"Setting\" > \"Optimization\".", MessageType.Info);
					Space(6);
				}

			}, "Selecting Files", ViewPanelOpen, true);

		}



		private static void CreateGUI () {
			LayoutF(() => {
				bool oldEnable = GUI.enabled;
				GUI.enabled = VoxNum > 0 || QbNum > 0;
				int buttonHeight = 34;
				string s = VoxNum + QbNum > 1 ? "s" : "";

				var dotStyle = new GUIStyle(GUI.skin.label) {
					richText = true,
					alignment = TextAnchor.MiddleLeft,
				};

				Space(6);

				Rect rect = new Rect();
				if (GUI.Button(rect = GUIRect(0, buttonHeight), "Create Prefab" + s)) {
					// Create Prefab
					DoTask(Task.Prefab);
				}
				GUI.Label(rect, GUI.enabled ? "   <color=#33ccff>●</color>" : "", dotStyle);

				Space(4);
				if (GUI.Button(rect = GUIRect(0, buttonHeight), "Create LOD Prefab" + s)) {
					// Create LOD Prefab
					DoTask(Task.Lod);
				}
				GUI.Label(rect, GUI.enabled ? "   <color=#33ccff>●</color>" : "", dotStyle);

				Space(4);
				if (GUI.Button(rect = GUIRect(0, buttonHeight), "Create Obj File" + s)) {
					// Create Obj File
					DoTask(Task.Obj);
				}
				GUI.Label(rect, GUI.enabled ? "   <color=#33ff66>●</color>" : "", dotStyle);

				Space(4);

				LayoutH(() => {

					GUI.enabled = VoxNum > 0 || QbNum > 0;
					if (GUI.Button(rect = GUIRect(0, buttonHeight), "   To Json")) {
						// To Json
						DoTask(Task.ToJson);
					}
					GUI.Label(rect, GUI.enabled ? "   <color=#cccccc>●</color>" : "", dotStyle);
					Space(2);

					GUI.enabled = JsonNum > 0 || QbNum > 0;
					if (GUI.Button(rect = GUIRect(0, buttonHeight), "  To Vox")) {
						// To Vox
						DoTask(Task.ToVox);
					}
					GUI.Label(rect, GUI.enabled ? "   <color=#cc66ff>●</color>" : "", dotStyle);
					Space(2);

					GUI.enabled = JsonNum > 0 || VoxNum > 0;
					if (GUI.Button(rect = GUIRect(0, buttonHeight), " To Qb")) {
						// To Qb
						DoTask(Task.ToQb);
					}
					GUI.Label(rect, GUI.enabled ? "   <color=#cc66ff>●</color>" : "", dotStyle);

				});

				Space(6);

				// Export To
				LayoutV(() => {
					GUI.enabled = true;

					Space(4);
					LayoutH(() => {
						GUI.Label(GUIRect(0, 18), "Export To:");
						TheExportMod = (ExportMod)EditorGUI.EnumPopup(GUIRect(110, 18), TheExportMod);
					});

					if (TheExportMod == ExportMod.Specified) {
						Space(4);
						LayoutH(() => {
							Space(6);
							EditorGUI.SelectableLabel(GUIRect(0, 18), ExportPath, GUI.skin.textField);
							if (GUI.Button(GUIRect(60, 18), "Browse", EditorStyles.miniButtonMid)) {
								BrowseExportPath();
							}
						});
						Space(2);
					}
					Space(2);

					GUI.enabled = oldEnable;
				}, true);

				Space(4);

				GUI.enabled = oldEnable;
			}, "Create", CreatePanelOpen, true);
		}



		private static void ToolGUI () {
			LayoutF(() => {


				var dotStyle = new GUIStyle(GUI.skin.label) {
					fontSize = 9,
					richText = true,
					alignment = TextAnchor.MiddleLeft,
				};


				// Rigging Editor
				LayoutH(() => {
					GUIRect(16, 26);
					Rect rect = new Rect();
					if (GUI.Button(rect = GUIRect(0, 28), " Rigging Editor")) {
						VoxelEditorWindow.OpenWindow(VoxelEditorWindow.EditorMode.Rigging, EditorDockToScene);
					}
					GUI.Label(rect, "   <color=#33ccff>●</color>", dotStyle);
					GUIRect(16, 26);
				});

				Space(4);

				// Sprite Editor
				LayoutH(() => {
					GUIRect(16, 26);
					Rect rect = new Rect();
					if (GUI.Button(rect = GUIRect(0, 28), " Sprite Editor")) {
						VoxelEditorWindow.OpenWindow(VoxelEditorWindow.EditorMode.Sprite, EditorDockToScene);
					}
					GUI.Label(rect, "   <color=#ffcc00>●</color>", dotStyle);
					GUIRect(16, 26);
				});

				Space(4);

				// Map Generator
				LayoutH(() => {
					GUIRect(16, 26);
					Rect rect = new Rect();
					if (GUI.Button(rect = GUIRect(0, 28), " Map Generator")) {
						var window = VoxelEditorWindow.OpenWindow(VoxelEditorWindow.EditorMode.MapGenerator, EditorDockToScene);
						if (window) {
							window.OpenGenerator();
						}
					}
					GUI.Label(rect, "   <color=#cc66ff>●</color>", dotStyle);
					GUIRect(16, 26);
				});

				Space(4);
				// Character Generator
				LayoutH(() => {
					GUIRect(16, 26);
					Rect rect = new Rect();
					if (GUI.Button(rect = GUIRect(0, 28), " Character Generator")) {
						var window = VoxelEditorWindow.OpenWindow(VoxelEditorWindow.EditorMode.CharacterGenerator, EditorDockToScene);
						if (window) {
							window.OpenGenerator();
						}
					}
					GUI.Label(rect, "   <color=#cc66ff>●</color>", dotStyle);
					GUIRect(16, 26);
				});

				Space(8);

			}, "Tools", ToolPanelOpen, true);
		}



		private static void SettingGUI () {
			LayoutF(() => {

				const int HEIGHT = 16;

				// Model Generation
				LayoutF(() => {

					Space(2);

					// Pivot
					LayoutH(() => {
						EditorGUI.LabelField(GUIRect(48, 18), "Pivot");
						ModelPivot.Value = EditorGUI.Vector3Field(GUIRect(0, 18), "", ModelPivot);
					});
					Space(2);

					// Scale
					ModelScale.Value = Mathf.Max(EditorGUI.FloatField(GUIRect(0, HEIGHT), "Scale (unit/voxel)", ModelScale), 0f);
					Space(4);

					// LOD
					LodNum.Value = Mathf.Clamp(EditorGUI.IntField(GUIRect(0, HEIGHT), "LOD Num", LodNum), 2, 4);
					Space(4);

					// LightMapSupportType
					LightMapSupportMode = (Core_Voxel.LightMapSupportType)EditorGUI.EnumPopup(GUIRect(0, HEIGHT), "Lightmap", LightMapSupportMode);
					Space(4);

					// Shader
					TheShader = (Shader)EditorGUI.ObjectField(GUIRect(0, 16), "Default Shader", TheShader, typeof(Shader), false);
					Space(4);

				}, "Model Generation", ModelGenerationSettingPanelOpen, true);


				// Optimization
				LayoutF(() => {
					LayoutH(() => {
						OptimizeLeft.Value = EditorGUI.ToggleLeft(GUIRect(0, HEIGHT), "Combine X-", OptimizeLeft);
						Space(2);
						OptimizeRight.Value = EditorGUI.ToggleLeft(GUIRect(0, HEIGHT), "Combine X+", OptimizeRight);
					});
					Space(2);
					LayoutH(() => {
						OptimizeDown.Value = EditorGUI.ToggleLeft(GUIRect(0, HEIGHT), "Combine Y-", OptimizeDown);
						Space(2);
						OptimizeUp.Value = EditorGUI.ToggleLeft(GUIRect(0, HEIGHT), "Combine Y+", OptimizeUp);
					});
					Space(2);
					LayoutH(() => {
						OptimizeBack.Value = EditorGUI.ToggleLeft(GUIRect(0, HEIGHT), "Combine Z-", OptimizeBack);
						Space(2);
						OptimizeFront.Value = EditorGUI.ToggleLeft(GUIRect(0, HEIGHT), "Combine Z+", OptimizeFront);
					});
					Space(6);
					EditorGUI.HelpBox(
						GUIRect(0, 32),
						"The check boxes above will make faces only combines on specified directions.",
						MessageType.Info
					);
					Space(6);
				}, "Optimization", OptimizationSettingPanelOpen, true);


				// System
				LayoutF(() => {
					Space(2);
					LayoutH(() => {
						LogMessage.Value = EditorGUI.Toggle(GUIRect(HEIGHT, HEIGHT), LogMessage);
						GUI.Label(GUIRect(0, 18), "Log To Console");
						Space(2);
						ShowDialog.Value = EditorGUI.Toggle(GUIRect(HEIGHT, HEIGHT), ShowDialog);
						GUI.Label(GUIRect(0, 18), "Dialog Window");
					});
					Space(2);
					LayoutH(() => {
						ColorfulTitle.Value = EditorGUI.Toggle(GUIRect(HEIGHT, HEIGHT), ColorfulTitle);
						GUI.Label(GUIRect(0, 18), "Colorful Title");
						EditorDockToScene.Value = EditorGUI.Toggle(GUIRect(HEIGHT, HEIGHT), EditorDockToScene);
						GUI.Label(GUIRect(0, 18), "Dock Editor To Scene");
					});
					Space(2);
				}, "System", SystemSettingPanelOpen, true);



			}, "Setting", SettingPanelOpen, true);
		}



		private static void AboutGUI () {
			LayoutF(() => {

				// Content
				LayoutV(() => {
					GUI.Label(GUIRect(0, 18), "MagicaVoxel to Unity II.");
					GUI.Label(GUIRect(0, 18), "Developed by 楠瓜Moenen.");
					Link(0, 18, "Give it ★★★★★.", @"http://u3d.as/tWS");
				}, true);
				Space(2);

				// Links
				LayoutV(() => {
					LayoutH(() => {
						GUI.Label(GUIRect(90, 18), "Twitter");
						Link(0, 18, "@_Moenen", @"https://twitter.com/_Moenen");
					});

					LayoutH(() => {
						GUI.Label(GUIRect(90, 18), "QQ");
						Link(0, 18, "1182032752", @"tencent://message/?Menu=yes&uin=1182032752&Service=300&sigT=45a1e5847943b64c6ff3990f8a9e644d2b31356cb0b4ac6b24663a3c8dd0f8aa12a595b1714f9d45");
					});

					LayoutH(() => {
						GUI.Label(GUIRect(90, 18), "Unity Store");
						Link(0, 18, "Moenen", @"https://assetstore.unity.com/publishers/15506");
					});

					LayoutH(() => {
						GUI.Label(GUIRect(90, 18), "Email");
						Link(0, 18, "moenenn@163.com", @"mailto:moenenn@163.com");
					});

					LayoutH(() => {
						GUI.Label(GUIRect(90, 18), "Google Photo");
						Link(0, 18, "Voxel Art", @"https://goo.gl/photos/cPpgGXN6PaHQsf6K8");
					});
				}, true);
				Space(2);

				// AD
				LayoutV(() => {
					GUI.Label(GUIRect(0, 18), "Free Assets:");
					Space(2);
					LayoutH(() => {
						Link(74, 18, "Santa Claus", @"http://u3d.as/u5V");
						Space(8);
						Link(60, 18, "Pixel Man", @"http://u3d.as/XvH");
						Space(8);
						Link(68, 18, "Hierponent", @"http://u3d.as/C3X");
					});

					Space(4);
					GUI.Label(GUIRect(0, 18), "Voxel Assets:");
					Space(2);
					LayoutH(() => {
						Link(68, 18, "Character", @"http://u3d.as/w5V");
						Space(8);
						Link(80, 18, "Environment", @"http://u3d.as/w5X");
						Space(8);
						Link(40, 18, "Props", @"http://u3d.as/w64");
					});

					LayoutH(() => {
						Link(68, 18, "Vegetation", @"http://u3d.as/wa0");
						Space(8);
						Link(46, 18, "Vehicle", @"http://u3d.as/wa1");
						Space(8);
						Link(40, 18, "Tanks", @"http://u3d.as/DTX");
					});

					LayoutH(() => {
						Link(68, 18, "Spaceships", @"http://u3d.as/E8d");
						Space(8);
						Link(46, 18, "Turrets", @"http://u3d.as/GbL");
						Space(8);
						Link(46, 18, "Robots", @"http://u3d.as/MKK");
					});

					LayoutH(() => {
						Link(46, 18, "Blocks", @"http://u3d.as/Nha");
						Space(8);
						Link(50, 18, "Particles", @"http://u3d.as/Mq0");
					});


					Space(4);
					GUI.Label(GUIRect(0, 18), "Pixel Assets:");
					LayoutH(() => {
						Link(68, 18, "Character", @"http://u3d.as/Tjd");
						Space(8);
						Link(80, 18, "Environment", @"http://u3d.as/Tjg");
						Space(8);
						Link(40, 18, "Props", @"http://u3d.as/Tjh");
					});

					LayoutH(() => {
						Link(68, 18, "Vegetation", @"http://u3d.as/Tjj");
						Space(8);
						Link(46, 18, "Vehicle", @"http://u3d.as/Tjo");
						Space(8);
						Link(50, 18, "Particles", @"http://u3d.as/1hbh");
					});

					LayoutH(() => {
						Link(68, 18, "Poker Card", @"http://u3d.as/1kWc");
					});


					Space(4);
					GUI.Label(GUIRect(0, 18), "Toolkit:");
					LayoutH(() => {
						Link(120, 18, "Fleck Map Generator", @"http://u3d.as/Nfa");
						Space(8);
						Link(80, 18, "CMD Gomoku", @"http://u3d.as/14yU");
					});

					LayoutH(() => {
						Link(100, 18, "Kaleidoscope UI", @"http://u3d.as/1a6E");
						Space(8);
						Link(110, 18, "Simple Map Editor", @"http://u3d.as/AAw");
					});

					LayoutH(() => {
						Link(80, 18, "uGUI Plus", @"http://u3d.as/Yje");
						Space(8);
						Link(130, 18, "2.5D Sprite Converter", @"http://u3d.as/GTd");
					});

					LayoutH(() => {
						Link(110, 18, "Cyan Level Editor", @"http://u3d.as/FuS");
						Space(8);
						Link(100, 18, "Simple 2D Shape", @"http://u3d.as/CEg");
					});

					LayoutH(() => {
						Link(120, 18, "Character Movement Pro", @"http://u3d.as/15TG");
						Space(8);
						Link(90, 18, "U File Browser", @"http://u3d.as/iyn");
					});


				}, true);

			}, "About", AboutPanelOpen, true);
		}



		#endregion




		#region --- TSK ---




		private static void DoTask (Task task) {

			if (TaskMap.Count == 0) { return; }

			if (TheExportMod == ExportMod.AskEverytime && !BrowseExportPath()) { return; }

			RefreshMergeSetting();

			string failMessage = "[Voxel] Failed to create model for {0} model{1}.";
			int successedNum = 0;
			int failedNum = 0;
			int taskCount = TaskMap.Count;
			var resultList = new List<Core_Voxel.Result>();
			Util.ProgressBar("Creating", "Starting task...", 0f);
			Util.StartWatch();
			ForAllSelection((pathData) => {

				try {
					string fileName = Util.GetNameWithoutExtension(pathData.Path);

					Util.ProgressBar("Creating", string.Format("[{1}/{2}] Creating {0}", fileName, successedNum + failedNum + 1, taskCount), (float)(successedNum + failedNum + 1) / (taskCount + 1));

					VoxelData voxelData = null;
					switch (task) {
						case Task.Prefab:
						case Task.Lod:
						case Task.Obj:
							// Model
							voxelData = VoxelFile.GetVoxelData(Util.FileToByte(pathData.Path), pathData.Extension == ".vox");
							if (pathData.Extension == ".vox" || pathData.Extension == ".qb") {
								var result = Core_Voxel.CreateLodModel(
									voxelData,
									ModelScale,
									task == Task.Lod ? LodNum : 1,
									LightMapSupportMode,
									ModelPivot
								);
								if (TheExportMod == ExportMod.OriginalPath) {
									result.ExportRoot = Util.GetParentPath(pathData.Path);
									result.ExportSubRoot = "";
								} else {
									result.ExportRoot = ExportPath;
									result.ExportSubRoot = pathData.Root;
								}
								result.FileName = fileName;
								result.Extension = task == Task.Obj ? ".obj" : ".prefab";
								result.IsRigged = false;
								result.WithAvatar = false;
								resultList.Add(result);
							}
							break;
						case Task.ToJson:
							if (pathData.Extension == ".vox" || pathData.Extension == ".qb") {
								// Voxel To Json
								voxelData = VoxelFile.GetVoxelData(Util.FileToByte(pathData.Path), pathData.Extension == ".vox");
								var json = Core_Voxel.VoxelToJson(voxelData);
								string path = TheExportMod == ExportMod.OriginalPath ?
									Util.ChangeExtension(pathData.Path, ".json") :
									Util.CombinePaths(ExportPath, pathData.Root, fileName + ".json");
								Util.CreateFolder(Util.GetParentPath(path));
								Util.Write(json, path);
							}
							break;
						case Task.ToVox:
						case Task.ToQb:
							// Json To Voxel
							string aimEx = task == Task.ToVox ? ".vox" : ".qb";
							if (pathData.Extension == ".json") {
								voxelData = Core_Voxel.JsonToVoxel(Util.Read(pathData.Path));
							} else if (pathData.Extension == ".vox" || pathData.Extension == ".qb") {
								if (aimEx != pathData.Extension) {
									voxelData = VoxelFile.GetVoxelData(Util.FileToByte(pathData.Path), pathData.Extension == ".vox");
								}
							}
							if (voxelData) {
								string aimPath = TheExportMod == ExportMod.OriginalPath ?
									Util.ChangeExtension(pathData.Path, aimEx) :
									Util.CombinePaths(ExportPath, pathData.Root, fileName + aimEx);
								Util.ByteToFile(
									VoxelFile.GetVoxelByte(voxelData, task == Task.ToVox),
									aimPath
								);
							}
							break;
					}
					successedNum++;
				} catch (System.Exception ex) {
					failMessage += "\n" + ex.Message;
					failedNum++;
				}

			});

			// File
			try {

				Core_File.CreateFileForResult(resultList, TheShader, ModelScale, ModelPivot);

				double taskTime = Util.StopWatchAndGetTime();

				// Log Messages
				if (successedNum > 0) {
					string msg = string.Format("[Voxel] {0} model{1} created in {2}sec.", successedNum, (successedNum > 1 ? "s" : ""), taskTime.ToString("0.00"));
					if (LogMessage) {
						Debug.Log(msg);
					}
					if (ShowDialog) {
						Util.Dialog("Success", msg, "OK");
					}
				}
				if (failedNum > 0) {
					string msg = string.Format(failMessage, failedNum.ToString(), (failedNum > 1 ? "s" : ""));
					if (LogMessage) {
						Debug.LogWarning(msg);
					}
					if (ShowDialog) {
						Util.Dialog("Warning", msg, "OK");
					}
				}
			} catch (System.Exception ex) {
				Debug.LogError(ex.Message);
			}

			Util.ClearProgressBar();

		}




		#endregion




		#region --- LGC ---



		private static void LoadSetting () {
			ViewPanelOpen.Load();
			CreatePanelOpen.Load();
			SettingPanelOpen.Load();
			AboutPanelOpen.Load();
			ModelGenerationSettingPanelOpen.Load();
			SpriteGenerationSettingPanelOpen.Load();
			SystemSettingPanelOpen.Load();
			ToolPanelOpen.Load();
			ColorfulTitle.Load();
			ExportPath.Load();
			ExportModIndex.Load();
			ModelScale.Load();
			LodNum.Load();
			ShaderPath.Load();
			LogMessage.Load();
			ShowDialog.Load();
			LightMapSupportTypeIndex.Load();
			OptimizationSettingPanelOpen.Load();
			OptimizeFront.Load();
			OptimizeBack.Load();
			OptimizeUp.Load();
			OptimizeDown.Load();
			OptimizeLeft.Load();
			OptimizeRight.Load();
			EditorDockToScene.Load();
			ModelPivot.Load();
		}



		private static void SaveSetting () {
			ViewPanelOpen.TrySave();
			CreatePanelOpen.TrySave();
			SettingPanelOpen.TrySave();
			AboutPanelOpen.TrySave();
			ModelGenerationSettingPanelOpen.TrySave();
			SpriteGenerationSettingPanelOpen.TrySave();
			SystemSettingPanelOpen.TrySave();
			ToolPanelOpen.TrySave();
			ColorfulTitle.TrySave();
			ExportPath.TrySave();
			ExportModIndex.TrySave();
			ModelScale.TrySave();
			LodNum.TrySave();
			ShaderPath.TrySave();
			LogMessage.TrySave();
			ShowDialog.TrySave();
			LightMapSupportTypeIndex.TrySave();
			OptimizationSettingPanelOpen.TrySave();
			OptimizeFront.TrySave();
			OptimizeBack.TrySave();
			OptimizeUp.TrySave();
			OptimizeDown.TrySave();
			OptimizeLeft.TrySave();
			OptimizeRight.TrySave();
			EditorDockToScene.TrySave();
			ModelPivot.TrySave();
		}



		private static void RefreshSelection () {

			VoxNum = 0;
			QbNum = 0;
			FolderNum = 0;
			JsonNum = 0;

			// Fix Selection
			var fixedSelection = new List<KeyValuePair<Object, string>>();
			for (int i = 0; i < Selection.objects.Length; i++) {
				fixedSelection.Add(new KeyValuePair<Object, string>(
					Selection.objects[i],
					AssetDatabase.GetAssetPath(Selection.objects[i]))
				);
			}

			for (int i = 0; i < fixedSelection.Count; i++) {
				if (!fixedSelection[i].Key) { continue; }
				var pathI = fixedSelection[i].Value;
				for (int j = 0; j < fixedSelection.Count; j++) {
					if (i == j || !fixedSelection[j].Key) { continue; }
					var pathJ = fixedSelection[j].Value;
					if (Util.IsChildPathCompair(pathJ, pathI)) {
						fixedSelection[j] = new KeyValuePair<Object, string>(null, null);
					}
				}
			}

			// Get Task Map
			TaskMap.Clear();
			for (int i = 0; i < fixedSelection.Count; i++) {
				if (!fixedSelection[i].Key) { continue; }
				var obj = fixedSelection[i].Key;
				var path = fixedSelection[i].Value;
				path = Util.FixPath(path);
				var ex = Util.GetExtension(path);
				if (AssetDatabase.IsValidFolder(path)) {
					FolderNum++;
					var files = Util.GetFilesIn(path, "*.vox", "*.qb", "*.json");
					for (int j = 0; j < files.Length; j++) {
						var filePath = Util.FixedRelativePath(files[j].FullName);
						var fileEx = Util.GetExtension(filePath);
						if (fileEx == ".vox" || fileEx == ".qb" || fileEx == ".json") {
							var fileObj = AssetDatabase.LoadAssetAtPath<Object>(filePath);
							if (fileObj && !TaskMap.ContainsKey(fileObj)) {
								TaskMap.Add(fileObj, new PathData() {
									Path = filePath,
									Extension = fileEx,
									Root = Util.FixPath(filePath.Substring(
										path.Length,
										filePath.Length - path.Length - Util.GetNameWithExtension(filePath).Length
									)),
								});

								if (fileEx == ".vox") {
									VoxNum++;
									FixVoxIcon(fileObj);
								} else if (fileEx == ".qb") {
									QbNum++;
									FixQbIcon(fileObj);
								} else if (fileEx == ".json") {
									JsonNum++;
									FixJsonIcon(fileObj);
								}
							}
						}
					}
				} else if (ex == ".vox" || ex == ".qb" || ex == ".json") {
					if (!TaskMap.ContainsKey(obj)) {
						TaskMap.Add(obj, new PathData() {
							Path = path,
							Extension = ex,
							Root = "",
						});
						if (ex == ".vox") {
							VoxNum++;
							FixVoxIcon(obj);
						} else if (ex == ".qb") {
							QbNum++;
							FixQbIcon(obj);
						} else if (ex == ".json") {
							JsonNum++;
							FixJsonIcon(obj);
						}
					}
				}
			}

			ObjNum = Selection.objects.Length;

		}



		private static void ForAllSelection (System.Action<PathData> action) {
			foreach (var key_Value in TaskMap) {
				action(key_Value.Value);
			}
		}



		private static void FixVoxIcon (Object vox) {
			if (!VoxFileIcon) {
				VoxFileIcon = AssetPreview.GetMiniThumbnail(vox);
			}
		}
		private static void FixQbIcon (Object qb) {
			if (!QbFileIcon) {
				QbFileIcon = AssetPreview.GetMiniThumbnail(qb);
			}
		}
		private static void FixJsonIcon (Object json) {
			if (!JsonFileIcon) {
				JsonFileIcon = AssetPreview.GetMiniThumbnail(json);
			}
		}



		private static bool BrowseExportPath () {
			string newPath = Util.FixPath(EditorUtility.OpenFolderPanel("Select Export Path", ExportPath, ""));
			if (!string.IsNullOrEmpty(newPath)) {
				newPath = Util.FixedRelativePath(newPath);
				if (!string.IsNullOrEmpty(newPath)) {
					ExportPath.Value = newPath;
					return true;
				} else {
					Util.Dialog("Warning", "Export path must in Assets folder.", "OK");
				}
			}
			return false;
		}





		#endregion




		#region --- UTL ---



		private static void RefreshMergeSetting () {
			Core_Voxel.SetMergeInDirection(OptimizeFront, OptimizeBack, OptimizeUp, OptimizeDown, OptimizeLeft, OptimizeRight);
		}



		#endregion



	}







	[CustomEditor(typeof(DefaultAsset)), CanEditMultipleObjects]
	public class VoxelInspector : Editor {


		private void OnEnable () {
			if (HasVoxelTarget()) {
				VoxelToUnityWindow.Window_Enable();
			}
		}


		public override void OnInspectorGUI () {
			base.OnInspectorGUI();
			if (HasVoxelTarget()) {
				bool oldE = GUI.enabled;
				GUI.enabled = true;
				VoxelToUnityWindow.Window_Main();
				GUI.enabled = oldE;
			}
		}


		private bool HasVoxelTarget () {
			for (int i = 0; i < targets.Length; i++) {
				string path = AssetDatabase.GetAssetPath(targets[i]);
				string ex = Util.GetExtension(path);
				if (ex == ".vox" || ex == ".qb" || Util.HasFileIn(path, "*.vox", "*.qb")) {
					return true;
				}
			}
			return false;
		}

	}


}