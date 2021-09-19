using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{     
    public float minZ;
    public float speedZ;
    public float maxZ;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0, 0, speedZ * Time.deltaTime, Space.Self);


        if (transform.localPosition.z < minZ)
            speedZ *= -1;
        else if (transform.localPosition.z > maxZ)
            speedZ *= -1;
    
    }


}
