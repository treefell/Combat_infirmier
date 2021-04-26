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
                    playerBase.myAnimator.Play(playerBase.GetAnimName("WalkL"));
                    break;
                case 6:
                    playerBase.myAnimator.Play(playerBase.GetAnimName("WalkR"));
                    break;
                case 2:
                    playerBase.myAnimator.Play(playerBase.GetAnimName("WalkD"));
                    break;
                case 8:
                    playerBase.myAnimator.Play(playerBase.GetAnimName("WalkU"));
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

       RunIdleInputs();
       



        Movement();

        if (moveVector == Vector2.zero)
        {
            playerBase.SetMovementState(new Idle(playerBase));
        }




    }

}

    


