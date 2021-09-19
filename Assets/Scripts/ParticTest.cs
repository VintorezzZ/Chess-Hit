using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticTest : MonoBehaviour
{
    public ParticleSystem ps;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ps.Play();
        }
    }
}
