using UnityEngine;
using UnityEngine.SceneManagement;

public class MainFunctions : MonoBehaviour
{
    public string menuName = "menu";

    public void GoToMenu()
    {
        SceneManager.LoadScene(menuName);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();

        GameMaster.money = 0;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
