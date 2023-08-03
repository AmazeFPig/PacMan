using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    private Vector3 direction = Vector3.left;

    [SerializeField]
    private float timeToMove = 0.1f;

    private bool isMoving;
    private Vector3 origPosition;
    private Vector3 targetPosition;

    private IEnumerator moveCoroutine;

    public Vector3 Direction { get => direction;}

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

        if (!isMoving)
        {
            MovePlayer();
        }
    }

    public void TeleportTo(Vector3 position)
    {
        StopCoroutine(moveCoroutine);

        AstarNode teleportNode = AStarGrid.GetInstance().WorldToAStarNode(position);
        origPosition = AStarGrid.GetInstance().AStarNodeToWorld(teleportNode);
        transform.position = origPosition;
        AstarNode nextNode = AStarGrid.GetInstance().GetNeighborNode(teleportNode, Direction);
        targetPosition = AStarGrid.GetInstance().AStarNodeToWorld(nextNode);

        isMoving = false;
    }

    private void MovePlayer()
    {
        origPosition = transform.position;
        AstarNode currentNode = AStarGrid.GetInstance().WorldToAStarNode(origPosition);
        AstarNode nextNode = AStarGrid.GetInstance().GetNeighborNode(currentNode, Direction);
        targetPosition = AStarGrid.GetInstance().AStarNodeToWorld(nextNode);

        if (!nextNode.IsObstacle)
        {
            moveCoroutine = MoveToTargetPosition();

            StartCoroutine(moveCoroutine);
        }
    }

    private IEnumerator MoveToTargetPosition()
    {
        isMoving = true;

        float elapsedTime = 0;
        Vector3 origPosition = transform.position;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPosition, targetPosition, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}
