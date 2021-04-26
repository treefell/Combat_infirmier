using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHitstun : MovementState
{

    float currentHitstunTime = 0f;
    Color baseColor;
    public InHitstun(PlayerBase playerBase) : base(playerBase)
    {
    }

    public override void Tick()
    {
        Debug.Log(playerBase.myTime);
        currentHitstunTime += Time.deltaTime*playerBase.myTime;
        if (currentHitstunTime >= GameManagerPersistent.Instance.playerHitstunTime)
        {
            if (playerBase.wasCombined)
            {
                playerBase.SetMovementState(new Combine(playerBase));
                playerBase.wasCombined = false;
            }
            else
            { playerBase.SetMovementState(new Idle(playerBase)); }
        }
    }
    public override void OnStateEnter()
    {
        playerBase.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        playerBase.GetComponent<AttackManager>().AttackInterrupt();
        currentHitstunTime = 0f;

        baseColor = playerBase.mySprite.color;
        playerBase.mySprite.color = Color.black;
    }

    public override void OnStateExit()
    {
        playerBase.mySprite.color = baseColor;
    }
}
