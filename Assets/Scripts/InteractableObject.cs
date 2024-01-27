using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InteractableObject : MonoBehaviour
{
    public enum InteractableType{
        Torch, Vase, Crate
    }
    public GameObject interactEffectPrefab;
    public InteractableType interactableType;


    public Light2D tiedToLight;
    private SpriteRenderer[] allSprites;

    public void Start(){
        allSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if(player != null){
            // the player has interacted with this, activate it. 
            player.PlayerStartTouch(this);
            SetColor(Color.red);
        }
    }

    private void OnTriggerExit(Collider other){
        var player = other.gameObject.GetComponent<Player>();
        if(player != null){
            // the player has interacted with this, activate it. 
            player.PlayerEndTouch(this);
            SetColor(Color.white);
        }
    }

    private void SetColor(Color c){
        foreach(var s in allSprites){
            s.color = c;
        }
    }

    public void PlayerInteract(){
        if(interactEffectPrefab != null){
            GameObject go = Instantiate(interactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(go, 10.0f);
        }

        if(tiedToLight != null){
            tiedToLight.enabled = !tiedToLight.enabled;
        }
    }


}
