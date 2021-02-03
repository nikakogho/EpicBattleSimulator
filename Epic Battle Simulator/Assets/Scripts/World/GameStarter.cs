using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public string battleFieldName = "battleField";
    public string shopName = "shop";

    void Awake()
    {
        GameMaster.money = PlayerPrefs.GetInt("money", GameMaster.money);
    }

    public void Custom()
    {
        LoadGame(0);
    }

    public void Level()
    {
        LoadGame(1);
    }

    public void LoadGame(int levelType)
    {
        PlayerPrefs.SetInt("LevelType", levelType);
        LoadGame();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(battleFieldName);
    }

    public void ResetLevels()
    {
        PlayerPrefs.DeleteKey("Level");
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();

        GameMaster.money = 0;
    }

    public void GoToShop()
    {
        SceneManager.LoadScene(shopName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
