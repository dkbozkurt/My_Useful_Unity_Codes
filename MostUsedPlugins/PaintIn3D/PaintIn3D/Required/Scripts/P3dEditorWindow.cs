#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This is the base class for all Paint in 3D editor windows.</summary>
	public class CwEditorWindow : EditorWindow
	{
		[SerializeField]
		private Vector2 mousePosition;

		[SerializeField]
		protected Vector2 scrollPosition;

		protected virtual void OnEnable()
		{
#if UNITY_2019_1_OR_NEWER
			SceneView.duringSceneGui += OnSceneGUI;
#else
			SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
		}

		protected virtual void OnDisable()
		{
#if UNITY_2019_1_OR_NEWER
			SceneView.duringSceneGui -= OnSceneGUI;
#else
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
		}

		protected virtual void OnGUI()
		{
			CwEditor.ClearStacks();

			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			{
				EditorGUI.BeginChangeCheck();
				{
					OnInspector();
				}
				if (EditorGUI.EndChangeCheck() == true)
				{
					Repaint();
				}
			}
			GUILayout.EndScrollView();
		}

		protected virtual void OnSceneGUI(SceneView sceneView)
		{
			var camera = sceneView.camera;

			mousePosition = Event.current.mousePosition;

			if (camera != null)
			{
				Handles.BeginGUI();
				{
					OnScene(sceneView, camera, mousePosition);
				}
				Handles.EndGUI();

				//sceneView.Repaint();
			}
		}

		protected virtual void OnSelectionChange()
		{
			Repaint();
		}

		protected virtual void OnInspector()
		{
		}

		protected virtual void OnScene(SceneView sceneView, Camera camera, Vector2 mousePosition)
		{
		}

		private static bool fog;
		//private static float oldAmbientIntensity;

		protected void BeginPreview(PreviewRenderUtility util, Rect rect)
		{
			util.BeginPreview(rect, GUIStyle.none);

			//oldAmbientIntensity = RenderSettings.ambientIntensity;

			fog = RenderSettings.fog;

			Unsupported.SetRenderSettingsUseFogNoDirty(false);

			UnityEditorInternal.InternalEditorUtility.SetCustomLighting(util.lights, util.ambientColor);
		}

		protected Texture EndPreview(PreviewRenderUtility util)
		{
			util.Render(true);

			UnityEditorInternal.InternalEditorUtility.RemoveCustomLighting();

			Unsupported.SetRenderSettingsUseFogNoDirty(fog);

			var texture = util.EndPreview();

			return texture;
		}

		protected void EndPreview(PreviewRenderUtility util, Rect rect)
		{
			GUI.DrawTexture(rect, EndPreview(util));
		}
	}
}
#endif