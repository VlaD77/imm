using System;
using UnityEngine;

namespace imm.UI
{
	public interface IFactory
	{
		T CreateInstance<T>();
	}
	
	public interface IFactory<T>
	{
		T CreateInstance();
	}

	public abstract class PrefabFactory<TThis,T>:MonoBehaviour, IFactory<T>
		where TThis:new()
		where T:Component
	{

		[SerializeField]
		private GameObject _prefab;
		
		private static TThis _this;

		public static TThis Instance
		{
			get
			{
				if(_this == null)
					_this = new TThis();

				return _this;
			}
		}

		#region IFactory implementation

		public T CreateInstance ()
		{
			GameObject gameObject = (GameObject)GameObject.Instantiate( _prefab );
			
			return gameObject.GetComponent<T>();
		}

		#endregion


	}
}

