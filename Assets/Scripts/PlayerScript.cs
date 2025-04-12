using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Scripts
    public TutorialLevelScript Tutorial;
    public UIControllerScript uiControl;
    public GameObject CurrentLevelManager;
    

    [SerializeField] private float _move, _range;
    [SerializeField] private bool _tutorialLevel;

    public float baseSpeed, runSpeedMult;

    private Rigidbody2D _rb;

    void Start()
    {
        // Collects Rigidbody from Player
        _rb = GetComponent<Rigidbody2D>();
        CurrentLevelManager = GameObject.FindGameObjectWithTag("Current Level");
        uiControl = CurrentLevelManager.GetComponent<UIControllerScript>();

        if (CurrentLevelManager.gameObject.name == "TutorialLevelManager")
        {
            Tutorial = CurrentLevelManager.GetComponent<TutorialLevelScript>();
        }
        else if (CurrentLevelManager.gameObject.name == "MainLevelManager")
        {
            //Put Main Game Manager Script here.
        }

    }
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        HorizontalMove();

        if (scene.name == "Tutorial") 
        {
            _tutorialLevel = true;
        }
    }

    // Handles Horizontal Movement
    public void HorizontalMove()
    {
        // Just to clarify, how this works is basically it takes the default game horizontal inputs (A and D) and allows move to be either -1 or 1 respectively.
        _move = Input.GetAxis("Horizontal");
        // It then allows the thing to move on the horizontal axis via rigidbody.Velocity (or in the case of Unity 6, LinearVelocity).
        _rb.linearVelocity = new Vector2(_move * baseSpeed, _rb.linearVelocity.y);

        //The Run Function. Now I know this should be in the update, but putting it here is much more convenient as it lessens the clutter in Update. Note to self, update needs more functions/commands.
        RunFunction();
    }

    /// Note, I legit fucking made this in seconds.
    /// Holy shit I got it to work first try. Now all I need to do is make it accelerate slowly, like ease in.
    /// Future Note: I literally cannot make this accelerate smoothly. Just letting it go like that because I am crunching time.
    /// Even Further Future Note: Okay, on the bright side if this is shown, I can at least be content that this entire script is mine... minus me understanding jack shit.

    public void RunFunction()
    {
        // Basically, if you press down Left Shift, the baseSpeed will become multiplied by the Run Speed up to the clamp point of 10. I did this without the clamp and the thing went ZOOMING.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            baseSpeed = Mathf.Clamp(baseSpeed * runSpeedMult, 0, 10);
        }
        // Once you let go of shift, it goes back to baseSpeed.
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            baseSpeed = 5;
        }
    }

    // This is both the death detection system AND the victory detection system. Yep. Just a OnTriggerEnter2D. As to why it's like this... because the killbricks are basically JUST triggers.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // This is what happens if you die.
        if (collision.CompareTag("Killbrick") || collision.CompareTag("Enemy"))
        {
            if (_tutorialLevel)
            {
                Debug.Log("Because This is a Tutorial Level, you don't actually die.");
                Tutorial.hasFallen = true;
            }
            else
            {
                uiControl._isDead = true;
                Debug.Log("Player Should Die Here");
            }
        }
        // And this is what happens when you win the level.
        else if (collision.CompareTag("Victory"))
        {
            uiControl._isWin = true;
            _tutorialLevel = false;
            Debug.Log("Player Should Win here");
        }
    }
}
