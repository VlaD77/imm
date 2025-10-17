
using imm.Core;
using UnityEngine;

namespace imm
{
    public sealed class AppController :imm.Core.MonoSingleton<AppController>,Observer,IStateMachineContainer
    {
        public AppModel Model;

        public readonly StateMachine StateMachine;

        public AppController()
        {
            StateMachine = new StateMachine(this);
        }

        protected override void Init()
        {
            base.Init();

            try
            {
                Model = AppModel.Load(AppModel.FILE_NAME);                         
            }
            catch
            {
                Model = new AppModel
                {
                    
                   

                };
                //TODO REMO THIS its FOR TEST
                
                
               
            }
           
            Model.AddObserver(this);
        }

        public void OnObjectChanged(Observable observable)
        {
            Model.Save();
        }



        public void Next(StateCommand previousState)
        {
        }

        public GameObject GameObject => this.gameObject;

       
    }
    
}


