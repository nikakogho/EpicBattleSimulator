using UnityEditor;
using UnityEngine;

public class ButtonEditorWindow : EditorWindow
{
    [MenuItem("Window/Button Editor")]
    public static void ShowWindow()
    {
        GetWindow<ButtonEditorWindow>("Button Editor");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Apply"))
        {
            foreach(GameObject gameObject in Selection.gameObjects)
            {
                ButtonEditor buttonEditor = gameObject.GetComponent<ButtonEditor>();

                if(buttonEditor != null)
                {
                    buttonEditor.Apply();
                }
            }
        }
    }
}
