using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour {
    //What Weapon to attack with
    public Gun weapon;
    //The range the player must be within for the AI to attack
    public float AttackRange;
    //AIController component to get the distance to player
    private AIController aiController;

    void Awake()
    {
        aiController = GetComponent<AIController>();
        //half AI gun damage to balence
        weapon.Damage /= 2;
    }

    // Update is called once per frame
    void Update()
    {
        //check if the ai has deteced the player
        if (aiController.Detected)
        {
            //check if the player is withing attack range
            if (aiController.GetDistanceToPlayer() <= AttackRange)
            {
                if (Random.value < .1f) // make the AI shoot un random bursts
                    weapon.Fire();
            }
        }
    }
}
