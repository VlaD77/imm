using UnityEngine;
using System.Collections;
using imm.Core;
using System;

namespace imm.Commands
{
	public sealed class ApplyStateInCommand : StateCommand 
	{
		Type _type;
		StateMachine _sm;

		protected override void OnStart (object[] args)
		{
			base.OnStart (args);

			_type = (Type)args[1];
			_sm = (StateMachine)args[0];

			ScheduleUpdate((float)args[2],false);
		}

		protected override void OnScheduledUpdate ()
		{
			base.OnScheduledUpdate ();

			_sm.ApplyState(_type);
		}

	}
}