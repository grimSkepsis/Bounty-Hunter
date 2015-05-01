﻿//#define debug_draw

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour {

    [SerializeField]
    protected LayerMask groundLayers;

    [SerializeField]
    private Transform groundDetectionLocation;

    [SerializeField]
    protected float skinWidth = 0.2f;

    [SerializeField]
    protected float slopLimit = 0.9f;

    [SerializeField]
    public float baseMoveSpeed;
    
    [SerializeField]
    public float baseJumpSpeed;

    /* Major movement values: may not want these to be the same b/w player and ai */
    private float gravityAccel = 9.8f;

    /* The actor that this controller is controlling */
    private Actor owner;
    private BoxCollider2D bodyCollider;

    /* Main velocity */
    private Vector3 velocity;

    /* If its ont he ground then that means last update it found a floor nearby */
    private bool bIsOnGround;

    /* game object that controller is standing on, will be null if in air */
    private GameObject standingBase;


    void Start() {
        owner = GetComponent<Actor>();
        if(owner == null) {
            Debug.LogError(name + " : MovementController is not attached to an Actor; Thats dumb!");
        }

        bodyCollider = GetComponent<BoxCollider2D>();
    }


    void Update() {
        standingBase = GetGround();

        // Accelerate toward ground if not on the ground
        if(!bIsOnGround) {
            velocity.y += -gravityAccel * Time.deltaTime;
        }

        // Get the base move step, and then modify it and resolve collisions
        Vector3 moveDelta = velocity * Time.deltaTime;

        if(moveDelta.x != 0.0f) {
            moveDelta = HorizontalMove(moveDelta);
        }

        if(moveDelta.y != 0.0f) {
            moveDelta = VerticalMove(moveDelta);
        }

        transform.position += moveDelta;
    }


    /* Move left and right, assuming that actor is on ground 
     * xAmplitude = the ratio of move speed to be applied as horizonatl movement
     */
    public void Move(float xAmplitude) {
        // TODO: Modifiers? 
        // TODO: ground friction or what?

        if(bIsOnGround) {
            velocity.x = xAmplitude * baseMoveSpeed;
        }
    }



    public void Jump() {
        if(bIsOnGround) {
            velocity.y = baseJumpSpeed;
        }
    }



    public GameObject GetGround() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(groundDetectionLocation.position, 0.01f, groundLayers);

        // did we hit (not ourselves)?
        if(hits.Length > 0) {
            for(int i = 0; i < hits.Length; i++) {
                if(hits[i].gameObject != owner.gameObject) {
                    bIsOnGround = true;
                    return hits[i].gameObject;
                }
            }
        }

        bIsOnGround = false;
        return null;
    }



    // adjusts the input time normalized move to account for horizoontal collisions
    private Vector3 HorizontalMove(Vector3 moveDelta) {
        // Get the top and bottom corners on the side that its moving
        Vector3 top = GetHeadCorner(moveDelta.x < 0.0f);
        Vector3 bottom = GetFootCorner(moveDelta.x < 0.0f);

        Vector3 horzeDelta = moveDelta;
        horzeDelta.y = 0.0f;
        float horzRayCastDistance = horzeDelta.magnitude;

        // Check horizontal movement by casting rays in the direction of movement, plus skin width
        List<RaycastHit2D> raysToCheck = new List<RaycastHit2D>();
        raysToCheck.Add(Physics2D.Raycast(top, horzeDelta.normalized, horzRayCastDistance));
        raysToCheck.Add(Physics2D.Raycast(bottom, horzeDelta.normalized, horzRayCastDistance));

#if debug_draw
        Debug.DrawLine(top, top + horzeDelta.normalized * horzRayCastDistance, Color.red, 0);
        Debug.DrawLine(bottom, bottom + horzeDelta.normalized * horzRayCastDistance, Color.red, 0);
#endif

        bool collided = false;
        int successfulHit = 0;

        // check the horizontal rays and limit horizontal velocity
        int idx = 0;
        foreach(RaycastHit2D ray in raysToCheck) {
            if(ray && !ray.collider.isTrigger) {
                moveDelta.x = (new Vector3(ray.point.x, ray.point.y, 0.0f) - bottom).x;

                moveDelta.x -= moveDelta.normalized.x * skinWidth;

                collided = true;
                successfulHit = idx;
            }
            idx++;
        }

        // Callback for collision
        if(collided) {
            OnHit(ref moveDelta, raysToCheck[successfulHit].normal, raysToCheck[successfulHit].point);
        }

        return moveDelta;
    }
	


    // adjusts the input time normalized move to account for vertical collisions and falling
    private Vector3 VerticalMove(Vector3 moveDelta) {
        // do different cast depending on if were moving up or down
        Vector3 vertDelta = moveDelta;
        vertDelta.x = 0.0f;
        float vertRayCastDistance = vertDelta.magnitude;
        Vector3 left;
        Vector3 right;

        if(moveDelta.y < 0.0f) {
            // Moving down, main case
            left = GetFootCorner(true);
            right = GetFootCorner(false);
        }
        else {
            left = GetHeadCorner(true);
            right = GetHeadCorner(false);
        }

        // Check horizontal movement by casting rays in the direction of movement, plus skin width
        List<RaycastHit2D> raysToCheck = new List<RaycastHit2D>();
        raysToCheck.Add(Physics2D.Raycast(left, vertDelta.normalized, vertRayCastDistance));
        raysToCheck.Add(Physics2D.Raycast(right, vertDelta.normalized, vertRayCastDistance));

#if debug_draw
        // DEBUG MOVEMENT
        Debug.DrawLine(left, left + vertDelta.normalized * vertRayCastDistance, Color.yellow, 0);
        Debug.DrawLine(right, right + vertDelta.normalized * vertRayCastDistance, Color.yellow, 0);
#endif

        bool collided = false;
        int successfulHit = 0;

        // check the horizontal rays and limit horizontal velocity to move right up to the collision point
        int idx = 0;
        foreach(RaycastHit2D ray in raysToCheck) {
            if(ray && !ray.collider.isTrigger) {
                moveDelta.y = (new Vector3(ray.point.x, ray.point.y, 0.0f) - left).y;

                moveDelta.y -= moveDelta.normalized.y * skinWidth;

                collided = true;
                successfulHit = idx;
            }
            idx++;
        }

        // Callback for collision
        if(collided) {
            OnHit(ref moveDelta, raysToCheck[successfulHit].normal, raysToCheck[successfulHit].point);
        }

        return moveDelta;
    }



    private void OnHit(ref Vector3 vel, Vector3 normal, Vector3 point) {
        // Determine how flat the surfae is:
        float flatness = Mathf.Abs(Vector3.Dot(normal, Vector3.up));

        if(flatness > slopLimit) {
            // mostly vertical
            velocity.y = 0.0f;
        }
        else {
            // mostly horizontal surface
            velocity.x = 0.0f;
        }

    }



    // Get the location of the top corner (left or right based on move left)
    public Vector3 GetHeadCorner(bool bMoveLeft) {
        float offsetDistX = bodyCollider.size.x / 2.0f + skinWidth;
        float offsetDistY = bodyCollider.size.y / 2.0f + skinWidth;

        Vector3 center = transform.position + (Vector3)bodyCollider.offset;
        Vector3 toCorner = new Vector3(bMoveLeft? -offsetDistX : offsetDistX, offsetDistY, 0.0f);
        return (center + toCorner);
    }

    // Get the location of the bottom corner (left or right based on move left)
    public Vector3 GetFootCorner(bool bMoveLeft) {
        float offsetDistX = bodyCollider.size.x / 2.0f + skinWidth;
        float offsetDistY = -bodyCollider.size.y + skinWidth;

        Vector3 center = transform.position + (Vector3)bodyCollider.offset;
        Vector3 toCorner = new Vector3(bMoveLeft? -offsetDistX : offsetDistX, offsetDistY, 0.0f);
        return (center + toCorner);
    }


}