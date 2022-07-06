Shader "ForceFieldShader"
{
    Properties
    {
        Vector1_20a170bd068d4c2f97c15bb78f846414("Offset", Float) = 0.6
        [HDR]Color_28dc4cd8921349cd9d4d9c2d0171dddd("Emission", Color) = (0, 5.016724, 6.062866, 1)
        Vector1_ee56f45005b749e58acca2ec1fecb03b("Fresnel Power", Float) = 2.8
        [NoScaleOffset]Texture2D_4138e472b31e49de895778b2624cb292("Pattern", 2D) = "white" {}
        Vector1_21e22c6c655546c7924b321aaec5885e("Scroll Speed", Float) = 0.05
        Vector1_b43d930ad93a4d16ac916e23e036d5fb("Fill", Float) = 0.01
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
        #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_FORWARD
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 tangentWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            float2 lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 sh;
            #endif
            float4 fogFactorAndVertexLight;
            float4 shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 TangentSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float4 interp3 : TEXCOORD3;
            float3 interp4 : TEXCOORD4;
            #if defined(LIGHTMAP_ON)
            float2 interp5 : TEXCOORD5;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 interp6 : TEXCOORD6;
            #endif
            float4 interp7 : TEXCOORD7;
            float4 interp8 : TEXCOORD8;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp5.xy =  input.lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp6.xyz =  input.sh;
            #endif
            output.interp7.xyzw =  input.fogFactorAndVertexLight;
            output.interp8.xyzw =  input.shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if defined(LIGHTMAP_ON)
            output.lightmapUV = input.interp5.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp6.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp7.xyzw;
            output.shadowCoord = input.interp8.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_28dc4cd8921349cd9d4d9c2d0171dddd) : Color_28dc4cd8921349cd9d4d9c2d0171dddd;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Emission = (_Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0.xyz);
            surface.Metallic = 0;
            surface.Smoothness = 0.5;
            surface.Occlusion = 1;
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
            output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        #pragma multi_compile _ _GBUFFER_NORMALS_OCT
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_GBUFFER
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 tangentWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            float2 lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 sh;
            #endif
            float4 fogFactorAndVertexLight;
            float4 shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 TangentSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float4 interp3 : TEXCOORD3;
            float3 interp4 : TEXCOORD4;
            #if defined(LIGHTMAP_ON)
            float2 interp5 : TEXCOORD5;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 interp6 : TEXCOORD6;
            #endif
            float4 interp7 : TEXCOORD7;
            float4 interp8 : TEXCOORD8;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp5.xy =  input.lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp6.xyz =  input.sh;
            #endif
            output.interp7.xyzw =  input.fogFactorAndVertexLight;
            output.interp8.xyzw =  input.shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if defined(LIGHTMAP_ON)
            output.lightmapUV = input.interp5.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp6.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp7.xyzw;
            output.shadowCoord = input.interp8.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_28dc4cd8921349cd9d4d9c2d0171dddd) : Color_28dc4cd8921349cd9d4d9c2d0171dddd;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Emission = (_Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0.xyz);
            surface.Metallic = 0;
            surface.Smoothness = 0.5;
            surface.Occlusion = 1;
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
            output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        ColorMask 0

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float3 interp3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.viewDirectionWS = input.interp3.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        ColorMask 0

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHONLY
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float3 interp3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.viewDirectionWS = input.interp3.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 tangentWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 TangentSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float4 interp3 : TEXCOORD3;
            float3 interp4 : TEXCOORD4;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 NormalTS;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
            output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }

            // Render State
            Cull Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_META
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float3 interp3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.viewDirectionWS = input.interp3.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_28dc4cd8921349cd9d4d9c2d0171dddd) : Color_28dc4cd8921349cd9d4d9c2d0171dddd;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
            surface.Emission = (_Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0.xyz);
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            // Name: <None>
            Tags
            {
                "LightMode" = "Universal2D"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_2D
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float3 interp3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.viewDirectionWS = input.interp3.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

            ENDHLSL
        }
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
        #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_FORWARD
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 tangentWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            float2 lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 sh;
            #endif
            float4 fogFactorAndVertexLight;
            float4 shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 TangentSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float4 interp3 : TEXCOORD3;
            float3 interp4 : TEXCOORD4;
            #if defined(LIGHTMAP_ON)
            float2 interp5 : TEXCOORD5;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 interp6 : TEXCOORD6;
            #endif
            float4 interp7 : TEXCOORD7;
            float4 interp8 : TEXCOORD8;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp5.xy =  input.lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp6.xyz =  input.sh;
            #endif
            output.interp7.xyzw =  input.fogFactorAndVertexLight;
            output.interp8.xyzw =  input.shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if defined(LIGHTMAP_ON)
            output.lightmapUV = input.interp5.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp6.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp7.xyzw;
            output.shadowCoord = input.interp8.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_28dc4cd8921349cd9d4d9c2d0171dddd) : Color_28dc4cd8921349cd9d4d9c2d0171dddd;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Emission = (_Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0.xyz);
            surface.Metallic = 0;
            surface.Smoothness = 0.5;
            surface.Occlusion = 1;
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
            output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        ColorMask 0

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float3 interp3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.viewDirectionWS = input.interp3.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        ColorMask 0

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHONLY
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float3 interp3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.viewDirectionWS = input.interp3.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 tangentWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 TangentSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float4 interp3 : TEXCOORD3;
            float3 interp4 : TEXCOORD4;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 NormalTS;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
            output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }

            // Render State
            Cull Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_META
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float3 interp3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.viewDirectionWS = input.interp3.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0 = IsGammaSpace() ? LinearToSRGB(Color_28dc4cd8921349cd9d4d9c2d0171dddd) : Color_28dc4cd8921349cd9d4d9c2d0171dddd;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
            surface.Emission = (_Property_34ecf59b09ae4cec8f9122a8ecee4208_Out_0.xyz);
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            // Name: <None>
            Tags
            {
                "LightMode" = "Universal2D"
            }

            // Render State
            Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_2D
        #define REQUIRE_DEPTH_TEXTURE
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            float3 interp3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.viewDirectionWS = input.interp3.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float Vector1_20a170bd068d4c2f97c15bb78f846414;
        float4 Color_28dc4cd8921349cd9d4d9c2d0171dddd;
        float Vector1_ee56f45005b749e58acca2ec1fecb03b;
        float4 Texture2D_4138e472b31e49de895778b2624cb292_TexelSize;
        float Vector1_21e22c6c655546c7924b321aaec5885e;
        float Vector1_b43d930ad93a4d16ac916e23e036d5fb;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_4138e472b31e49de895778b2624cb292);
        SAMPLER(samplerTexture2D_4138e472b31e49de895778b2624cb292);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }

        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }

        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_4138e472b31e49de895778b2624cb292);
            float _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0 = Vector1_21e22c6c655546c7924b321aaec5885e;
            float _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2;
            Unity_Multiply_float(IN.TimeParameters.x, _Property_99c95b52b18f45ea96a64db5d08b45d7_Out_0, _Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2);
            float2 _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (3, 3), (_Multiply_827e30d81f6a43dd9175cef31d45a822_Out_2.xx), _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float4 _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.tex, _Property_bfdcc9b0f47c45b4acf3957d9de2620a_Out_0.samplerstate, _TilingAndOffset_545cffc8100d4297afde63e490910f32_Out_3);
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_R_4 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.r;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_G_5 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.g;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_B_6 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.b;
            float _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_A_7 = _SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0.a;
            float _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0 = Vector1_ee56f45005b749e58acca2ec1fecb03b;
            float _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_91a9027ec3564d5a87001e2f42cbdf15_Out_0, _FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3);
            float _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1;
            Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1);
            float4 _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0 = IN.ScreenPosition;
            float _Split_732b456d509744a5b4f4d389031a72f5_R_1 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[0];
            float _Split_732b456d509744a5b4f4d389031a72f5_G_2 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[1];
            float _Split_732b456d509744a5b4f4d389031a72f5_B_3 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[2];
            float _Split_732b456d509744a5b4f4d389031a72f5_A_4 = _ScreenPosition_60727612eae547b8afd1a8d2fb74a0b5_Out_0[3];
            float _Property_d9b0268faf174704bd04835f38f11bd0_Out_0 = Vector1_20a170bd068d4c2f97c15bb78f846414;
            float _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2;
            Unity_Subtract_float(_Split_732b456d509744a5b4f4d389031a72f5_A_4, _Property_d9b0268faf174704bd04835f38f11bd0_Out_0, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2);
            float _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2;
            Unity_Subtract_float(_SceneDepth_3f7b70b2f2a34184b5ffbee4c047146f_Out_1, _Subtract_76f072a30c634eca82c3b5f48b132ccc_Out_2, _Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2);
            float _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1;
            Unity_OneMinus_float(_Subtract_d02c190a6d1849dd8fc7ffbe7b4267a1_Out_2, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1);
            float _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3;
            Unity_Smoothstep_float(0, 1, _OneMinus_20a22c6c5f924fc2b30df052f2882f68_Out_1, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3);
            float _Add_82e0325c81694cc085fdb865f792134a_Out_2;
            Unity_Add_float(_FresnelEffect_3d091958efb54d31aa5b001656940b27_Out_3, _Smoothstep_cbc1f2857d884c558a24c465e3c1ba7b_Out_3, _Add_82e0325c81694cc085fdb865f792134a_Out_2);
            float4 _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2;
            Unity_Multiply_float(_SampleTexture2D_92d91cbcd154464e96eb6b1e4cc147f8_RGBA_0, (_Add_82e0325c81694cc085fdb865f792134a_Out_2.xxxx), _Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2);
            float _Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0 = Vector1_b43d930ad93a4d16ac916e23e036d5fb;
            float4 _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2;
            Unity_Add_float4(_Multiply_799fe7be160a4d14a16714f90c29ffc3_Out_2, (_Property_f2326cf1ae304a0ba06f69514e5a491e_Out_0.xxxx), _Add_80d543d4ca4a4c129490caeeb4070d65_Out_2);
            surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
            surface.Alpha = (_Add_80d543d4ca4a4c129490caeeb4070d65_Out_2).x;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        	float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);


            output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


            output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.WorldSpacePosition =          input.positionWS;
            output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 =                         input.texCoord0;
            output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

            ENDHLSL
        }
    }
    CustomEditor "ShaderGraph.PBRMasterGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}