//
// Used by Texture packer fast. It ignores fog and renders the alpha channel of source textures.

Shader "MeshBaker/Unlit/UnlitWithAlpha" {
Properties {
    _MainTex ("Base (RGBA)", 2D) = "white" {}
    [Toggle(_SWIZZLE_NORMAL_CHANNELS_NM)] _SwizzleNormalMapChannelsNM("_SwizzleNormalMapChannelsNM", Float) = 0
}

SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 100

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma shader_feature _SWIZZLE_NORMAL_CHANNELS_NM

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.texcoord);
                
#ifdef _SWIZZLE_NORMAL_CHANNELS_NM
                float3 normal = UnpackNormal(tex2D(_MainTex, i.texcoord));
                float3 packedNormal = saturate(normalize(normal) * .5 + .5);
                col = float4(packedNormal, 1);
#endif
                return col;
            }
        ENDCG
    }
}

}
