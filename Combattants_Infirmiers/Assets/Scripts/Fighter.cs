using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Fighter : MonoBehaviour
{
    //public Iris irisColor;

    //life and death
    public int hp = 100;
    //public int mp = 50;
    public int strength = 5;
    

    public GameObject dmgPopup;
    
    public int currentHp;
    //private int currentMp;
    private int currentStrength;

    private Collider2D col;

    private bool isPlayer = false;
    private PlayerBase playerBase;
    private Character character;
    private bool isEnemy = false;
    private AIController aiController;

    public int blockPercentage = 0;

    private GameObject lastHitBy;
    public Attack taughtAttack;

    [Title("ForRandomizer")]
    public int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<PlayerBase>())
        {
            isPlayer = true;
            playerBase = this.GetComponent<PlayerBase>();
            character = playerBase.character;
            InitializeStats(character.baseHp, character.baseStrength);
        }
        else if (this.GetComponent<AIController>())
        {
            isEnemy = true;
            aiController = this.GetComponent<AIController>();
            InitializeStats(hp, strength);
        }

        
        col = this.GetComponent<Collider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    

    public void InitializeStats(int maxHp, int str)
    {
        hp = maxHp;
        currentHp = maxHp;
        currentStrength = str;
    }

    void UpdateStats()
    {

    }

   

    //damage
    public Collider2D TakeDamage(int damage, GameObject origin)
    {
        //afficher le popup de degats
        if (!playerBase.canGetHit)
        { return col; }

        currentHp -= damage;
        if (currentHp > character.baseHp)
        {
            currentHp = character.baseHp;
        }
        else if (currentHp < 0)
        {
            currentHp = 0;
        }


        GameObject dmg = Instantiate(dmgPopup);
        dmg.transform.position = this.transform.position + (Vector3)dmg.GetComponent<DamagePopup>().offset;
        dmg.GetComponent<DamagePopup>().SetTxt(Mathf.Abs(damage).ToString());
        if (damage < 0)
        {
            dmg.GetComponent<DamagePopup>().SetColor(Color.green);
        }
        else
        {
            if (isPlayer)
            {
                playerBase.GetHit(origin);
            }
            else if (isEnemy)
            {
                aiController.GetHit(origin);
            }

            if (origin.GetComponent<Projectile>() != null)
            {
                lastHitBy = origin.GetComponent<Projectile>().originator;

            }
            else
            {
                lastHitBy = origin;
            }
        }


        


        


        CheckDeath();
        return (col);
        
    }

    public Collider2D Block(int damage)
    {
        int finalDamage;

        finalDamage = damage * blockPercentage / 100;

        if (finalDamage != 0)
        {
            //afficher le popup de degats
            /*GameObject dmg = Instantiate(dmgPopup);
            dmg.transform.position = this.transform.position + (Vector3)dmg.GetComponent<DamagePopup>().offset;
            dmg.GetComponent<DamagePopup>().SetTxt(finalDamage.ToString());
            if (finalDamage < 0)
            {
                dmg.GetComponent<DamagePopup>().SetColor(Color.green);
            }
            */
            currentHp -= finalDamage;
        }
        
        if (isPlayer)
        {
            // block hitlag here maybe
        }
        else if (isEnemy)
        {
            //aiController.GetHit();
        }
        Debug.Log("blocked! my current hp : " + currentHp);
        CheckDeath();
        return (col);

    }

    void CheckDeath()
    { 
        if(currentHp <=0)
        {
            if (isPlayer)
            {
                playerBase.Death();

            }
            else if (isEnemy)
            {
                
                aiController.Death();
            
            }
        }
    }




    public int GetCurrentHp()
    {
        return currentHp;
    }

    public void Popup(string text)
    {
        GameObject popup = Instantiate(dmgPopup);
        popup.transform.position = this.transform.position + (Vector3)popup.GetComponent<DamagePopup>().offset;
        popup.GetComponent<DamagePopup>().SetTxt(text);
        popup.GetComponent<DamagePopup>().SetColor(Color.yellow);
    }
}
