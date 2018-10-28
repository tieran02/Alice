using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public class Grenade : MonoBehaviour
{
    public float Damage = 30;
    public float ExplodeRadius = 5;
    public float FuseTime = 3;
    public ParticleSystem ExplosionParticles;
    public MeshRenderer Model;
    private Rigidbody rb;
    private AudioSource audioSource;

    void Awake()
    {
        //get rigidbody and audio
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //start explod cooutine
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(FuseTime);

        //explode Radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplodeRadius);
        foreach (var collision in hitColliders)
        {
            CharacterData character = collision.transform.GetComponent<CharacterData>();
            if (character != null)
                Hit(character, Damage);

            Destructable destructable = collision.transform.GetComponent<Destructable>();
            if (destructable != null)
                Hit(destructable, Damage);
        }
        //play sound
        audioSource.Play();
        //display explosion particles
        if (ExplosionParticles != null)
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
        Model.enabled = false;
        rb.isKinematic = true;
    }
}
