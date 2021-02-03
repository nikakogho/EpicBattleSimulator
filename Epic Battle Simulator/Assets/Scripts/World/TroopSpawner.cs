using UnityEngine;
using UnityEngine.UI;

public class TroopSpawner : MonoBehaviour
{
    public static TroopSpawner instance;
    public TroopData selectedTroop;

    public GameObject troopUI;

    public Text descriptionText;
    public Text healthText, rangeText, damageText, moveSpeedText, fireRateText, canHitText;

    void Awake()
    {
        instance = this;
    }

    public void SelectTroop(TroopData data)
    {
        selectedTroop = data;

        troopUI.SetActive(true);
        descriptionText.text = data.description;
        healthText.text = data.CurrentBlueprint.health.ToString();
        damageText.text = data.CurrentBlueprint.damage.ToString();
        rangeText.text = data.range.ToString();
        moveSpeedText.text = data.CurrentBlueprint.moveSpeed.ToString();
        fireRateText.text = data.CurrentBlueprint.hitSpeed.ToString();
        canHitText.text = data.canHit.ToString();
    }
}
