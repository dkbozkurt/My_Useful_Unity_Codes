using System.Collections.Generic;
using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This class manages the decal painting command.</summary>
	public class P3dCommandDecal : P3dCommand
	{
		public P3dBlendMode     Blend;
		public bool             In3D;
		public Vector3          Position;
		public Vector3          EndPosition;
		public Vector3          Position2;
		public Vector3          EndPosition2;
		public int              Extrusions;
		public bool             Clip;
		public Matrix4x4        Matrix;
		public Vector3          Direction;
		public Color            Color;
		public float            Opacity;
		public float            Hardness;
		public float            Wrapping;
		public P3dHashedTexture Texture;
		public P3dHashedTexture Shape;
		public Vector4          ShapeChannel;
		public Vector2          NormalFront;
		public Vector2          NormalBack;
		public P3dHashedTexture TileTexture;
		public Matrix4x4        TileMatrix;
		public float            TileOpacity;
		public float            TileTransition;
		public Matrix4x4        MaskMatrix;
		public P3dHashedTexture MaskShape;
		public Vector4          MaskChannel;
		public Vector3          MaskStretch;

		public static P3dCommandDecal Instance = new P3dCommandDecal();

		private static Stack<P3dCommandDecal> pool = new Stack<P3dCommandDecal>();

		private static Material cachedSpotMaterial;
		private static Material cachedLineMaterial;
		private static Material cachedQuadMaterial;
		private static Material cachedLineClipMaterial;
		private static Material cachedQuadClipMaterial;

		private static int cachedSpotMaterialHash;
		private static int cachedLineMaterialHash;
		private static int cachedQuadMaterialHash;
		private static int cachedLineClipMaterialHash;
		private static int cachedQuadClipMaterialHash;

		public override bool RequireMesh { get { return true; } }

		private static int _In3D           = Shader.PropertyToID("_In3D");
		private static int _Position       = Shader.PropertyToID("_Position");
		private static int _EndPosition    = Shader.PropertyToID("_EndPosition");
		private static int _Position2      = Shader.PropertyToID("_Position2");
		private static int _EndPosition2   = Shader.PropertyToID("_EndPosition2");
		private static int _Matrix         = Shader.PropertyToID("_Matrix");
		private static int _Direction      = Shader.PropertyToID("_Direction");
		private static int _Color          = Shader.PropertyToID("_Color");
		private static int _Opacity        = Shader.PropertyToID("_Opacity");
		private static int _Hardness       = Shader.PropertyToID("_Hardness");
		private static int _Wrapping       = Shader.PropertyToID("_Wrapping");
		private static int _Texture        = Shader.PropertyToID("_Texture");
		private static int _Shape          = Shader.PropertyToID("_Shape");
		private static int _ShapeChannel   = Shader.PropertyToID("_ShapeChannel");
		private static int _NormalFront    = Shader.PropertyToID("_NormalFront");
		private static int _NormalBack     = Shader.PropertyToID("_NormalBack");
		private static int _TileTexture    = Shader.PropertyToID("_TileTexture");
		private static int _TileMatrix     = Shader.PropertyToID("_TileMatrix");
		private static int _TileOpacity    = Shader.PropertyToID("_TileOpacity");
		private static int _TileTransition = Shader.PropertyToID("_TileTransition");
		private static int _MaskMatrix     = Shader.PropertyToID("_MaskMatrix");
		private static int _MaskTexture    = Shader.PropertyToID("_MaskTexture");
		private static int _MaskChannel    = Shader.PropertyToID("_MaskChannel");
		private static int _MaskStretch    = Shader.PropertyToID("_MaskStretch");

		static P3dCommandDecal()
		{
			BuildMaterial(ref cachedSpotMaterial, ref cachedSpotMaterialHash, "Hidden/Paint in 3D/Decal");
			BuildMaterial(ref cachedLineMaterial, ref cachedLineMaterialHash, "Hidden/Paint in 3D/Decal", "P3D_LINE");
			BuildMaterial(ref cachedQuadMaterial, ref cachedQuadMaterialHash, "Hidden/Paint in 3D/Decal", "P3D_QUAD");
			BuildMaterial(ref cachedLineClipMaterial, ref cachedLineClipMaterialHash, "Hidden/Paint in 3D/Decal", "P3D_LINE_CLIP");
			BuildMaterial(ref cachedQuadClipMaterial, ref cachedQuadClipMaterialHash, "Hidden/Paint in 3D/Decal", "P3D_QUAD_CLIP");
		}

		public override void Apply(Material material)
		{
			base.Apply(material);

			Blend.Apply(material);

			var inv = Matrix.inverse;

			material.SetFloat(_In3D, In3D ? 1.0f : 0.0f);
			material.SetVector(_Position, inv.MultiplyPoint(Position));
			material.SetVector(_EndPosition, inv.MultiplyPoint(EndPosition));
			material.SetVector(_Position2, inv.MultiplyPoint(Position2));
			material.SetVector(_EndPosition2, inv.MultiplyPoint(EndPosition2));
			material.SetMatrix(_Matrix, inv);
			material.SetVector(_Direction, Direction);
			material.SetColor(_Color, CwHelper.ToLinear(Color));
			material.SetFloat(_Opacity, Opacity);
			material.SetFloat(_Hardness, Hardness);
			material.SetFloat(_Wrapping, Wrapping);
			material.SetTexture(_Texture, Texture);
			material.SetTexture(_Shape, Shape);
			material.SetVector(_ShapeChannel, ShapeChannel);
			material.SetVector(_NormalFront, NormalFront);
			material.SetVector(_NormalBack, NormalBack);
			material.SetTexture(_TileTexture, TileTexture);
			material.SetMatrix(_TileMatrix, TileMatrix);
			material.SetFloat(_TileOpacity, TileOpacity);
			material.SetFloat(_TileTransition, TileTransition);
			material.SetMatrix(_MaskMatrix, MaskMatrix);
			material.SetTexture(_MaskTexture, MaskShape);
			material.SetVector(_MaskChannel, MaskChannel);
			material.SetVector(_MaskStretch, MaskStretch);
		}

		public override void Pool()
		{
			pool.Push(this);
		}

		public override void Transform(Matrix4x4 posMatrix, Matrix4x4 rotMatrix)
		{
			Position     = posMatrix.MultiplyPoint(Position);
			EndPosition  = posMatrix.MultiplyPoint(EndPosition);
			Position2    = posMatrix.MultiplyPoint(Position2);
			EndPosition2 = posMatrix.MultiplyPoint(EndPosition2);
			Matrix       = rotMatrix * Matrix;
			Direction    = Matrix.MultiplyVector(Vector3.forward).normalized;
		}

		public override P3dCommand SpawnCopy()
		{
			var command = SpawnCopy(pool);

			command.Blend          = Blend;
			command.In3D           = In3D;
			command.Position       = Position;
			command.EndPosition    = EndPosition;
			command.Position2      = Position2;
			command.EndPosition2   = EndPosition2;
			command.Extrusions     = Extrusions;
			command.Clip           = Clip;
			command.Matrix         = Matrix;
			command.Direction      = Direction;
			command.Color          = Color;
			command.Opacity        = Opacity;
			command.Hardness       = Hardness;
			command.Wrapping       = Wrapping;
			command.Texture        = Texture;
			command.Shape          = Shape;
			command.ShapeChannel   = ShapeChannel;
			command.NormalFront    = NormalFront;
			command.NormalBack     = NormalBack;
			command.TileTexture    = TileTexture;
			command.TileMatrix     = TileMatrix;
			command.TileOpacity    = TileOpacity;
			command.TileTransition = TileTransition;
			command.MaskMatrix     = MaskMatrix;
			command.MaskShape      = MaskShape;
			command.MaskChannel    = MaskChannel;
			command.MaskStretch    = MaskStretch;

			return command;
		}

		public override void Apply(P3dPaintableTexture paintableTexture)
		{
			base.Apply(paintableTexture);

			if (Blend.Index == P3dBlendMode.REPLACE_ORIGINAL)
			{
				Blend.Color   = paintableTexture.Color;
				Blend.Texture = paintableTexture.Texture;
			}
		}

		/// <summary>This method allows you to set the shape and rotation of the decal.
		/// NOTE: The <b>rotation</b> argument is in world space, where <b>Quaternion.identity</b> means the paint faces forward on the +Z axis, and up is +Y.</summary>
		public void SetShape(Quaternion rotation, Vector3 size, float angle)
		{
			if (In3D == true)
			{
				Matrix = Matrix4x4.TRS(Vector3.zero, rotation * Quaternion.Euler(0.0f, 0.0f, angle), size);
			}
			else
			{
				Matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, angle), size);
			}

			Direction = rotation * Vector3.forward;
		}

		public void SetLocation(Vector3 position, bool in3D = true)
		{
			In3D       = in3D;
			Extrusions = 0;
			Clip       = false;
			Position   = position;
		}

		public void SetLocation(Vector3 position, Vector3 endPosition, bool in3D = true, bool clip = false)
		{
			In3D        = in3D;
			Extrusions  = 1;
			Clip        = clip;
			Position    = position;
			EndPosition = endPosition;
		}

		public void SetLocation(Vector3 positionA, Vector3 positionB, Vector3 positionC, bool in3D = true)
		{
			In3D         = in3D;
			Extrusions   = 2;
			Clip         = false;
			Position     = positionA;
			EndPosition  = positionB;
			Position2    = positionC;
			EndPosition2 = positionA;
		}

		public void SetLocation(Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2, bool in3D = true, bool clip = false)
		{
			In3D         = in3D;
			Extrusions   = 2;
			Clip         = clip;
			Position     = position;
			EndPosition  = endPosition;
			Position2    = position2;
			EndPosition2 = endPosition2;
		}

		public void ClearMask()
		{
			MaskShape   = null;
			MaskChannel = Vector3.one;
		}

		public void SetMask(Matrix4x4 matrix, Texture shape, P3dChannel channel, Vector3 stretch)
		{
			MaskMatrix  = matrix;
			MaskShape   = shape;
			MaskChannel = P3dCommon.IndexToVector((int)channel);
			MaskStretch = new Vector3(stretch.x * 2.0f, stretch.y * 2.0f, 2.0f);
		}

		public void ApplyAspect(Texture texture)
		{
			if (texture != null)
			{
				var width  = texture.width;
				var height = texture.height;

				if (width > height)
				{
					Matrix.m00 *= height / (float)width;
				}
				else
				{
					Matrix.m00 *= width / (float)height;
				}
			}
		}

		public void SetMaterial(P3dBlendMode blendMode, Texture texture, Texture shape, P3dChannel shapeChannel, float hardness, float wrapping, float normalBack, float normalFront, float normalFade, Color color, float opacity, Texture tileTexture, Matrix4x4 tileMatrix, float tileOpacity, float tileTransition)
		{
			switch (Extrusions)
			{
				case 0:
				{
					Material = new P3dHashedMaterial(cachedSpotMaterial, cachedSpotMaterialHash);
				}
				break;

				case 1:
				{
					if (Clip == true)
					{
						Material = new P3dHashedMaterial(cachedLineClipMaterial, cachedLineClipMaterialHash);
					}
					else
					{
						Material = new P3dHashedMaterial(cachedLineMaterial, cachedLineMaterialHash);
					}
				}
				break;

				case 2:
				{
					if (Clip == true)
					{
						Material = new P3dHashedMaterial(cachedQuadClipMaterial, cachedQuadClipMaterialHash);
					}
					else
					{
						Material = new P3dHashedMaterial(cachedQuadMaterial, cachedQuadMaterialHash);
					}
				}
				break;
			}

			Blend          = blendMode;
			Pass           = blendMode;
			Color          = color;
			Opacity        = opacity;
			Hardness       = hardness;
			Wrapping       = wrapping;
			Texture        = texture;
			Shape          = shape;
			ShapeChannel   = P3dCommon.IndexToVector((int)shapeChannel);
			TileTexture    = tileTexture;
			TileMatrix     = tileMatrix;
			TileOpacity    = tileOpacity;
			TileTransition = tileTransition;

			var pointA = normalFront - 1.0f - normalFade;
			var pointB = normalFront - 1.0f;
			var pointC = 1.0f - normalBack + normalFade;
			var pointD = 1.0f - normalBack;

			NormalFront = new Vector2(pointA, CwHelper.Reciprocal(pointB - pointA));
			NormalBack  = new Vector2(pointC, CwHelper.Reciprocal(pointD - pointC));
		}
	}
}