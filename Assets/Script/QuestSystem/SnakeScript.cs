using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScript : NPCClass
{
    
    public override void Start()
    {
        base.Start();
    }

    
    public override void Update()
    {
        base.Update();
    }

    public override void HandleIdle()
    {
        base.HandleIdle();
    }

    public override void HandleSad()
    {
        base.HandleSad();
    }

    public new void HandleOnReward()
    {
        base.HandleOnReward();
    }

    public override void OnReward()
    {
        throw new System.NotImplementedException();
    }
}
