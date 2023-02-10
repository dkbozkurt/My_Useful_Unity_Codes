#include "UnityCG.cginc"
#include "UnityStandardUtils.cginc"

// REPLACE_ORIGINAL + REPLACE_CUSTOM
float4    _ReplaceColor;
sampler2D _ReplaceTexture;
float2    _ReplaceTextureSize;

// BLUR + FLOW
float _Kernel;

void ContributeMaskedSample(float2 coord, inout float4 totalColor, inout float totalWeight)
{
	float weight = dot(SampleMip0(_LocalMaskTexture, coord), _LocalMaskChannel);

	totalColor  += weight * SampleMip0(_Buffer, coord);
	totalWeight += weight;
}

float4 DoBlend(float4 current, float4 color, float strength, float2 coord, float rot, float4 channels)
{
	float4 old = current;
#if BLEND_MODE_INDEX == 0 // ALPHA_BLEND
	color.a *= strength;
	float str = 1.0f - color.a;
	float div = color.a + current.a * str;

	current.rgb = (color.rgb * color.a + current.rgb * current.a * str) / div;
	current.a   = div;
#elif BLEND_MODE_INDEX == 1 // ALPHA_BLEND_INVERSE
	color.a *= strength;
	float str = 1.0f - current.a;
	float div = current.a + color.a * str;

	current.rgb = (current.rgb * current.a + color.rgb * color.a * str) / div;
	current.a   = div;
#elif BLEND_MODE_INDEX == 2 // PREMULTIPLIED
	color.a *= strength;
	color.rgb *= color.a;
	current = color + (1.0f - color.a) * current;
#elif BLEND_MODE_INDEX == 3 // ADDITIVE
	current += color * strength;
#elif BLEND_MODE_INDEX == 4 // ADDITIVE_SOFT
	current += color * strength * (1.0f - current);
#elif BLEND_MODE_INDEX == 5 // SUBTRACTIVE
	current -= color * strength;
#elif BLEND_MODE_INDEX == 6 // SUBTRACTIVE_SOFT
	current -= color * strength * current;
#elif BLEND_MODE_INDEX == 7 // REPLACE
	current += (color - current) * strength;
#elif BLEND_MODE_INDEX == 8 // REPLACE_ORIGINAL
	float4 rep = SampleMip0(_ReplaceTexture, coord) * _ReplaceColor;
	current += (rep - current) * strength;
#elif BLEND_MODE_INDEX == 9 // REPLACE_CUSTOM
	float4 rep = SampleMip0(_ReplaceTexture, coord) * _ReplaceColor;
	current += (rep - current) * strength;
#elif BLEND_MODE_INDEX == 10 // MULTIPLY_INVERSE_RGB
	//current.rgb *= 1.0f - (1.0f - color.rgb) * strength;
	color.rgb = lerp(color.rgb, float3(1, 1, 1), 1 - color.a * strength);
	current.rgb *= color.rgb;
#elif BLEND_MODE_INDEX == 11 // BLUR
	float2 k = _Kernel / _BufferSize;
	float4 c = 0.0f;
	float  w = 0.0f;
	ContributeMaskedSample(coord + float2(-k.x, 0.0f), c, w);
	ContributeMaskedSample(coord + float2(+k.x, 0.0f), c, w);
	ContributeMaskedSample(coord + float2(0.0f, -k.y), c, w);
	ContributeMaskedSample(coord + float2(0.0f, +k.y), c, w);
	current += w != 0.0f ? (c / w - current) * strength : 0.0f;
#elif BLEND_MODE_INDEX == 12 // NORMAL BLEND
	float3 curVec = UnpackNormal(current);
	float3 dstVec = UnpackNormal(color);
	dstVec = RotateNormal(dstVec, rot);
	dstVec = lerp(float3(0.0f, 0.0f, 1.0f), dstVec, strength);
	curVec = normalize(float3(curVec.xy + dstVec.xy, curVec.z * dstVec.z));
	current = PackNormal(curVec);
#elif BLEND_MODE_INDEX == 13 // NORMAL REPLACE
	float3 curVec = UnpackNormal(current);
	float3 dstVec = UnpackNormal(color);
	dstVec = RotateNormal(dstVec, rot);
	curVec = normalize(lerp(curVec, dstVec, strength));
	current = PackNormal(curVec);
#elif BLEND_MODE_INDEX == 14 // FLOW
	float3 dstVec = UnpackNormal(color);
	dstVec = RotateNormal(dstVec, rot);
	float2 k = _Kernel / _BufferSize;
	float4 c = 0.0f;
	float  w = 0.0f;
	ContributeMaskedSample(coord - k * dstVec.xy * strength, c, w);
	current += w != 0.0f ? (c / w - current) * strength : 0.0f;
#endif
	return old + (current - old) * channels;
}

float4 Blend(float4 color, float strength, float2 coord, float rot, float4 channels)
{
	coord = SnapToPixel(coord, _BufferSize);
	float4 current = SampleMip0(_Buffer, coord);

	return DoBlend(current, color, strength, coord, rot, channels);
}

float4 Blend(float4 color, float strength, float2 coord, float4 channels)
{
	return Blend(color, strength, coord, 0.0f, channels);
}

float4 BlendMinimum(float4 color, float mask, float strength, float2 coord, float4 step, float4 channels)
{
	coord = SnapToPixel(coord, _BufferSize);
	float4 current = SampleMip0(_Buffer, coord);
	float4 result  = DoBlend(current, color, 1.0f, coord, 0.0f, channels);
	float4 change  = result - current;
	float4 maximum = abs(change);

	return current + sign(change) * clamp(strength, step, maximum) * mask;
}