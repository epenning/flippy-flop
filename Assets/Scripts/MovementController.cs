using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour
{
    // Which objects we want to collide with
    public LayerMask collisionMask;

    // Small gap between bottom of sprite and origin of rays (to allow for resting on objects)
    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public float maxClimbAngle = 80;
    public float maxDescendAngle = 80;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;

    PlayerController pController;

    // Where the first ray on each respective side of the collider should start
    RaycastOrigins raycastOrigins;

    // Info regarding what we're currently colliding with
    public CollisionInfo collisions;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        pController = GetComponent<PlayerController>();
        CalculateRaySpacing();
    }

    public void Move(Vector2 moveAmount)
    {
        // Make the origins follow where we're going
        UpdateRaycastOrigins();

        // Reset collision info, as it may change (it will get updated when we call our collision detection methods)
        collisions.Reset();

        collisions.moveAmountOld = moveAmount;


        if (moveAmount.y < 0)
        {
            // Check to see if we need to descend a slope
            DescendSlope(ref moveAmount);
        }
        if (moveAmount.x != 0)
        {
            HorizontalCollisions(ref moveAmount);
        }
        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);
    }

    void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x);
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            // Decide wihch side we're shooting rays from
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            // Separate the rays
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            // Shoot a ray to where we're trying to move and record the result
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                // Our ray hit something

                // Get the angle of whatever we hit
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    // This is occuring when we're shooting the bottom ray (the only one we care about when we're on a slope)
                    // This slope is climbable

                    // Switch from descending an old slope to climbing a new slope
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }

                    float distanceToSlopeStart = 0;

                    // We're starting to climb a new slope
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        // Use this to get right up against a new slope
                        distanceToSlopeStart = hit.distance - skinWidth;

                        // This ensures that the ClimbSlope method will only be using our horizontal moveAmount once we are actually at the slope
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope(ref moveAmount, slopeAngle);

                    // Because we subtracted this before, we need to add it back on once we're done climbing the slope
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    // We're not climbing a slope, so we can look at other rays (or we're touching a slope that we can't climb)

                    // Adjust our moveAmount so it only takes us to the edge of whatever we're about to collide with
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;


                    if (collisions.climbingSlope)
                    {
                        // We're touching a slope that we can't climb

                        // This is to account for obstacles appearing on slopes (in which case we still need to be updating our Y moveAmount)
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }




            }
        }
    }

    // Same as HorizontalCollisions, but vertical (and minus slope checks)
    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                // This is to account for obstacles appearing on slopes, but in a way such that we only touch them vertically (in which case we still need to be updating our X moveAmount)
                if (collisions.climbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                if(!collisions.below && directionY == -1)
                {
                    pController.updatePlatform(hit.collider.gameObject);
                }
                collisions.below = (directionY == -1);
                collisions.above = (directionY == 1);
            }
        }


        if (collisions.climbingSlope)
        {
            // Need to fire out a horizontal ray to check for a potential new slope (this is similar to what we do in HorizontalCollisions)
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                // Hit a slope!
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    // Found a new slope!
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    // Treat horizontal moveAmount as the total distance we want to move along the slope, use this along with the slope angle to find new target X and Y
    void ClimbSlope(ref Vector2 moveAmount, float slopeAngle)
    {

        float moveDistance = Mathf.Abs(moveAmount.x);

        // trig stuff (moveDistance is how much we want to move along the slope, climbmoveAmountY is it's vertical component)
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbmoveAmountY)
        {
            // This means that we're not trying to jump
            // We should only set our velocities in this case, otherwise we wouldn't be able to jump (these velocities would just overwrite whatever the jump is trying to do)

            moveAmount.y = climbmoveAmountY;

            // more trig stuff
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);

            // we're on a slope
            collisions.below = true;

            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x);
        // Cast an infinite ray downwards (from bottom right or bottom left, depending on which direction we're moving in)
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            // We're above something
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                // We're above a slope
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    // We're moving in the direction of the normal's X component (i.e. moving down the slope)
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                    {
                        // Since ray was infinite, we needed to make sure that we were actually ON the slope and not just above it

                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);

                        // Move us down to hug the slope
                        moveAmount.y -= descendmoveAmountY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector2 moveAmountOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}