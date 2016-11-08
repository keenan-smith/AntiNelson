using UnityEngine;
using System.Collections;

public class OverlayScript : MonoBehaviour
{
   public GUISkin skin;

    public float UIX = 10f;
    public float UIY = 10f;

    private int menu_selected = 0;
    private string[] menu_selections = new string[] { "ESP", "Aimbot", "Triggerbot", "VFly", "Exploits", "Overlay" };
	private bool[] menu_active;

    private float menu_width = 0f;
    private float menu_height = 0f;

    private float button_height = 30f;

    void Start()
    {
		menu_active = new bool[menu_selections.Length];

        foreach (string sel in menu_selections)
            if (sel.Length * 18 > menu_width)
                menu_width = sel.Length * 18;
        menu_height = (menu_selections.Length * button_height) + (menu_selections.Length);

		for (int i = 0; i < menu_active.Length; i++)
			menu_active [i] = false;
    }

	void Update()
	{

		 if (Input.GetKeyDown (KeyCode.UpArrow))

			 if (menu_selected <= 0)
				menu_selected = menu_selections.Length -1;
			else
				menu_selected--;
		if (Input.GetKeyDown(KeyCode.DownArrow))
			if (menu_selected >= menu_selections.Length - 1)
				menu_selected = 0;
			else
				menu_selected++;
		if (Input.GetKeyDown (KeyCode.Return))
			menu_active [menu_selected] = !menu_active [menu_selected];

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            menu_active[menu_selected] = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            menu_active[menu_selected] = false;
        }

    }

    void OnGUI()
    {


		if(GUI.skin != skin)
			GUI.skin = skin;
		GUI.Box (new Rect (UIX, UIY, menu_width, menu_height), "");
        for (int i = 0; i < menu_selections.Length; i++)
        {
			if (menu_active [i]) {
				GUI.backgroundColor = Color.black; // Background color when the option is enabled
				GUI.contentColor = Color.green; // Foreground color when the option is enabled

			}
            else if (i == menu_selected)
            {

				GUI.backgroundColor = Color.black; // Background color when the option is selected
				GUI.contentColor = Color.red; // Foreground color when the option is selected

            }
            else
            {
				GUI.backgroundColor = Color.white; // Background color when the option isn't selected
				GUI.contentColor = Color.black; // Foreground color when the option isn't selected
            }
			GUI.Button(new Rect(UIX, UIY + (i * (button_height + 1)), menu_width, button_height), menu_selections[i]);


            }
    }
}
