using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : Ghost
{

    [SerializeField]
    private Blinky blinky;

    protected override void Start()
    {
        base.Start();

        EventCenter.GetInstance().AddEventListener("600Score", () =>
        {
            isReadyExitBox = true;
            EnterChaseState();
        });
    }

    protected override void InChaseState()
    {

        Vector3 offset = target.GridMovement.Direction * 2;

        Vector3 vector = (target.transform.position + offset) - blinky.transform.position;

        vector = vector * 2;

        AstarNode targetNode = AStarGrid.GetInstance().WorldToAStarNode(vector);

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
