using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to move the current <b>Transform</b> using editor events (e.g. UI buttons).</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dTranslate")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Translate")]
	public class P3dTranslate : MonoBehaviour
	{
		/// <summary>This allows you to set the coordinate space the movement will use.</summary>
		public Space Space { set { space = value; } get { return space; } } [SerializeField] private Space space = Space.Self;

		/// <summary>The movement values will be multiplied by this before use.</summary>
		public float Multiplier { set { multiplier = value; } get { return multiplier; } } [SerializeField] private float multiplier = 1.0f;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		public float Damping { set { damping = value; } get { return damping; } } [SerializeField] private float damping = 10.0f;

		/// <summary>The position will be incremented by this each second.</summary>
		public Vector3 PerSecond { set { perSecond = value; } get { return perSecond; } } [SerializeField] private Vector3 perSecond;

		[SerializeField]
		private Vector3 remainingDelta;

		/// <summary>This method allows you to translate along the X axis, with the specified value.</summary>
		public void TranslateX(float magnitude)
		{
			Translate(Vector3.right * magnitude);
		}

		/// <summary>This method allows you to translate along the Y axis, with the specified value.</summary>
		public void TranslateY(float magnitude)
		{
			Translate(Vector3.up * magnitude);
		}

		/// <summary>This method allows you to translate along the Z axis, with the specified value.</summary>
		public void TranslateZ(float magnitude)
		{
			Translate(Vector3.forward * magnitude);
		}

		/// <summary>This method allows you to translate along the specified vector.</summary>
		public void Translate(Vector3 vector)
		{
			if (Space == Space.Self)
			{
				vector = transform.TransformVector(vector);
			}

			TranslateWorld(vector);
		}

		/// <summary>This method allows you to translate along the specified vector in world space.</summary>
		public void TranslateWorld(Vector3 vector)
		{
			remainingDelta += vector * Multiplier;
		}

		protected virtual void Update()
		{
			var factor   = CwHelper.DampenFactor(Damping, Time.deltaTime);
			var newDelta = Vector3.Lerp(remainingDelta, Vector3.zero, factor);

			transform.position += remainingDelta - newDelta;

			transform.Translate(perSecond * Time.deltaTime, space);

			remainingDelta = newDelta;
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dTranslate;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dTranslate_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("space", "This allows you to set the coordinate space the movement will use.");
			Draw("multiplier", "The movement values will be multiplied by this before use.");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
			Draw("perSecond", "The position will be incremented by this each second.");
		}
	}
}
#endif