bool IsSelected;
bool IsClicked;
bool IsPreviousFrame;
bool IsSelectedNode;

texture texSprite;
float4 Color;

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
		
		// Bleed it out
		final.rgba *= Color;

		if(IsSelected)
		{
			//final.rgb *= 3.0f;
		}
		
		if(IsPreviousFrame)
		{
			final.a *= 0.3f;
		}
		
		if(IsClicked)
		{
		 final.gb *= 2.0f;
		}

		if(IsSelectedNode)
		{
			final.rgb = 0;
			final.gb = 3.0;
		}

		final = final * Color.a;

	  return final;
}

technique TintTechnique
{
  pass Pass1
  {
		AlphaBlendEnable  = true;

		PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}