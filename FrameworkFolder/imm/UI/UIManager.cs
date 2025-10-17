using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using imm.Core;
using imm.Commands;

namespace imm.UI
{
	/*public enum SceneName
	{
		Loading,
		Room,
		House,
		Social,
		Roof,
		GameHouse,
		Fashion,
		HomeDepot,
		Furniture,
		MakeOver,
		Collectible,
		Splash,
		SSLoadingScene,
		SmashTheSpider,
		FWLoadingScene,
		FliingWelo,
		WJLoadingScene,
		WeloJump,
		Map,
		Story,
		Egg,
		Customization
	}*/

	public interface UIManagerHandler
	{
		UIControllerStrategyFactory GetStrategyFactory();
	}
	
	public static class UIManager
	{
		private static bool IsUseLoader
		{
			get
			{
				return LoadingSceneType != null;
			}
		}

		private static Stack<object[]> _args;
		private static Stack<Type> _sceneStack;
		//private static Stack<object> _popUpResult;
		private static Type _currentSceneType;
		private static UISceneController _currentSceneController;
		
		public static event System.Action SceneLoaded;
		public static event System.Action SceneUnloaded;

		private static Stack<UIController> _controllers;
		
		private static readonly List<UIManagerHandler> _handlers;
		
		public static Type LoadingSceneType;
		
		static UIManager()
		{
			_args = new Stack<object[]>();
			_sceneStack = new Stack<Type>();
			_controllers = new Stack<UIController>();
			_handlers = new List<UIManagerHandler>();	
		}
					
		public static void AddHandler( UIManagerHandler handler )
		{
			_handlers.Add( handler );
		}
		
		internal static void RegisterController(UIController controller)
		{
			_controllers.Push( controller );
			
			if(controller is UISceneController)
			{
				_currentSceneController = (UISceneController)controller;
				
				if(SceneLoaded != null)
					SceneLoaded();
			}
		}
		
		internal static void UnregisterController()
		{
			if(_controllers.Count > 0)
            {
				 _controllers.Pop();
			}


		}
		
		public static UISceneController CurrentSceneController
		{
			get
			{
				return _currentSceneController;
			}
		}
		
		public static UIControllerStrategyFactory StrategyFactory
		{
			get
			{
				UIControllerStrategyFactory factory = null;
				
				foreach(UIManagerHandler handler in _handlers)
				{
					factory = handler.GetStrategyFactory();
					
					if(factory != null)
						break; 
				}
				
				return factory;
			}
		}
		
		public static Type CurrentSceneType
		{
			get
			{
				return _currentSceneType;
			}
		}
		
		public static object[] PollArgs(UIController controller)
		{
			if(controller is UISceneController)
			{
				_currentSceneController = (UISceneController)controller;				
			}
						
			if(_args.Count > 0)
				return _args.Pop();
			
			return new object[]{};
		}
		
		private static T GetAttribute<T>(Type type)
			where T:Attribute
		{
			object[] attributes = type.GetCustomAttributes(true);
			
			foreach(object attribute in attributes)
			{
				if(attribute is T)
				{
					return attribute as T;
				}
			}
			
			return null;
		}
		
		
		public static AsyncOperation Load(Type type, object[] args)
		{
			if(SceneUnloaded != null)
				SceneUnloaded();
			
			_currentSceneController = null;
			_controllers.Clear();
			
			List<object> loading_args = new List<object>(args);

			if(_currentSceneType == LoadingSceneType || !IsUseLoader)
			{
				_currentSceneType = type;

				if(IsUseLoader && loading_args.Count > 0)
					loading_args.RemoveAt(0);
				
				_args.Push(loading_args.ToArray());				
			}

			else
			{
				_currentSceneType = LoadingSceneType;
				
				loading_args.Insert(0,type);
				
				_args.Push(loading_args.ToArray());				
			}		
						
			UISceneNameAttribute sceneName = GetAttribute<UISceneNameAttribute>(_currentSceneType);			
			
			if(sceneName == null)
				throw new Exception(string.Format("Scene name for type {0} not defined. Use UISceneNameAttribute", _currentSceneType));

			return SceneManager.LoadSceneAsync(sceneName.Name);		
		}
		
		public static AsyncOperation Load<T>(params object[] args)
			where T:UISceneController
		{
			return Load(typeof(T),args);
		}
	
		public static void ClosePopUp(UIPopUpSceneController controller)
		{
			Load(_sceneStack.Pop(), _args.Pop());
		}

		public static void OpenPopUpScene<T>(UIController parentController)
			where T:UIPopUpSceneController
		{
			OpenPopUpScene<T>(parentController, new object[]{});	
		}
		
		public static void OpenPopUpScene<T>(UIController parentController, params object[] args)
			where T:UIPopUpSceneController
		{
			OpenPopUpScene(typeof(T),parentController, args);
		}
		
		private static void OpenPopUpScene(Type type, UIController parentController, object[] args)
		{
			_args.Push(parentController.Args);
			_sceneStack.Push(_currentSceneType);			
			
			Load(type,args);
		}		

		public static AsyncToken<PopUpOpenCommand> OpenPopUp<T>(UIController parentController,object[] args)
			where T:UIPopUpController
		{
			T popUp = parentController.GetComponentInChildren<T>(true);

			if (popUp == null) 
			{
				throw new Exception ("Unable to find " + typeof(T) + " popUp");
			}


			popUp.Parent = parentController;

            parentController.Siblings.Add(popUp);

			_args.Push (args);

			return MonoCommand.ExecuteOn<PopUpOpenCommand> (parentController.gameObject, 
				new object[]{ popUp }).AsyncToken;
				
		}
		
		public static void OpenPopUpScene<T>(params object[] args)
		{
			OpenPopUpScene(typeof(T),_currentSceneController,args);
		}

		public static void OpenPopUpScene<T>()
		{
			OpenPopUpScene<T>(new object[]{});
		}


	}
}

