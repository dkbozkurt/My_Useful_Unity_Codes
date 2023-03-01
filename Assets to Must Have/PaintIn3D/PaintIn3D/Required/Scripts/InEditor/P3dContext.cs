#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace PaintIn3D
{
	public class P3dContext
	{
		[MenuItem("CONTEXT/MeshRenderer/Make Paintable (Paint in 3D)", true)]
		private static bool MeshRendererMakePaintableValidate(MenuCommand menuCommand)
		{
			var gameObject = GetGameObject(menuCommand); return gameObject.GetComponent<P3dPaintable>() == null;
		}

		[MenuItem("CONTEXT/MeshRenderer/Make Paintable (Paint in 3D)", false)]
		private static void MeshRendererMakePaintable(MenuCommand menuCommand)
		{
			var gameObject = GetGameObject(menuCommand); AddSingleComponent<P3dPaintable>(gameObject);
		}

		[MenuItem("CONTEXT/SkinnedMeshRenderer/Make Paintable (Paint in 3D)", true)]
		private static bool SkinnedMeshRendererMakePaintableValidate(MenuCommand menuCommand)
		{
			var gameObject = GetGameObject(menuCommand); return gameObject.GetComponent<P3dPaintable>() == null;
		}

		[MenuItem("CONTEXT/SkinnedMeshRenderer/Make Paintable (Paint in 3D)", false)]
		private static void SkinnedMeshRendererMakePaintable(MenuCommand menuCommand)
		{
			var gameObject = GetGameObject(menuCommand); AddSingleComponent<P3dPaintable>(gameObject);
		}

		private static void AddSingleComponent<T>(GameObject gameObject, System.Action<T> action = null)
			where T : Component
		{
			if (gameObject != null)
			{
				if (gameObject.GetComponent<T>() == null)
				{
					var component = Undo.AddComponent<T>(gameObject);

					if (action != null)
					{
						action(component);
					}
				}
			}
		}

		private static GameObject GetGameObject(MenuCommand menuCommand)
		{
			if (menuCommand != null)
			{
				var component = menuCommand.context as Component;

				if (component != null)
				{
					return component.gameObject;
				}
			}

			return null;
		}
	}
}
#endif