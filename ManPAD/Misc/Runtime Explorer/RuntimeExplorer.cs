using UnityEngine;
using System.Collections.Generic;

public class RuntimeExplorer : MonoBehaviour {


    private Rect windowRect = new Rect(10, 10, Screen.width / 4, Screen.height / 2);
    private Vector2 scrollPos = Vector2.zero;

    private Vector2 orgSize = new Vector2();
    private GUIContent noContent = new GUIContent();
    private Vector3 lastClicked = Vector3.zero;
    private bool doingResize = false;

    private HashSet<ExplorerContainerType> gameObjects = new HashSet<ExplorerContainerType>();
    private HashSet<ExplorerContainerType> openGOs = new HashSet<ExplorerContainerType>();

    public static GUIStyle subSkin = new GUIStyle();

    void Awake()
    {
        
        subSkin.normal.background = makeColor(new Color(1F, 1F, 1F, .1F));

    }

    void OnGUI()
    {

        handleResize(15);
        windowRect = GUI.Window(6287, windowRect, drawExplorer, "Winduw");

    }

    void drawExplorer(int id)
    {

        if (GUILayout.Button("Refresh"))
        {

            openGOs.Clear();
            gameObjects.Clear();

            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
                if(go.transform.parent == null)
                    gameObjects.Add(new ExplorerContainerType(go));
        }
        
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        foreach(ExplorerContainerType go in gameObjects)
                handleGameObjects(go);

        GUILayout.EndScrollView();

        if(!doingResize)
            GUI.DragWindow();

    }

    void handleGameObjects(ExplorerContainerType go)
    {

        if (openGOs.Contains(go))
        {

            if (GUILayout.Button("- "+ go.go.name + " -")) openGOs.Remove(go);

            go.drawContent();

        }
        else if (GUILayout.Button("+ " + go.go.name + " +")) openGOs.Add(go);

    }

    void handleResize(int size)
    {

        Vector3 mousePos = Input.mousePosition;
        mousePos.y = Screen.height - mousePos.y;

        Rect resizeRect = new Rect(windowRect.x + windowRect.width - size, windowRect.y + windowRect.height - size, size, size);
        GUI.Box(resizeRect, noContent);

        if(Input.GetMouseButtonDown(0) && resizeRect.Contains(mousePos))
        {

            lastClicked = mousePos;
            orgSize = new Vector2(windowRect.width, windowRect.height);
            doingResize = true;

        }

        if (doingResize)
        {

            if (Input.GetMouseButton(0))
            {

                windowRect.width = orgSize.x + (mousePos.x - lastClicked.x);
                windowRect.height = orgSize.y + (mousePos.y - lastClicked.y);

            } else doingResize = false;

        }

    }

    static Texture2D makeColor(Color col)
    {

        Color[] pix = new Color[1];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(1, 1);
        result.SetPixels(pix);
        result.Apply();

        return result;

    }

}
