using System;

namespace imm.Core
{
	public interface Observer
	{
		 void OnObjectChanged(Observable observable);
	}
}

