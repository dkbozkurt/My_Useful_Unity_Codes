using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component automatically destroys this GameObject after some time.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dDestroyAfterTime")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Destroy After Time")]
	public class P3dDestroyAfterTime : MonoBehaviour
	{
		/// <summary>If this component has been active for this many seconds, the current GameObject will be destroyed.
		/// -1 = DestroyNow must be manually called.</summary>
		public float Seconds { set { seconds = value; } get { return seconds; } } [SerializeField] private float seconds = 5.0f;

		[SerializeField]
		private float age;

		[ContextMenu("Destroy Now")]
		public void DestroyNow()
		{
			Destroy(gameObject);
		}

		protected virtual void Update()
		{
			if (seconds >= 0.0f)
			{
				age += Time.deltaTime;

				if (age >= seconds)
				{
					DestroyNow();
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dDestroyAfterTime;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dDestroyAfterTime_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("seconds", "If this component has been active for this many seconds, the current GameObject will be destroyed.\n-1 = DestroyNow must be manually called.");
		}
	}
}
#endif