Shader "Custom/RevealShaderHorizontal" {
	Properties {
		_DissolveFactor ("Dissolve Factor", Range(-150, 150)) = 1
		_WiggleFactor ("Wiggle Factor", Range(0, 1)) = 1
		_WiggleTime ("Wiggle Time", Float) = 0
		_EdgeColor ("Edge Color", Vector) = (1,0.5,0,1)
		_DisableColor ("Disabled Color", Vector) = (0.2,0.2,0.2,1)
		_EdgeWidth ("Edge Width", Range(0, 20)) = 10
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_WiggleOffset ("Wiggle Offset", Range(-3, 3)) = 0
		_WiggleSpeed ("Wiggle Speed", Range(-10, 10)) = 1
		_WiggleXZScale ("Wiggle XZ scale", Range(-10, 10)) = 1
		_PlanePos ("Plane Position", Vector) = (0,0,0,0)
		_PlaneNorm ("Plane Normal", Vector) = (0,0,1,0)
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
	Fallback "Diffuse"
}