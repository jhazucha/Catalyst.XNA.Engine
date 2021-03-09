bool IsSelected;
bool IsPreviousFrame;

texture texSprite;

sampler2D samp1 : TEXUNIT0 = sampler_state
{
  Texture	  = (texSprite);
  MipFilter = LINEAR;
  MagFilter = LINEAR;
  MinFilter = LINEAR;
};

struct PixelShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

float4 PixelShaderFunction(PixelShaderOutput input) : COLOR0
{
	  float4 final = tex2D(samp1, input.TexCoord);
		
		if(IsSelected)
		{
			final.rgb *= 3.0f;
		}
		
		if(IsPreviousFrame)
		{
			final.a *= 0.3f;
		}
		
	  return final;
}

technique TintTechnique
{
  pass Pass1
  {
		PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}