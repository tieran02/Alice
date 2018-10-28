using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DamageZone : MonoBehaviour
{
    //Tickrate to deal damage
    public float Frequency = 1.0f;
    //damage amount
    public float Damage = 10.0f;
    //current ticks
    private float ticks = 0.0f;

    // Use this for initialization
    void Awake()
    {
        //set collider to trigger if not already
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        //get the characterData if attached to other
        CharacterData characterData = other.GetComponent<CharacterData>();
        //if characterData is null then return
        if (characterData == null)
            return;
        //add delta time to ticks
        ticks += Time.deltaTime;

        //if ticks is greater than Frequency deal damage and reset
        if (ticks >= Frequency)
        {
            characterData.TakeDamage(Damage);
            ticks = 0.0f;
        }
    }
}
