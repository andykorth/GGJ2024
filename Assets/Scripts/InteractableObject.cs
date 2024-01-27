using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InteractableObject : MonoBehaviour
{
    public enum InteractableType{
        Torch, Vase, Crate, Candle, Portrait
    }
    public GameObject interactEffectPrefab;
    public InteractableType interactableType;

    public Light2D tiedToLight;
    private SpriteRenderer[] allSprites;

    public bool hasBeenEnabledByPlayer = false;

    public static List<InteractableObject> allInteractables;

    public enum InteractionMode {
        NoneOrCustom, CrookedPainting, FlipUpsideDown
    }
    public InteractionMode interactionMode;
    public bool randomInteractionModeState = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ReloadAssembly()
    {
        allInteractables = new List<InteractableObject>();
    }

    public void Start(){
        allSprites = GetComponentsInChildren<SpriteRenderer>();
        allInteractables.Add(this);
        // Debug.Log("Interactable count: " + allInteractables.Count);

        if(randomInteractionModeState){
            hasBeenEnabledByPlayer = Random.value > 0.5f;
        }

        if(interactionMode == InteractionMode.CrookedPainting){
            float angle = Random.value > 0.5f ? 25f: -25f;
            transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, hasBeenEnabledByPlayer ? 0f : angle);
        }else if (interactionMode == InteractionMode.FlipUpsideDown){
            transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, hasBeenEnabledByPlayer ? 0f : 180f);
        }

        if(tiedToLight != null){
            tiedToLight.enabled = !hasBeenEnabledByPlayer;
        }
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

        hasBeenEnabledByPlayer = !hasBeenEnabledByPlayer;
        if(tiedToLight != null){
            tiedToLight.enabled = !hasBeenEnabledByPlayer;
        }

        if(interactionMode == InteractionMode.CrookedPainting){
            float angle = Random.value > 0.5f ? 25f: -25f;
            Quaternion original = transform.GetChild(0).rotation;
            Quaternion target = transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, hasBeenEnabledByPlayer ? 0f : angle);
            this.AddTween(0.7f, (a) => {
                transform.GetChild(0).rotation = Quaternion.SlerpUnclamped(original, target, Mathfx.Berp(0, 1, a));
            });
        }else if (interactionMode == InteractionMode.FlipUpsideDown){
            float angle = hasBeenEnabledByPlayer ? 0f : 180f;
            Quaternion original = transform.GetChild(0).rotation;
            Quaternion target = transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, hasBeenEnabledByPlayer ? 0f : angle);
            this.AddTween(0.7f, (a) => {
                transform.GetChild(0).rotation = Quaternion.SlerpUnclamped(original, target, Mathfx.Berp(0, 1, a));
            });
        }

    }


}
