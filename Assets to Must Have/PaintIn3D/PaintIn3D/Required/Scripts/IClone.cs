using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This interface allows you to define classes that can clone paint points (e.g. mirror).</summary>
	public interface IClone
	{
		void Transform(ref Matrix4x4 posMatrix, ref Matrix4x4 rotMatrix);
	}
}