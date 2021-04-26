using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MovementState
{
    private Attack currentAttack;
    public Attacking(PlayerBase playerBase, Attack attack) : base(playerBase)
    {
        currentAttack = attack;
    }

    public override void Tick()
    {
        //Debug.Log(playerBase.GetComponent<Rigidbody2D>().velocity);
        if (attackManager.canMove)
        {
            CheckInputs();
        }
    }
    public override void OnStateEnter()
    {
        //playerBase.animator.SetBool("attacking", true);
        playerBase.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (playerBase.sideways)
        {
            playerBase.myAnimator.Play("Base Layer.FighterHitH");
        }
        else
        {
            playerBase.myAnimator.Play("Base Layer.FighterHitV");
        }

        attackManager.InitializeAttack(playerBase.currentAttack);
    }

    public override void OnStateExit()
    {
        //playerBase.animator.SetBool("attacking", false);
    }


    void CheckInputs()
    {

        Movement();
        if (Input.GetButtonDown(playerBase.GetButtonName("LB")))
        {
            Dodge();
        }
        

        

    }
}
