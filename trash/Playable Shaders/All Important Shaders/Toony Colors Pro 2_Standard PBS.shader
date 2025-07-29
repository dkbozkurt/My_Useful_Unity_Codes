Shader "Toony Colors Pro 2/Standard PBS" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {}
		_Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_GlossMapScale ("Smoothness Scale", Range(0, 1)) = 1
		[Enum(Metallic Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel ("Smoothness texture channel", Float) = 0
		[Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
		_MetallicGlossMap ("Metallic", 2D) = "white" {}
		[ToggleOff] _SpecularHighlights ("Specular Highlights", Float) = 1
		[ToggleOff] _GlossyReflections ("Glossy Reflections", Float) = 1
		_BumpScale ("Scale", Float) = 1
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Parallax ("Height Scale", Range(0.005, 0.08)) = 0.02
		_ParallaxMap ("Height Map", 2D) = "black" {}
		_OcclusionStrength ("Strength", Range(0, 1)) = 1
		_OcclusionMap ("Occlusion", 2D) = "white" {}
		_EmissionColor ("Color", Vector) = (0,0,0,1)
		_EmissionMap ("Emission", 2D) = "white" {}
		_DetailMask ("Detail Mask", 2D) = "white" {}
		_DetailAlbedoMap ("Detail Albedo x2", 2D) = "grey" {}
		_DetailNormalMapScale ("Scale", Float) = 1
		_DetailNormalMap ("Normal Map", 2D) = "bump" {}
		[Enum(UV0,0,UV1,1)] _UVSec ("UV Set for secondary textures", Float) = 0
		[HideInInspector] _Mode ("__mode", Float) = 0
		[HideInInspector] _SrcBlend ("__src", Float) = 1
		[HideInInspector] _DstBlend ("__dst", Float) = 0
		[HideInInspector] _ZWrite ("__zw", Float) = 1
		_HColor ("Highlight Color", Vector) = (1,1,1,1)
		_SColor ("Shadow Color", Vector) = (0.25,0.25,0.25,1)
		[Toggle(TCP2_DISABLE_WRAPPED_LIGHT)] _TCP2_DISABLE_WRAPPED_LIGHT ("Disable Wrapped Lighting", Float) = 1
		[Toggle(TCP2_RAMPTEXT)] _TCP2_RAMPTEXT ("Ramp Texture", Float) = 0
		[TCP2Gradient] _Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_RampThreshold ("Threshold", Range(0, 1)) = 0.5
		_RampSmooth ("ML Smoothing", Range(0, 1)) = 0.2
		_RampSmoothAdd ("AL Smoothing", Range(0, 1)) = 0.75
		[Toggle(TCP2_SPEC_TOON)] _TCP2_SPEC_TOON ("Stylized Specular", Float) = 0
		_SpecSmooth ("Specular Smoothing", Range(0, 1)) = 1
		_SpecBlend ("Stylized Specular Blending", Range(0, 1)) = 1
		[Toggle(TCP2_STYLIZED_FRESNEL)] _TCP2_STYLIZED_FRESNEL ("Stylized Fresnel", Float) = 0
		_RimColor ("Fresnel Color (RGB) / Strength (A)", Vector) = (0.8,0.8,0.8,0.6)
		[PowerSlider(3)] _RimStrength ("Fresnel Strength", Range(0, 2)) = 0.5
		_RimMin ("Fresnel Ramp Min", Range(0, 1)) = 0.6
		_RimMax ("Fresnel Ramp Max", Range(0, 1)) = 0.85
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
	//CustomEditor "TCP2_MaterialInspector_PBS"
}