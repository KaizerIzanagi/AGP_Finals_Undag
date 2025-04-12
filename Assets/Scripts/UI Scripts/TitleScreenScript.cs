using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenScript : MonoBehaviour
{
    public static TitleScreenScript instance;
    public Button MainGame;
    public GameObject YouWin;
    public bool _tutorialDone, _MainGameDone;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        MainGame = GameObject.Find("Main Game Button").GetComponent<Button>();
        YouWin = GameObject.Find("You Win");
        YouWin.SetActive(false);
    }

    void Update()
    {
        SceneChecker();
    }

    public void TutorialLoad()
    {
        Debug.Log("I am being pressed against my will!");
        SceneManager.LoadScene("Tutorial");
    }

    public void MainGameLoad()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SceneChecker()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "TitleScreen")
        {
            if (_tutorialDone)
            {
                MainGame.interactable = true;
            }
            else
            {
                MainGame.interactable = false;
            }

            if (_MainGameDone)
            {
                YouWin.SetActive(true);
            }
            else
            {
                YouWin.SetActive(false);
            }
        }
        
    }
}
