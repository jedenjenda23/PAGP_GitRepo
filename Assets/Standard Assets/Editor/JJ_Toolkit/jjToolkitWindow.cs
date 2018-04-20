using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class jjToolkitWindow : EditorWindow
{
    [MenuItem("JJToolkit/JJ Toolkit Window")]
    public static void ShowTab()
    {
        EditorWindow.GetWindow(typeof(jjToolkitWindow));
    }

    private void OnGUI()
    {
            if (GUILayout.Button("Group Objects"))
            {
            jjGroupObjects.GroupObject();
            }

            if (GUILayout.Button("SetParent"))
            {

            }

        Repaint();
    }
}
