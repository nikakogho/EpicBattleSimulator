using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Army))]
public class ArmyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Army army = (Army)target;

        if(GameMaster.levelType == LevelType.Custom || army.armySide == PlaySide.Blue)
        {
            if(army.armySide == PlaySide.Blue)
            {
                army.blueUI = (GameObject)EditorGUILayout.ObjectField("UI", army.blueUI, typeof(GameObject), true);
                army.bluePrice = (Text)EditorGUILayout.ObjectField("Price Text", army.bluePrice, typeof(Text), true);
                army.blueAmount = (Text)EditorGUILayout.ObjectField("Amount Text", army.blueAmount, typeof(Text), true);
            }
            else
            {
                army.redUI = (GameObject)EditorGUILayout.ObjectField("UI", army.redUI, typeof(GameObject), true);
                army.redPrice = (Text)EditorGUILayout.ObjectField("Price Text", army.redPrice, typeof(Text), true);
                army.redAmount = (Text)EditorGUILayout.ObjectField("Amount Text", army.redAmount, typeof(Text), true);
            }

            army.ui = (GameObject)EditorGUILayout.ObjectField("UI", army.ui, typeof(GameObject), true);
        }

        if(army.troops.Count == 0)
        {
            if (GUILayout.Button("Apply Troops"))
            {
                army.Apply();
            }
        }
        else if (GUILayout.Button("Re-Apply Troops"))
        {
            army.troops.Clear();

            army.Apply();
        }
    }
}
