using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunned : MovementState
{

    float currentStunTime = 0f;
    Color baseColor;
    public Stunned(PlayerBase playerBase) : base(playerBase)
    {
    }

    public override void Tick()
    {
        currentStunTime += Time.deltaTime*playerBase.myTime;
        if (currentStunTime >= GameManagerPersistent.Instance.playerStunTime)
        {
            playerBase.SetMovementState(new Idle(playerBase));
        }
    }
    public override void OnStateEnter()
    {
        playerBase.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        playerBase.GetComponent<AttackManager>().AttackInterrupt();
        currentStunTime = 0f;
        playerBase.canGetHit = false;

        baseColor = playerBase.mySprite.color;
        playerBase.mySprite.color = Color.yellow;
    }

    public override void OnStateExit()
    {

        playerBase.canGetHit = true;
        playerBase.mySprite.color = baseColor;
        playerBase.GetComponent<Fighter>().currentHp = playerBase.character.baseHp;
    }
}
