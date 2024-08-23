using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Controller))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 5f)] private float noInputMultiplier = 2f;
    [SerializeField, Range(0f, 1f)] private float coyoteTime = 0.5f;
    [SerializeField, Range(0f, 1f)] private float apexTime = 0.1f;
    [SerializeField] private bool useGravityChanges = true;

    private Controller controller;
    private Rigidbody2D body;
    private Ground ground;
    private Vector2 velocity;

    private int jumpPhase;
    private bool blockJump;
    private float defaultGravityScale;

    private bool desiredJump;
    private bool onGround;
    private bool onGroundBuffer;
    private bool onGroundOverride;
    private bool goingUp;

    private float coyoteTimer = 0f;

    private Coroutine coyoteTimerCor;

    private bool apexTimeCheck = false;
    private float apexTimer = 1f;

    public bool ApexTimeCheck => apexTimeCheck;
    public bool OnGroundOverride => onGroundOverride;
    public bool OnGround => onGround;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        controller = GetComponent<Controller>();

        defaultGravityScale = 1f;
    }

    private void Update()
    {
        desiredJump = controller.input.RetrieveJumpInput();
        if (blockJump && !desiredJump)
            blockJump = false;
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;
        GroundCheck();
        GravityCheck();
        if (!blockJump)
            if (desiredJump)
            {
                desiredJump = false;
                JumpAction();
            }

        body.velocity = velocity;
        onGroundBuffer = onGround;
    }
    private void JumpAction()
    {

        if ((onGround || onGroundOverride) || jumpPhase < maxAirJumps)
        {
            blockJump = true;
            goingUp = true;
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight * (useGravityChanges ? upwardMovementMultiplier : 1));
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(body.velocity.y);
            }

            if (coyoteTimer > 0)            
                onGroundOverride = false;            

            velocity.y += jumpSpeed;
        }
    }

    private void GroundCheck()
    {
        onGround = ground.OnGround;

        if (onGround)
        {
            jumpPhase = 0;
            coyoteTimer = 0;
            apexTimer = 0;
            onGroundOverride = false;
            if (coyoteTimerCor != null)
                StopCoroutine(coyoteTimerCor);
        }
    }

    private void GravityCheck()
    {
        if (!apexTimeCheck)
            if (useGravityChanges)
                if (body.velocity.y > 0)
                {
                    body.gravityScale = upwardMovementMultiplier;
                    if (!desiredJump)
                    {
                        body.gravityScale = downwardMovementMultiplier * noInputMultiplier;
                    }
                }
                else if (body.velocity.y < 0)
                {
                    if (!onGround && onGroundBuffer && coyoteTimer == 0 && jumpPhase == 0)
                        coyoteTimerCor = StartCoroutine(CoyoteTimeValidation());
                    else if (apexTime > 0 && apexTimeCheck == false && apexTimer == 0 && !onGroundOverride && goingUp)
                    {
                        StartCoroutine(ApexTimeValidation());
                        goingUp = false;
                    }
                    else
                        body.gravityScale = (desiredJump ? downwardMovementMultiplier : downwardMovementMultiplier * noInputMultiplier);

                }
                else if (body.velocity.y == 0)
                {
                    body.gravityScale = defaultGravityScale;
                }
    }

    private IEnumerator CoyoteTimeValidation()
    {
        while (velocity.y < 0f && !onGround && jumpPhase == 0 && coyoteTimer < coyoteTime)
        {
            coyoteTimer += Time.deltaTime;
            onGroundOverride = true;
            yield return new WaitForFixedUpdate();

        }
        onGroundOverride = false;
    }

    private IEnumerator ApexTimeValidation()
    {
        apexTimeCheck = true;
        body.gravityScale = 0;
        velocity.y = 0;
        while (apexTimer < apexTime)
        {
            apexTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (OnGround)
                break;
        }
        apexTimeCheck = false;
    }
    private void OnGUI()
    {
        float left = 55;
        // Sliders for parameters from 0 to 5
        jumpHeight = GUI.HorizontalSlider(new Rect(left + 10, 10, 200, 20), jumpHeight, 0f, 5f);
        maxAirJumps = int.Parse(GUI.TextField(new Rect(left + 10, 40, 200, 20), maxAirJumps.ToString(), 3));

        downwardMovementMultiplier = GUI.HorizontalSlider(new Rect(left + 10, 70, 200, 20), downwardMovementMultiplier, 0f, 5f);
        upwardMovementMultiplier = GUI.HorizontalSlider(new Rect(left + 10, 100, 200, 20), upwardMovementMultiplier, 0f, 5f);
        noInputMultiplier = GUI.HorizontalSlider(new Rect(left + 10, 130, 200, 20), noInputMultiplier, 0f, 5f);

        // Sliders for parameters from 0 to 1
        coyoteTime = GUI.HorizontalSlider(new Rect(left + 10, 160, 200, 20), coyoteTime, 0f, 1f);
        apexTime = GUI.HorizontalSlider(new Rect(left + 10, 190, 200, 20), apexTime, 0f, 1f);

        // Toggle for "useGravity"
        useGravityChanges = GUI.Toggle(new Rect(left + 10, 220, 200, 20), useGravityChanges, "Modify Gravity On Hold");

        // Labels to display the current values
        GUI.Label(new Rect(left + 220, 10, 150, 20), "Jump Height: " + jumpHeight.ToString("F2"));
        GUI.Label(new Rect(left + 220, 40, 150, 20), "Max Air Jumps: " + maxAirJumps);
        GUI.Label(new Rect(left + 220, 70, 150, 20), "Downward Multiplier: " + downwardMovementMultiplier.ToString("F2"));
        GUI.Label(new Rect(left + 220, 100, 150, 20), "Upward Multiplier: " + upwardMovementMultiplier.ToString("F2"));
        GUI.Label(new Rect(left + 220, 130, 150, 20), "No Input Multiplier: " + noInputMultiplier.ToString("F2"));
        GUI.Label(new Rect(left + 220, 160, 150, 20), "Coyote Time: " + coyoteTime.ToString("F2"));
        GUI.Label(new Rect(left + 220, 190, 150, 20), "Apex Time: " + apexTime.ToString("F2"));
    }
}