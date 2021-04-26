using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodging : MovementState
{

    private Vector2 dodgeVector;
    private float currentTime;

    public Dodging(PlayerBase playerBase) : base(playerBase)
    {
    }

    public override void Tick()
    {
        if (currentTime >= character.dodgeTotalTime)
        {
            playerBase.SetMovementState(new Idle(playerBase));
            return;
        }
        rb.velocity = dodgeVector * playerBase.myTime;
        currentTime += Time.deltaTime*playerBase.myTime;        
    }
    

    public override void OnStateEnter()
    {
        currentTime = 0;
        rb.velocity = Vector2.zero;


        dodgeVector = new Vector2(Input.GetAxis(playerBase.GetButtonName("Horizontal")), Input.GetAxis(playerBase.GetButtonName("Vertical")));
        if (dodgeVector.magnitude == 0f)
        {
            dodgeVector = playerBase.transform.up;
        }
        dodgeVector*=character.dodgeSpeed;

        //playerBase.animator.SetBool("dashing", true);
    }

    public override void OnStateExit()
    {
        rb.velocity = Vector2.zero;
        //playerBase.animator.SetBool("dashing", false);
    }
}
