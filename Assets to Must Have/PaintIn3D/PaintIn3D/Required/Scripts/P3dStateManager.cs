namespace PaintIn3D
{
	/// <summary>This component allows you to manage undo/redo states on all P3dPaintableTextures in your scene.</summary>
	public static class P3dStateManager
	{
		public static bool CanUndo
		{
			get
			{
				foreach (var paintableTexture in P3dPaintableTexture.Instances)
				{
					if (paintableTexture.CanUndo == true)
					{
						return true;
					}
				}

				return false;
			}
		}

		public static bool CanRedo
		{
			get
			{
				foreach (var paintableTexture in P3dPaintableTexture.Instances)
				{
					if (paintableTexture.CanRedo == true)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>This method will call StoreState on all active and enabled P3dPaintableTextures.</summary>
		public static void StoreAllStates()
		{
			foreach (var paintableTexture in P3dPaintableTexture.Instances)
			{
				paintableTexture.StoreState();
			}
		}

		/// <summary>This method will call StoreState on all active and enabled P3dPaintableTextures.</summary>
		public static void ClearAllStates()
		{
			foreach (var paintableTexture in P3dPaintableTexture.Instances)
			{
				paintableTexture.ClearStates();
			}
		}

		/// <summary>This method will call Undo on all active and enabled P3dPaintableTextures.</summary>
		public static void UndoAll()
		{
			foreach (var paintableTexture in P3dPaintableTexture.Instances)
			{
				paintableTexture.Undo();
			}
		}

		/// <summary>This method will call Redo on all active and enabled P3dPaintableTextures.</summary>
		public static void RedoAll()
		{
			foreach (var paintableTexture in P3dPaintableTexture.Instances)
			{
				paintableTexture.Redo();
			}
		}
	}
}