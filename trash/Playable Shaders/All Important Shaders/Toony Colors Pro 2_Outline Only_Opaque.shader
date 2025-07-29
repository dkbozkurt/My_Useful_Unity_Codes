Shader "Toony Colors Pro 2/Outline Only/Opaque" {
	Properties {
		_Outline ("Outline Width", Float) = 1
		_OutlineColor ("Outline Color", Vector) = (0.2,0.2,0.2,1)
		_TexLod ("#OUTLINETEX# Texture LOD", Range(0, 10)) = 5
		_MainTex ("#OUTLINETEX# Texture (RGB)", 2D) = "white" {}
		_ZSmooth ("#OUTLINEZ# Z Correction", Range(-3, 3)) = -0.5
		_Offset1 ("#OUTLINEZ# Z Offset 1", Float) = 0
		_Offset2 ("#OUTLINEZ# Z Offset 2", Float) = 0
		_SrcBlendOutline ("#BLEND# Blending Source", Float) = 0
		_DstBlendOutline ("#BLEND# Blending Dest", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	//CustomEditor "TCP2_OutlineInspector"
}