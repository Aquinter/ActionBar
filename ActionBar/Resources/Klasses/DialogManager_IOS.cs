using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoTouch.UIKit;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class DialogManager
	{
		UIAlertView dialog;

		public DialogManager(string title, string content, string firstButton, string secondButton)
		{
			dialog = new UIAlertView(title, content, null, firstButton, secondButton);
		}

		public void Show()
		{
			dialog.Show();
		}
	}
}