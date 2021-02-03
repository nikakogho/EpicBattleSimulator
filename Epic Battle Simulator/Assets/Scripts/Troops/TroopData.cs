using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Troop", menuName = "Troop")]
public class TroopData : ScriptableObject
{
    new public string name;
    public int cost;
    public float range;
    public int canHit = 1;
    public TroopType type;
    public TroopBlueprint troopBlueprint;
    [HideInInspector] public List<TroopBlueprint> upgrades = new List<TroopBlueprint>();
    [HideInInspector] public bool unlocked = false;
    [HideInInspector] public int unclockPrice;
    public int currentLevel = 1;

    [TextArea(2, 5)]
    public string description;

    public TroopBlueprint LevelBlueprint(int level)
    {
        level = Mathf.Clamp(level, 1, upgrades.Count + 1);

        return level == 1 ? troopBlueprint : upgrades[level - 2];
    }

    public TroopBlueprint CurrentBlueprint { get { return LevelBlueprint(currentLevel); } }
    public TroopBlueprint NextBlueprint { get { return currentLevel < upgrades.Count + 1 ? LevelBlueprint(currentLevel + 1) : LevelBlueprint(currentLevel); } }
    public TroopBlueprint LastBlueprint { get { return upgrades.Count == 0 ? troopBlueprint : upgrades[upgrades.Count - 1]; } }

    public enum TroopType { Melee, Range, Cavalry, Heavy, Special, Epic }

    [System.Serializable]
    public class TroopBlueprint
    {
        public GameObject prefab;
        public float health;
        public float damage;
        public float hitSpeed;
        public float moveSpeed;
        [HideInInspector]public int cost;
        public Sprite icon;
    }
}
