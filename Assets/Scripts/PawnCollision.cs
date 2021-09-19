using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnCollision : MonoBehaviour
{
    public ParticleSystem HitParticles;

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Collision"))
        {
            HitParticles.Play();
        }
    }
}
