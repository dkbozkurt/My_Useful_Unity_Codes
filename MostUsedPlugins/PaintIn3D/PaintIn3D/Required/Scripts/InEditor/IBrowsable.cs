using UnityEngine;

namespace PaintIn3D
{
	public interface IBrowsable
	{
		string GetCategory();

		string GetTitle();

		Texture2D GetIcon();

		Object GetObject();
	}
}