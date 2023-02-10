Shader "Hidden/Paint in 3D/Replace"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			Blend Off
			Cull Off
			ZWrite Off

			CGPROGRAM
				#pragma vertex Vert
				#pragma fragment Frag

				sampler2D _Texture;
				float4    _Color;

				struct a2v
				{
					float4 vertex    : POSITION;
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
					float4 texcoord = float4(i.texcoord, 0.0f, 0.0f);

					o.color = tex2Dlod(_Texture, texcoord) * _Color;
				}
			ENDCG
		} // Pass
	} // SubShader
} // Shader