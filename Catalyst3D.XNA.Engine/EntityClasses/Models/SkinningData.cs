using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses.Models
{
	public class CTSkinningData
	{
		private readonly IDictionary<string, CTAnimationClip3D> animationClipsValue;
		private readonly IList<Matrix> bindPoseValue;
		private readonly IList<Matrix> inverseBindPoseValue;
		private readonly IList<int> skeletonHierarchyValue;

    public IDictionary<string, CTAnimationClip3D> AnimationClips { get { return animationClipsValue; } }
    public IList<Matrix> BindPose { get { return bindPoseValue; } }
    public IList<Matrix> InverseBindPose { get { return inverseBindPoseValue; } }
    public IList<int> SkeletonHierarchy { get { return skeletonHierarchyValue; } }

		public CTSkinningData(IDictionary<string, CTAnimationClip3D> animationClips, IList<Matrix> bindPose, IList<Matrix> inverseBindPose, IList<int> skeletonHierarchy)
		{
			animationClipsValue = animationClips;
			bindPoseValue = bindPose;
			inverseBindPoseValue = inverseBindPose;
			skeletonHierarchyValue = skeletonHierarchy;
		}

		
	}
}