using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "newChar", menuName = "RPG/Character")]
[System.Serializable]
public class Character : ScriptableObject
{
   
    [Title("Stats")]
    //public int startingLevel;
    public int baseHp;
    public int baseStrength;

    [Title("Run and air movement")]
    public float runSpeed;


    [Title("Dodge")]
    public float dodgeSpeed;
    //public float dodgeLength;
    public float dodgeTotalTime;

    [Title("Attacks")]
    public Attack defaultAttack;
    public Attack interceptAttack;
    public Attack[] attackList;
    //public LearnedAttack[] learnedAttacks;
}
