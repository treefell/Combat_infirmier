using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : MovementState
{
    
    public Idle(PlayerBase playerBase) : base(playerBase)
    {
    }

    public override void Tick()
    {
        CheckInputs();
        //playerBase.transform.rotation = Quaternion.LookRotation(lookVec, Vector3.back);
        //rb.velocity = new Vector2(character.runSpeed * moveVector.x, rb.velocity.y);

        if (playerBase is Nurse)
        {
            switch (playerBase.direction)
            {
                case 4:
                    playerBase.myAnimator.Play("Base Layer.IdleL");
                    break;
                case 6:
                    playerBase.myAnimator.Play("Base Layer.IdleR");
                    break;
                case 2:
                    playerBase.myAnimator.Play("Base Layer.IdleD");
                    break;
                case 8:
                    playerBase.myAnimator.Play("Base Layer.IdleU");
                    break;

            }
        }
        else
        {
            if (playerBase.sideways)
            {
                playerBase.myAnimator.Play("Base Layer.IdleH");
            }
            else
            {
                playerBase.myAnimator.Play("Base Layer.IdleV");
            }
        }
    }
    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExit()
    {
        
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
            Debug.Log((playerBase as Nurse).canInject);
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

        if (moveVector != Vector2.zero)
        {
            playerBase.SetMovementState(new Running(playerBase));
        }
        
        
    }
}
