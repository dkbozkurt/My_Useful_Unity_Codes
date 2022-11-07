// Ref : https://www.youtube.com/watch?v=1QPA3s0S3Oo&ab_channel=NedMakesGames

#ifndef BACKFACEOUTLINES_INCLUDED
#define BACKFACEOUTLINES_INCLUDED

// Include helper functions from URP
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

// Data from the meshes
struct Attributes{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
};

// Output from the vertex function and input to the fragment function
struct VertexOutput
{
    float4 positionCS   : SV_POSITION;
};

// Properties
float _Thickness;
float4 _Color;

VertexOutput Vertex(Attributes input)
{
    VertexOutput output = (VertexOutput)0;

    float3 normalOS = input.normalOS;

    // Extrude the object space position along a normal vector
    float3 posOS = input.positionOS.xyz + normalOS * _Thickness;
    // Convert this position to world and clip space
    output.positionCS = GetVertexPositionInputs(posOS).positionCS;

    return output;
}

float4 Fragment(VertexOutput input) : SV_Target {
    return _Color;
}

#endif