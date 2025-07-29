Shader "Toony Colors Pro 2/Examples/PBS/Outline Behind" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {}
		_Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_GlossMapScale ("Smoothness Scale", Range(0, 1)) = 1
		[Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
		[HideInInspector] _Mode ("__mode", Float) = 0
		[HideInInspector] _SrcBlend ("__src", Float) = 1
		[HideInInspector] _DstBlend ("__dst", Float) = 0
		[HideInInspector] _ZWrite ("__zw", Float) = 1
		_HColor ("Highlight Color", Vector) = (0.785,0.785,0.785,1)
		_SColor ("Shadow Color", Vector) = (0.195,0.195,0.195,1)
		[Header(Ramp Shading)] _RampThreshold ("Threshold", Range(0, 1)) = 0.5
		_RampSmooth ("Main Light Smoothing", Range(0, 1)) = 0.2
		_RampSmoothAdd ("Other Lights Smoothing", Range(0, 1)) = 0.75
		[Header(Stylized Specular)] _SpecSmooth ("Specular Smoothing", Range(0, 1)) = 1
		_SpecBlend ("Specular Blend", Range(0, 1)) = 1
		[Header(Stylized Fresnel)] [PowerSlider(3)] _RimStrength ("Strength", Range(0, 2)) = 0.5
		_RimMin ("Min", Range(0, 1)) = 0.6
		_RimMax ("Max", Range(0, 1)) = 0.85
		[Header(Outline)] _OutlineColor ("Outline Color", Vector) = (0.2,0.2,0.2,1)
		_Outline ("Outline Width", Float) = 1
		[Toggle(TCP2_OUTLINE_TEXTURED)] _EnableTexturedOutline ("Color from Texture", Float) = 0
		[TCP2KeywordFilter(TCP2_OUTLINE_TEXTURED)] _TexLod ("Texture LOD", Range(0, 10)) = 5
		[Toggle(TCP2_OUTLINE_CONST_SIZE)] _EnableConstSizeOutline ("Constant Size Outline", Float) = 0
		[Toggle(TCP2_ZSMOOTH_ON)] _EnableZSmooth ("Correct Z Artefacts", Float) = 0
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _ZSmooth ("Z Correction", Range(-3, 3)) = -0.5
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _Offset1 ("Z Offset 1", Float) = 0
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _Offset2 ("Z Offset 2", Float) = 0
		[TCP2OutlineNormalsGUI] __outline_gui_dummy__ ("_unused_", Float) = 0
		_StencilRef ("Stencil Outline Group", Range(0, 255)) = 1
		[HideInInspector] __dummy__ ("__unused__", Float) = 0
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
	//CustomEditor "TCP2_MaterialInspector_SurfacePBS_SG"
}