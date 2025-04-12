using UnityEngine;

public class GrapplingScript : MonoBehaviour
{
    [Header("General Refernces:")]
    // The Code for the mechanical part of the grappling hook
    public GrapplerNew grapplingGun;
    // Renders the Grapple Texture
    public LineRenderer m_lineRenderer;

    [Header("General Settings:")]
    //The precise number that allows it to
    [SerializeField] private int precision = 40;
    // The Speed of which how fast the line straightens up.
    [Range(0, 20)][SerializeField] private float straightenLineSpeed = 5;

    [Header("Rope Animation Settings:")]
    // How the Rope/Tentacle would look like during the grappling process.
    public AnimationCurve ropeAnimationCurve;
    // How big the starting size of the wave is.
    [Range(0.01f, 4)][SerializeField] private float StartWaveSize = 2;
    // The wave size.
    float waveSize = 0;

    [Header("Rope Progression:")]
    // The Progression curve, which shows the animation from the rope curve to straightening.
    public AnimationCurve ropeProgressionCurve;
    // The Speed of the animation.
    [SerializeField][Range(1, 50)] private float ropeProgressionSpeed = 1;

    // The Move Time
    float moveTime = 0;

    // Hidde Bool that shows if the grappling code is doing it's job or not.
    [HideInInspector] public bool isGrappling = true;

    // A bool to see if the line is straight or not.
    bool straightLine = true;

    // When the object itself is enabled, it basically sets the movetime to it's default value, equates the positionCount to the starting precision number, sets the waveSize to it's starting size, and makes.
    // The straight line false because there's no line.
    private void OnEnable()
    {
        moveTime = 0;
        m_lineRenderer.positionCount = precision;
        waveSize = StartWaveSize;
        straightLine = false;

        // It then makes it so when this is enabled, it runs the code below.
        LinePointsToFirePoint();

        //Then it finally enables linerenderer.
        m_lineRenderer.enabled = true;
    }
    // Disables the line renderer and ticks isGrappling off to signify it's no longer grappling.
    private void OnDisable()
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }

    // This code checks to see the distance from Line point to fire point.
    private void LinePointsToFirePoint()
    {
        // Basically, so long as the variable i is less than precision it will set the linerenderer's position towards the firepoint.
        for (int i = 0; i < precision; i++)
        {
            m_lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
        }
    }

    // This Draws the Rope and Makes it so it's possible to have rope animation.
    private void Update()
    {
        // Movetime is made to be constantly be shifting up by the incriment of time.deltatime.
        moveTime += Time.deltaTime;
        // It then executes this command constantly to check on what to do when it comes to drawing the rope.
        DrawRope();
    }

    // Draws the Rope.
    void DrawRope()
    {
        // If the Rope is not a straight line, it does this.
        if (!straightLine)
        {
            // if the precision is equal to the grapple point, it's a straight line.
            if (m_lineRenderer.GetPosition(precision - 1).x == grapplingGun.grapplePoint.x)
            {
                straightLine = true;
            }
            // Else, it draws wavy ropes.
            else
            {
                DrawRopeWaves();
            }
        }
        // If it is a straight line, then it does this.
        else
        {
            // If it's not grappling it will do the following.
            if (!isGrappling)
            {
                // It calls the grapple script to execute the grapple command discussed earlier.
                grapplingGun.Grapple();
                // Then it sets things to true.
                isGrappling = true;
            }
            // It then checks if the wave size is greater than 0
            if (waveSize > 0)
            {
                // Which then reduces the wavesize by deltatime times the speed it takes to straighten the line, thus turning the wavy rope into a straight line rope.
                waveSize -= Time.deltaTime * straightenLineSpeed;
                // Then it draws the wavy rope.
                DrawRopeWaves();
            }
            // If neither of the two above fit the criteria, it does this.
            else
            {
                // It sets the wave size to 0
                waveSize = 0;

                // Then checks if the line renderer's positioncount (the precision) does not equal 2. If it does not, it sets it to 2.
                if (m_lineRenderer.positionCount != 2) { m_lineRenderer.positionCount = 2; }
                // Then draws the non-wavy waves.
                DrawRopeNoWaves();
            }
        }
    }

    // This command draws the actual wavy rope.
    void DrawRopeWaves()
    {
        // So long as it's less than precision, it repeats this code.
        for (int i = 0; i < precision; i++)
        {
            // First, it takes the delta (the in between of the start position of the rope and the endpoint), basically getting the midpoint of the entire process.
            float delta = (float)i / ((float)precision - 1f);
            // Then it basically gets a perpendicular vector, normalizes it, and then evaluetes the animation curve using the delta multiplied by wavesize (Which shows how big the wave gets).
            Vector2 offset = Vector2.Perpendicular(grapplingGun.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            // then creates a vector2 called targetposition, which is a lerp of the position of the grappling gun's position, towards the grapple point, using delta as a form of time
            // + the offset to account for the animation that will play.
            Vector2 targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grapplePoint, delta) + offset;
            // The currentposition is the actual movement of the rope towards the endpoint that is the target position.
            Vector2 currentPosition = Vector2.Lerp(grapplingGun.firePoint.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            // This line of code is the animation part.
            m_lineRenderer.SetPosition(i, currentPosition);
        }
    }

    // This is called if the rope has no waves.
    void DrawRopeNoWaves()
    {
        // The vertex in question is just the start/end points, and it's just drawing a straight line from the firepoint to the grapplepoint.
        m_lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
        m_lineRenderer.SetPosition(1, grapplingGun.grapplePoint);
    }

    // Disclaimer: All Credit to the code goes to One Minute Unity/Vespper. This code is open-source, and I'm using the code at my discretion. 
    //Yes, I understand the code to a certain degree. Not absolutely, but enough to understand the basic process from start to end.
}