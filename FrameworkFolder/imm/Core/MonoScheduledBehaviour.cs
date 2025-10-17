using System;
using UnityEngine;

namespace imm.Core
{
	public abstract class MonoScheduledBehaviour:MGMonoBehaviour
	{
		private float _scheduleUpdateInterval;
		private float _scheduleUpdateTime;
		
		[SerializeField]
		private bool _scheduledUpdate;
		private bool _scheduleRepeat;
		public bool ignoreTimeScale;
		public MonoScheduledBehaviour ()
		{
		}
		
		protected void UnscheduleUpdate()
		{
			_scheduledUpdate = false;			
		}
		
		protected void ScheduleUpdate(float interval, bool repeat = true)
		{
			_scheduledUpdate = true;
			_scheduleUpdateInterval = interval;
			_scheduleRepeat = repeat;
			if (!ignoreTimeScale)
			{
				_scheduleUpdateTime = Time.time + _scheduleUpdateInterval;
			}
			else {
				_scheduleUpdateTime = Time.unscaledTime + _scheduleUpdateInterval;
			}
		}
		
		protected virtual void Update()
		{
			//Debug.Log(this.name+" "+Time.time +" : "+ Time.unscaledTime + "  "+Time.unscaledDeltaTime +"  un "+ Time.fixedUnscaledTime
			//	+"   "+ ((!ignoreTimeScale && Time.time > _scheduleUpdateTime) || (ignoreTimeScale && Time.unscaledTime > _scheduleUpdateTime)));


			if(_scheduledUpdate &&((!ignoreTimeScale && Time.time > _scheduleUpdateTime) ||(ignoreTimeScale && Time.unscaledTime > _scheduleUpdateTime)))
			{
				if (_scheduleRepeat)
				{
					if (!ignoreTimeScale)
					{
						_scheduleUpdateTime = Time.time + _scheduleUpdateInterval;
					}
					else {
						_scheduleUpdateTime = Time.unscaledTime + _scheduleUpdateInterval;
					}
				}
				else
					_scheduledUpdate = false;
				
				OnScheduledUpdate();						
			}	

			OnUpdate ();
		}
		
		protected virtual void OnScheduledUpdate()
		{
			
		}

		protected virtual void OnUpdate()
		{
			
		}
	}
}

