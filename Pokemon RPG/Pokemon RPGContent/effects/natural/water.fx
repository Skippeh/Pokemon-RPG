sampler Texture : register(s0);

float4 main(float2 texCoord: TEXCOORD0) : COLOR0
{
	return tex2D(Texture, texCoord);
}

technique Water
{
	/*pass Pass0
	{
		PixelShader = compile vs_2_0 main();
	}*/
}