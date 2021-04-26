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
}


