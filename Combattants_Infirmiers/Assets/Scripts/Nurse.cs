using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurse : PlayerBase
{
    [HideInInspector]
    public Item heldItem;
    [HideInInspector]
    public List<Attack> attackList = new List<Attack>();
    [HideInInspector]
    public bool canGrab = false; //for items in closet
    [HideInInspector]
    public GrabZone armoire;
    
    public SyringeZone injectZone;
    [HideInInspector]
    public bool canInject;

    public void Inject(Injection injection, PlayerBase target, SyringeZone zone)
    {
        switch (injection)
        {
            case Injection.Hyperactive:
                target.LevelUp(1);
                break;
            case Injection.Hadoken:
                target.LevelUp(2);
                break;
            case Injection.Sabotage:
                target.Sabotaged();
                break;
            case Injection.Heal:
                target.GetComponent<Fighter>().TakeDamage(-GameManagerPersistent.Instance.healValue, this.gameObject);
                break;
        }

        tiredness += 1;
        injectZone.gameObject.SetActive(false);
        heldItem = null;
        canInject = false;
        GameManagerPersistent.Instance.UpdateSyringe();
        zone.playersInZone.Clear();
        zone.gameObject.SetActive(false);
    }

    public void PickUp()
    {
        injectZone.gameObject.SetActive(true);
        
        if (heldItem == null)
        {
            heldItem = armoire.healItem;
            armoire.EmptyShelf();
        }
        else if (heldItem.injection == Injection.Crate)
        {
            heldItem = armoire.items[(int)Random.Range(0, armoire.items.Length - 1)];
            
        }


        GameManagerPersistent.Instance.UpdateSyringe();
        canGrab = false;
        armoire.message.SetActive(false);
    }

    public void GetCrate(Item crate)
    {
        heldItem = crate;
        GameManagerPersistent.Instance.UpdateSyringe();
    }

}
