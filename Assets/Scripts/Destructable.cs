using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {
    //Health of the Destructable
    public float Health;
    //current health
    private float currentHealth;

    void Awake()
    {
        //set current health to health
        currentHealth = Health;
    }

    public void TakeDamage(float amount)
    {
        //take damage
        currentHealth -= amount;
        //check if current health is below 0 and if so destroy the object
        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}
