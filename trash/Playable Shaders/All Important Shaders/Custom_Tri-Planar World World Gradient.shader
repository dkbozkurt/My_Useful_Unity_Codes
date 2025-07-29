Shader "Custom/Tri-Planar World World Gradient" {
	Properties {
		_Side ("Side", 2D) = "white" {}
		_Top ("Top", 2D) = "white" {}
		_Bottom ("Bottom", 2D) = "white" {}
		_SideScale ("Side Scale", Float) = 2
		_TopScale ("Top Scale", Float) = 2
		_BottomScale ("Bottom Scale", Float) = 2
		_ColorLow ("Color Low", Vector) = (1,1,1,1)
		_ColorHigh ("Color High", Vector) = (1,1,1,1)
		_yPosLow ("Y Pos Low", Float) = 0
		_yPosHigh ("Y Pos High", Float) = 10
		_GradientStrength ("Graident Strength", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Diffuse"
}