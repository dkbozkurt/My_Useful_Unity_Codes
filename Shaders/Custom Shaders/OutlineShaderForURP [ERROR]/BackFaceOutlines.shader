// Ref : https://www.youtube.com/watch?v=1QPA3s0S3Oo&ab_channel=NedMakesGames
Shader "Outlines/BackFaceOutlines"
{
    Properties
    {
        _Thickness ("Thickness", Float) = 1
        _Color("Color", Color) = (1,1,1,1) 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque""RenderPipeline" = "UniversalPipeline"}
            
        Pass 
        {
            
            Name "Outlines"
            
            Cull front
            
            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex Vertex
            #pragma fragment Fragment

            #include  "BackFaceOutlines.hlsl"

            ENDHLSL
        }
    }
}
