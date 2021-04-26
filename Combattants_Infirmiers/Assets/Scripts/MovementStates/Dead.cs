using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MovementState
{
    Color baseColor;
    public Dead(PlayerBase playerBase) : base(playerBase)
    {
    }

    public override void Tick()
    {
        CheckInputs();
    }
    public override void OnStateEnter()
    {
        playerBase.canGetHit = false;
        playerBase.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        baseColor = playerBase.mySprite.color;
        playerBase.mySprite.color = Color.gray;
        GameManagerPersistent.Instance.deadPlayers += 1;
        //playerBase.GetComponent<Collider2D>().enabled = false;
    }

    public override void OnStateExit()
    {
        playerBase.canGetHit = true;
        playerBase.mySprite.color = baseColor;
        playerBase.GetComponent<Collider2D>().enabled = true;
        GameManagerPersistent.Instance.deadPlayers -= 1;
    }


    void CheckInputs()
    {

        //Movement();
        

    }

}
