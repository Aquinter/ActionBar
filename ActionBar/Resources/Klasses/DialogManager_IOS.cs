using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class DialogManager
	{
		UIAlertView dialog;

		public DialogMnager(string title, string content, firstButton, secondButton)
		{
			new UIAlertView(title, content, null, firstButton, secondButton);
		}

		public void Show()
		{
			dialog.Show();
		}
	}
}