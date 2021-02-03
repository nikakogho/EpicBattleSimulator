using UnityEngine.UI;
using UnityEngine;

public class MoneyUIFixer : MonoBehaviour
{
    public Text text;

    void FixedUpdate()
    {
        text.text = GameMaster.money.ToString();
    }
}
