using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component enables or disables the specified ParticleSystem based on mouse or finger presses.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dToggleParticles")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Toggle Particles")]
	public class P3dToggleParticles : MonoBehaviour
	{
		/// <summary>Fingers that began touching the screen on top of these UI layers will be ignored.</summary>
		public LayerMask GuiLayers { set { guiLayers = value; } get { return guiLayers; } } [SerializeField] private LayerMask guiLayers = 1 << 5;

		/// <summary>The key that must be held for this component to activate.
		/// None = Any mouse button or finger.</summary>
		public KeyCode Key { set { key = value; } get { return key; } } [SerializeField] private KeyCode key = KeyCode.Mouse0;

		/// <summary>The particle system that will be enabled/disabled based on mouse/touch.</summary>
		public ParticleSystem Target { set { target = value; } get { return target; } } [SerializeField] private ParticleSystem target;

		/// <summary>Should painting triggered from this component be eligible for being undone?</summary>
		public bool StoreStates { set { storeStates = value; } get { return storeStates; } } [SerializeField] protected bool storeStates = true;

		protected virtual void LateUpdate()
		{
			if (target != null)
			{
				if (key >= KeyCode.Mouse0 || key <= KeyCode.Mouse6)
				{
					
				}

				if (CwInput.GetKeyIsHeld(key) == true)
				{
					if (storeStates == true && target.isPlaying == false)
					{
						P3dStateManager.StoreAllStates();
					}

					target.Play();
				}
				else
				{
					target.Stop();
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dToggleParticles;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dToggleParticles_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("key", "The key that must be held for this component to activate.\n\nNone = Any mouse button or finger.");
			BeginError(Any(tgts, t => t.Target == null));
				Draw("target", "The particle system that will be enabled/disabled based on mouse/touch.");
			EndError();
			Draw("storeStates", "Should painting triggered from this component be eligible for being undone?");
		}
	}
}
#endif