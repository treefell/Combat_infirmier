using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combine : MovementState
{

    

    public Combine(PlayerBase playerBase) : base(playerBase)
    {
        
    }

    public override void Tick()
    {

        Movement();

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
        

        if (/*Input.GetAxis(playerBase.GetButtonName("LTrigger")) < 0.5f*/Input.GetButtonUp(playerBase.GetButtonName("Y")))
        {
            Drop();
            playerBase.SetMovementState(new Idle(playerBase));
            
            return;
        }
        rb.velocity = new Vector2(moveVector.x, moveVector.y).normalized * character.runSpeed * playerBase.myTime;
            
            
    }
    

    public override void OnStateEnter()
    {

        //character.runSpeed *= 2;

        //rb.velocity = Vector2.zero;

        playerBase.dragging = true;
        joint.enabled = true;



    }

    public override void OnStateExit()
    {
        playerBase.dragging = false;
        rb.velocity = Vector2.zero;
        //character.runSpeed /= 2;
        joint.enabled = false;
    }

    void Drop()
    {
        if (playerBase.canDrop)
        {
            playerBase.canRez = false;
            playerBase.otherChar.GetComponent<PlayerBase>().StartHeal();
            
        }
    }
    
}
