using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public bool Freezed;
    public bool Rotated;
    [Range(0, 3)] public float Power = 0f;
    [Range(1,10)] public int Mass=1;
    //public AudioSource audioSource;
    public ParticleSystem particle;
    public MeshRenderer meshRenderer;
    public Rigidbody Rigidbody;
    public void SetComponents()
    {
        Rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        //audioSource = GetComponent<AudioSource>();
    }
    public void RotateRandom()
    {
        if (Rotated) return;
        transform.rotation = Random.rotation;
        Rotated = true;
    }
    public void UnRotated()
    {
        Rotated = false;
    }
    public void Freeze()
    {
        Freezed = true;
        Rigidbody.isKinematic = true;
        
    }
    public void Unfreeze()
    {
        Freezed = false;
        Rigidbody.isKinematic = false;
    }
    public void Destroy()
    {
        ParticleSystem debrisEffect = Instantiate(particle, transform.position, Quaternion.identity);
        debrisEffect.GetComponent<ParticleSystemRenderer>().material = meshRenderer.material;
        //audioSource.transform.SetParent(null);
        //audioSource.Play();
        //Destroy(audioSource.gameObject, 1f);
        Destroy(gameObject);
    }

}
