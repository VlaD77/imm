using System;
using System.Collections.Generic;
using UnityEngine;
using imm.UI.Diagnostics;
using imm.Core;
using imm.Commands;

namespace imm.UI
{
	public sealed class UseGesterRecognizerAttribute:Attribute
	{
		
	}
	
	public sealed class UISceneNameAttribute:Attribute
	{
		public readonly String Name;
		
		public UISceneNameAttribute(string name)
		{
			this.Name = name;
		}
	}

    public abstract class UISceneController : imm.UI.UIController
    {
        private DiagnosticAttribute[] _diagnostics;
        private bool _containsDiagnostics;

        public UISceneController()
        {
            //_menuHandlers = new Dictionary<Type,UI.Menu.Menu>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            List<DiagnosticAttribute> diagnostics = new List<DiagnosticAttribute>();

            foreach (Attribute attribute in this.GetType().GetCustomAttributes(true))
            {
                if (attribute is DiagnosticAttribute)
                {
                    diagnostics.Add(attribute as DiagnosticAttribute);
                }
            }

            _diagnostics = diagnostics.ToArray();
            _containsDiagnostics = _diagnostics.Length > 0;

            //			CameraFactory.CreateInstance(CameraName.PopUp);

            //			object[] attibutes = this.GetType().GetCustomAttributes(false);
            //
            //			foreach(Attribute attribute in attibutes)
            //			{
            //				if(attribute is UseGesterRecognizerAttribute)
            //				{
            //					if(!Application.isEditor)
            //					{
            //#if UNITY_IPHONE
            //						_fingerGestures = PrefabFactory.CreateInstance<TouchScreenGestures>();
            //#elif UNITY_ANDROID
            //						_fingerGestures = PrefabFactory.CreateInstance<TouchScreenGestures>();
            //#else
            //						_fingerGestures = PrefabFactory.CreateInstance<MouseGestures>();
            //#endif
            //					}
            //					else
            //					{
            //						_fingerGestures = PrefabFactory.CreateInstance<MouseGestures>();
            //					}
            //				}
            //			}

            //			InitializeTk2d();

        }

        /*protected virtual void InitializeTk2d()
		{
			tk2dUIManager manager = (tk2dUIManager)UnityEngine.Object.FindObjectOfType(typeof(tk2dUIManager));
			
			if(manager == null)
			{
				 GameObject tk2dUIManagerGameObject = new GameObject("tk2dUIManager");
			  	 manager = tk2dUIManagerGameObject.AddComponent<tk2dUIManager>();				
			}
						
			GameObject cameraGameObject = CameraFactory.CreateInstance(CameraName.DepthMask);
			
			manager.UICamera = cameraGameObject.camera;
			
			manager.raycastLayerMask = (1 << cameraGameObject.layer);
			manager.areHoverEventsTracked = false;			
		}*/

        //		public void ToogleGesters(bool enable)
        //		{			
        //			if(_fingerGestures != null)
        //			{
        //				if(enable)
        //					_gesterSuppress--;
        //				else
        //					_gesterSuppress++;				
        //				
        //				_fingerGestures.enabled = _gesterSuppress < 1;
        //				
        //				Debug.Log("Gesters is " + _fingerGestures.enabled + " suppress count " + _gesterSuppress);
        //			}
        //			else
        //			{
        //				Debug.Log("Gesters is null");
        //			}
        //		}

        internal override void OnAfterStart()
        {
            if (_containsDiagnostics)
            {
                for (int i = 0; i < _diagnostics.Length; i++)
                {
                    _diagnostics[i].OnStart();
                }
            }

            base.OnAfterStart();

            //BuildMenu();
        }

        protected virtual void OnGUI()
        {
            if (_containsDiagnostics)
            {
                float heightPerCounter = 20f;
                float startY = Screen.height / 2 - (_diagnostics.Length * heightPerCounter) / 2;
                float x = Screen.width - 20f;

                for (int i = 0; i < _diagnostics.Length; i++)
                {
                    DiagnosticAttribute counter = _diagnostics[i];

                    counter.Style.alignment = TextAnchor.MiddleRight;

                    string text = counter.ToString();
                    float counterWidth = counter.Style.fixedWidth * text.Length;

                    GUI.Label(new Rect(x - counterWidth, startY, counterWidth, heightPerCounter), text, counter.Style);

                    startY += heightPerCounter;
                }
            }
        }

        public AsyncToken<PopUpOpenCommand> OpenPopUp<T>(params object[] args)
            where T : UIPopUpController
        {
            return UIManager.OpenPopUp<T>(this, args);
        }

        public T FindPopUp<T>()
            where T:UIPopUpController
        {
            foreach(UIController eachController in this.Siblings)
            {
                if (eachController is T)
                    return (T)eachController;                 
            }

            return null;
        }


		protected sealed override void Update ()
		{
			base.Update ();
			
			if(_containsDiagnostics)
			{
				for(int i = 0; i < _diagnostics.Length;i++)
				{
					_diagnostics[i].OnUpdate();
				}
			}
		}
		
		/*public void AddMenu(UI.Menu.Menu menu)
		{
			_menuHandlers.Add(menu.GetType(), menu);
		}
		
		public T AddMenu<T>()
			where T:UI.Menu.Menu
		{
			T menu = MenuFactory.CreateInstance<T>(this);
			AddMenu(menu);
			return menu;
		}
		
		protected virtual void BuildMenu()
		{
			CameraFactory.CreateInstance(CameraName.Menu);
			
			AddMainMenu();
			AddCustomMenu();
		}
		
		protected virtual bool  IsNeedToCreateMenu(MenuName name)
		{
			return true;
		}
		
		protected virtual void AddMainMenu()
		{
			if(IsNeedToCreateMenu(MenuName.RightTop))
				AddMenu<RightTopMenu>().Listener = new DefaultRightTopMenuListener(this);
			
			if(IsNeedToCreateMenu(MenuName.LeftTop))
				AddMenu<LeftTopMenu>().Listener = new DefaultLeftTopMenuListener(this);
		}
		
		protected virtual void AddCustomMenu()
		{
			
		}
		
		public T GetMenu<T>()
			where T:UI.Menu.Menu
		{
			return (T)_menuHandlers[typeof(T)];
		}*/
		
	}
}

