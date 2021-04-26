using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStun : AIState
{
    private float waitingTime;

    public AIStun(AIController controller, float time)  : base(controller)
    {
        waitingTime = time;
    }

    public override void Tick()
    {
    }

    public override void OnStateEnter()
    {
        GameManagerPersistent.Instance.stunnedEnemies.Add(controller.gameObject);
        controller.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public override void OnStateExit()
    {
        GameManagerPersistent.Instance.stunnedEnemies.Remove(controller.gameObject);
        controller.GetComponent<SpriteRenderer>().color = Color.red;

    }


}
