Shader "Toon/Lit TriPlanar" {
	Properties {
		_Color ("Main Color", Vector) = (0.5,0.5,0.5,1)
		_MainTex ("Top Texture", 2D) = "white" {}
		_MainTexSide ("Side/Bottom Texture", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_Normal ("Normal/Noise", 2D) = "bump" {}
		_Scale ("Top Scale", Range(-2, 2)) = 1
		_SideScale ("Side Scale", Range(-2, 2)) = 1
		_NoiseScale ("Noise Scale", Range(-2, 2)) = 1
		_TopSpread ("TopSpread", Range(-2, 2)) = 1
		_EdgeWidth ("EdgeWidth", Range(0, 0.5)) = 1
		_RimPower ("Rim Power", Range(-2, 20)) = 1
		_RimColor ("Rim Color Top", Vector) = (0.5,0.5,0.5,1)
		_RimColor2 ("Rim Color Side/Bottom", Vector) = (0.5,0.5,0.5,1)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}