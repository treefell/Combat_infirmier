using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//public enum PlayableChar {Alvis, Ash, Scarlet, Mia};
public enum HitboxShape { Cube, Sphere };
public enum ColliderState { Closed, Open, Colliding };
public enum Iris {Red, Blue, Yellow, Green, Grey};

public enum Injection { Hyperactive, Hadoken, Sabotage };



[System.Serializable]
public struct Hitbox
{
    
    private ColliderState _state;

    public Vector2 frames;
    
    [EnumToggleButtons]
    public HitboxShape _shape;
    public Vector2 size;
    public Vector2 position;

    public int damageOverride;
    public LayerMask layersOverride;

    public bool unblockable;
}

[System.Serializable]
public struct Hit
{
    //new public string name = "newHit";
    public Hitbox[] hitboxes;
    //public LayerMask layersToHit;
    public bool friendlyFire;

    public int damage;

    //public int totalFrames;



    public bool unblockable;


    [HideInInspector]
    public List<Collider2D> hitExceptions;
}


[System.Serializable]
public struct LearnedAttack
{
    public int level;
    public Attack attack;

}

[System.Serializable]
public struct SpawnedObject
{
    public GameObject objectToSpawn;
    public int frame;
    public Vector3 position;
    public Quaternion rotation;
    public bool ignoreRotation;
}

[System.Serializable]
public struct AttackMovement
{
    public Vector2 frames;
    public Vector2 targetPosition;
    public bool absolute;
}


[System.Serializable]
public struct EnemyWave
{
    public List<GameObject> enemies;
}

