using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to manually associate a <b>Texture</b> with a hash code so it can be de/serialized.</summary>
	[DefaultExecutionOrder(-200)]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dTextureHash")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Texture Hash")]
	public class P3dTextureHash : MonoBehaviour
	{
		/// <summary>The texture that will be hashed.</summary>
		public Texture Texture { set { texture = value; } get { return texture; } } [SerializeField] private Texture texture;

		/// <summary>The hash code for the texture.</summary>
		public P3dHash Hash { set { hash = value; P3dSerialization.TryRegister(texture, hash); } get { return hash; } } [SerializeField] private P3dHash hash;

		protected virtual void OnEnable()
		{
			P3dSerialization.TryRegister(texture, hash);
		}

		protected virtual void OnDestroy()
		{
			P3dSerialization.TryRegister(texture, default(P3dHash));
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dTextureHash;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dTextureHash_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("texture", "The texture that will be hashed.");
			Draw("hash", "The hash code for the texture.");
		}
	}
}
#endif