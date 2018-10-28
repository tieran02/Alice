using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour {
    //What Weapon to attack with
    public MeleeWeapon weapon;
    //The range the player must be within for the AI to attack
    public float AttackRange;
    //AIController component to get the distance to player
    private AIController aiController;

	void Awake ()
    {
        //assign the AI controller
        aiController = GetComponent<AIController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //check if the ai has deteced the player
        if (aiController.Detected)
        {
            //check if the player is withing attack range
            if(aiController.GetDistanceToPlayer() <= AttackRange)
            {
                //attack using the weapon assigned
                weapon.RandomAttack();
            }
        }
	}
}
