using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : Collectible
{
    private int score = 10;
    protected override void OnCollected()
    {
        Debug.Log("DotEaten");
        EventCenter.GetInstance().EventTrigger("DotEaten", score);
    }
}
