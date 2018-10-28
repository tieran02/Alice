using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour {
    //public varibles that define a projectile
    public float Speed = 10;
    public float Damage = 40;
    public float Range = 100;
    public float ExplodeDamage = 0;
    public float ExplodeRadius = 5f; // set explode radius to 0 for no explosion
    public ParticleSystem ExplosionParticles;
    public Transform Target; //Set target if you want the projectile to follow the target

    private Rigidbody rb;
    private AudioSource audioSource;
    //The current range covered by the projectile
    private float rangeCovered = 0;
    private Vector3 startPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startPos = transform.position; //set the start pos to calculate distance covered
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        //If target has been set then follow the target by looking at it
        if(Target != null)
        {
            transform.LookAt(Target);
        }

        //Set the velocity of the rigidbody
        rb.velocity = transform.forward * Time.deltaTime * Speed;
        //calculate distance covered
        rangeCovered = Vector3.Distance(startPos, transform.position);
        //If distance covered is greater than the range than Destroy this projectile
        if (rangeCovered >= Range)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        //Check if projectile hit a character and if so deal dammage
        CharacterData character = collision.transform.GetComponent<CharacterData>();
        if (character != null)
            Hit(character, Damage);
        //Check if projectile hit a destructable and if so deal dammage
        Destructable destructable = collision.transform.GetComponent<Destructable>();
        if (destructable != null)
            Hit(destructable, Damage);
        //Start the explode coroutine
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        //if the projectile has a impact sound play it on impact
        if (audioSource.clip != null)
            audioSource.Play();

        //projectile has explode damage so explode
        if (ExplodeDamage > 0)
        {
            //explode Radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplodeRadius);
            foreach (var collision in hitColliders)
            {
                CharacterData character = collision.transform.GetComponent<CharacterData>();
                if (character != null)
                    Hit(character,ExplodeDamage);

                Destructable destructable = collision.transform.GetComponent<Destructable>();
                if (destructable != null)
                    Hit(destructable, ExplodeDamage);
            }

            //display explosion particles
            if(ExplosionParticles != null)
            {
                ExplosionParticles.Play();
                //destroy after explosion particles
                
                //get the audio clip length
                float audioTime = audioSource.clip.length;
                //get the explosion length
                float particleTime = ExplosionParticles.main.duration;

                //hide object
                Hide();
                //wait for audio or particles to finish
                yield return new WaitForSeconds(audioTime < particleTime ? particleTime : audioTime);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //just Destroy if no particles attached
            Destroy(gameObject);
        }
    }

    //Damage a character
    void Hit(CharacterData character, float damage)
    {
        character.TakeDamage(damage);
    }
    //Damage a destructable
    void Hit(Destructable destructable, float damage)
    {
        destructable.TakeDamage(damage);
    }

    void Hide()
    {
        //disable the orb collider and renderer and make the rigigbody kinimatic till object gets destroyed
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        rb.isKinematic = true;
    }
}
