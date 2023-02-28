using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	public static class P3dBlit
	{
		private static Material cachedWhite;

		private static bool cachedWhiteSet;

		private static Material cachedTexture;

		private static bool cachedTextureSet;

		private static Material cachedNormal;

		private static bool cachedNormalSet;

		private static Material cachedPremultiply;

		private static bool cachedPremultiplySet;

		private static int _Buffer     = Shader.PropertyToID("_Buffer");
		private static int _BufferSize = Shader.PropertyToID("_BufferSize");
		private static int _Texture    = Shader.PropertyToID("_Texture");
		private static int _Color      = Shader.PropertyToID("_Color");

		public static void White(RenderTexture rendertexture, Mesh mesh, int submesh, P3dCoord coord)
		{
			CwHelper.BeginActive(rendertexture);

			if (mesh != null)
			{
				if (cachedWhiteSet == false)
				{
					cachedWhite    = P3dCommon.BuildMaterial("Hidden/Paint in 3D/White");
					cachedWhiteSet = true;
				}

				GL.Clear(true, true, Color.black);

				P3dCommon.Draw(cachedWhite, 0, mesh, Matrix4x4.identity, submesh, coord);
			}
			else
			{
				GL.Clear(true, true, Color.white);
			}

			CwHelper.EndActive();
		}

		public static void Texture(RenderTexture rendertexture, Mesh mesh, int submesh, Texture texture, P3dCoord coord)
		{
			if (cachedTextureSet == false)
			{
				cachedTexture    = P3dCommon.BuildMaterial("Hidden/Paint in 3D/Texture");
				cachedTextureSet = true;
			}

			CwHelper.BeginActive(rendertexture);

			cachedTexture.SetTexture(_Buffer, texture);
			cachedTexture.SetVector(_BufferSize, new Vector2(texture.width, texture.height));

			P3dCommon.Draw(cachedTexture, 0, mesh, Matrix4x4.identity, submesh, coord);

			CwHelper.EndActive();
		}

		public static void Normal(RenderTexture rendertexture, Texture texture)
		{
			if (cachedNormalSet == false)
			{
				cachedNormal    = P3dCommon.BuildMaterial("Hidden/Paint in 3D/Normal");
				cachedNormalSet = true;
			}

			cachedNormal.SetTexture(_Texture, texture);

			P3dCommon.Blit(rendertexture, cachedNormal, 0);
		}

		public static void Premultiply(RenderTexture rendertexture, Texture texture, Color tint)
		{
			if (cachedPremultiplySet == false)
			{
				cachedPremultiply    = P3dCommon.BuildMaterial("Hidden/Paint in 3D/Premultiply");
				cachedPremultiplySet = true;
			}

			cachedPremultiply.SetTexture(_Texture, texture);
			cachedPremultiply.SetColor(_Color, CwHelper.ToLinear(tint));

			P3dCommon.Blit(rendertexture, cachedPremultiply, 0);
		}
	}
}