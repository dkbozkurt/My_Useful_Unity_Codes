Shader "Hidden/Paint in 3D/Replace Channels"
{
	Properties
	{
		_TextureR("Texture R", 2D) = "white" {}
		_TextureG("Texture G", 2D) = "white" {}
		_TextureB("Texture B", 2D) = "white" {}
		_TextureA("Texture A", 2D) = "white" {}
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

				sampler2D _TextureR;
				sampler2D _TextureG;
				sampler2D _TextureB;
				sampler2D _TextureA;
				float4    _ChannelR;
				float4    _ChannelG;
				float4    _ChannelB;
				float4    _ChannelA;

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
					o.vertex = float4(i.texcoord0.xy * 2.0f - 1.0f, 0.5f, 1.0f);
					o.texcoord = i.texcoord0;
#if UNITY_UV_STARTS_AT_TOP
					o.vertex.y = -o.vertex.y;
#endif
				}

				void Frag(v2f i, out f2g o)
				{
					float4 texcoord = float4(i.texcoord, 0.0f, 0.0f);

					o.color.r = dot(tex2Dlod(_TextureR, texcoord), _ChannelR);
					o.color.g = dot(tex2Dlod(_TextureG, texcoord), _ChannelG);
					o.color.b = dot(tex2Dlod(_TextureB, texcoord), _ChannelB);
					o.color.a = dot(tex2Dlod(_TextureA, texcoord), _ChannelA);
				}
			ENDCG
		} // Pass
	} // SubShader
} // Shader