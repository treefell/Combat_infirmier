using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseToAttack : AIState
{
    
    private AttackManager manager;
    private Attack atk;

    public AIChaseToAttack(AIController controller, AttackManager attackManager, Attack attack, float speed) : base(controller)
    {
        runSpeed = speed;
        manager = attackManager;
        atk = attack;
    }

    public override void Tick()
    {
        Chase();
        

        if (manager.AttackSimulation(atk)) // launches attack if in range
        {
            controller.StopWaitingAndAttack(atk);

        }
    }

    public override void OnStateEnter()
    {
    }

    public override void OnStateExit()
    {
    }

  
}
