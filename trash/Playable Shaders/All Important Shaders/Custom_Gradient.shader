Shader "Custom/Gradient" {
	Properties {
		_Color ("Top Color", Vector) = (1,1,1,1)
		_Color2 ("Bottom Color", Vector) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GradientScale ("Gradient Scale", Float) = 1
		_GradientOffset ("Gradient Offset", Float) = 0.5
		[KeywordEnum(X, Y, Z)] _Direction ("Direction", Float) = 0
		[KeywordEnum(World, Local)] _Space ("Space", Float) = 0
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
	Fallback "VertexLit"
}