using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocking : MovementState
{
    public Blocking(PlayerBase playerBase) : base(playerBase)
    {
    }

    public override void Tick()
    {
        CheckInputs();
    }
    public override void OnStateEnter()
    {
        playerBase.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //playerBase.blockBox.GetComponent<Collider2D>().enabled = true;
    }

    public override void OnStateExit()
    {
        //playerBase.blockBox.GetComponent<Collider2D>().enabled = false;
    }


    void CheckInputs()
    {

        Movement();
        if (Input.GetButtonUp(playerBase.GetButtonName("LB")))
        {
            playerBase.SetMovementState(new Idle(playerBase));
        }

    }

    }
