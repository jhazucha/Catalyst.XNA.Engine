float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 DecalViewProj;

// Ambient Variables
float4 AmbientLightColor;
float AmbientLightIntensity;

// Diffuse Variables
float4 DiffuseLightColor;
float DiffuseLightIntensity;
float3 DiffuseLightDirection;

float TextureScale;

Texture2D TextureLayer1;
Texture2D TextureLayer2;
Texture2D TextureLayer3;
Texture2D TextureLayer4;

Texture2D PaintMask;

float3 CursorPosition;
float CursorSize = 0.5;

struct VertexShaderInput
{
  float4 Position	: POSITION0;
  float2 TexCoords : TEXCOORD0;
	float3 Normal		: NORMAL0;
};

struct VertexShaderOutput
{
  float4 Position	: POSITION0;
  float2 TexCoords	: TEXCOORD0;
	float3 Normal	: TEXCOORD1;
	float dist : TEXCOORD2;
};

struct PixelShaderInput
{
  float4 Position	: POSITION0;
	float2 TexCoords : TEXCOORD0;
	float3 Normal   : TEXCOORD1;
};

sampler samp1 = sampler_state
{
  Texture	= (TextureLayer1);
  MinFilter = LINEAR;
  MipFilter = LINEAR;
  MagFilter = LINEAR;
};

sampler samp2 = sampler_state
{
  Texture	= (TextureLayer2);
  MinFilter = LINEAR;
  MipFilter = LINEAR;
  MagFilter = LINEAR;
};

sampler samp3 = sampler_state
{
  Texture	= (TextureLayer3);
  MinFilter = LINEAR;
  MipFilter = LINEAR;
  MagFilter = LINEAR;
};

sampler samp4 = sampler_state
{
  Texture	= (TextureLayer4);
  MinFilter = LINEAR;
  MipFilter = LINEAR;
  MagFilter = LINEAR;
};

sampler sampPaintMask = sampler_state
{
  Texture	= (PaintMask);
  MinFilter = LINEAR;
  MipFilter = LINEAR;
  MagFilter = LINEAR;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
  VertexShaderOutput output;
  
	float4 worldPosition = mul(input.Position, World);
  float4 viewPosition = mul(worldPosition, View);
  
	output.Position = mul(viewPosition, Projection);
  
	// Transform the normal from model space to world space
  output.Normal = normalize(mul(input.Normal, World));

  output.TexCoords = input.TexCoords;

	output.dist = distance(input.Position.xz, CursorPosition.xz);

  return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
  // Calculate our ambient component
  float4 ambient = AmbientLightIntensity * AmbientLightColor;

	// Calculate our diffuse lighting
  float4 diffuse = dot(DiffuseLightDirection, input.Normal ) * DiffuseLightIntensity * DiffuseLightColor;
 
  // Return the total light component as a combination of the diffuse and ambient.
  // Saturate it to keep the color between 0 and 1.
	float f1 = saturate(diffuse + ambient);
	
	// Layers
	float4 layer1 = tex2D(samp1, input.TexCoords);
	float4 layer2 = tex2D(samp2, input.TexCoords);
	float4 layer3 = tex2D(samp3, input.TexCoords);
	float4 layer4 = tex2D(samp4, input.TexCoords);

	// Paint Mask
	float4 mask = tex2D(sampPaintMask, input.TexCoords);

	float4 final = (layer1 * mask.r) + (layer2 * mask.g) + (layer3 * mask.b) + (layer4 * mask.a);

	// Cursor display on the terrain
	if (input.dist < CursorSize)
	{
		// Show the cursor on the ground
		return float4(final.r + 0.2, 0, 0, 1);
	}

	// Return our final blended texture with our diffused lighting applied
	return final * f1;
}

technique Solid
{
  pass Pass1
  {
    CullMode = NONE;
    FillMode = SOLID;

    VertexShader = compile vs_2_0 VertexShaderFunction();
    PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}

technique WireFrame
{
  pass Pass1
  {
    CullMode = NONE;
    FillMode = WIREFRAME;

    VertexShader = compile vs_2_0 VertexShaderFunction();
    PixelShader = compile ps_2_0 PixelShaderFunction();
  }
}