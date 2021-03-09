using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Catalyst3D.XNA.Engine.EntityClasses.Models.ClipPlayer
{
	public class CTClipPlayer
	{
		public Matrix[] _BoneTransforms, _SkinTransforms, _WorldTransforms;
		public CTAnimationClip3D _CurrentClip;
		public IList<CTKeyframe3D> _KeyframeList;
		public CTSkinningData _SkinData;
		public float _FPS;
		public TimeSpan _StartTime, _EndTime, _CurrentTime;
		public TimeSpan _StartTimeSwitch, _EndTimeSwitch;
		public bool _IsSwitching;
		public bool _IsLooping;
		public float _Blend;

		public Matrix[] GetSkinTransforms()
		{
			return _SkinTransforms;
		}
		public Matrix GetWorldTransform(int id)
		{
			return _WorldTransforms[id];
		}

		public CTClipPlayer(CTSkinningData skd, float fps)
		{
			_SkinData = skd;
			_BoneTransforms = new Matrix[skd.BindPose.Count];
			_SkinTransforms = new Matrix[skd.BindPose.Count];
			_WorldTransforms = new Matrix[skd.BindPose.Count];
			_FPS = fps;
		}

		public void Play(CTAnimationClip3D clip, float startFrame, float endFrame, bool loop)
		{
			_CurrentClip = clip;
			_StartTime = TimeSpan.FromMilliseconds(startFrame / _FPS * 1000);
			_EndTime = TimeSpan.FromMilliseconds(endFrame / _FPS * 1000);
			_CurrentTime = _StartTime;
			_IsLooping = loop;
			_KeyframeList = _CurrentClip.Keyframes;
		}

		public void switchRange(float s, float e)
		{
			_IsSwitching = true;
			_StartTimeSwitch = TimeSpan.FromMilliseconds(s / _FPS * 1000);
			_EndTimeSwitch = TimeSpan.FromMilliseconds(e / _FPS * 1000);
		}

		public bool inRange(float s, float e)
		{
			TimeSpan sRange = TimeSpan.FromMilliseconds(s/_FPS*1000);
			TimeSpan eRange = TimeSpan.FromMilliseconds(e/_FPS*1000);
			if (_CurrentTime >= sRange && _CurrentTime <= eRange)
				return true;
			
			return false;
		}

		public Matrix[] GetTransformsFromTime(TimeSpan ts)
		{
			Matrix[] xforms = new Matrix[_SkinData.BindPose.Count];
			_SkinData.BindPose.CopyTo(xforms, 0);
			int keyNum = 0;
			while (keyNum < _KeyframeList.Count)
			{
				CTKeyframe3D key = _KeyframeList[keyNum];
				if (key.Time > ts) break;
				xforms[key.Bone] = key.Transform;
				keyNum++;
			}
			return xforms;
		}

		public Matrix[] GetTransformsFromTime(float a)
		{
			TimeSpan ts = TimeSpan.FromMilliseconds(a / _FPS * 1000);
			return GetTransformsFromTime(ts);
		}

		public Matrix[] BlendTransforms(Matrix[] fromTransforms,
		                                Matrix[] toTransforms)
		{
			for (int i = 0; i < fromTransforms.Length; i++)
			{
				Vector3 vt1; Vector3 vs1; Quaternion q1;
				fromTransforms[i].Decompose(out vs1, out q1, out vt1);

				Vector3 vt2; Vector3 vs2; Quaternion q2;
				toTransforms[i].Decompose(out vs2, out q2, out vt2);

				Vector3 vtBlend = Vector3.Lerp(vt1, vt2, _Blend);
				Vector3 vsBlend = Vector3.Lerp(vs1, vs2, _Blend);
				Quaternion qBlend = Quaternion.Slerp(q1, q2, _Blend);

				toTransforms[i] = Matrix.CreateScale(vsBlend) *
				                  Matrix.CreateFromQuaternion(qBlend) *
				                  Matrix.CreateTranslation(vtBlend);
			}
			return toTransforms;
		}

		public void update(TimeSpan time, bool relative, Matrix root)
		{
			if (relative)
				_CurrentTime += time;
			else
				_CurrentTime = time;

			_BoneTransforms = GetTransformsFromTime(_CurrentTime);

			if (_CurrentTime >= _EndTime)
			{
				if (_IsLooping)
					_CurrentTime = _StartTime;
				else
					_CurrentTime = _EndTime;
			}

			if (_IsSwitching)
			{
				_Blend += 0.1f;
				_BoneTransforms = BlendTransforms(_BoneTransforms,
				                                  GetTransformsFromTime(_StartTimeSwitch));
				if (_Blend > 1)
				{
					_IsSwitching = false;
					_StartTime = _StartTimeSwitch;
					_EndTime = _EndTimeSwitch;
					_CurrentTime = _StartTime;
					_Blend = 0;
				}

			}

			_WorldTransforms[0] = _BoneTransforms[0] * root;

			//adjust the children
			for (int i = 1; i < _WorldTransforms.Length; i++)
			{
				int parent = _SkinData.SkeletonHierarchy[i];
				_WorldTransforms[i] = _BoneTransforms[i] *
				                      _WorldTransforms[parent];
			}

			//update the skins
			for (int i = 0; i < _SkinTransforms.Length; i++)
			{
				_SkinTransforms[i] = _SkinData.InverseBindPose[i] * _WorldTransforms[i];
			}
		}
	}
}