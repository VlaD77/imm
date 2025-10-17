using System;
using System.Collections.Generic;
using UnityEngine;
using imm.Core;

namespace imm.UI
{
	public interface IPool
	{
		MonoBehaviour CreateComponent( Vector2 pos );
		void ReleaseComponent( MonoBehaviour component );
		void Initialize(int capacity);
	}

	public class Pool<T>:IPool
		where T:MonoBehaviour
	{
		#region IPool implementation

		public MonoBehaviour CreateComponent (Vector2 pos)
		{
			return Get(pos);
		}

		public void ReleaseComponent (MonoBehaviour component)
		{
			Return((T)component);
		}

		#endregion

		private readonly Queue<T> _units;
		
		private int _capacity;

		private IFactory<T> _factory;

		public Pool (IFactory<T> factory)
		{
			_units = new Queue<T>();
			_factory = factory;
		}
		
		public int ActiveCount
		{
			get
			{
				return _capacity - _units.Count;
			}
		}
		
		public int Capacity
		{
			get
			{
				return _capacity;
			}
		}		
		
		public void Initialize(int capacity)
		{
			Transform container = new GameObject("PoolOf"+typeof(T).Name).transform;

			_capacity = capacity;
			
			for(int i = 0; i < capacity; i++)
			{
				T item = CreateInstance( i );
				
				item.transform.position = new Vector3(-100,-100,item.transform.position.z);
				item.transform.SetParent(container);
				item.gameObject.SetActive( false );

				_units.Enqueue( item );
			}
		}

		protected virtual T CreateInstance( int index )
		{
			return _factory.CreateInstance();
		}
		
		public bool IsEmpty
		{
			get
			{
				return _units.Count == 0;
			}
		}
		
		
		public T Get(Vector2 position)
		{
			if(_units.Count == 0)
				return null;

			T unit = _units.Dequeue();

			unit.transform.position = new Vector3(position.x,position.y,unit.transform.position.z);
			unit.gameObject.SetActive( true );

			return unit;
		}
				
		public void Return(T unit)
		{
			unit.gameObject.SetActive( false );
			
			if(_units.Contains(unit))
				Debug.LogWarning(" UNIT ALREADY RETURNED TO POOL");
			
			_units.Enqueue( unit );
		}
	}
}

