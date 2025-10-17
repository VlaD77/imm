using System;
using imm.Core;
using imm.UI;

namespace imm.Commands
{
	public sealed class PopUpOpenCommand:MonoCommand<PopUpOpenCommand>
	{
		public UIPopUpController Controller { get; private set;}

		public PopUpOpenCommand ()
		{
		}

		protected override void OnStart (object[] args)
		{
			Controller = (UIPopUpController)args [0];

			Controller.Closed += OnClosed;

			Controller.Show();
		}

		void OnClosed (UIPopUpController popUp)
		{
			Controller.Closed -= OnClosed;

			FinishCommand ();
		}

	}
}

