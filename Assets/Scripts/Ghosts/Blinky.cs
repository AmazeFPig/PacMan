using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{

    protected override void Start()
    {
        base.Start();
        isReady = true;
        EnterChaseState();
    }

    protected override void InChaseState()
    {
        MoveTo(target.transform.position);
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
