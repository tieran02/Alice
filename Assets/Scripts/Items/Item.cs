using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic", fileName = "Generic File Name")]
[System.Serializable]
public class Item : ScriptableObject
{
    //Scriptable object to hold data about a item
    public string Name;
    public string Description;
    public Sprite Sprite;
    public GameObject Prefab;
    public GameObject DropedItem;
    public int StackSize;
    public bool QuestItem;
}
