using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
    protected AIController controller;
    public Vector3 lookVec;
    public float runSpeed;
    public Vector2 moveVector;
    public float distToTarget;

    public abstract void Tick();

    public virtual void OnStateEnter()
    {
    }
    public virtual void OnStateExit()
    {
    }

    public AIState(AIController aiController)
    {
        this.controller = aiController;
    }

    public void TurnTowardsPlayer()
    {
        if (controller.currentTarget == null) { return; }
        moveVector = Vector3.Normalize(controller.currentTarget.transform.position - controller.transform.position);
        moveVector = new Vector2(moveVector.x, moveVector.y);

        if (moveVector != Vector2.zero)
        {
            lookVec = new Vector3(moveVector.x, moveVector.y, 4096);
            //Debug.Log("lookvec : " + lookVec);
            controller.transform.rotation = Quaternion.LookRotation(lookVec, Vector3.back);
        }
    }

    public void Chase()
    {
        TurnTowardsPlayer();

        controller.transform.position += (Vector3)moveVector * runSpeed * Time.deltaTime*controller.myTime;

    }
    
 }
