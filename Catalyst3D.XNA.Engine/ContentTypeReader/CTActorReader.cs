using System.Collections.Generic;
using Catalyst3D.XNA.Engine.BaseClasses;
using Catalyst3D.XNA.Engine.EntityClasses.SpriteAnimation;
using Catalyst3D.XNA.Engine.EnumClasses;
using Catalyst3D.XNA.Studio.EntityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Catalyst3D.XNA.Engine.ContentTypeReader
{
  public class CTActorReader : ContentTypeReader<CTBaseActor>
  {
    protected override CTBaseActor Read(ContentReader input, CTBaseActor existingInstance)
    {
			/*
			-- Structure
			output.Write(value.AssetName);
			output.Write(value.AssetPath);
			output.WriteObject(value.ClipPlayer.Sequences);
			output.WriteObject(value.Role);
			output.WriteObject(value.Direction);
			output.WriteObject(value.PlayerIndex);
			*/

    	CTBaseActor actor = new CTBaseActor(existingInstance.Game);
    	actor.ClipPlayer = new CTAnimationPlayer2D(existingInstance.Game);
			
			actor.AssetName = input.ReadString();
    	actor.AssetPath = input.ReadString();
    	actor.ClipPlayer.Sequences = input.ReadObject<List<CTSequence2D>>();
    	actor.Role = input.ReadObject<ActorRole>();
    	actor.Direction = input.ReadObject<Direction>();
    	actor.PlayerIndex = input.ReadObject<PlayerIndex>();

    	actor.Initialize();

      return actor;
    }
  }
}
