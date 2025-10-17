using imm.Core;
using UnityEngine;

namespace imm.UI.Commands
{
	public sealed class SmoothFollowCommand:StateCommand
	{		
		private float _velocity;
		private float _distance;
		private Transform _follower;
		private Transform _target;

		protected override void OnStart (object[] args)
		{
			base.OnStart (args);

			_follower = (Transform)args[0];
			_target = (Transform)args[1];

			_distance = _target.position.x;
		}

		private void LateUpdate()
		{
			float x = Mathf.SmoothDamp(_target.position.x - _distance, _follower.position.x, 
				ref _velocity, 1f,1f,Time.deltaTime);
			
			_follower.position = new Vector3 (x, _follower.position.y, _follower.position.z);
		}
	}
}

