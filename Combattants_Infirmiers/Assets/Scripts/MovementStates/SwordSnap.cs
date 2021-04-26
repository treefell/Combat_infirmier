using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSnap : MovementState
{

    private Transform target;
    private int currentFrame;
    private float distTreshold = 1.5f;

    private LayerMask mask;

    public SwordSnap(PlayerBase playerBase) : base(playerBase)
    {
    }

    public override void Tick()
    {
        rb.velocity = (Vector2)(target.position - playerBase.transform.position)*20f*playerBase.myTime;
        //Movement();
        if (target != null)
        {
            lookVec = new Vector3(rb.velocity.x, rb.velocity.y, 4096);
            Debug.Log("lookvec : " + lookVec);
            playerBase.transform.rotation = Quaternion.LookRotation(lookVec, Vector3.back); ;
        }
        HandleArrival();
        
    }
    

    public override void OnStateEnter()
    {
        mask = LayerMask.GetMask("Enemies", "Default");
        rb.velocity = Vector2.zero;

        float shortestDist = Mathf.Infinity;
        foreach (GameObject o in GameManagerPersistent.Instance.stunnedEnemies)
        {
            float currentDist = Vector2.Distance(o.transform.position, playerBase.transform.position);
            if (currentDist < shortestDist)
            {
                shortestDist = currentDist;
                target = o.transform;
            }
        }

        RaycastHit2D rayhit = Physics2D.Raycast(playerBase.transform.position, target.position - playerBase.transform.position, Mathf.Infinity, mask);
        target = rayhit.transform;
        Debug.Log(mask.value);
    }

    public override void OnStateExit()
    {
        rb.velocity = Vector2.zero;


        //playerBase.gameObject.layer = LayerMask.NameToLayer("Allies");

        //playerBase.animator.SetBool("dashing", false);
    }

    void HandleArrival()
    {
        if (Vector2.Distance(playerBase.transform.position, target.position) < distTreshold)
        {


            Attack(interceptAttack);

            return;
        }
    }
}
