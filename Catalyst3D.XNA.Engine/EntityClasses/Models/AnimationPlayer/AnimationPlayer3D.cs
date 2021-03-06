using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses.Models.AnimationPlayer
{
	public class CTAnimationPlayer3D
	{
		// Information about the currently playing animation clip.
		private CTAnimationClip3D currentClipValue;
		private TimeSpan currentTimeValue;
		private int currentKeyframe;


		// Current animation transform matrices.
		private readonly Matrix[] boneTransforms;
		private readonly Matrix[] worldTransforms;
		private readonly Matrix[] skinTransforms;


		// Backlink to the bind pose and skeleton hierarchy data.
		private readonly CTSkinningData skinningDataValue;

		/// <summary>
		/// Gets the current bone transform matrices, relative to their parent bones.
		/// </summary>
		public Matrix[] GetBoneTransforms()
		{
			return boneTransforms;
		}

		/// <summary>
		/// Gets the current bone transform matrices, in absolute format.
		/// </summary>
		public Matrix[] GetWorldTransforms()
		{
			return worldTransforms;
		}

		/// <summary>
		/// Gets the current bone transform matrices,
		/// relative to the skinning bind pose.
		/// </summary>
		public Matrix[] GetSkinTransforms()
		{
			return skinTransforms;
		}


		/// <summary>
		/// Gets the clip currently being decoded.
		/// </summary>
		public CTAnimationClip3D CurrentClip
		{
			get { return currentClipValue; }
		}

		/// <summary>
		/// Gets the current play position.
		/// </summary>
		public TimeSpan CurrentTime
		{
			get { return currentTimeValue; }
		}

		/// <summary>
		/// Constructs a new animation player.
		/// </summary>
		public CTAnimationPlayer3D(CTSkinningData skinningData)
		{
			if (skinningData == null)
				throw new ArgumentNullException("skinningData");

			skinningDataValue = skinningData;

			boneTransforms = new Matrix[skinningData.BindPose.Count];
			worldTransforms = new Matrix[skinningData.BindPose.Count];
			skinTransforms = new Matrix[skinningData.BindPose.Count];
		}


		/// <summary>
		/// Starts decoding the specified animation clip.
		/// </summary>
		public void StartClip(CTAnimationClip3D clip)
		{
			if (clip == null)
				throw new ArgumentNullException("clip");

			currentClipValue = clip;
			currentTimeValue = TimeSpan.Zero;
			currentKeyframe = 0;

			// Initialize bone transforms to the bind pose.
			skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
		}


		/// <summary>
		/// Advances the current animation position.
		/// </summary>
		public void Update(TimeSpan time, bool relativeToCurrentTime,
		                   Matrix rootTransform)
		{
			UpdateBoneTransforms(time, relativeToCurrentTime);
			UpdateWorldTransforms(rootTransform);
			UpdateSkinTransforms();
		}


		/// <summary>
		/// Helper used by the Update method to refresh the BoneTransforms data.
		/// </summary>
		public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
		{
			if (currentClipValue == null)
				throw new InvalidOperationException(
					"AnimationPlayer.Update was called before StartClip");

			// Update the animation position.
			if (relativeToCurrentTime)
			{
				time += currentTimeValue;

				// If we reached the end, loop back to the start.
				while (time >= currentClipValue.Duration)
					time -= currentClipValue.Duration;
			}

			if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
				throw new ArgumentOutOfRangeException("time");

			// If the position moved backwards, reset the keyframe index.
			if (time < currentTimeValue)
			{
				currentKeyframe = 0;
				skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
			}

			currentTimeValue = time;

			// Read keyframe matrices.
			IList<CTKeyframe3D> keyframes = currentClipValue.Keyframes;

			while (currentKeyframe < keyframes.Count)
			{
				CTKeyframe3D keyframe = keyframes[currentKeyframe];

				// Stop when we've read up to the current time position.
				if (keyframe.Time > currentTimeValue)
					break;

				// Use this keyframe.
				boneTransforms[keyframe.Bone] = keyframe.Transform;

				currentKeyframe++;
			}
		}


		/// <summary>
		/// Helper used by the Update method to refresh the WorldTransforms data.
		/// </summary>
		public void UpdateWorldTransforms(Matrix rootTransform)
		{
			// Root bone.
			worldTransforms[0] = boneTransforms[0] * rootTransform;

			// Child bones.
			for (int bone = 1; bone < worldTransforms.Length; bone++)
			{
				int parentBone = skinningDataValue.SkeletonHierarchy[bone];

				worldTransforms[bone] = boneTransforms[bone] *
				                        worldTransforms[parentBone];
			}
		}


		/// <summary>
		/// Helper used by the Update method to refresh the SkinTransforms data.
		/// </summary>
		public void UpdateSkinTransforms()
		{
			for (int bone = 0; bone < skinTransforms.Length; bone++)
			{
				skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
				                       worldTransforms[bone];
			}
		}



	}
}