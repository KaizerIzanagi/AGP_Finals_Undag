using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public UIControllerScript uiControl;

    public bool gameRunning;
    public float time, min, sec;
    public TMP_Text timing;

    void Start()
    {
        gameRunning = true;
        uiControl = GetComponent<UIControllerScript>();
        timing = GameObject.Find("Timer").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            time += Time.deltaTime;

            min = Mathf.FloorToInt(time / 60);
            sec = Mathf.FloorToInt(time % 60);

            timing.text = string.Format("Timer: " + "{0:00} : {1:00}", min, sec);
        }
    }
}
