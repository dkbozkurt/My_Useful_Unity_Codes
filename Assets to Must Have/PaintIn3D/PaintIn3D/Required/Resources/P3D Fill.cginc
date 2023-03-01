#include "P3dShared.cginc"
#include "P3dMasking.cginc"
#include "P3dBlendModes.cginc"

float4    _Channels;
sampler2D _Texture;
float4    _Color;
float     _Opacity;
float4    _Minimum;

struct a2v
{
	float4 vertex    : POSITION;
	float2 texcoord0 : TEXCOORD0;
};

struct v2f
{
	float4 vertex   : SV_POSITION;
	float2 texcoord : TEXCOORD0;
};

struct f2g
{
	float4 color : SV_TARGET;
};

void Vert(a2v i, out v2f o)
{
	o.vertex = float4(i.texcoord0.xy * 2.0f - 1.0f, 0.5f, 1.0f);
	o.texcoord = i.texcoord0;
#if UNITY_UV_STARTS_AT_TOP
	o.vertex.y = -o.vertex.y;
#endif
}

void Frag(v2f i, out f2g o)
{
	float  strength = 1.0f;
	float4 color    = tex2D(_Texture, i.texcoord) * _Color;

	// Fade local mask
	strength *= GetLocalMask(i.texcoord);

	o.color = BlendMinimum(color, strength, _Opacity, i.texcoord, _Minimum, _Channels);
}