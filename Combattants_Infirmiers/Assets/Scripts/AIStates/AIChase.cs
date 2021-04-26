using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : AIState
{
    
    private float minDist;
    private float maxDist;
    

    public AIChase(AIController controller, float speed, float minDistance, float maxDistance) : base(controller)
    {
        runSpeed = speed;
        minDist = minDistance;
        maxDist = maxDistance;
    }

    public override void Tick()
    {
        //il se retourne si le player est derrière lui. Y'aura une anim à terme etc.



        Chase();

        distToTarget = Vector3.Distance(controller.transform.position, controller.currentTarget.transform.position);

        if (distToTarget <= minDist || distToTarget >= maxDist) // stops chasing if too close or too far
        {
            controller.NewAction();
        }
    }

    public override void OnStateEnter()
    {
    }

    public override void OnStateExit()
    {
    }

    
}
