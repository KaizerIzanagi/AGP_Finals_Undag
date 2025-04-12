using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float move;
    public TargetJoint2D joint;
    public float baseSpeed, runSpeedMult;

    private Rigidbody2D rb;

    void Start()
    {
        // Collects Rigidbody from Player
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<TargetJoint2D>();
    }

    void Update()
    {
        HorizontalMove();

        /*
        if (Input.GetMouseButtonDown(0))
        {
            joint.enabled = true;
        }
        */
    }

    // Handles Horizontal Movement
    public void HorizontalMove()
    {
        move = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(move * baseSpeed, rb.linearVelocity.y);

        //The Run Function.
        RunFunction();
    }

    /// Note, I legit fucking made this in seconds.
    /// Holy shit I got it to work first try. Now all I need to do is make it accelerate slowly, like ease in.
    public void RunFunction() 
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            baseSpeed = Mathf.Clamp(baseSpeed * runSpeedMult, 0, 10);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            baseSpeed = Mathf.Clamp(baseSpeed / runSpeedMult, 5, 10);
        }
    }

    public void JointTargeting()
    {
        joint.target = new Vector2(1, 1);
    }
}
