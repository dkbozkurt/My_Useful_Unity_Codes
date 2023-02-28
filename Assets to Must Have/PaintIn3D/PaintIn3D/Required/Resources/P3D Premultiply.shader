Shader "Hidden/Paint in 3D/Premultiply"
{
	Properties
	{
		_Texture("Texture", 2D) = "bump" {}
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

				sampler2D _Texture;
				float4    _Color;

				#include "UnityCG.cginc"

				struct a2v
				{
					float2 texcoord0 : TEXCOORD0;
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
					o.vertex   = float4(i.texcoord0.xy * 2.0f - 1.0f, 0.5f, 1.0f);
					o.texcoord = i.texcoord0;
#if UNITY_UV_STARTS_AT_TOP
					o.vertex.y = -o.vertex.y;
#endif
				}

				void Frag(v2f i, out f2g o)
				{
					o.color = tex2D(_Texture, i.texcoord) * _Color;

					o.color.rgb *= o.color.a;
				}
			ENDCG
		} // Pass
	} // SubShader
} // Shader