using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component automatically updates all P3dModel and P3dPaintableTexture instances at the end of the frame, batching all paint operations together.</summary>
	[DefaultExecutionOrder(100)]
	[DisallowMultipleComponent]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dPaintableManager")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Paintable Manager")]
	public class P3dPaintableManager : MonoBehaviour
	{
		/// <summary>This stores all active and enabled instances in the open scenes.</summary>
		public static LinkedList<P3dPaintableManager> Instances { get { return instances; } } private static LinkedList<P3dPaintableManager> instances = new LinkedList<P3dPaintableManager>(); private LinkedListNode<P3dPaintableManager> instancesNode;

		public static P3dPaintableManager GetOrCreateInstance()
		{
			if (instances.Count == 0)
			{
				var paintableManager = new GameObject(typeof(P3dPaintableManager).Name);

				//paintableManager.hideFlags = HideFlags.DontSave;

				paintableManager.AddComponent<P3dPaintableManager>();
			}

			return instances.First.Value;
		}

		public static void SubmitAll(P3dCommand command, Vector3 position, float radius, int layerMask, P3dGroup group, P3dModel targetModel, P3dPaintableTexture targetTexture)
		{
			DoSubmitAll(command, position, radius, layerMask, group, targetModel, targetTexture);

			// Repeat paint?
			P3dClone.BuildCloners();

			for (var c = 0; c < P3dClone.ClonerCount; c++)
			{
				for (var m = 0; m < P3dClone.MatrixCount; m++)
				{
					var copy = command.SpawnCopy();

					P3dClone.Clone(copy, c, m);

					DoSubmitAll(copy, position, radius, layerMask, group, targetModel, targetTexture);

					copy.Pool();
				}
			}
		}

		private static void DoSubmitAll(P3dCommand command, Vector3 position, float radius, int layerMask, P3dGroup group, P3dModel targetModel, P3dPaintableTexture targetTexture)
		{
			if (targetModel != null)
			{
				if (targetTexture != null)
				{
					Submit(command, targetModel, targetTexture);
				}
				else
				{
					SubmitAll(command, targetModel, group);
				}
			}
			else
			{
				if (targetTexture != null)
				{
					Submit(command, targetTexture.Paintable, targetTexture);
				}
				else
				{
					SubmitAll(command, position, radius, layerMask, group);
				}
			}
		}

		private static void SubmitAll(P3dCommand command, Vector3 position, float radius, int layerMask, P3dGroup group)
		{
			var models = P3dModel.FindOverlap(position, radius, layerMask);

			for (var i = models.Count - 1; i >= 0; i--)
			{
				SubmitAll(command, models[i], group);
			}
		}

		private static void SubmitAll(P3dCommand command, P3dModel model, P3dGroup group)
		{
			var paintableTextures = P3dPaintableTexture.FilterAll(model, group);

			for (var i = paintableTextures.Count - 1; i >= 0; i--)
			{
				Submit(command, model, paintableTextures[i]);
			}
		}

		public static P3dCommand Submit(P3dCommand command, P3dModel model, P3dPaintableTexture paintableTexture)
		{
			var copy = command.SpawnCopy();

			copy.Apply(paintableTexture);

			copy.Model   = model;
			copy.Submesh = model.GetSubmesh(paintableTexture);

			paintableTexture.AddCommand(copy);

			return copy;
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode); instancesNode = null;
		}

		protected virtual void LateUpdate()
		{
			if (this == instances.First.Value && P3dModel.Instances.Count > 0)
			{
				ClearAll();
				UpdateAll();
			}
			else
			{
				CwHelper.Destroy(gameObject);
			}
		}

		private void ClearAll()
		{
			foreach (var model in P3dModel.Instances)
			{
				model.Prepared = false;
			}
		}

		private void UpdateAll()
		{
			foreach (var paintableTexture in P3dPaintableTexture.Instances)
			{
				paintableTexture.ExecuteCommands(true, true);
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dPaintableManager;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dPaintableManager_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Info("This component automatically updates all P3dModel and P3dPaintableTexture instances at the end of the frame, batching all paint operations together.");
		}
	}
}
#endif