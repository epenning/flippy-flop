using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementController))]
public class PlayerTest : MonoBehaviour
{

    public float jumpHeight = 4;

    // Time to reach the highest point of the jump
    public float timeToJumpApex = .4f;

    // Time to reach max horizontal speed (or come to rest)
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    // Calculated based on jumpHeight and timeToJumpApex
    public float jumpVelocity;
    public float gravity;

    public Vector3 velocity;

    // Used for horizontal drag
    public float velocityXSmoothing;

    MovementController controller;

    void Start()
    {
        controller = GetComponent<MovementController>();

        // Use kinematics equations to calculate necessary gravity and jumpVelocity
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        // Target velocity at full speed
        float targetVelocityX = input.x * moveSpeed;

        // Smooth out horizontal velocity (changes depending on if we're grounded or airborne)
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}