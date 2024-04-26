using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    bool wasJustClicked = true;
    bool canMove;
    Vector2 playerSize;

    Rigidbody2D rb;
    Vector2 startingPosition;

    public Transform BoundaryHolder;

    Boundary playerBoundary;

    Collider2D playerCollider;

    struct Boundary
    {
        public float Up, Down, Left, Right;
        public Boundary(float up, float down, float left, float right)
        {
            Up = left; Down = right; Left = down; Right = up;
        }
    }

    // Use this for initialization
    void Start()
    {
        playerSize = GetComponent<SpriteRenderer>().bounds.extents;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        startingPosition = rb.position;

        playerBoundary = new Boundary(BoundaryHolder.GetChild(0).position.y,
                                      BoundaryHolder.GetChild(1).position.y,
                                      BoundaryHolder.GetChild(2).position.x,
                                      BoundaryHolder.GetChild(3).position.x);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (wasJustClicked)
            {
                wasJustClicked = false;

                if (playerCollider.OverlapPoint(mousePos))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
            }

            if (canMove)
            {
                Vector2 clampedMousePos = new Vector2(Mathf.Clamp(mousePos.y, playerBoundary.Left,
                                                                  playerBoundary.Right),
                                                      Mathf.Clamp(mousePos.x, playerBoundary.Down,
                                                                  playerBoundary.Up));
                rb.MovePosition(mousePos);
            }
        }
        else
       
            wasJustClicked = true;
        }
    public void ResetPosition()
    {
        rb.position = startingPosition;
    }
}
