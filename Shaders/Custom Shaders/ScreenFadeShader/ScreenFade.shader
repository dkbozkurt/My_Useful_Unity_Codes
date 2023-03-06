Shader "Unlit/ScreenFade"
{
    Properties
    {
        _MainTex("MainTexture", 2D) = "white" {} 
        _OuterRange("OuterRange", Range(0,1))  = 1
        _InnerRange("InnerRange", Range(-0.1,1))  = 0.2

        _AnimationFrequency("AnimationFrequency", float) = 1 
        _OuterColor("OuterColor", Color) = (1,1,1,1)
    }
    
    SubShader
    {
        
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _AnimationFrequency;

            UNITY_INSTANCING_BUFFER_START(Props)
            
            UNITY_DEFINE_INSTANCED_PROP(float, _OuterRange)
            UNITY_DEFINE_INSTANCED_PROP(float, _InnerRange)
            UNITY_DEFINE_INSTANCED_PROP(float4, _OuterColor)
            
            #define _OuterRange_arr Props
            #define _InnerRange_arr Props
            #define _Color_arr Props

            UNITY_INSTANCING_BUFFER_END(Props)
            
            v2f vert (appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = 2 * v.uv - 1;

                UNITY_TRANSFER_INSTANCE_ID(v, o);

                return o;
            }

            float inverseLerp(float a, float b, float v){
                return (v-a)/(b-a);
            }

            float4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                float outerRange = UNITY_ACCESS_INSTANCED_PROP(_OuterRange_arr, _OuterRange);
                float4 outerColor = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _OuterColor);
                
                float2 outerDist  = length(i.uv) - outerRange;
 
                outerDist = step(outerDist, 0);

                float pointDistance = distance(0 , i.uv);

                float t =  (1 - inverseLerp(_InnerRange, outerRange, pointDistance));
                

                float4 outColor = ( 1 - lerp(float4(0,0,0,0), outerDist.xxxx, t)) * outerColor;
                return outColor;
            }
            ENDCG
        }
    }
}
