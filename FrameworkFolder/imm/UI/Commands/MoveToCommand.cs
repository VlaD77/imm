using System;
using UnityEngine;
using imm.Core;

namespace imm.UI.Commands
{

	public interface IMoveToCommandSpeed
	{
		float MoveToCommandSpeed
		{
			get;
		}
	}

	public sealed class MoveToCommand:StateCommand
	{
		public sealed class Parameters 
		{			
			public readonly Transform Target;
			public readonly Transform Destination;
			public readonly IMoveToCommandSpeed Speed;
			public readonly Vector3 DestinationVector;
			
			public Parameters (Transform target, Transform destination, IMoveToCommandSpeed speed)
			{
				this.Target = target;
				this.Destination = destination;
				this.Speed = speed;
			}
			
			public Parameters (Transform target, Vector3 destination, IMoveToCommandSpeed speed)
			{
				this.Target = target;
				this.DestinationVector = destination;
				this.Speed = speed;
			}

			public Parameters (Transform destination, IMoveToCommandSpeed speed)
			{
				this.Target = null;
				this.Destination = destination;
				this.Speed = speed;
			}
			
			public Parameters (Vector3 destination, IMoveToCommandSpeed speed)
			{
				this.Target = null;
				this.DestinationVector = destination;
				this.Speed = speed;
			}
		}
		
		private Parameters _parameters; 

		public Parameters Params
		{
			get
			{
				return _parameters;
			}
		}

		private Transform _target;
		
		public MoveToCommand ()
		{
		}
		
		
		protected override void OnStart (object[] args)
		{
			_parameters = (Parameters)args[0];

			_target = _parameters.Target;

			if(_target == null)
				_target = this.CachedTransform;
		}
		
		
		protected override void OnUpdate ()
		{
			Vector3 destination =  (_parameters.Destination!=null)?_parameters.Destination.position:_parameters.DestinationVector;

			Vector2 pos = Vector2.MoveTowards( _target.position,destination,_parameters.Speed.MoveToCommandSpeed * Time.deltaTime);
				
			_target.position = new Vector3(pos.x,pos.y, _target.position.z);
				
			if(Vector2.Distance( _target.position, destination) < 0.02f)
				FinishCommand();	

		}
	}
}

