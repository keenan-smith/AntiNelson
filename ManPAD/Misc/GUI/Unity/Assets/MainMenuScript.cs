using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class MainMenuScript : MonoBehaviour {

    public GUISkin skin;



    public float UIX = 1f;
	public float UIY = 1f;
	public float ButtonHeight = 30f;
    bool kek = false;
    public float tits = 1;
    private List<MenuOption> menu = new List<MenuOption>();
	private float UISize = 0f;

	void Start(){
		UISize = Screen.height - 100f;
		float allocPos = UIX;

        menu.Add(new MenuOption("Test UI1", 200f, 250f, testUI1));
		menu.Add (new MenuOption ("Test UI2", 130f, 150f, testUI1));
		menu.Add (new MenuOption ("Test UI3", 130f, 150f, testUI1));
		menu.Add (new MenuOption ("Test UI4", 130f, 150f, testUI1));
		menu.Add (new MenuOption ("Test UI5", 130f, 150f, testUI1));
		menu.Add (new MenuOption ("Test UI6", 130f, 150f, testUI1));
		menu.Add (new MenuOption ("Test UI7", 130f, 150f, testUI1));
		menu.Add (new MenuOption ("Test UI8", 130f, 150f, testUI1));
		menu.Add (new MenuOption ("Test UI9", 130f, 150f, testUI1));

		foreach (MenuOption opt in menu) {
			Rect btnRect = new Rect (UIX + allocPos, UIY, opt.UIWidth, ButtonHeight);

			allocPos += (opt.text.Length * 16) + 2f;
			opt.button = btnRect;
			if (opt.UIHeight > UISize)
				opt.UIHeight = UISize;
		}
	}

	void Update(){
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {
			if ((new Rect (UIX, UIY, Screen.width - 1f, ButtonHeight + 1f)).Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
				if (menu [menu.Count - 1].button.x + menu [menu.Count - 1].UIWidth > Screen.width - 3f) {
					float newJump = 0f;
					for (int i = 0; i < menu.Count; i++) {
						if (menu[i].button.x + menu[i].UIWidth > Screen.width - 3f) {
							newJump = ((menu[i].button.x + menu[i].UIWidth) - Screen.width) + 3f;
							break;
						}
					}

					for (int i = 0; i < menu.Count; i++) {
						menu [i].button.x -= newJump;
					}
				}
			}
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {
			if ((new Rect (UIX, UIY, Screen.width - 1f, ButtonHeight + 1f)).Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
				if (menu [0].button.x < 3f) {
					float newJump = 0f;
					for (int i = menu.Count - 1; i > -1; i--) {
						if (menu[i].button.x < 3f) {
							newJump = -menu[i].button.x + 3f;
							break;
						}
					}


					for (int i = 0; i < menu.Count; i++) {
						menu [i].button.x += newJump;
					}
				}
			}
		}
	}

	void OnGUI(){
               GUI.skin = skin;

		GUI.Box (new Rect (UIX, UIY, Screen.width - 1f, ButtonHeight + 1f), "");
		for (int i = 0; i < menu.Count; i++) {
          //  GUI.contentColor = Color.black;

            if (menu [i].isMouseOver ()) {


              //  GUI.backgroundColor = Color.black; // Changes the background color when the button is active
                //GUI.contentColor = Color.red;
                //  GUI.te// Changes the foreground color when the button is active
            } else {

              ///  GUI.backgroundColor = Color.white; // Changes the background color when the button is inactive
              //  GUI.contentColor = Color.black; // Changes the foreground color when the button is inactive


            }
			GUI.Button (menu [i].button, menu [i].text);


            GUI.skin = skin;


    // GUI.backgroundColor = Color.white;
        //  GUI.contentColor = Color.black;
            if (menu [i].isMouseOver ()) {

				menu [i].open = true;
				GUI.Box (new Rect (menu [i].button.x, UIY + ButtonHeight + 2f, menu [i].UIWidth, menu [i].UIHeight), "");
				GUILayout.BeginArea (new Rect (menu [i].button.x, UIY + ButtonHeight + 2f, menu [i].UIWidth, menu [i].UIHeight));
				menu [i].scrollPos = GUILayout.BeginScrollView (menu [i].scrollPos);
				menu [i].UIDisplay ();
              //  GUI.contentColor = Color.red;
				GUILayout.EndScrollView ();
				GUILayout.EndArea ();
			} else {
				menu [i].open = false;

            }

        }
	}

	// UI code
	public bool testUI1(){
     
        GUI.skin = skin;
        GUILayout.Button ("Test Button 1");
		GUILayout.Label ("Test Label 1");
        kek = GUILayout.Toggle (kek, "Test Toggle 1");

		GUILayout.Button ("Test Button 2");
		GUILayout.Label ("Test Label 2");
	kek =	GUILayout.Toggle (kek, "Test Toggle 2");
        tits = GUILayout.HorizontalSlider(tits, 1, 10);
		return true;
	}
}
