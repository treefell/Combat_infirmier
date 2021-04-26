using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : MovementState
{
    

    public Running(PlayerBase playerBase) : base(playerBase)
    {
    }

    public override void Tick()
    {
        CheckInputs();
        rb.velocity = new Vector2(moveVector.x, moveVector.y).normalized*character.runSpeed * playerBase.myTime;

        if (playerBase is Nurse)
        {
            switch (playerBase.direction)
            {
                case 4:
                    playerBase.myAnimator.Play("Base Layer.WalkL");
                    break;
                case 6:
                    playerBase.myAnimator.Play("Base Layer.WalkR");
                    break;
                case 2:
                    playerBase.myAnimator.Play("Base Layer.WalkD");
                    break;
                case 8:
                    playerBase.myAnimator.Play("Base Layer.WalkU");
                    break;

            }
        }
        else
        {
            if (playerBase.sideways)
            {
                playerBase.myAnimator.Play("Base Layer.WalkH");

            }
            else
            {
                playerBase.myAnimator.Play("Base Layer.WalkV");

            }
        }

    }
    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExit()
    {
        //playerBase.animator.SetBool("running", false);
    }

    void CheckInputs()
    {

        
       if (Input.GetButtonDown(playerBase.GetButtonName("RB")))
        {
            Attack(baseAttack);
            return;
        }
        else if (/*Input.GetAxis(playerBase.GetButtonName("LTrigger")) >= 0.5f*/Input.GetButtonDown(playerBase.GetButtonName("Y")) && playerBase is Nurse && playerBase.canRez)
        {
            Combine();
            return;
        }
        else if (Input.GetButtonDown(playerBase.GetButtonName("LB")))
        {
            Dodge();
            return;
        }
        else if (Input.GetAxis(playerBase.GetButtonName("RTrigger")) >= 0.5f && playerBase is Nurse)
        {
            if ((playerBase as Nurse).canGrab)
            {
                (playerBase as Nurse).PickUp();
            }
            else if ((playerBase as Nurse).canInject)
            {
                (playerBase as Nurse).Inject((playerBase as Nurse).heldItem.injection, (playerBase as Nurse).injectZone.GetTarget().GetComponent<PlayerBase>(), (playerBase as Nurse).injectZone);
            }

        }



        Movement();

        if (moveVector == Vector2.zero)
        {
            playerBase.SetMovementState(new Idle(playerBase));
        }




    }

}

    


