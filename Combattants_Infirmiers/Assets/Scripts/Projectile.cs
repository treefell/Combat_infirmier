using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AttackManager
{
    public float initialSpeed;
    public float duration = 10;

    public bool homing = false;
    public float delayHoming = 1f;
    private bool delayedHoming;
    public Transform target;

    [HideInInspector]
    public GameObject originator;

    private float currentTime=0f;

    // le start est archi sale vu qu'on copie colle celui de l'attack manager, à nettoyer plus tard
    void Start()
    {
        if (this.GetComponent<PlayerBase>() != null)
        {
            SetPlayer(true, this.GetComponent<PlayerBase>());
        }
        else if (this.GetComponent<AIController>() != null)
        {
            SetAI(true, this.GetComponent<AIController>());
        }

        //defines which layers are friend and foe
        friendLayers = GameManagerPersistent.Instance.friends;
        foeLayers = GameManagerPersistent.Instance.foes;

        circleHitbox = GameManagerPersistent.Instance.circleHitbox;
        squareHitbox = GameManagerPersistent.Instance.squareHitbox;


        //truc de projo
        if (homing)
        {
            if (delayHoming > 0f)
            {
                homing = false;
                delayedHoming = true;
            }
            else
            {
                SetTarget();
            }
            
        }
        


        InitializeAttack(currentAttack);

        if(!homing)
        {
            
            this.GetComponent<Rigidbody2D>().velocity = initialSpeed * transform.up*myTime;
        }
        
        StartCoroutine(DestroyAfterTime());
    }

    private void Update()
    {
        if (delayedHoming)
        {

            if (delayHoming > currentTime)
            {
                currentTime += Time.deltaTime * myTime;
            }
            else
            {
                homing = true;
                SetTarget();
                delayedHoming = false;
            }
        }

        
        
    }

    void SetTarget()
    {

        if (originator.layer == LayerMask.NameToLayer("TeamA"))
        {
            target = GameManagerPersistent.Instance.BoxerB.transform;
        }
        else
        {
            target = GameManagerPersistent.Instance.BoxerA.transform;
        }

    }


    public void MoveTowardsTarget()
    {
        if (target == null)
        {
            SetTarget();
        }
        
        this.GetComponent<Rigidbody2D>().velocity = initialSpeed * (target.position - transform.position).normalized*myTime;
    }

    public void TargetLost()
    {
        homing = false;
        this.GetComponent<Rigidbody2D>().velocity = initialSpeed * transform.up * myTime;
    }

    public void HitSomething()
    {
        if (!piercing)
        {
            Destroy(this.gameObject);
        }
    }


    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }


    
}
