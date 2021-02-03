using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TroopData))]
public class TroopDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TroopData data = (TroopData)target;

        if(data.type != TroopData.TroopType.Epic)
        {
            int upgrades = EditorGUILayout.IntField("Upgrades", data.upgrades.Count);

            if (upgrades < 0) upgrades = 0;

            if(upgrades != data.upgrades.Count)
            {
                if(upgrades < data.upgrades.Count)
                {
                    for (int i = upgrades; i < data.upgrades.Count;) data.upgrades.RemoveAt(i);
                }
                else
                {
                    for (int i = data.upgrades.Count; i < upgrades; i++) data.upgrades.Add(new TroopData.TroopBlueprint());
                }
            }

            for(int i = 0; i < upgrades; i++)
            {
                GUILayout.Label("Upgrade " + i);

                data.upgrades[i].icon = (Sprite)EditorGUILayout.ObjectField("Icon", data.upgrades[i].icon, typeof(Sprite), false);
                data.upgrades[i].health = EditorGUILayout.FloatField("Health", data.upgrades[i].health);
                data.upgrades[i].damage = EditorGUILayout.FloatField("Damage", data.upgrades[i].damage);
                data.upgrades[i].prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", data.upgrades[i].prefab, typeof(GameObject), false);
                data.upgrades[i].hitSpeed = EditorGUILayout.FloatField("Hit Speed", data.upgrades[i].hitSpeed);
                data.upgrades[i].moveSpeed = EditorGUILayout.FloatField("Move Speed", data.upgrades[i].moveSpeed);
                data.upgrades[i].cost = EditorGUILayout.IntField("Upgrade Cost", data.upgrades[i].cost);
            }
        }
        else
        {
            data.unlocked = EditorGUILayout.Toggle("Unlocked", data.unlocked);

            data.unclockPrice = EditorGUILayout.IntField("Unlock Price", data.unclockPrice);
        }
    }
}
