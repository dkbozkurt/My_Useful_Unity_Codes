Shader "Hidden/Paint in 3D/Texture"
{
	Properties
	{
	}
	SubShader
	{
		Pass
		{
			Cull Off
			ZWrite Off
			ZTest Always

			CGPROGRAM
				#pragma vertex Vert
				#pragma fragment Frag

				float4 _Coord;

				#include "P3dShared.cginc"

				struct a2v
				{
					float2 texcoord0 : TEXCOORD0;
					float2 texcoord1 : TEXCOORD1;
					float2 texcoord2 : TEXCOORD2;
					float2 texcoord3 : TEXCOORD3;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct f2g
				{
					float4 color : SV_TARGET;
				};

				void Vert(a2v i, out v2f o)
				{
					float2 texcoord = i.texcoord0 * _Coord.x + i.texcoord1 * _Coord.y + i.texcoord2 * _Coord.z + i.texcoord3 * _Coord.w;
					o.vertex   = float4(texcoord.xy * 2.0f - 1.0f, 0.5f, 1.0f);
					o.texcoord = texcoord;
#if UNITY_UV_STARTS_AT_TOP
					o.vertex.y = -o.vertex.y;
#endif
				}

				void Frag(v2f i, out f2g o)
				{
					o.color = SampleMip0(_Buffer, SnapToPixel(i.texcoord, _BufferSize));
				}
			ENDCG
		} // Pass
	} // SubShader
} // Shader