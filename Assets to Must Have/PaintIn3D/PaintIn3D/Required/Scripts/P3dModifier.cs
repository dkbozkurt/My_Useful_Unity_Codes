using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This is the base class for all paint modifiers. To make a paint modifier, simply inherit this class, and implement one of the virtual methods to modify its data.</summary>
	[System.Serializable]
	public abstract class P3dModifier
	{
		/// <summary>Should this modifier apply to preview paint as well?</summary>
		public bool Preview { set { preview = value; } get { return preview; } } [SerializeField] private bool preview = true;

		/// <summary>Should this modifier use a unique seed?</summary>
		public bool Unique { set { unique = value; } get { return unique; } } [SerializeField] private bool unique = true;

		public void ModifyAngle(ref float angle, float pressure)
		{
			if (unique == true)
			{
				CwHelper.BeginSeed();
					OnModifyAngle(ref angle, pressure);
				CwHelper.EndSeed();
			}
			else
			{
				OnModifyAngle(ref angle, pressure);
			}
		}

		protected virtual void OnModifyAngle(ref float angle, float pressure)
		{
		}

		public void ModifyColor(ref Color color, float pressure)
		{
			if (unique == true)
			{
				CwHelper.BeginSeed();
					OnModifyColor(ref color, pressure);
				CwHelper.EndSeed();
			}
			else
			{
				OnModifyColor(ref color, pressure);
			}
		}

		protected virtual void OnModifyColor(ref Color color, float pressure)
		{
		}

		public void ModifyHardness(ref float hardness, float pressure)
		{
			if (unique == true)
			{
				CwHelper.BeginSeed();
					OnModifyHardness(ref hardness, pressure);
				CwHelper.EndSeed();
			}
			else
			{
				OnModifyHardness(ref hardness, pressure);
			}
		}

		protected virtual void OnModifyHardness(ref float hardness, float pressure)
		{
		}

		public void ModifyOpacity(ref float opacity, float pressure)
		{
			if (unique == true)
			{
				CwHelper.BeginSeed();
					OnModifyOpacity(ref opacity, pressure);
				CwHelper.EndSeed();
			}
			else
			{
				OnModifyOpacity(ref opacity, pressure);
			}
		}

		protected virtual void OnModifyOpacity(ref float opacity, float pressure)
		{
		}

		public void ModifyRadius(ref float radius, float pressure)
		{
			if (unique == true)
			{
				CwHelper.BeginSeed();
					OnModifyRadius(ref radius, pressure);
				CwHelper.EndSeed();
			}
			else
			{
				OnModifyRadius(ref radius, pressure);
			}
		}

		protected virtual void OnModifyRadius(ref float radius, float pressure)
		{
		}

		public void ModifyScale(ref Vector3 scale, float pressure)
		{
			if (unique == true)
			{
				CwHelper.BeginSeed();
					OnModifyScale(ref scale, pressure);
				CwHelper.EndSeed();
			}
			else
			{
				OnModifyScale(ref scale, pressure);
			}
		}

		protected virtual void OnModifyScale(ref Vector3 scale, float pressure)
		{
		}

		public void ModifyTexture(ref Texture texture, float pressure)
		{
			if (unique == true)
			{
				CwHelper.BeginSeed();
					OnModifyTexture(ref texture, pressure);
				CwHelper.EndSeed();
			}
			else
			{
				OnModifyTexture(ref texture, pressure);
			}
		}

		protected virtual void OnModifyTexture(ref Texture texture, float pressure)
		{
		}

		public void ModifyPosition(ref Vector3 position, float pressure)
		{
			if (unique == true)
			{
				CwHelper.BeginSeed();
					OnModifyPosition(ref position, pressure);
				CwHelper.EndSeed();
			}
			else
			{
				OnModifyPosition(ref position, pressure);
			}
		}

		protected virtual void OnModifyPosition(ref Vector3 position, float pressure)
		{
		}
#if UNITY_EDITOR
		public abstract void DrawEditorLayout();
#endif
	}
}