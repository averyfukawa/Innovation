using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WordLibrary))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WordLibrary wordLib = (WordLibrary)target;
        if(GUILayout.Button("Compile words"))
        {
            wordLib.CompileLibrary();
        }
    }
}
