using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistBaron : MonoBehaviour
{
    public float speed = 0.1f;
    public Transform[] mists;
    public void Start(){

    }

    // Update is called once per frame
    void Update()
    {
        bool odd = true;
        foreach(Transform t in mists){
            t.rotation *= Quaternion.Euler(0f, 0f, Time.deltaTime * (odd ? speed : -speed) );
            odd = !odd;
        }
    }
}
