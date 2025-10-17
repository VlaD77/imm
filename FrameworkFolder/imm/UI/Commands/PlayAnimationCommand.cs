using System;
using imm.Core;
using UnityEngine;
using System.Collections.Generic;

namespace imm.UI.Commands
{
	public sealed class PlayAnimationCommand:StateCommand
	{
		private Parameters _parameters;
		private int _circleCount;
		
		public sealed class Parameters
		{
			public string Name;
			public int CirclesCount;
			public bool Repeat;
			public Animation FBX;
			public bool CrossFade;
			
			public Parameters ( Animation fbx, string name, int circlesCount, bool crossFade)
			{
				this.Name = name;
				this.CirclesCount = circlesCount;
				this.Repeat = circlesCount == 0;
				this.FBX = fbx;
				this.CrossFade = crossFade;
			}
			
			public Parameters (Animation fbx, string name, bool repeat):this(fbx,name,repeat ? 0 : 1,false)
			{

			}			
			
			public Parameters (Animation fbx, string name):this(fbx,name,false)
			{
	
			}			
		}
		
		public PlayAnimationCommand ()
		{
		}
		
		protected override void OnStart (object[] args)
		{
			base.OnStart(args);
			
			_parameters = (Parameters)args[0];

			AnimationState animationState = _parameters.FBX[_parameters.Name];

			if( animationState == null )
			{
				List<string> animations = new List<string>();

				foreach(AnimationState eachState in _parameters.FBX)
				{
					if(eachState.name.StartsWith(_parameters.Name,StringComparison.OrdinalIgnoreCase))
					{
						animations.Add(eachState.name);
					}
				}

				if( animations.Count > 0 )
				{
					_parameters.Name = animations[ UnityEngine.Random.Range(0,animations.Count) ];

					animationState = _parameters.FBX[ _parameters.Name ];
				}
				else
				{
					FinishCommand();

					return;
				}
			}

			
			if(_parameters.Repeat)			
				animationState.wrapMode = UnityEngine.WrapMode.Loop;
			else
				animationState.wrapMode = UnityEngine.WrapMode.Once;

			if(_parameters.CrossFade)
			{
				_parameters.FBX.CrossFade(_parameters.Name);
			}
			else
			{
				_parameters.FBX.Play(_parameters.Name);	
			}

			if(_parameters.Repeat)
				FinishCommand();
		}
		
		protected override void OnUpdate ()
		{
			if(!_parameters.FBX.isPlaying)
			{
				_circleCount++;
				
				if(!_parameters.Repeat 
				   && _circleCount >= _parameters.CirclesCount)
				{
					FinishCommand();
				}
				else
					_parameters.FBX.Play(_parameters.Name);								
			}
		}
	}
}

