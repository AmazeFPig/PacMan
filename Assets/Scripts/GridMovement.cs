using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public Vector3 Direction = Vector3.left;

    [SerializeField]
    private bool isPlayer;

    public float timeToMove = 0.1f;

    [SerializeField]
    private Transform ghostHomeGate;

    private bool isMoving; 

    private IEnumerator moveCoroutine;

    public bool IsMoving { get => isMoving;}

    public void TeleportTo(Vector3 position)
    {
        StopCoroutine(moveCoroutine);

        AstarNode teleportNode = AStarGrid.GetInstance().WorldToAStarNode(position);
        transform.position = AStarGrid.GetInstance().AStarNodeToWorld(teleportNode);

        isMoving = false;
    }

    public void MoveStraight()
    {
        AstarNode currentNode = AStarGrid.GetInstance().WorldToAStarNode(transform.position);
        AstarNode nextNode = AStarGrid.GetInstance().GetNeighborNode(currentNode, Direction);
        Vector3 targetPosition = AStarGrid.GetInstance().AStarNodeToWorld(nextNode);

        if (IsValidMove(nextNode))
        {
            moveCoroutine = MoveTo(targetPosition);

            StartCoroutine(moveCoroutine);
        }
    }

    public void MoveToNode(AstarNode node)
    {
        moveCoroutine = MoveTo(AStarGrid.GetInstance().AStarNodeToWorld(node));
        StartCoroutine(moveCoroutine);
    }

    public bool IsValidDirection(Vector3 newDirection)
    {
        AstarNode currentNode = AStarGrid.GetInstance().WorldToAStarNode(transform.position);
        AstarNode nextNode = AStarGrid.GetInstance().GetNeighborNode(currentNode, newDirection);

        return IsValidMove(nextNode);
    }

    public bool IsValidMove(AstarNode node)
    {
        return !node.IsObstacle && node != AStarGrid.GetInstance().WorldToAStarNode(ghostHomeGate.transform.position);
    }

    private IEnumerator MoveTo(Vector3 targetPosition)
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
