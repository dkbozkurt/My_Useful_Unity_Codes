using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component grabs paint hits and connected hits, mirrors the data, then re-broadcasts it.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dCloneMirror")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Clone Mirror")]
	public class P3dCloneMirror : P3dClone
	{
		/// <summary>When a decal is mirrored it will appear backwards, should it be flipped back around?</summary>
		public bool Flip { set { flip = value; } get { return flip; } } [SerializeField] private bool flip;

		public override void Transform(ref Matrix4x4 posMatrix, ref Matrix4x4 rotMatrix)
		{
			var p  = transform.position;
			var r  = transform.rotation;
			var s  = Matrix4x4.Scale(new Vector3(1.0f, 1.0f, -1.0f));
			var tp = Matrix4x4.Translate(p);
			var rp = Matrix4x4.Rotate(r);
			var ti = Matrix4x4.Translate(-p);
			var ri = Matrix4x4.Rotate(Quaternion.Inverse(r));

			if (flip == true)
			{
				posMatrix = tp * rp * s * ri * ti * posMatrix;
			}
			else
			{
				posMatrix = tp * rp * s * ri * ti * posMatrix;
				rotMatrix = rp * s * ri * rotMatrix;
			}
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;

			for (var i = 1; i <= 10; i++)
			{
				Gizmos.DrawWireCube(Vector3.zero, new Vector3(i, i, 0.0f));
			}
		}
#endif
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dCloneMirror;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dCloneMirror_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("flip", "When a decal is mirrored it will appear backwards, should it be flipped back around?");
		}
	}
}
#endif