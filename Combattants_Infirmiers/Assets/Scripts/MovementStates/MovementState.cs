using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MovementState
{

    protected PlayerBase playerBase;
    protected Rigidbody2D rb;
    protected Character character;

    public AttackManager attackManager;
    public Attack baseAttack;
    public Attack interceptAttack;
    public Vector2 moveVector;
    public Vector2 rotVector;
    public Vector3 lookVec;

    public DistanceJoint2D joint;
    

    private bool twinSticks;

    
    public abstract void Tick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }


    public MovementState(PlayerBase playerBase)
    {
        this.playerBase = playerBase;
        this.rb = playerBase.GetComponent<Rigidbody2D>();
        this.character = playerBase.character;
        this.attackManager = playerBase.GetComponent<AttackManager>();
        this.baseAttack = character.defaultAttack;
        this.interceptAttack = character.interceptAttack;
        this.joint = playerBase.GetComponent<DistanceJoint2D>();
        this.twinSticks = playerBase.twinSticks;
    }

    public void Movement()
    {

       
        moveVector = new Vector2(Input.GetAxis(playerBase.GetButtonName("Horizontal")), Input.GetAxis(playerBase.GetButtonName("Vertical")));

        if (twinSticks)
        {
            rotVector = new Vector2(Input.GetAxis(playerBase.GetButtonName("RHorizontal")), Input.GetAxis(playerBase.GetButtonName("RVertical")));
            
        }
        else
        {
            rotVector = moveVector;
        }

        if (rotVector != Vector2.zero)
        {

            lookVec = new Vector3(rotVector.x, rotVector.y, 4096);
            //Debug.Log("lookvec : " + lookVec);
            playerBase.transform.rotation = Quaternion.LookRotation(lookVec, Vector3.back);
        }

        
        
        if (Mathf.Abs(rotVector.x) >= Mathf.Abs(rotVector.y))
        {
            if(rotVector!= Vector2.zero)
            {
                playerBase.sideways = true;
                if (rotVector.x < 0)
                {
                    playerBase.direction = 4;
                }
                else
                {
                    playerBase.direction = 6;
                }

            }
            

        }
        else
        {
            playerBase.sideways = false;
            if (rotVector.y < 0)
            {
                playerBase.direction = 2;
            }
            else
            {
                playerBase.direction = 8;
            }
        }

        
        


        if (playerBase is Nurse)
        {

        }
        else
        {
            if (playerBase.sideways && playerBase.direction == 4)
            {
                playerBase.mySprite.transform.localScale = new Vector3(-1, playerBase.mySprite.transform.localScale.y, playerBase.mySprite.transform.localScale.z);
            }
            else
            {
                playerBase.mySprite.transform.localScale = new Vector3(1, playerBase.mySprite.transform.localScale.y, playerBase.mySprite.transform.localScale.z);
            }

            if (!playerBase.sideways && playerBase.direction == 2)
            {
                playerBase.mySprite.transform.localScale = new Vector3(playerBase.mySprite.transform.localScale.x, -1, playerBase.mySprite.transform.localScale.z);
            }
            else
            {
                playerBase.mySprite.transform.localScale = new Vector3(playerBase.mySprite.transform.localScale.x, 1, playerBase.mySprite.transform.localScale.z);
            }
            
                
        }

    }

    public void RunIdleInputs()
    {
        if (Input.GetButtonDown(playerBase.GetButtonName("RB")))
        {
            Attack(baseAttack);
            return;
        }
        else if (Input.GetAxis(playerBase.GetButtonName("LTrigger")) >= 0.5f && playerBase is Nurse && playerBase.canRez)
        {
            Combine();
            return;
        }
        /*else if (Input.GetButtonDown(playerBase.GetButtonName("LB")))
        {
            Dodge();
            return;
        }*/
        else if (Input.GetAxis(playerBase.GetButtonName("RTrigger")) >= 0.5f && !playerBase.RTPressed)
        {
            playerBase.RTPressed = true;
            if (playerBase is Nurse)
            {
                if ((playerBase as Nurse).canGrab)
                {
                    (playerBase as Nurse).PickUp();
                }
                else if ((playerBase as Nurse).canInject)
                {
                    (playerBase as Nurse).Inject((playerBase as Nurse).heldItem.injection, (playerBase as Nurse).injectZone.GetTarget().GetComponent<PlayerBase>(), (playerBase as Nurse).injectZone);
                }
            }


        }
    }

    

    public void Attack(Attack attack)
    {
        if (playerBase is Nurse)
        {
            if ((playerBase as Nurse).attackList.Count > 0)
            {
                playerBase.SetMovementState(new Attacking(playerBase, (playerBase as Nurse).attackList[0]));
            }
            
        }
        else
        {
            playerBase.SetMovementState(new Attacking(playerBase, attack));
        }
    }


    public void Dodge()
    {

        playerBase.SetMovementState(new Dodging(playerBase));
    }

    public void Combine()
    {
        playerBase.SetMovementState(new Combine(playerBase));
    }

    public void Block()
    {
        playerBase.SetMovementState(new Blocking(playerBase));
        
    }

}
