using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class PacMan : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction = Vector3.left;

    private GridMovement gridMovement;

    public GridMovement GridMovement { get => gridMovement;}

    private void Awake()
    {
        gridMovement = GetComponent<GridMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector3.right;
        }

        TryChangeDirection();

        if (!gridMovement.IsMoving)
        {
            gridMovement.MoveStraight();
        }
    }

    public void TryChangeDirection()
    {
        if (gridMovement.IsValidDirection(direction))
        {
            gridMovement.Direction = direction;
        }
    }
}
