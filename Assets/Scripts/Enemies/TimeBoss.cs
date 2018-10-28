using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class TimeBoss : MonoBehaviour {
    //Boss weapons
    public MeleeWeapon MeleeWeapon;
    public ProjectileWeapon StaffWeapon;
    public ForceField Shield;
    //Melee range of boss
    public float MeleeRange = 20;
    //Projectile range of boss
    public float StaffRange = 40;

    //Health stages of the boss with the amount of health for each stage
    public float[] HealthStages;
    //the current stage the boss is one
    private int currentStage;

    //component variables
    private AIController aIController;
    private CharacterData characterData;
    //Player's transform
    private Transform playerTransform;
    //cooldown time for charge to be used
    private bool chargeCooldown;
    //is the boss currently Charging
    private bool currentlyCharging;

    void Awake()
    {
        //assign component variables
        aIController = GetComponent<AIController>();
        characterData = GetComponent<CharacterData>();
        //Find and set the players transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //set current stage to the start
        currentStage = 0;
        //Set AI max health to current health stage 
        characterData.SetMaxHealth(HealthStages[currentStage]);
        //add the OnDeath method to the characters death event
        characterData.OnDeathEvent += OnDeath;

        chargeCooldown = false;
    }

	// Update is called once per frame
	void Update ()
    {
        //Attack with melee weapon if within melee range and shield is not active
        if (aIController.GetDistanceToPlayer() <= MeleeRange && !Shield.Active)
            MeleeWeapon.RandomAttack();
        //else if the within melee rance and staff range and is past the second stage use staff
        else if(aIController.GetDistanceToPlayer() >= MeleeRange && aIController.GetDistanceToPlayer() <= StaffRange && currentStage >= 1)
        {
            //randomly use staff if shield is not active
            if(Random.value > .8f && !Shield.Active)
                StaffWeapon.Attack(playerTransform); //attack with staff and pass player Transform so the projectiles follow the player
        }

        //if stage 3 is active then use the shield
        if(currentStage >= 2)
        {
            Shield.Use();
        }
        if (Shield.Active || currentlyCharging) // if shield is active stop movement
            aIController.Stop();
        else
            aIController.Start();

        //if on stage 4 charge
        if(currentStage >= 3 && !chargeCooldown)
        {
            StartCoroutine(Charge(transform, transform.position, playerTransform.position, 50f));
        }

        // check if health is less than 0 and current stage is less than the amount of stages
        if(characterData.CurrentHealth <= 0.0f && currentStage < HealthStages.Length)
        {
            //enter next stage
            currentStage++;

            //alert the plater of next stage
            AlertManager.GetInstance().AddAlert("Stage " + (currentStage+1) + " about to start!");
            //set the bosses health to next stage and heal him
            characterData.SetMaxHealth(HealthStages[currentStage]);
            characterData.Heal(HealthStages[currentStage]);
        }
        else if(characterData.CurrentHealth <= 0.0f && currentStage >= HealthStages.Length)
        {
            //if the boss dies and it and is on the last stage kill the boss
            characterData.Kill();
        }

    }

    private IEnumerator Charge(Transform objectToMove, Vector3 a, Vector3 b, float speed)
    {
        //set currently charging and the cooldown to true
        currentlyCharging = true;
        chargeCooldown = true;

        //Notify player and wait .75 seconds
        AlertManager.GetInstance().AddAlert("Time is about to charge! Get out the way!");
        yield return new WaitForSeconds(.75f);

        //calcuate the time step to lerp between postions
        float timeStep = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float currentTime = 0; //current time 0-1
        //while current time has not reached the end, lerp postion
        while (currentTime <= 1.0f)
        {
            //add time step to current time
            currentTime += timeStep; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, currentTime); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         //wait for the nexted fixed update to continue lerp
        }
        //set the postion to the target at the end as it won't be exactly correct due to floating point precision 
        objectToMove.position = b;
        //set currently charing to false
        currentlyCharging = false;
        //damge player if within a distance
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= 5)
            playerTransform.GetComponent<CharacterData>().TakeDamage(40);
        //in 10 seconds reset charge
        Invoke("ResetCharge",10f);
    }

    void ResetCharge()
    {
        //set charge cooldown back to false
        chargeCooldown = false;
    }

    void OnDeath()
    {
        //when the boss dies load the game over scene
        SceneManager.LoadScene("GameOver");
    }
}
