using UnityEngine;
using System.Collections.Generic;

public class ExplorerContainerType
{

    private HashSet<ExplorerContainerType> childObjects = new HashSet<ExplorerContainerType>();
    private HashSet<ExplorerContainerType> openGOs = new HashSet<ExplorerContainerType>();
    private HashSet<Component> components = new HashSet<Component>();

    private int depth;
    private string depthString = " ";

    public GameObject go;

    public ExplorerContainerType(GameObject go, int depth = 1)
    {

        this.go = go;
        this.depth = depth;

        for (int i = 0; i < depth; i++)
            depthString += ">";

        depthString += " ";

        foreach (Component c in go.GetComponents<Component>())
            if (c.gameObject == go || c.transform.parent == go.transform)
                components.Add(c);

        foreach (Transform t in go.GetComponentsInChildren<Transform>())
            if (t.parent == go.transform)
                childObjects.Add(new ExplorerContainerType(t.gameObject, depth + 1));

    }

    public void drawContent()
    {

        GUILayout.BeginHorizontal();
        GUILayout.Space(depth * 10);
        GUILayout.BeginVertical();

        go.SetActive(GUILayout.Toggle(go.activeSelf, "Active"));

        Rect lastRect = GUILayoutUtility.GetLastRect();

        foreach (Component c in components)
        {

            GUILayout.BeginHorizontal();

            if (c.GetType().IsSubclassOf(typeof(MonoBehaviour)))
            {
                GUILayout.Label(depthString + c.GetType() + " (Script)");
                (c as MonoBehaviour).enabled = GUILayout.Toggle((c as MonoBehaviour).enabled, "Active");
                goto skip;
            }

            if (c.GetType().IsSubclassOf(typeof(Collider)))
            {
                GUILayout.Label(depthString + c.GetType());
                (c as Collider).enabled = GUILayout.Toggle((c as Collider).enabled, "Active");
                goto skip;
            }

            if (c.GetType().IsSubclassOf(typeof(Renderer)))
            {
                GUILayout.Label(depthString + c.GetType());
                (c as Renderer).enabled = GUILayout.Toggle((c as Renderer).enabled, "Active");
                goto skip;
            }

            if (c.GetType().ToString().ToLower().IndexOf("rigidbody") != -1)
            {
                GUILayout.Label(depthString + c.GetType());
                (c as Rigidbody).useGravity = GUILayout.Toggle((c as Rigidbody).useGravity, "Gravity");
                goto skip;
            }

            if (c.GetType().ToString().ToLower().IndexOf("transform") != -1)
            {

                GUILayout.Label(depthString + c.GetType() + " (" + (c as Transform).position.ToString() + ")");
                goto skip;

            }

            GUILayout.Label(depthString + c.GetType() + "");

            skip:

            GUILayout.EndHorizontal();
            GUI.Box(GUILayoutUtility.GetLastRect(), "", RuntimeExplorer.subSkin);

        }

        foreach (ExplorerContainerType c in childObjects)
        {

            if (openGOs.Contains(c))
            {

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);

                if (GUILayout.Button(depthString + "- " + c.go.name + " -")) openGOs.Remove(c);

                GUILayout.EndHorizontal();

                c.drawContent();


            }
            else
            {

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);

                if (GUILayout.Button(depthString + "+ " + c.go.name + " +")) openGOs.Add(c);

                GUILayout.EndHorizontal();

            }



        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

    }

}
