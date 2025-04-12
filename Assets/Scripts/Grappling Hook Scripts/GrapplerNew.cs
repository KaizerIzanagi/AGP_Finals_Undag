using UnityEngine;

public class GrapplerNew : MonoBehaviour
{
    [Header("Scripts Ref:")]
    // The Script that animates the rope both mechanically and visually.
    public GrapplingScript grappleRope;

    [Header("Layers Settings:")]
    // Which layer it can grapple to. Made so you can have non-grappleable surfaces.
    [SerializeField] private int grappableLayerNumber = 7;

    [Header("Main Camera:")]
    // Main Camera used for tracking the rotation/where you're aiming.
    public Camera m_camera;

    [Header("Transform Ref:")]
    // Self Explainitory. It's the Transformation References as to what holds the grappling object (Player), Where the Gun circles around (Gun Pivot), and where it will shoot out the rope from (FirePoint)
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    // Component Grabber. It grapples using the spring joint (My Favorite Joint).
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Distance:")]
    // The Max Range of The Grapple
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 4.5f;

    // Launch Speed. It's how fast you zoom from your position to your destination.
    [Header("Launching:")]
    [SerializeField] private float launchSpeed = 1;

    // The both of these are hidden from the inspector, basically private, but can still be accessed. The opposite of using SerializeField on a private variable.
    // This tells you where in the world (scene) does the grapple end up at
    [HideInInspector] public Vector2 grapplePoint;
    // This is the distance between the player and the object.
    [HideInInspector] public Vector2 grappleDistanceVector;

    // This makes sure that it disables the rope on startup, both visually and funcionally.
    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void Update()
    {
        // If the mouse is down, then it sets the grapple point, which allows it to grapple in the first place.
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        // This part checks to see if the rope is enabled, if it's not, it simply runs the same command as below.
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos);
            }

        }
        // this happens if the mouse is released.
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // Turns off the grapple animation and disables the springjoint grappling, then returns the gravity scale, allowing the player to be affected by gravity in the event that gravity is set to 0.
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }
        // basically it does the command above in the event all else is passed.
        else
        {
            // mousePos is literally just translating where the mouse is on the game screen, and turning it into a variable that can be read by other lines of code. 
            //The reason why Input.mousePosition is able to be put in there is because the mousePosition is a Vector3 that's tracking where your mouse is on the screen by it's x and y coordinates.
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            // This sends the value to lookPoint, allowing RotateGun to work.
            RotateGun(mousePos);
        }
    }
    // Rotates the place where the grappling rope comes from.
    void RotateGun(Vector3 lookPoint)
    {
        // DistanceVector is basically an input of lookPoint minus the pivot's position.
        Vector3 distanceVector = lookPoint - gunPivot.position;

        // The angle takes the DistanceVector above and converts it into radians, then it splits it in two, dividing Y over X. Finally, it converts Radians to degrees via Mathf.Rad2Deg
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;

        // Quaternion.AngleAxis just takes the angle value made, and creates a rotation via using the z-axis of Vector3.Forward.
        gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Allows Grappling to Happen.
    void SetGrapplePoint()
    {
        // Quite Frankly is just the distance between where the mouse is within the screen minus the gun pivot's position.
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        // This checks if the raycast hit is possible
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            // Fires a Raycast in the direction until it hits a valid object.
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            // Checks if its the correct layer to grapple to
            if (_hit.transform.gameObject.layer == grappableLayerNumber)
            {
                // This checks if the distance between the object is within range
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                }
            }
        }
    }

    // The grappling action in question.
    public void Grapple()
    {
        // Attaches the anchor of the springjoint to the grapplepoint set.
        m_springJoint2D.connectedAnchor = grapplePoint;
        // Creates a distanceBector to take the position of the firing point and reduces it by the position of the gun holder.
        Vector2 distanceVector = firePoint.position - gunHolder.position;

        // This is the distance the spring and the attached object has.
        m_springJoint2D.distance = distanceVector.magnitude;
        // Basically how fast the speed it pulls itself towards the hooked object.
        m_springJoint2D.frequency = launchSpeed;
        // Enables the springjoint to pull the player towards it's destination.
        m_springJoint2D.enabled = true;
    }

    // Editor Only Range Radius
    private void OnDrawGizmosSelected()
    {
        // It just creates a visible range on how far your grapple can reach.
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

    // Disclaimer: All Credit to the code goes to One Minute Unity/Vespper. This code is open-source, and I'm using the code at my discretion. 
    //Yes, I understand the code to a certain degree. Not absolutely, but enough to understand the basic process from start to end.
}