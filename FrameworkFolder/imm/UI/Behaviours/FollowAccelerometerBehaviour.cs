using System;
using UnityEngine;
using imm.Core;


namespace imm.UI.Behaviours
{
	public sealed class FollowAccelerometerBehaviour:MGMonoBehaviour
	{
		public bool AxisY = false;

		public bool AxisX = true;

		public float MoveTime = 0.5f;

		public Vector2 Delta = new Vector2(0.1f,0.1f);

		private Vector2 Min;

		private Vector2 Max;

		public FollowAccelerometerBehaviour ()
		{
		}

		private void Awake()
		{
			Vector2 pos = CachedTransform.position;

			Min = pos - Delta;
			Max = pos + Delta;
		}

		private void LateUpdate()
		{
			Vector3 targetPos = CachedTransform.position;

			if(AxisX)
				targetPos.x = Mathf.Clamp(targetPos.x + (Input.acceleration.x * Time.smoothDeltaTime)*9.81f,Min.x,Max.x);

			if(AxisY)
				targetPos.y = Mathf.Clamp(targetPos.y + (Input.acceleration.y * Time.smoothDeltaTime)*9.81f,Min.y,Max.y);


			Vector2 pos = Vector2.MoveTowards(CachedTransform.position,targetPos,MoveTime * Time.smoothDeltaTime);

			CachedTransform.position = new Vector3(pos.x,pos.y,targetPos.z);

			//gameObject.MoveUpdate(pos,MoveTime);
		}
	}
}

