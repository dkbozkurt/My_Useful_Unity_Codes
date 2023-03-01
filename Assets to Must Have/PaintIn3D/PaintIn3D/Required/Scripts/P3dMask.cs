using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to block paint from being applied at the current position using the specified shape.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dMask")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Mask")]
	public class P3dMask : MonoBehaviour
	{
		/// <summary>The mask will use this texture shape.</summary>
		public Texture Texture { set { texture = value; } get { return texture; } } [SerializeField] private Texture texture;

		/// <summary>The mask will use pixels from this texture channel.</summary>
		public P3dChannel Channel { set { channel = value; } get { return channel; } } [SerializeField] private P3dChannel channel = P3dChannel.Alpha;

		/// <summary>If you want the sides of the mask to extend farther out, then this allows you to set the scale of the boundary.
		/// 1 = Default.
		/// 2 = Double size.</summary>
		public Vector2 Stretch { set { stretch = value; } get { return stretch; } } [SerializeField] private Vector2 stretch = Vector2.one;

		/// <summary>This stores all active and enabled instances in the open scenes.</summary>
		public static LinkedList<P3dMask> Instances { get { return instances; } } private static LinkedList<P3dMask> instances = new LinkedList<P3dMask>(); private LinkedListNode<P3dMask> instancesNode;

		public Matrix4x4 Matrix
		{
			get
			{
				return transform.worldToLocalMatrix;
			}
		}

		public static P3dMask Find(Vector3 position, LayerMask layers)
		{
			var bestMask     = default(P3dMask);
			var bestDistance = float.PositiveInfinity;

			foreach (var instance in instances)
			{
				if (CwHelper.IndexInMask(instance.gameObject.layer, layers) == true)
				{
					var distance = Vector3.SqrMagnitude(position - instance.transform.position);

					if (distance < bestDistance)
					{
						bestDistance = distance;
						bestMask     = instance;
					}
				}
			}

			return bestMask;
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode); instancesNode = null;
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;

			Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.0f, 1.0f, 0.0f));
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.0f, 1.0f, 1.0f));
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(stretch.x, stretch.y, 0.0f));
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(stretch.x, stretch.y, 1.0f));
		}
#endif
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dMask;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dMask_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginError(Any(tgts, t => t.Texture == null));
				Draw("texture", "The mask will use this texture shape.");
			EndError();
			Draw("channel", "The mask will use pixels from this texture channel.");
			Draw("stretch", "If you want the sides of the mask to extend farther out, then this allows you to set the scale of the boundary.\n\n1 = Default.\n\n2 = Double size.");
		}
	}
}
#endif