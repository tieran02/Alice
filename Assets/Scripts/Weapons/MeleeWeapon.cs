using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTypes
{
    Stab,
    Slash,
    Chop
}
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class MeleeWeapon : MonoBehaviour {
    //Name of the weapon
    public string Name;
    //damage of the weapon
    public float Damage = 20;
    //attack speed
    public float AttackSpeed = 1.0f;
    //Owner of the weapon (used to make sure you dont shoot yourself)
    public GameObject Owner;
    //Sword animator
    public Animator animator;
    //Is the weapon on cooldown?
    public bool Cooldown { get; private set; }
    private float currentTime = 0;

    private BoxCollider boxCollider;
    private AudioSource audioSource;

    void Awake()
    {
        //get the box collider and set trigger to true and disable the collider
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        //check if the weapon is on cooldown
        if (Cooldown)
        {
            //if current time is greater than attack speed reset cooldown
            if (currentTime > AttackSpeed)
            {
                Cooldown = false;
                currentTime = 0;
            }
            else //else add dealt time to current
                currentTime += Time.deltaTime; 
        }
        //check if the owner is the player
        if(Owner.tag == "Player")
        {
            //if player and left click whislt not on cooldown then do a random attack
            if (Input.GetMouseButtonDown(0) && !Cooldown)
                RandomAttack();
        }
	}

    public void Attack(AttackTypes attackType)
    {
        //check if not on cooldown
        if (!Cooldown)
        {
            //play sound effect if exists
            if (audioSource != null && audioSource.clip != null)
                audioSource.Play();
            //enable the box collider
            boxCollider.enabled = true;
            //make the animation do the correct attack type
            switch (attackType)
            {
                case AttackTypes.Stab:
                    Cooldown = true;
                    animator.SetTrigger("Stab");
                    break;
                case AttackTypes.Slash:
                    Cooldown = true;
                    animator.SetTrigger("Slash");
                    break;
                case AttackTypes.Chop:
                    Cooldown = true;
                    animator.SetTrigger("Chop");
                    break;
                default:
                    break;
            }
            //get the current animation time and invoke DisableCollider after that time
            float animationTime = GetCurrentAnimationTime();
            Invoke("DisableCollider", animationTime);
        }
    }

    public void RandomAttack()
    {
        //random number between 0 and 3
        int randomAttack = Random.Range(0, 3);

        //depending on randomAttack attack using that attack tpye
        if (randomAttack == 0)
            Attack(AttackTypes.Chop);
        else if (randomAttack == 1)
            Attack(AttackTypes.Slash);
        else if (randomAttack == 2)
            Attack(AttackTypes.Stab);
    }

    void OnTriggerEnter(Collider collision)
    {
        //make sure the weapon does not hit the owner
        if (collision.gameObject == Owner)
            return;
        //get the character data of the collison
        CharacterData characterData = collision.gameObject.GetComponent<CharacterData>();
        if(characterData != null) // check if it is not null
        {
            //Deal damage and disable the collider
            characterData.TakeDamage(Damage);
            boxCollider.enabled = false;
        }
    }

    //Get the current running animation length
    float GetCurrentAnimationTime()
    {
        AnimatorStateInfo currInfo = animator.GetCurrentAnimatorStateInfo(0);
        return currInfo.length;
    }
    //Disable the colidr
    void DisableCollider()
    {
        boxCollider.enabled = false;
    }
}
