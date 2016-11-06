using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class MenuOption
	{
		public Rect button;
		public string text;
		public Func<bool> UIDisplay;
		public float UIHeight;
		public float UIWidth;
		public bool open;
		public Vector2 scrollPos;

		public MenuOption (string text, float UIWidth, float UIHeight, Func<bool> UIDisplay)
		{
			this.text = text;
			this.UIHeight = UIHeight;
			this.UIWidth = UIWidth;
			this.UIDisplay = UIDisplay;
			this.open = false;
			this.scrollPos = new Vector2 (1, 1);
		}

		public bool isMouseOver(){
			return button.Contains (new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) || (open ? new Rect (button.x, button.y + button.height, UIWidth, UIHeight + 2f).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) : false);
		}
	}
}