Shader "Hidden/Paint in 3D/Fill"
{
	Properties
	{
		_ReplaceTexture("Replace Texture", 2D) = "white" {}
		_Texture("Texture", 2D) = "white" {}
		_MaskTexture("Mask Texture", 2D) = "white" {}
		_LocalMaskTexture("Local Mask Texture", 2D) = "white" {}
	}

	SubShader
	{
		Blend Off
		Cull Off
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 0 // ALPHA_BLEND
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 1 // ALPHA_BLEND_INVERSE
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 2 // PREMULTIPLIED
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 3 // ADDITIVE
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 4 // ADDITIVE_SOFT
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 5 // SUBTRACTIVE
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 6 // SUBTRACTIVE_SOFT
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 7 // REPLACE
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 8 // REPLACE_ORIGINAL
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 9 // REPLACE_CUSTOM
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 10 // MULTIPLY_INVERSE_RGB
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 11 // BLUR
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 12 // NORMAL_BLEND
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 13 // NORMAL_REPLACE
			#include "P3D Fill.cginc"
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma multi_compile_local __ P3D_LINE P3D_QUAD
			#define BLEND_MODE_INDEX 14 // FLOW
			#include "P3D Fill.cginc"
			ENDCG
		}
	} // SubShader
} // Shader