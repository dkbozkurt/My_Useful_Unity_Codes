using UnityEngine;
using System.Collections.Generic;

namespace PaintIn3D
{
	/// <summary>This class stores information about a particular paintable texture state. Either a full texture copy, or a list of commands used to draw it.</summary>
	public class P3dPaintableState
	{
		public RenderTexture Texture;

		public List<P3dCommand> Commands = new List<P3dCommand>();

		private static Stack<P3dPaintableState> pool = new Stack<P3dPaintableState>();

		public static P3dPaintableState Pop()
		{
			return pool.Count > 0 ? pool.Pop() : new P3dPaintableState();
		}

		public void Write(RenderTexture current)
		{
			Clear();

			Texture = P3dCommon.GetRenderTexture(current.descriptor, current);

			P3dCommon.Blit(Texture, current);
		}

		public void Write(List<P3dCommand> commands)
		{
			Clear();

			Commands.AddRange(commands);
		}

		public void Write(RenderTexture current, List<P3dCommand> commands)
		{
			Clear();

			Texture = P3dCommon.GetRenderTexture(current.descriptor, current);

			P3dCommon.Blit(Texture, current);

			Commands.AddRange(commands);
		}

		private void Clear()
		{
			if (Texture != null)
			{
				P3dCommon.ReleaseRenderTexture(Texture);

				Texture = null;
			}

			for (var i = Commands.Count - 1; i >= 0; i--)
			{
				Commands[i].Pool();
			}

			Commands.Clear();
		}

		public void Pool()
		{
			Clear();

			pool.Push(this);
		}
	}
}