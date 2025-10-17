using UnityEngine;
using System.Collections;

namespace imm.UI.Behaviours
{
	public sealed class RendererProxyBehaviour : MonoBehaviour 
	{
		public IRendererListener Listener;

		void OnBecameVisible()
		{
			if(Listener != null)
				Listener.OnBecameVisible();
		}
		
		void OnBecameInvisible()
		{
			if(Listener != null)
				Listener.OnBecameInvisible();
		}
	}
}
