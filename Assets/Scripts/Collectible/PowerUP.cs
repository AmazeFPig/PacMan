using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : Collectible
{
    protected override void OnCollected()
    {
        Debug.Log("DotEaten");
        EventCenter.GetInstance().EventTrigger("DotEaten");
    }
}
