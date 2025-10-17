using System;

namespace imm.UI
{
	public abstract class UIPopUpController:UIController
	{
		public event Action<UIPopUpController> Closed;

		internal UIController Parent;

		protected override void Awake()
		{
			
		}

		protected override void Start()
		{
			
		}

		public void Show()
		{			
			gameObject.SetActive (true);

			_args = UIManager.PollArgs(this);
				
			DoStart();
		}

		public void Close()
		{
			if (Closed != null)
				Closed (this);

			OnClose ();
		}

		protected virtual void OnClose()
		{
			UIManager.UnregisterController();

            this.Parent.Siblings.Remove(this);
			
			this.gameObject.SetActive (false);
		}
	}
}

