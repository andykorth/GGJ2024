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

    void Update()
    {
        Vector2 target = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.AddForce( new Vector3(target.x, target.y) * speed * Time.deltaTime);
        // rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(target.x, 0f, target.y) * speed, accel);

        if(Input.GetKeyDown(KeyCode.Space)){
            if(mostRecentTouch != null){
                Debug.Log("Interact with " + mostRecentTouch.name);
                mostRecentTouch.PlayerInteract();
                
                GhostPlayerManager.i.AssessRoomStates();
                
            }else{
                Debug.Log("Nothing to interact with!");
            }
            
        }

    }

    public InteractableObject mostRecentTouch;

    internal void PlayerStartTouch(InteractableObject interactableObject)
    {
        mostRecentTouch = interactableObject;
    }

    internal void PlayerEndTouch(InteractableObject interactableObject)
    {
        if(mostRecentTouch == interactableObject){
            mostRecentTouch = null;
        }
    }
}
