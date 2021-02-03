using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TroopDataWindow : EditorWindow
{
    public TroopData data;
    int cost = 100;
    int canHit = 1;
    float range = 2.34f;
    TroopData.TroopType type = TroopData.TroopType.Melee;
    int currentLevel = 1;
    string dataName;
    
    string description;

    TroopData.TroopBlueprint blueprint;
    List<TroopData.TroopBlueprint> upgrades;
    int upgradeLength = 0;

    int lastUpgradeLength = 0;
    TroopData lastData = null;

    [MenuItem("Window/Troop Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<TroopDataWindow>("Troop Data Editor");
    }

    void ApplyToStats()
    {
        lastData = data;
        if (data == null) return;

        cost = data.cost;
        canHit = data.canHit;
        range = data.range;
        type = data.type;
        currentLevel = data.currentLevel;
        dataName = data.name;
        blueprint = data.troopBlueprint;
        upgrades = data.upgrades;
        upgradeLength = upgrades.Count;
        description = data.description;
    }

    void OnGUI()
    {
        GUILayout.Label("Edit troop data here:");

        data = (TroopData)EditorGUILayout.ObjectField("Troop Data", data, typeof(TroopData), false);

        if (data == null) return;

        if (data != lastData)
        {
            ApplyToStats();
        }

        dataName = EditorGUILayout.TextField("name", dataName);

        GUILayout.Label("Description");
        description = GUILayout.TextArea(description);
        
        cost = EditorGUILayout.IntField("Cost", cost);
        canHit = EditorGUILayout.IntField("Can Hit", canHit);
        range = EditorGUILayout.FloatField("Range", range);
        type = (TroopData.TroopType)EditorGUILayout.EnumFlagsField("Troop Type", type);

        blueprint.icon = (Sprite)EditorGUILayout.ObjectField("Icon", blueprint.icon, typeof(Sprite), false);
        blueprint.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", blueprint.prefab, typeof(GameObject), false);
        blueprint.health = EditorGUILayout.FloatField("Health", blueprint.health);
        blueprint.damage = EditorGUILayout.FloatField("Damage", blueprint.damage);
        blueprint.hitSpeed = EditorGUILayout.FloatField("Hit Speed", blueprint.hitSpeed);
        blueprint.moveSpeed = EditorGUILayout.FloatField("Move Speed", blueprint.moveSpeed);

        if (type != TroopData.TroopType.Epic)
        {
            upgradeLength = EditorGUILayout.IntField("Upgrades Length", upgradeLength);

            if (upgradeLength < 0) upgradeLength = 0;

            if (upgradeLength != lastUpgradeLength)
            {
                lastUpgradeLength = upgradeLength;

                if (upgradeLength < data.upgrades.Count)
                {
                    for (int i = upgradeLength; i < data.upgrades.Count;) data.upgrades.RemoveAt(i);
                }
                else
                {
                    for (int i = data.upgrades.Count; i < upgradeLength; i++) data.upgrades.Add(new TroopData.TroopBlueprint());
                }
            }

            for (int i = 0; i < upgradeLength; i++)
            {
                //GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Label("Upgrade " + i + " : ");

                upgrades[i].icon = (Sprite)EditorGUILayout.ObjectField("Icon", upgrades[i].icon, typeof(Sprite), false);
                upgrades[i].prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", upgrades[i].prefab, typeof(GameObject), false);
                upgrades[i].health = EditorGUILayout.FloatField("Health", upgrades[i].health);
                upgrades[i].damage = EditorGUILayout.FloatField("Damage", upgrades[i].damage);
                upgrades[i].hitSpeed = EditorGUILayout.FloatField("Hit Speed", upgrades[i].hitSpeed);
                upgrades[i].moveSpeed = EditorGUILayout.FloatField("Move Speed", upgrades[i].moveSpeed);
                upgrades[i].cost = EditorGUILayout.IntField("Upgrade Cost", upgrades[i].cost);

                //GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
        }

        if (GUILayout.Button("Apply"))
        {
            ApplyToTroopData();
        }
    }

    void ApplyToTroopData()
    {
        if (data == null) return;

        data.name = dataName;
        data.cost = cost;
        data.canHit = canHit;
        data.range = range;
        data.currentLevel = currentLevel;
        data.troopBlueprint = blueprint;
        data.upgrades = upgrades;
        data.type = type;
        data.description = description;
    }
}
