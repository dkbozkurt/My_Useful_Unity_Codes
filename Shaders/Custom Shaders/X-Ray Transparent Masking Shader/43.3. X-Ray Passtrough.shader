Shader "SFS/X-Ray Passthrough" {
	
	// Makes the geometry visible only on those areas which are not written on the stencil buffer
	// Combined with the 43.1. shader (put into another object) it allows to simulate that the geometry has a see through hole

	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
	}

	SubShader {

		Tags { "Queue" = "Geometry+1" }

		Stencil
		{
			Ref 1
			Comp notequal // If not equal 1 on the Stencil don't draw it. Otherwise draw it
			Pass keep
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