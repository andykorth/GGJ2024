using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum InteractableType{
        Torch, Vase, Crate
    }
    public GameObject interactEffectPrefab;
    public InteractableType interactableType;

    private void OnTriggerEnter(Collider other)
    {
        var potentiallyPlayer = other.gameObject.GetComponent<Player>();
        if(potentiallyPlayer != null){
            // the player has interacted with this, activate it. 
            if(interactEffectPrefab != null){
                GameObject go = Instantiate(interactEffectPrefab, transform.position, Quaternion.identity);
                Destroy(go, 10.0f);
            }
            potentiallyPlayer.PlayerInteract(this);
        }
    }


}
