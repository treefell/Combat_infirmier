using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MovementState
{
    Transform thisBed;
    Vector3 previousPos;
    public Healing(PlayerBase playerBase, Transform bed) : base(playerBase)
    {
        thisBed = bed;
    }

    public override void Tick()
    {
        playerBase.Heal();
    }
    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExit()
    {
        
    }


    

    }
