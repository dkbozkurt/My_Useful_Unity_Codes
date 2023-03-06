Shader "SFS/X-Ray Scanner" {
	
	// Writes the geometry to the stencil buffer without considering writing depth and frames buffer
	// This could be used as a see through hole when combine with the proper shader on another object

	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
	}

	SubShader {

		Tags { "Queue" = "Geometry-1" } // -1 so we make sure this object is drawn to the Stencil Buffer first

		ZWrite Off
		ColorMask 0

		Stencil
		{
			Ref 1
			Comp always		// Always want to put a 1 on Stencil for the scanner geometry
			Pass replace	// Replace current Stencil with the new one
			ZFail zero
		}

		CGPROGRAM

		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}

		ENDCG
	}

	FallBack "Diffuse"
}