using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion rnd = Random.rotation;
        //Quaternion rotation = new Quaternion(0, 0, rnd.z, rnd.w);
        transform.Rotate(-transform.forward * Time.deltaTime * 180);
    }
}
