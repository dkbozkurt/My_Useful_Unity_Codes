Shader "Dkbozkurt/Custom/SimpleOutlineForSRP"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("Albedo", 2D) = "white" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline Width", Range(0,0.1)) = .005
    }
    SubShader
    {
        ZWrite off
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        struct Input
        {
            float2 uv_MainTex;
        };

        float _Outline;
        float4 _OutlineColor;

        void vert (inout appdata_full v)
        {
            v.vertex.xyz += v.normal * _Outline;
        }
        sampler2D _MainTex;
        void surf (Input IN, inout SurfaceOutput o){
            o.Emission = _OutlineColor.rgb;
        }        
        ENDCG
        
        ZWrite on
        CGPROGRAM
        #pragma surface surf Lambert
        struct Input
        {
            float2 uv_MainTex;
        };
        
        sampler2D _MainTex;
        float4 _Color;
        
        void surf (Input IN, inout SurfaceOutput o){
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
