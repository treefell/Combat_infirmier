using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInHitstun : AIState
{

    float currentHitstunTime = 0f;
    Color initialColor;

    public AIInHitstun(AIController controller)  : base(controller)
    {
        
    }

    


    public override void Tick()
    {
        currentHitstunTime += Time.deltaTime*controller.myTime;
        if (currentHitstunTime >= GameManagerPersistent.Instance.enemyHitstunTime)
        {
            controller.NewAction();
        }
    }
    public override void OnStateEnter()
    {
        controller.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        controller.GetComponent<AttackManager>().AttackInterrupt();
        currentHitstunTime = 0f;
        initialColor = controller.GetComponent<SpriteRenderer>().color;

        controller.GetComponent<SpriteRenderer>().color = Color.black;
    }

    public override void OnStateExit()
    {
        controller.GetComponent<SpriteRenderer>().color = initialColor;
    }


}
