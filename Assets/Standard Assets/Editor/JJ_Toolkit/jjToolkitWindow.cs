using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class jjToolkitWindow : EditorWindow
{
    static string outputText;

    Vector3 positionVector;
    Vector3 rotationVector;
    Vector3 scaleVector = new Vector3(1,1,1);
    Quaternion savedRotation;

    bool toggleTransformations = false;

    [MenuItem("JJToolkit/JJ Toolkit Window", priority = 0)]
    public static void ShowTab()
    {
        EditorWindow.GetWindow(typeof(jjToolkitWindow));
    }

    private void OnGUI()
    {
        //if (Selection.transforms.Length > 0) GUILayout.Label("Selected:" + Selection.transforms[Selection.transforms.Length - 1]);
        GUILayout.Box("Selection Control");

        if(GUILayout.Button("Clear Selection"))
        {
            Selection.activeTransform = null;
        }

        if (GUILayout.Button("Group Objects (Ctrl+G)"))
            {
            GroupObjects();
            }

        //////////////////
        //GUILayout.Label("");
        GUILayout.Box("Parent Operations");
        if (GUILayout.Button("Select Parent"))
        {
            SelectParent();
        }


        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Set Parent (P)"))
        {
            SetLastAsParent();
        }

        if (GUILayout.Button("Clear Parent (Shift+P)"))
        {
            ClearParent();
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Box("Transformations");

        //EDIT TRANSFORMATIONS
        toggleTransformations = EditorGUILayout.Foldout(toggleTransformations, "Edit Transforms", true);
        if(toggleTransformations)
        {
            ///////POSITION
            positionVector = EditorGUILayout.Vector3Field("Position", positionVector);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add"))
            {
                AddSetTransform(positionVector, 0, true);
            }

            if (GUILayout.Button("Get"))
            {
                positionVector = Selection.activeTransform.position;
            }

            if (GUILayout.Button("Set"))
            {
                AddSetTransform(positionVector, 0, false);
            }

            if (GUILayout.Button("Clear"))
            {
                positionVector = Vector3.zero;
            }

            EditorGUILayout.EndHorizontal();

            ///////ROTATION
            rotationVector = EditorGUILayout.Vector3Field("Rotation", rotationVector);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                AddSetTransform(rotationVector, 1, true);
            }
            if (GUILayout.Button("Get"))
            {
                savedRotation = Selection.activeTransform.localRotation;
                rotationVector = savedRotation.eulerAngles;
            }

            if (GUILayout.Button("Set"))
            {
                AddSetTransform(rotationVector, 1, false);
            }

            if (GUILayout.Button("Clear"))
            {
                rotationVector = Vector3.zero;
                savedRotation = new Quaternion(0, 0, 0, 0);
            }
            EditorGUILayout.EndHorizontal();

            ///////SCALE
            scaleVector = EditorGUILayout.Vector3Field("Scale", scaleVector);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                AddSetTransform(scaleVector, 2, true);
            }

            if (GUILayout.Button("Get"))
            {
                scaleVector = Selection.activeTransform.localScale;
            }

            if (GUILayout.Button("Set"))
            {
                AddSetTransform(scaleVector, 2, false);
            }

            if (GUILayout.Button("Clear"))
            {
                scaleVector = new Vector3(1, 1, 1);
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Box("last:" + outputText);
        Repaint();
    }
    
    [MenuItem("JJToolkit/Functions/GroupSelectedObjects %g")]
    // create group of objects
    static void GroupObjects()
    {
        Vector3 groupOrigin = FindSelectionCenter();

        GameObject groupEmpty = null;
        Undo.RegisterCreatedObjectUndo(groupEmpty = new GameObject("GROUP (size: " + Selection.transforms.Length + ")" + "(" + Selection.transforms[0].name + ")"), "" + groupEmpty);
        

        groupEmpty.transform.position = groupOrigin;

        foreach (Transform gameObj in Selection.transforms)
        {
            Undo.SetTransformParent(gameObj, groupEmpty.transform, "setParent" + gameObj);
        }

        outputText = "Created Group:" + groupEmpty.name;
    }

    static Vector3 FindSelectionCenter()
    {
        Vector3 center = new Vector3(0, 0, 0);
        float count = 0f;

        foreach (Transform obj in Selection.transforms)
        {
            center += obj.position;
            count++;
        }

        return center / count;
    }

    [MenuItem("JJToolkit/Functions/SetLastAsParent _p")]
    static void SetLastAsParent()
    {
        Undo.RecordObjects(Selection.transforms, "SetLastAsParent" + Selection.transforms[Selection.transforms.Length - 1]);
        if (Selection.transforms.Length > 1)
        {
            for (int i = 0; i < Selection.transforms.Length; i++)
            {
                Selection.transforms[i].SetParent(Selection.transforms[Selection.transforms.Length - 1]);
                //Undo.SetTransformParent(Selection.activeTransform, Selection.transforms[Selection.transforms.Length - 1], "setParent" + i);
            }

            outputText = "SetLastAsParent: " + Selection.transforms[Selection.transforms.Length - 1].name + "of " + (Selection.transforms.Length - 1) + "objects";
        }       
    }
    [MenuItem("JJToolkit/Functions/ClearParent &p")]
    static void ClearParent()
    {
        foreach (Transform obj in Selection.transforms)
        {
            //obj.SetParent(null);

            Undo.SetTransformParent(obj, null, "clearParent");

            outputText = "Clear Parent";
        }
    }
    [MenuItem("JJToolkit/Functions/SelectParent")]
    static void SelectParent()
    {
        Selection.activeTransform = Selection.activeTransform.parent;
    }
    void AddSetTransform(Vector3 newVector, int method, bool add)
    {
        if (Selection.activeTransform != null)
        {
            //pos
            if (method == 0)
            {
                foreach (Transform obj in Selection.transforms)
                {
                    Undo.RecordObject(obj, "pos" + obj);
                    if (add) obj.position += newVector;
                    else obj.position = newVector;
                }
                outputText = "EditWorldPosition";
            }
            //rot
            if (method == 1)
            {

                foreach (Transform obj in Selection.transforms)
                {
                    Undo.RecordObject(obj, "rot" + obj);
                    Vector3 myVector = obj.transform.localRotation.eulerAngles;
                    if (add)
                    {
                        obj.localEulerAngles += newVector;
                    }
                    else obj.localEulerAngles = newVector;
                }
                outputText = "EditLocalRotation";
            }
            //scale
            if (method == 2)
            {
                foreach (Transform obj in Selection.transforms)
                {
                    Undo.RecordObject(obj, "scale" + obj);
                    if (add) obj.localScale += newVector;
                    else obj.localScale = newVector;
                }
                outputText = "EditLocalScale";
            }
        }
    }
}
