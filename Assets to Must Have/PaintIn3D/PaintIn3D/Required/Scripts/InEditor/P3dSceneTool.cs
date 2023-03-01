#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

namespace PaintIn3D
{
	[EditorTool("Paint in 3D")]
	public class P3dSceneTool : EditorTool
	{
		[SerializeField]
		private Texture2D lightIcon = null;

		[SerializeField]
		private Texture2D darkIcon = null;

		[System.NonSerialized]
		private static GUIContent lightContent;

		[System.NonSerialized]
		private static GUIContent darkContent;

		public static Camera LastCamera;

		public static Vector2 LastPosition;

		public static float LastPressure;

		public static bool LastSet;

		public static bool LastValid;

		private static int activeTime;

		public static event System.Action OnToolUpdate;

		public override GUIContent toolbarIcon
		{
			get
			{
				return EditorGUIUtility.isProSkin == true ? darkContent : lightContent;
			}
		}

		public static bool IsActive
		{
			get
			{
				return activeTime > 0;
			}
		}

		public override bool IsAvailable()
		{
			return true;
		}

		public static Ray GetRay(Vector2 screenPosition)
		{
			if (LastCamera != null)
			{
				return LastCamera.ScreenPointToRay(screenPosition);
			}

			return default(Ray);
		}

		public static void SelectThisTool()
		{
			Tools.current = Tool.Custom;

			EditorTools.SetActiveTool<P3dSceneTool>();
		}

		protected virtual void OnEnable()
		{
			SceneView.beforeSceneGui -= HandleBeforeSceneGUI;
			SceneView.beforeSceneGui += HandleBeforeSceneGUI;

			if (lightIcon != null)
			{
				lightContent = new GUIContent("Paint in 3D", lightIcon, "Paint Tool");
			}

			if (darkIcon != null)
			{
				darkContent  = new GUIContent("Paint in 3D", darkIcon , "Paint Tool");
			}
		}

		protected virtual void OnDisable()
		{
			activeTime = 0;
		}

		public override void OnToolGUI(EditorWindow window)
		{
			if (EditorWindow.HasOpenInstances<P3dWindow>() == false)
			{
				P3dWindow.OpenWindow();
			}

			var sceneView = window as SceneView;

			if (sceneView != null)
			{
				HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

				activeTime = 2;

				LastCamera   = sceneView.camera;
				LastPosition = HandleUtility.GUIPointToScreenPixelCoordinate(Event.current.mousePosition);
				LastPressure = Event.current.pressure;

				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					LastSet = true;
				}

				if (Event.current.type == EventType.MouseUp)
				{
					LastSet = false;
				}

				if (OnToolUpdate != null)
				{
					OnToolUpdate.Invoke();
				}

				LastValid = EditorWindow.mouseOverWindow != null && EditorWindow.mouseOverWindow is SceneView;
			}
		}

		private void HandleBeforeSceneGUI(SceneView sceneView)
		{
			if (sceneView == (object)SceneView.sceneViews[0])
			{
				activeTime -= 1;
			}
		}
	}
}
#endif