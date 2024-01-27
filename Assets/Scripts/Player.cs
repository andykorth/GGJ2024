using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : Singleton<Player>
{
    public float speed = 3f;
    public float accel = 0.5f;

    public Rigidbody rb;

    private List<InteractableObject> haveInteractedWith = new List<InteractableObject>();

    void Update()
    {
        Vector2 target = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.AddForce( new Vector3(target.x, 0f, target.y) * speed * Time.deltaTime);
        // rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(target.x, 0f, target.y) * speed, accel);

    }

    public void PlayerInteract(InteractableObject o){
        if(!haveInteractedWith.Contains(o)){
            haveInteractedWith.Add(o);
        }
    }

    internal int InteractCount(InteractableObject.InteractableType interactionType)
    {
        int count = 0;
        foreach(var v in haveInteractedWith){
            if(v.interactableType == interactionType){
                count += 1;
            }
        }
        return count;
    }
}