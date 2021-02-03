using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Army : MonoBehaviour
{
    public static int limit = int.MaxValue;
    public int cost;
    public bool ready = false;
    public List<Troop> troops = new List<Troop>();
    public PlaySide armySide;
    public int maxAmount = 150;

    [HideInInspector] public GameObject blueUI, redUI, ui;
    [HideInInspector] public Text bluePrice, redPrice, blueAmount, redAmount;
    
    public static Army red, blue;

    Text PriceText { get { return armySide == PlaySide.Blue ? bluePrice : redPrice; } }
    Text AmountText { get { return armySide == PlaySide.Blue ? blueAmount : redAmount; } }

    public void RemoveTroop(Troop troop)
    {
        troops.Remove(troop);
        cost -= troop.data.cost;

        UpdateUI();
    }

    public void Apply()
    {
        troops = new List<Troop>();

        Troop[] troopArr = GetComponentsInChildren<Troop>();

        cost = 0;

        foreach (Troop troop in troopArr)
        {
            troops.Add(troop);
            troop.side = armySide;
            troop.army = this;
            cost += troop.data.cost;
        }
    }

    void Start()
    {
        if(armySide == PlaySide.Red)
        {
            if(GameMaster.levelType == LevelType.Actual)
            {
                Ready();
            }
            else
            {
                blueUI.SetActive(false);
                redUI.SetActive(true);
            }
        }

        UpdateUI();
    }

    public void Begin()
    {
        foreach (Troop troop in troops)
        {
            troop.Begin();
        }

        InvokeRepeating("EndCheck", 3, 3);
    }

    void EndCheck()
    {
        if (troops.Count == 0) GameMaster.instance.EndGame(this);
    }

    public void Ready()
    {
        if (troops.Count == 0) return;

        ready = true;

        if(ui != null)
        Destroy(ui);

        if (blue.ready && red.ready)
        {
            GameMaster.instance.Begin();
        }
    }

    string CostLimit { get { return limit < int.MaxValue ? "/" + limit : ""; } }

    public void UpdateUI()
    {
        if (PriceText != null)
            PriceText.text = cost + CostLimit;

        if (AmountText != null)
            AmountText.text = troops.Count + "/" + maxAmount;
    }
    
    public void Clear()
    {
        foreach (Troop troop in troops) { Destroy(troop.gameObject); }

        troops.Clear();
        cost = 0;

        UpdateUI();
    }

    public void OnClicked(Vector3 point, TroopData data = null, int level = 0)
    {
        if (data == null)
        {
            if (ready) return;
            if (TroopSpawner.instance.selectedTroop == null) return;
            if (TroopSpawner.instance.selectedTroop.cost + cost > limit) return;
            if (troops.Count == maxAmount) return;
        }

        cost += data == null ? TroopSpawner.instance.selectedTroop.cost : data.cost;
        Troop troop = Instantiate(data == null ? TroopSpawner.instance.selectedTroop.CurrentBlueprint.prefab : (level == 0 ? data.CurrentBlueprint : data.LevelBlueprint(level)).prefab, point, transform.rotation, transform).GetComponent<Troop>();

        troop.side = armySide;
        troop.army = this;
        troops.Add(troop);

        UpdateUI();
    }
}