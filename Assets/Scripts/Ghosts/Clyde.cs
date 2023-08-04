using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Ghost
{
    protected override void Start()
    {
        base.Start();

        EventCenter.GetInstance().AddEventListener("1000Score", () =>
        {
            isReady = true;
            EnterChaseState();
        });
    }

    protected override void InChaseState()
    {
        if (AStarGrid.GetInstance().GetManhattanDistance(transform.position, target.transform.position) < 8)
        {
            AstarNode targetNode = AStarGrid.GetInstance().WorldToAStarNode(target.transform.position);
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
        else
        {
            EnterScatterState();
        }
    }

    protected override void InScatterState()
    {
        if (AStarGrid.GetInstance().GetManhattanDistance(transform.position, target.transform.position) >= 8)
        {
            base.InScatterState();

        }
        else
        {
            EnterChaseState();
        }

    }
}
