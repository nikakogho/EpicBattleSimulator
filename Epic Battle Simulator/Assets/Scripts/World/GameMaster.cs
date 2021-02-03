using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public string menuSceneName;
    public static LevelType levelType = LevelType.Actual;
    public int level = 1;
    private int maxLevel;
    public Level[] levels;
    public GameObject armyPrefab;
    public Army blueArmy, redArmy;
    public bool began = false;
    public GameObject winUI;
    public Text winText;
    private ClickType clickType = ClickType.None;
    public LayerMask touchMask;
    private Vector3? lastClickPoint = null;
    public float minimumClickDelta = 0.5f;
    Camera cam;
    public static int money = 0;
    Level CurrentLevel { get { return levels[level - 1]; } }

    public GameObject troopUI, showUI;

    public TroopData[] allTroops;
    
    public GameObject redTroopBelowSign, blueTroopBelowSign;

    [Header("Level UI")]
    public GameObject levelUI;
    public Text levelText;
    public Button nextLevelButton;
    public Button previousLevelButton;

    void Awake()
    {
        instance = this;

        money = PlayerPrefs.GetInt("money", 0);
        
        foreach(var troop in allTroops)
        {
            troop.currentLevel = PlayerPrefs.GetInt(troop.name + "level", 1);
        }

        levelType = (LevelType)PlayerPrefs.GetInt("LevelType", 1);

        Time.timeScale = 1;
        cam = Camera.main;
        maxLevel = PlayerPrefs.GetInt("Level", level);
        if (maxLevel > levels.Length) maxLevel--;
        level = maxLevel;
        blueArmy = Instantiate(armyPrefab, transform.position, Quaternion.Euler(0, 90, 0)).GetComponent<Army>();

        if (levelType == LevelType.Actual)
            Army.limit = CurrentLevel.money;
        else Army.limit = int.MaxValue;

        switch (levelType)
        {
            case LevelType.Actual:
                GenerateLevel(0);
                break;
            case LevelType.Custom:
                redArmy = Instantiate(armyPrefab, transform.position, Quaternion.Euler(0, -90, 0)).GetComponent<Army>();
                redArmy.armySide = PlaySide.Red;

                levelUI.SetActive(false);
                break;
        }

        blueArmy.cost = 0;

        Army.blue = blueArmy;
        Army.red = redArmy;
    }

    public void Begin()
    {
        blueArmy.Begin();
        redArmy.Begin();
        TroopSpawner.instance.gameObject.SetActive(false);
        levelUI.SetActive(false);
        CameraMovement.began = true;
        began = true;
    }

    void Update()
    {
        if (began) return;

        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }

        if (Input.GetMouseButtonUp(0)) clickType = ClickType.None;
    }

    public void GenerateLevel(int delta)
    {
        level += delta;

        if (redArmy != null) Destroy(redArmy.gameObject);
        redArmy = Instantiate(levels[level - 1].army, transform.position, Quaternion.Euler(0, -90, 0)).GetComponent<Army>();

        levelText.text = "Level " + level;

        Army.limit = CurrentLevel.money;
        blueArmy.Clear();

        blueArmy.UpdateUI();

        nextLevelButton.enabled = level < levels.Length - 1 && maxLevel > level;
        previousLevelButton.enabled = level > 1;
    }

    void CastRay()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000, touchMask))
        {
            Transform target = hit.transform;

            Troop troop = target.GetComponent<Troop>();

            if (clickType != ClickType.None && (troop == null ^ clickType != ClickType.Remove)) return;
            if (lastClickPoint != null && Vector3.Distance(hit.point, lastClickPoint.Value) < minimumClickDelta) return;

            HideShow(false);

            lastClickPoint = hit.point;

            if (troop != null)
            {
                troop.OnClicked();
                clickType = ClickType.Remove;
            }
            else
            {
                if (hit.point.x < 0)
                {
                    blueArmy.OnClicked(hit.point);
                }
                else if (levelType != LevelType.Actual)
                {
                    redArmy.OnClicked(hit.point);
                }

                clickType = ClickType.Add;
            }
        }
    }

    void FixedUpdate()
    {
        if (began) return;

        if (Input.GetMouseButton(0))
        {
            CastRay();
        }
    }

    public void EndGame(Army loser)
    {
        string won = "Blue";

        if (loser == blueArmy)
        {
            winText.color = Color.red;
            won = "Red";
        }

        winUI.SetActive(true);
        winText.text = won + " Army Won!";

        money += CurrentLevel.reward / 2;

        if (won == "Blue" && level == maxLevel && levelType == LevelType.Actual)
        {
            PlayerPrefs.SetInt("Level", level + 1);
            money += CurrentLevel.reward / 2;
        }
        
        PlayerPrefs.SetInt("money", money);
    }

    public void Next()
    {
        Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        Save();

        SceneManager.LoadScene(menuSceneName);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        money = 0;
    }

    public void Exit()
    {
        Save();

        Application.Quit();
    }

    public void HideShow(bool state)
    {
        troopUI.SetActive(state);
        showUI.SetActive(!state);
    }

    public static void Save()
    {
        PlayerPrefs.SetInt("money", money);

        foreach (var troop in instance.allTroops)
        {
            PlayerPrefs.SetInt(troop.name + "level", troop.currentLevel);
        }
    }
}

[System.Serializable]
public struct Level
{
    public GameObject army;
    public int money;
    public int reward;
}

public enum ClickType { None, Add, Remove }
public enum LevelType { Custom, Actual }
