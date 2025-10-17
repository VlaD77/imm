using System;
using System.Collections.Generic;
using UnityEngine;
using imm.Core;
using imm.Commands;


namespace imm.UI
{
	public abstract class UIControllerStrategy
	{
		internal UIController _controller;
		
		internal void Initialize()
		{
			OnInitialize();
		}
		
		protected virtual void OnInitialize(){}
		
		public virtual bool TriggerSuppressed
		{
			get
			{
				return false;
			}
		}
		
	}
	
	public abstract class UIControllerStrategyTyped<T>:UIControllerStrategy
		where T:UIController
	{
		public T Controller
		{
			get
			{
				return (T)_controller;
			}
			set
			{
				_controller = value;
			}
		}
	}
	
	public interface UIControllerStrategyFactory
	{
		UIControllerStrategy CreateStrategy(UIController controller);
        //void WeaponSpecialHit(EnemyObstacleComponent obj);
    }
	
	public abstract class UIControllerTriggerAttribute:Attribute
	{
		public event System.Action<UIControllerTriggerAttribute> Finish;
		
		public abstract void OnControllerStart(UIController controller);
		
		protected void FinishTrigger()
		{
			if(Finish != null)
				Finish(this);
		}
	}
	
	public abstract class UIController : MonoScheduledBehaviour,UIControllerStrategyFactory
	{		

		protected object[] _args;
		
		public object[] Args
		{
			get
			{
				return _args;
			}
		}

        public List<UIController> Siblings = new List<UIController>();

		protected UIControllerStrategy _strategy;
		
		protected virtual UIControllerStrategy CreateStrategy()
		{
			return null;
		}
		
		public UIControllerStrategy CreateStrategy(UIController controller)
		{
			return CreateStrategy();
		}

		protected virtual void OnInitialize()
		{
			
		}

		protected virtual void Awake()
		{
			_args = UIManager.PollArgs(this);
			
			OnInitialize();							
		}
		
		protected override  void Start()
		{													
			DoStart();
		}

		protected virtual void DoStart()
		{
			Queue<UIControllerTriggerAttribute> triggers = new Queue<UIControllerTriggerAttribute>();
						
			foreach(Attribute attribute in this.GetType().GetCustomAttributes(true))
			{
				if(attribute is UIControllerTriggerAttribute)
					triggers.Enqueue( (UIControllerTriggerAttribute) attribute );
			}

			
			UIManager.RegisterController(this);			
			
			ProcessTriggers( triggers );
		}

		private void OnTriggersFinished()
		{
//			if (_args != null && _args.Length > 0)
				OnStart(_args);
//			else
//				OnStart();	
			
			OnAfterStart();				
		}

		private void ProcessTriggers(Queue<UIControllerTriggerAttribute> triggers)
		{			
			if(triggers.Count > 0)
			{
				UIControllerTriggerAttribute trigger = triggers.Dequeue();
				
				trigger.Finish += delegate(UIControllerTriggerAttribute obj) 
				{
					ProcessTriggers(triggers);
				};
				
				trigger.OnControllerStart( this );
			}
			else
				OnTriggersFinished();
		}
		
		
		
		protected sealed override  void OnDestroy()
		{			
			base.OnDestroy();
			
			UIManager.UnregisterController();
		}
				
		internal virtual void OnAfterStart()
		{
			
		}
		[Obsolete("Old! Use  OnStart(object[] args)",true)]
		protected virtual void OnStart() { }

		protected virtual void OnStart(object[] args) { }

		public void LoadScene<T>()
		{
			LoadScene<T>(new object[]{});
		}
		
		public void LoadScene<T>(params object[] args)
		{
			MonoLog.Log(MonoLogChannel.Debug,string.Format( "Loading scene {0}", typeof(T)));
			
			UIManager.Load(typeof(T),args);
		}

	}
	
	/*public abstract class UIController<TModel,TView>:UIController
		where TModel:new()
		where TView:UIView<UIController<TModel,TView>,TModel>
	{
		[SerializedField]
		public TModel Model;
		
		public TView View;
	}*/
}

