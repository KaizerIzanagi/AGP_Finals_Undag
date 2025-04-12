using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIControllerScript : MonoBehaviour
{
    public TitleScreenScript instance;
    public TimerScript finishTime;
    public TMP_Text endTimer;
    public GameObject victoryScreen, deathScreen;
    public bool _isWin, _isDead;
    public Scene scene;
    void Start()
    {
        //Collectors
        instance = GameObject.Find("Title Screen").GetComponent<TitleScreenScript>();
        endTimer = GameObject.Find("Clear Time").GetComponent<TMP_Text>();
        victoryScreen = GameObject.FindGameObjectWithTag("VictoryScreen");
        deathScreen = GameObject.FindGameObjectWithTag("DeathScreen");
        finishTime = GetComponent<TimerScript>();

        //Flippers
        instance.gameObject.SetActive(false);
        victoryScreen.SetActive(false);
        deathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        scene = SceneManager.GetActiveScene();
        //If I win, I die
        if (_isWin)
        {
            finishTime.gameRunning = false;
            victoryScreen.SetActive(true);
            instance.gameObject.SetActive(true);
            endTimer.text = string.Format("Clear Time: " + "{0:00} : {1:00}", finishTime.min, finishTime.sec);
        }
        //If I lose I die
        if (_isDead)
        {
            finishTime.gameRunning = false;
            deathScreen.SetActive(true);
        }
    }

    //Does it matter? Of course it does. This is to see if the tutorial is done or not. And it has different outcomes!
    public void ToTitleSceen()
    {
        if (scene.name == "Tutorial")
        {
            instance._tutorialDone = true;
            SceneManager.LoadScene("TitleScreen");
        }
        else if (scene.name == "MainGame")
        {
            instance._MainGameDone = true;
            SceneManager.LoadScene("TitleScreen");
        }
    }

    //This only works in the main game due to failing in the main game only.
    public void RetryLevel()
    {
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadScene("MainGame");
        
    }
    
    //If you really suck (I do)
    public void ExitGame()
    {
        Application.Quit();
    }
}
