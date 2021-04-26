using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Item", menuName = "RPG/Items")]
[System.Serializable]
public class Item : ScriptableObject
{
    new public string name = "newItem";
    public Sprite icon;
    public Injection injection;
    public bool canTargetBoxer;
    public bool canTargetNurse;
    public bool canTargetBed;
    public bool canTargetCloset;
}


