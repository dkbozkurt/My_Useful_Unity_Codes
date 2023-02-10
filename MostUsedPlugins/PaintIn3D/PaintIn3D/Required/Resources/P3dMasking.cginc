float4x4  _MaskMatrix;
sampler2D _MaskTexture;
float4    _MaskChannel;
float3    _MaskStretch;

float GetMask(float3 p)
{
	float2 uv = p.xy + 0.5f;

	float3 a = abs(p * _MaskStretch);
	float  b = max(a.x, max(a.y, a.z));
	float  c = saturate((b - 1.0f) * 1000.0f);
	float  m = dot(tex2D(_MaskTexture, uv), _MaskChannel);

	return saturate(m + c);
}

// Local masking
sampler2D _LocalMaskTexture;
float4    _LocalMaskChannel;

float GetLocalMask(float2 uv)
{
	return dot(SampleMip0(_LocalMaskTexture, uv), _LocalMaskChannel);
}