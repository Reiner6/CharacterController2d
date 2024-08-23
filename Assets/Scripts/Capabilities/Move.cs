using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Move : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Controller controller;
    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private Ground ground;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;

    public Rigidbody2D RB { get { return rb; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        controller = GetComponent<Controller>();
    }

    private void Update()
    {
        direction.x = controller.input.RetrieveMoveInput();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.FrictionValue, 0f);
    }

    private void FixedUpdate()
    {
        onGround = ground.OnGround;
        currentVelocity = rb.velocity;
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = !onGround && desiredVelocity.x == 0 ? 0f : acceleration * Time.deltaTime;
        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, desiredVelocity.x, maxSpeedChange);
        rb.velocity = currentVelocity;
    }
}