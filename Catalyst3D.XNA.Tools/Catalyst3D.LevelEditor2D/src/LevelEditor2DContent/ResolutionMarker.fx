float4 Color = float4(80,80,100,80);

struct PixelShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

float4 PixelShaderFunction(PixelShaderOutput input) : COLOR0
{
	return Color;
}

technique Render
{
  pass Pass1
  {
		PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}