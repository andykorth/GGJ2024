using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public float accel = 0.5f;

    public Rigidbody rb;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 target = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.AddForce( new Vector3(target.x, 0f, target.y) * speed * Time.deltaTime);
        // rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(target.x, 0f, target.y) * speed, accel);

    }
}
