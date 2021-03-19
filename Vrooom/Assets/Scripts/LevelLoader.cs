using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private int IndexSceneMenu = 0;
    private int IndexSceneGameplay = 1;
    private int IndexSceneFin = 2;

    void Update()
    { }

    public void LoadMenu()
    {
        LoadLevel(IndexSceneMenu);
    }
    public void LoadGameplay()
    {
        LoadLevel(IndexSceneGameplay);
    }
    public void LoadFin()
    {
        LoadLevel(IndexSceneFin);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void test()
    {
        Debug.Log("Toto content");
    }
}
