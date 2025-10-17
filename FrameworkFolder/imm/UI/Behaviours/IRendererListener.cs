using UnityEngine;
using System.Collections;

namespace imm.UI.Behaviours
{
	public interface IRendererListener 
	{
		 void OnBecameVisible();
	
		 void OnBecameInvisible();
	}
}
