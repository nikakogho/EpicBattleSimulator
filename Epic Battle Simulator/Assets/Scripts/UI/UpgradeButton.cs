using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButton : MonoBehaviour
{
    public TroopData troop;
    public Image image;
    public Text priceText;

    delegate void ApplyUI();
    static ApplyUI applyUI;

    private float startRecoloringAfter = 0.4f;
    private float recolorTime = 1.5f;

    private float colorCountdown;

    void Awake()
    {
        applyUI += Apply;
    }

    void Start()
    {
        Apply();
    }

    int Cost { get { return troop.NextBlueprint.cost; } }

    [ContextMenu("Apply")]
    void Apply()
    {
        if (troop != null)
        {
            image.sprite = troop.NextBlueprint.icon;
            priceText.text = troop.currentLevel == troop.upgrades.Count + 1 ? "MAX" : Cost.ToString();
        }
    }

    IEnumerator CannotUpgrade()
    {
        image.color = Color.red;
        yield return new WaitForSeconds(startRecoloringAfter);

        colorCountdown = recolorTime;
    }

    void Update()
    {
        if (colorCountdown > 0)
        {
            colorCountdown -= Time.deltaTime;
            if (colorCountdown < 0) colorCountdown = 0;
            image.color = Color.Lerp(Color.red, Color.white, (recolorTime - colorCountdown) / recolorTime);
        }
    }

    public void Upgrade()
    {
        if (MaxLevelCheck() || !HasMoneyCheck())
        {
            StartCoroutine(CannotUpgrade());
            return;
        }

        GameMaster.money -= Cost;
        troop.currentLevel++;
        PlayerPrefs.SetInt("money", GameMaster.money);

        GameMaster.Save();

        applyUI.Invoke();
    }

    bool HasMoneyCheck()
    {
        return GameMaster.money >= Cost;
    }

    bool MaxLevelCheck()
    {
        if (troop.currentLevel == troop.upgrades.Count + 1)
        {
            priceText.text = "MAX";
            return true;
        }

        return false;
    }
}
