using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Types of fire modes
public enum FireType
{
    Auto,
    Burst,
    Single
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour {
    //Name of the gun
    public string Name;
    //Damage of the gun on hit
    public float Damage = 10;
    //Fire rate of the gun
    public float FireRate = 0.2f;
    //Max ammo of the gun
    public int MaxAmmo = 10;
    //How long it takes to reload the gun
    public float ReloadTime = 1;
    //Range of the gun
    public float Range = 100;
    //Firetype of the gun
    public FireType Type;
    //Is the weapon on cooldown?
    public bool Cooldown { get; private set; }
    //Get the CurrentAmmo amount
    public int CurrentAmmo { get; private set; }
    //Impact prefab
    public GameObject ImpactPrefab;
    //Muzzle flash prefab
    public ParticleSystem MuzzleFlash;
    //gun shot sound
    private AudioSource weaponAudio;
    //Owner of the weapon (used to make sure you dont shoot yourself)
    public GameObject Owner;

    void Awake()
    {
        //get the weapon audiosource
        weaponAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        //set the current ammo to max ammo
        CurrentAmmo = MaxAmmo;
    }

    // Update is called once per frame
    void Update ()
    {
        //check if the owner is the player
        if (Owner.tag == "Player")
        {
            //if left click and the firetype is auto and not on cooldown fire the gun in automatic mode
            if (Input.GetMouseButton(0) && !Cooldown && Type == FireType.Auto)
            {
                Fire();
            }
            //if left click and the firetype is single and not on cooldown fire the gun in single mode
            else if (Input.GetMouseButtonDown(0) && !Cooldown && Type == FireType.Single)
            {
                Fire();
            }
            //if left click and the firetype is burst and not on cooldown fire the gun in burst mode
            else if (Input.GetMouseButtonDown(0) && !Cooldown && Type == FireType.Burst)
            {
                StartCoroutine(FireBurst(3));
            }
            //Reload the gun
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
        }
    }

    public void Fire()
    {
        //if the weapon is on cooldown return
        if (Cooldown)
            return;

        //check if there is ammo in the gun
        if (CurrentAmmo > 0)
        {
            //set cooldown to true and fire the weapon
            Cooldown = true;
            //Play animation
            Animation();
            //run attack coroutine and pass in muzzle flash pos
            StartCoroutine(Attack(MuzzleFlash.transform));
            //play the sound and muzzle flash
            weaponAudio.Play();
            MuzzleFlash.Play();
        }
        else if (CurrentAmmo <= 0) //if ammo is <= 0 then reload
            StartCoroutine(Reload());
    }

    IEnumerator FireBurst(int burstSize)
    {
        //Fire burst for how ever many bursts there is
        for (int i = 0; i < burstSize; i++)
        {
            Fire();
            //wait for firerate
            yield return new WaitForSeconds(FireRate);
        }
    }

    public IEnumerator Attack(Transform origin)
    {
        //Create a ray from the muzzle flash pos
        Ray ray = new Ray(origin.position, origin.forward);

        //If the player is using the gun then make the ray shoot from the center of the screen instead of muzzle flash pos
        if(Owner.tag == "Player")
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1));
        }

        //Raycast
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Range))
        {
            //Check if you hit a character
            CharacterData character = rayHit.transform.GetComponent<CharacterData>();
            //Check if you hit a character
            Rigidbody rigidbody = rayHit.transform.GetComponent<Rigidbody>();
            if (character != null && rayHit.transform.tag != Owner.tag) // if hit a character and it was not the owner hit that character
                Hit(character);
            else if (rigidbody != null && rayHit.transform.tag != Owner.tag) // or if you hit a rigid body hit that instead
                Hit(rigidbody, rayHit);
            //Check if you hit a destructable and if so hit the destructable
            Destructable destructable = rayHit.transform.GetComponent<Destructable>();
            if (destructable != null)
                Hit(destructable);

            //Check if you hit a projectile and if so destroy it
            Projectile projectile = rayHit.transform.GetComponent<Projectile>();
            if (projectile != null)
                Destroy(projectile.gameObject);
            //Instantiate the impact prefab at the hit point
            Instantiate(ImpactPrefab, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        }
        //decrement ammo
        CurrentAmmo--;
        yield return new WaitForSeconds(FireRate); // wait for firerate
        //set cooldown to false
        Cooldown = false;
    }

    public IEnumerator Reload()
    {
        //set cooldown to true and wait for reload time
        Cooldown = true;
        yield return new WaitForSeconds(ReloadTime);
        //set current ammo back to max and set cooldown to false
        CurrentAmmo = MaxAmmo;
        Cooldown = false;
    }

    private void Animation()
    {
        //play the fire animation
        GetComponent<Animator>().SetTrigger("onFire");
    }

    //Hit a character
    void Hit(CharacterData character)
    {
        //make the character take damage
        character.TakeDamage(Damage);
    }

    //Hit a destructable
    void Hit(Destructable destructable)
    {
        //damage the destructable
        destructable.TakeDamage(Damage);
    }

    //Hit a rigidbody
    void Hit(Rigidbody rigidbody, RaycastHit hit)
    {
        //add an impulse force to the rigidbody using the negative of hit normal
        rigidbody.AddForce(-hit.normal * Damage, ForceMode.Impulse);
    }
}
