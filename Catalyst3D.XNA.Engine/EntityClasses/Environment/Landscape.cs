using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst3D.XNA.Engine.EntityClasses.Environment
{
  public class Landscape : QuadTree
  {
    private FillMode _fillMode = FillMode.Solid;
		private CullMode _cullMode = CullMode.CullCounterClockwiseFace;
    
    public event CustomShader SetShaderParameters;

		[XmlIgnore]
		public QuadTreeNode Trunk;

		[XmlIgnore]
    public EffectTechnique EffectTechnique;

		[XmlIgnore]
		public Effect CustomEffect { get; set; }

		[XmlIgnore]
    public BasicEffect BasicEffect;

		[XmlIgnore]
    public Texture2D HeightmapTexture;

		[XmlIgnore]
		public Texture2D GroundTexture;

		[XmlIgnore]
    public LandscapeBlock[][] Blocks;

		[XmlIgnore]
    public Stream HeightMapStream;

    public string HeightMap;

		[XmlIgnore]
    public float[,] HeightData;

    public int numBlocksX;
    public int numBlocksZ;

    public int Width = 128;
    public int Height = 128;

    public int BlockSize = 16;

    public float MaxHeight = 3000;
    public float Smoothness;

    public int TotalVisibleTriangles;

    public new Vector3 Position { get; set; }
		
		public new Vector3 Scale { get; set; }

    public FillMode FillMode
    {
      get { return _fillMode; }
      set { _fillMode = value; }
    }

    public CullMode CullMode
    {
      get { return _cullMode; }
      set { _cullMode = value;  }
    }

    /// <summary>
    /// Generates a Landscape which uses Quad Tree spatial partitioning
    /// </summary>
    /// <param name="game">Game Class</param>
    /// <param name="blocksize">Size of each landscape block</param>
    /// <param name="heightmap">Heightmap Asset Name</param>
    /// <param name="camera">Camera Class</param>
    public Landscape(Game game, int blocksize, string heightmap, BasicCamera camera)
      : base(game)
    {
      BlockSize = blocksize;
      HeightMap = heightmap;
      Camera = camera;
    }

    /// <summary>
    /// Generates a Landscape which uses Quad Tree spatial partitioning.
    /// </summary>
    /// <param name="game">Game Class</param>
    /// <param name="blockSize" value="int" remarks="Must be in powers of 2">Size of each block of landscape in the Qaud Tree</param>
    /// <param name="width" value="int" remarks="Must be in powers of 2">Total width of landscape</param>
    /// <param name="height" value="int" remarks="Must be in powers of 2">Total height of landscape</param>
    /// <param name="camera" value="CTBaseCamera">Camera Class</param>
    public Landscape(Game game, int blockSize, int width, int height, BasicCamera camera)
      : base(game)
    {
      Camera = camera;
      Height = height;
      Width = width;
      BlockSize = blockSize;
    }

    /// <summary>
    /// Generates a Landscape which uses Quad Tree spatial partitioning.
    /// </summary>
    /// <param name="game">Game Class</param>
    /// <param name="blockSize">Size of each block of the landscape in the Quad Tree</param>
    /// <param name="stream">Memory Stream of the Heightmap to generate</param>
    /// <param name="camera">Camera Class</param>
    public Landscape(Game game, int blockSize, Stream stream, BasicCamera camera)
      : base(game)
    {
      // Load heightmap from stream
      Camera = camera;
      BlockSize = blockSize;
      HeightMapStream = stream;
    }

    public override void LoadContent()
    {
      base.LoadContent();

      // Init our Basic Effect
      BasicEffect = new BasicEffect(GraphicsDevice);
      BasicEffect.EnableDefaultLighting();

      BasicEffect.DirectionalLight0.Direction = new Vector3(1, 1, 0);
      BasicEffect.DirectionalLight0.SpecularColor = Color.Tan.ToVector3();
      BasicEffect.SpecularPower = 200f;

      if (!string.IsNullOrEmpty(HeightMap))
      {
        // Load our Heightmap
        HeightmapTexture = Game.Content.Load<Texture2D>(HeightMap);

        // Store the width and height
        Width = HeightmapTexture.Width;
        Height = HeightmapTexture.Height;
      }
      else if (HeightMapStream != null)
      {
        // Load our texture up from stream
        HeightmapTexture = Texture2D.FromStream(Game.GraphicsDevice, HeightMapStream);
        HeightMapStream.Seek(0, SeekOrigin.Begin);
      }

      BuildLandscapeBlocks(HeightmapTexture);

      if (Trunk != null)
        RebuildBounding(Trunk);
    }

    public void BuildLandscapeBlocks(Texture2D heightmap)
    {
      // Calculate how many blocks we should make based off total Width / desired size of each block
      numBlocksX = Width / BlockSize;
      numBlocksZ = Height / BlockSize;

      Blocks = new LandscapeBlock[numBlocksX][];

      // create blocks
      for (int x = 0; x < numBlocksX; x++)
      {
        Blocks[x] = new LandscapeBlock[numBlocksZ];

        for (int z = 0; z < numBlocksZ; z++)
        {
          LandscapeBlock block = new LandscapeBlock(Game, Width, Height, x * BlockSize, z * BlockSize);
        	block.Position = new Vector2(x*BlockSize, z*BlockSize);
          block.Width = BlockSize;
          block.Height = BlockSize;
          block.HeightColorData = new Color[(BlockSize + 1) * (BlockSize + 1)];
          block.MaxHeight = MaxHeight;
          block.Smoothness = Smoothness;
          block.Camera = Camera;

          // Height map data provided
          if (heightmap != null)
          {
            heightmap.GetData(0,
                              new Rectangle((int)block.Position.X, (int)block.Position.Y, (BlockSize + 1), (BlockSize + 1)),
                              block.HeightColorData, 0, (BlockSize + 1) * (BlockSize + 1));
          }

          block.Initialize();

          Blocks[x][z] = block;
        }
      }

      FindNeighborBlocks();

      Trunk = BuildTreeNode(0, 0, numBlocksX, numBlocksZ) as QuadTree;
    }

    public void FindNeighborBlocks()
    {
      for (int x = 0; x < numBlocksX; x++)
      {
        for (int z = 0; z < numBlocksZ; z++)
        {
          Blocks[x][z].Top = null;
          Blocks[x][z].Bottom = null;
          Blocks[x][z].Left = null;
          Blocks[x][z].Right = null;

          if (x < (numBlocksX - 1))
          {
            // Top
            Blocks[x][z].Bottom = Blocks[x + 1][z];
          }

          if (x > 0)
          {
            // Bottom
            Blocks[x][z].Top = Blocks[x - 1][z];
          }
          if (z > 0)
          {
            // Left
            Blocks[x][z].Left = Blocks[x][z - 1];
          }

          if (z < (numBlocksZ - 1))
          {
            // Right
            Blocks[x][z].Right = Blocks[x][z + 1];
          }
        }
      }
    }

    public QuadTreeNode BuildTreeNode(int x, int z, int width, int length)
    {
      if (width == 0 || length == 0)
        return null;

      if (width == 1 && length == 1)
        return Blocks[x][z];

      int left = (int)Math.Round(width * 0.5f);
      int right = width - left;
      int top = (int)Math.Round(length * 0.5f);
      int bottom = length - top;

      QuadTree tree = new QuadTree(Game);

      tree.UpperLeft = BuildTreeNode(x, z, left, top);
      tree.UpperRight = BuildTreeNode(x + left, z, right, top);
      tree.LowerLeft = BuildTreeNode(x, z + top, left, bottom);
      tree.LowerRight = BuildTreeNode(x + left, z + top, right, bottom);

      // no point in maintaining limbs with no leafs
      if (tree.UpperLeft == null && tree.UpperRight == null &&
          tree.LowerLeft == null && tree.LowerRight == null)
      {
        tree.Dispose();
        return null;
      }

      return tree;
    }

    public void RebuildBounding(QuadTreeNode node)
    {
      Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
      Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

      Type type = node.GetType();

      if (type == typeof(QuadTree))
      {
        QuadTree tree = (QuadTree)node;

        if (tree.UpperLeft != null)
        {
          RebuildBounding(tree.UpperLeft);
          min = Vector3.Min(min, tree.UpperLeft.BoundingBox.Min);
          max = Vector3.Max(max, tree.UpperLeft.BoundingBox.Max);
        }

        if (tree.UpperRight != null)
        {
          RebuildBounding(tree.UpperRight);
          min = Vector3.Min(min, tree.UpperRight.BoundingBox.Min);
          max = Vector3.Max(max, tree.UpperRight.BoundingBox.Max);
        }

        if (tree.LowerLeft != null)
        {
          RebuildBounding(tree.LowerLeft);
          min = Vector3.Min(min, tree.LowerLeft.BoundingBox.Min);
          max = Vector3.Max(max, tree.LowerLeft.BoundingBox.Max);
        }

        if (tree.LowerRight != null)
        {
          RebuildBounding(tree.LowerRight);
          min = Vector3.Min(min, tree.LowerRight.BoundingBox.Min);
          max = Vector3.Max(max, tree.LowerRight.BoundingBox.Max);
        }
      }
      else if (type == typeof(LandscapeBlock))
      {
        LandscapeBlock block = (LandscapeBlock)node;
        min = Vector3.Min(min, block.BoundingBox.Min);
        max = Vector3.Max(max, block.BoundingBox.Max);
      }

      node.BoundingBox = new BoundingBox(min, max);
    }

    public override void Update(GameTime gameTime)
    {
      World = Matrix.CreateTranslation(Position);

      if (Trunk != null)
        UpdateTreeNode(Camera.BoundingFrustum, Trunk, false, gameTime);

      // Setup our Shader
      if (SetShaderParameters != null)
        SetShaderParameters.Invoke(gameTime);

      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);

      TotalVisibleTriangles = 0;

      if (!Enabled || !Visible)
        return;

      World = Matrix.CreateScale(new Vector3(Scale.X, 0, Scale.Y)) *
              Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z));

      if (CustomEffect != null)
      {
        foreach (EffectPass p in CustomEffect.CurrentTechnique.Passes)
        {
          p.Apply();

          TotalVisibleTriangles += DrawTreeNode(Camera.BoundingFrustum, Trunk, gameTime);
        }
      }
      else
      {
        RasterizerState state = new RasterizerState();
        state.FillMode = FillMode;
        state.CullMode = CullMode;
        GraphicsDevice.RasterizerState = state;

        BasicEffect.World = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0));
        BasicEffect.Projection = Camera.Projection;
        BasicEffect.View = Camera.View;

        if (GroundTexture != null && !GroundTexture.IsDisposed)
        {
          BasicEffect.Texture = GroundTexture;
          BasicEffect.TextureEnabled = true;
        }

        foreach (EffectPass p in BasicEffect.CurrentTechnique.Passes)
        {
          p.Apply();

          TotalVisibleTriangles += DrawTreeNode(Camera.BoundingFrustum, Trunk, gameTime);
        }
      }
    }

    public void UpdateTreeNode(BoundingFrustum frustum, QuadTreeNode node, bool skipFrustumCheck, GameTime gameTime)
    {
      node.Visible = false;

      if (skipFrustumCheck != true)
      {
        ContainmentType containment = frustum.Contains(node.BoundingBox);

        if (containment == ContainmentType.Disjoint)
          return;

        // if the entire node is contained within, then assume all children are as well
        if (containment == ContainmentType.Contains)
          skipFrustumCheck = true;
      }

      node.Visible = true;

      Type type = node.GetType();

      if (type == typeof(QuadTree))
      {
        QuadTree tree = (QuadTree)node;

        if (tree.UpperLeft != null)
        {
          UpdateTreeNode(frustum, tree.UpperLeft, skipFrustumCheck, gameTime);
        }

        if (tree.UpperRight != null)
        {
          UpdateTreeNode(frustum, tree.UpperRight, skipFrustumCheck, gameTime);
        }

        if (tree.LowerLeft != null)
        {
          UpdateTreeNode(frustum, tree.LowerLeft, skipFrustumCheck, gameTime);
        }

        if (tree.LowerRight != null)
        {
          UpdateTreeNode(frustum, tree.LowerRight, skipFrustumCheck, gameTime);
        }
      }
      else if (type == typeof(LandscapeBlock))
      {
        LandscapeBlock block = (LandscapeBlock)node;
        {
					if(block.BoundingBox.Intersects(frustum))
					{
						block.Visible = true;
						block.Update(gameTime);
					}
					else
					{
						block.Visible = false;
					}
        }
      }
    }

    public int DrawTreeNode(BoundingFrustum frustum, QuadTreeNode node, GameTime gameTime)
    {
      int totalTriangles = 0;

      if (node != null && node.Visible != true)
        return totalTriangles;

      if (node is QuadTree)
      {
        QuadTree tree = (QuadTree)node;

        if (!tree.BoundingBox.Intersects(frustum))
          return 0;

        if (tree.UpperLeft != null)
        {
          if (tree.UpperLeft.BoundingBox.Intersects(frustum))
            totalTriangles += DrawTreeNode(frustum, tree.UpperLeft, gameTime);
        }

        if (tree.UpperRight != null)
        {
          if (tree.UpperRight.BoundingBox.Intersects(frustum))
            totalTriangles += DrawTreeNode(frustum, tree.UpperRight, gameTime);
        }

        if (tree.LowerLeft != null)
        {
          if (tree.LowerLeft.BoundingBox.Intersects(frustum))
            totalTriangles += DrawTreeNode(frustum, tree.LowerLeft, gameTime);
        }

        if (tree.LowerRight != null)
        {
          if (tree.LowerRight.BoundingBox.Intersects(frustum))
            totalTriangles += DrawTreeNode(frustum, tree.LowerRight, gameTime);
        }
      }
      else if (node is LandscapeBlock)
      {
        LandscapeBlock block = (LandscapeBlock) node;
        {
          if(block.Visible)
          {
            if (block.BoundingBox.Intersects(frustum))
            {
              totalTriangles += block.DrawBlock(gameTime);
            }
          }
        }
      }

      return totalTriangles;
    }

    public List<LandscapeBlock> Intersection(Ray ray)
    {
      var blocks = new List<LandscapeBlock>();

      for (int x = 0; x < Blocks.Length; x++)
      {
        for (int z = 0; z < Blocks[x].Length; z++)
        {
          // If the bound box on this block is even visible by the camera and intersects our ray then add it to the <T> collection
          if (Blocks[x][z].BoundingBox.Intersects(Camera.BoundingFrustum) && Blocks[x][z].Intersects(ray))
          {
            blocks.Add(Blocks[x][z]);
          }
        }
      }

      return blocks;
    }

    public List<LandscapeBlock> Intersection(BoundingBox box)
    {
      var blocks = new List<LandscapeBlock>();

      for (int x = 0; x < Blocks.Length; x++)
      {
        for (int z = 0; z < Blocks[x].Length; z++)
        {
          // If the block is even visible by the camera and is flagged visible then check it
          if (Blocks[x][z].BoundingBox.Intersects(Camera.BoundingFrustum) && Blocks[x][z].Visible)
          {
            // If the block also intersects our bounding box we are testing against then add it to the <T> collection
            if (Blocks[x][z].BoundingBox.Intersects(box))
            {
              blocks.Add(Blocks[x][z]);
            }
          }
        }
      }

      return blocks;
    }

    //protected override void Dispose(bool disposing)
    //{
    //  lock (this)
    //  {
    //    if (disposing)
    //    {
    //      if (Blocks != null)
    //      {
    //        for (int x = 0; x < numBlocksX; x++)
    //        {
    //          for (int z = 0; z < numBlocksZ; z++)
    //          {
    //            Blocks[x][z].Dispose();
    //          }
    //        }
    //      }
    //    }

    //    GroundTexture = null;
    //    HeightData = null;
    //    Blocks = null;
    //  }
    //}
  }
}