using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private int IndexSceneMenu = 0;
    private int IndexSceneGameplay = 1;
    private int IndexSceneFin = 2;

    public Animator transition;
    public float transitionTime = 1f;

    void Update()
    { }

    public void LoadMenu()
    {
        StartCoroutine(LoadLevel(IndexSceneMenu));
    }
    public void LoadGameplay()
    {
        StartCoroutine(LoadLevel(IndexSceneGameplay));
    }
    public void LoadFin()
    {
        StartCoroutine(LoadLevel(IndexSceneFin));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
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
