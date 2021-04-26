using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : AIState
{
    private AttackManager attackManager;
    private Attack currentAttack;

    public AIAttack(AIController controller, Attack attack) : base(controller)
    {
        attackManager = controller.GetComponent<AttackManager>();
        currentAttack = attack;
    }

    public override void Tick()
    {
    }

    public override void OnStateEnter()
    {
        //attackManager.SetAI(true, controller);
        TurnTowardsPlayer();
        attackManager.InitializeAttack(currentAttack);
    }

    public override void OnStateExit()
    {
        
    }
}
