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
                    playerBase.myAnimator.Play(playerBase.GetAnimName("IdleL"));
                    break;
                case 6:
                    playerBase.myAnimator.Play(playerBase.GetAnimName("IdleR"));
                    break;
                case 2:
                    playerBase.myAnimator.Play(playerBase.GetAnimName("IdleD"));
                    break;
                case 8:
                    playerBase.myAnimator.Play(playerBase.GetAnimName("IdleU"));
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

        RunIdleInputs();
        
        
       
        Movement();

        if (moveVector != Vector2.zero)
        {
            playerBase.SetMovementState(new Running(playerBase));
        }

        
        
    }
}
