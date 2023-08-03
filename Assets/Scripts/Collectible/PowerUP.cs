using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : Collectible
{
    private int score = 50;
    protected override void OnCollected()
    {
        Debug.Log("DotEaten");
        EventCenter.GetInstance().EventTrigger("DotEaten", score);
    }
}
