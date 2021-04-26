using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWait : AIState
{
    private float waitingTime;
    private bool autoBlock;

    public AIWait(AIController controller, float time, bool setAutoBlock) : base(controller)
    {
        waitingTime = time;
        autoBlock = setAutoBlock;
    }

    public override void Tick()
    {
    }

    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExit()
    {
        
    }

    
}
