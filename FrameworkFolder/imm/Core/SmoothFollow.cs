#pragma warning disable 0649

using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour 
{
	[SerializeField]
	private Transform target;
	
	[SerializeField]
	private float smoothTime = 0.3f;
	
	private Vector2 velocity;
	
	public Vector3 offset;
	
	void Start()
	{
		offset = new Vector3(transform.position.x - target.position.x,
			transform.position.y - target.position.y,
			transform.position.z - target.position.z);
		//offset = Vector3.zero;
	}
	
	void Update () 
	{
		float positionX = Mathf.SmoothDamp( transform.position.x, target.position.x + offset.x, ref velocity.x, smoothTime);
		//float positionY = Mathf.SmoothDamp( transform.position.y, target.position.y + offset.y, ref velocity.y, smoothTime);
		//float positionY = UiModel.instance.zoomCamera;
		transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
	}
}
