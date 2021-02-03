using UnityEngine;
using UnityEditor;

public class ArmyLevelEditorWindow : EditorWindow
{
    TroopData currentTroop = null;

    Army army;
    public static LayerMask interactableMask;
    int level = 1;
    
    [MenuItem("Window/Army Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<ArmyLevelEditorWindow>("Army Level Editor");

        interactableMask.value = LayerMask.GetMask(new string[] { "Ground", "Troop" });
    }

    void OnGUI()
    {
        army = (Army)EditorGUILayout.ObjectField("Army", army, typeof(Army), true);
        currentTroop = (TroopData)EditorGUILayout.ObjectField("Troop", currentTroop, typeof(TroopData), false);

        if (army == null) return;

        string troopName = "nobody";

        if (currentTroop != null)
        {
            troopName = currentTroop.name;

            GUILayout.Label("Selected Troop is " + troopName);

            level = EditorGUILayout.IntField("Troop Level", level);

            level = Mathf.Clamp(level, 1, currentTroop.upgrades.Count + 1);
        }

        OnSceneGUI();
    }

    void OnSceneGUI()
    {
        if(currentTroop != null)
        if (GUILayout.Button("Spawn Troop"))
        {
            army.OnClicked(army.transform.position, currentTroop, level);
        }

        if(GUILayout.Button("Re-Apply Troops"))
        {
            for (int i = army.troops.Count - 1; i >= 0; i--) army.troops[i].ReApply();

            army.Apply();
        }
    }
}
