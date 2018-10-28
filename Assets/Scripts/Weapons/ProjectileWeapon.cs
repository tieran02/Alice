using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileWeapon : MonoBehaviour {

    public string Name;
    public float FireRate = 1.0f;
    public int MaxAmmo = 1;
    public float ReloadTime = 1.0f;
    public float Range = 200;
    //Should the weapon wait for the current projectile to be destroyed?
    public bool WaitForProjectile = false;
    //Current ammo of the weapon
    public int CurrentAmmo { get; private set; }
    //Is the weapon on cooldown
    public bool Cooldown { get; private set; }
    private float currentTime = 0;
    //The projectile that would get spawned
    public GameObject ProjectilePrefab;
    public Transform SpawnPos;
    private AudioSource weaponAudio;
    //current projectile(used with WaitForProjectile)
    private Projectile currentProjectile;
    //The owner of the weapon
    public GameObject Owner;

    void Awake()
    {
        weaponAudio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        //set current ammo to max ammo
        CurrentAmmo = MaxAmmo;
        currentTime = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //check if weapon is on cooldown
        if (Cooldown)
        {
            //check if current time is greater than fire rate and if so reset cooldown 
            if (currentTime > FireRate)
            {
                Cooldown = false;
                currentTime = 0;
            }
            else // else add delta time to the current time
                currentTime += Time.deltaTime;
        }
        //check if the owner is the player
        if (Owner.tag == "Player")
        {
            //On left click attack
            if (Input.GetMouseButtonDown(0))
                Fire();
            //on R reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
        }
    }

    public void Fire()
    {
        //check if character has ammo and attack if they do
        if (CurrentAmmo > 0)
        {
            Attack();
            weaponAudio.Play();
        }
        else //else reload for them
            StartCoroutine(Reload());
    }

    public IEnumerator Reload()
    {
        //set cooldown to true
        Cooldown = true;
        //wait for reload time
        yield return new WaitForSeconds(ReloadTime);
        //reset ammo and cooldown
        CurrentAmmo = MaxAmmo;
        Cooldown = false;
    }

    public void Attack(Transform target = null)
    {
        if (!Cooldown)
        {
            Cooldown = true;

            //if current projectile is null of does not need to wait for projectile then instantiate new projectile
            if(currentProjectile == null || WaitForProjectile == false)
            currentProjectile = Instantiate(ProjectilePrefab, SpawnPos.position, SpawnPos.rotation).GetComponent<Projectile>();

            //folow target if set
            if (target != null)
                currentProjectile.Target = target;

            CurrentAmmo--;
        }
    }
}
