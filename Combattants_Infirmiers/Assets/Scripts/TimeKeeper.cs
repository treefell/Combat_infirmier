using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    public float myTime = 1f;



    //______________HITLAG__________________________
    public void Hitlag()
    {
        StopCoroutine("WaitHitlag"); // pour pas que ca reprenne trop tot quand 2 hitlag se superposent

        Vector2 myVel = Vector2.zero;
        //faudra retenir le time actuel pour gerer le hitlag pendant que le temps est ralenti, si besoin est, mais faudra aussi faire en sorte que ca retient pas un time de 0 si on a 2 hitlag qui se superposent. on verra plus tard.

        //on set les mytime partout à zero
        
        GetComponent<AttackManager>().myTime = 0;

        if (GetComponent<PlayerBase>() != null)
        {
            GetComponent<PlayerBase>().myTime = 0;
        }
        if (GetComponent<AIController>() != null)
        {
            GetComponent<AIController>().myTime = 0;
        }
        if (GetComponent<Rigidbody2D>() != null)
        {
            myVel = GetComponent<Rigidbody2D>().velocity;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        //on attend le temps du hitlag
        StartCoroutine(WaitHitlag(GameManagerPersistent.Instance.hitlagTime, myVel));
    }

    IEnumerator WaitHitlag(float waitingTime, Vector2 vel)
    {
        yield return new WaitForSeconds(waitingTime);

        HitlagEnd(vel);
    }

    public void HitlagEnd(Vector2 velocity)
    {

        //on restart les mytime partout à 1
        GetComponent<AttackManager>().myTime = 1;
        if (GetComponent<PlayerBase>() != null)
        {
            GetComponent<PlayerBase>().myTime = 1;
        }
        if (GetComponent<AIController>() != null)
        {
            GetComponent<AIController>().myTime = 1;
        }
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().velocity = velocity;
        }


        if (GetComponent<Projectile>() != null)
        {
            GetComponent<Projectile>().HitSomething();
        }
    }


    

    //____________________________________________________________

}
