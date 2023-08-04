using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Inky : Ghost
{
    protected override void Start()
    {
        base.Start();

        isReady = true;
        EnterChaseState();
    }

    protected override void InChaseState()
    {
        AstarNode currentNode = AStarGrid.GetInstance().WorldToAStarNode(target.transform.position);

        Vector2 offset = target.GridMovement.Direction * 2;

        AstarNode targetNode = AStarGrid.GetInstance().NodeGrid[(int)(currentNode.CoordinateX + offset.x), (int)(currentNode.CoordinateY + offset.y)];
        
        MoveTo(targetNode);
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            EnterScatterState();
        }
    }
}
