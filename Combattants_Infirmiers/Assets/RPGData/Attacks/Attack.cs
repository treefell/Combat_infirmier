using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "newAttack", menuName = "RPG/Attack")]
[System.Serializable]
public class Attack : ScriptableObject
{
    new public string name = "newAttack";


    public Hit[] hits;
    public int totalFrames;


    //[HideInInspector]
    //public Hit currentHit;
    public Animation atkAnim;

    public bool detached;

    public bool ignoreRotation;


    public Attack nextAttack;
    public int cancellableFromFrame;
    public Vector2[] canMoveFrames;
    public bool autoCombo;
    public bool isFinisher;
    public bool isSkill;

    public bool unblockable;
    public bool hitboxLoops;

    public bool addMovement;
    [ShowIf("@this.addMovement == true")]
    public AttackMovement[] movement;
    public bool hitboxesIgnoreMovement;

    public bool addIntangibility; // not coded yet
    [ShowIf("@this.addIntangibility == true")]
    public Vector2 intangibilityFrames;

    public bool spawnObjects;
    [ShowIf("@this.spawnObjects == true")]
    public SpawnedObject[] objectsToSpawn;


    public bool parry = false;
    [ShowIf("@this.parry == true")]
    public Hit[] parryBoxes;
    [ShowIf("@this.parry == true")]
    public Vector2 perfectWindow = new Vector2(1f, 4f);


}


