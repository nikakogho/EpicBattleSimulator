using UnityEngine;
using UnityEngine.UI;

public class ButtonEditor : MonoBehaviour
{
    public Image image;
    public Text text;
    public TroopData data;

    public void Select()
    {
        TroopSpawner.instance.SelectTroop(data);
    }

    void Awake()
    {
        Apply();
    }

    [ContextMenu("Apply")]
    public void Apply()
    {
        if (data == null) return;

        name = data.name;
        image.sprite = data.CurrentBlueprint.icon;
        text.text = "$" + data.cost;
        text.resizeTextMaxSize = 50;
        text.resizeTextForBestFit = true;
        text.alignment = TextAnchor.LowerRight;
        text.color = Color.white;
    }
}
