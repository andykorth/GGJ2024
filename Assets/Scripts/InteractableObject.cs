using System;
using System.Collections.Generic;
using Sonic;
using Swoonity.CSharp;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;


[Serializable]
public class StateOption
{
	[Header("Config")]
	public string StateName = "?";
	// public string Verb = "poke"; // TODO?
		
	[Header("Ghosts")]
	[Tooltip("if this state is chosen to be 'wanted' (meaning the object MUST be in this state)")]
	[TextArea]
	public List<string> Wants = new();
	
	[Tooltip("if this state is chosen to be 'hated' (meaning the object CANNOT be in this state)")]
	[TextArea]
	public List<string> Hates = new();
	
	[Header("Exit")]
	[Tooltip("if this state is chosen to be 'wanted' (meaning the object MUST be in this state)")]
	[TextArea]
	public List<string> ExitWants = new();
	
	[Tooltip("if this state is chosen to be 'hated' (meaning the object CANNOT be in this state)")]
	[TextArea]
	public List<string> ExitHates = new();
}

public class InteractableObject : MonoBehaviour
{
	public enum InteractableType
	{
		Cauldron,
		Vase,
		Crate,
		Candle,
		Portrait
	}

	public enum InteractableColor
	{
		Any,
		Blue,
		Purple,
		Green
	}

	public string verb = "poke";

	public GameObject interactEffectPrefab;
	public InteractableType interactableType;
	public InteractableColor interactableColor = InteractableColor.Any;

	public Light2D tiedToLight;
	public Sprite spriteWhenActivated;
	private Sprite originalSprite;
	private SpriteRenderer[] allSprites;
	public SpriteRenderer mainSprite;
	private Color originalSpriteColor;

	public bool hasBeenEnabledByPlayer = false;


	[Header("Room State")]
	public List<StateOption> Options = new();
	public int CurrentStateId;

	[Header("Sounds")] 
	public SonicSfx SfxInteract;
	public SonicSfx SfxLoop;


	public static List<InteractableObject> allInteractables;

	public enum InteractionMode
	{
		NoneOrCustom,
		CrookedPainting,
		FlipUpsideDown,
		SwapToActivatedSprite
	}

	public InteractionMode interactionMode;
	public bool randomInteractionModeState = false;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ReloadAssembly()
	{
		allInteractables = new List<InteractableObject>();
	}

	public void Start()
	{
		allSprites = GetComponentsInChildren<SpriteRenderer>();
		allInteractables.Add(this);
		// Debug.Log("Interactable count: " + allInteractables.Count);
		originalSprite = mainSprite.sprite;
		originalSpriteColor = mainSprite.color;

		if (randomInteractionModeState)
		{
			hasBeenEnabledByPlayer = Random.value > 0.5f;
		}

		switch (interactionMode)
		{
			case InteractionMode.CrookedPainting:
				float angle = Random.value > 0.5f ? 25f : -25f;
				transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, hasBeenEnabledByPlayer ? 0f : angle);
				break;
			case InteractionMode.FlipUpsideDown:
				transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, hasBeenEnabledByPlayer ? 0f : 180f);
				break;
			case InteractionMode.SwapToActivatedSprite:
				mainSprite.sprite = hasBeenEnabledByPlayer ? spriteWhenActivated : originalSprite;
				break;
			case InteractionMode.NoneOrCustom:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		if (tiedToLight != null)
		{
			tiedToLight.enabled = hasBeenEnabledByPlayer;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		var player = other.gameObject.GetComponent<Player>();
		if (player != null)
		{
			// the player has interacted with this, activate it. 
			player.PlayerStartTouch(this);
			// SetColor(player.interactColor);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var player = other.gameObject.GetComponent<Player>();
		if (player != null)
		{
			// the player has interacted with this, activate it. 
			player.PlayerEndTouch(this);
			SetColor(originalSpriteColor);
		}
	}

	private void SetColor(Color c)
	{
		foreach (var s in allSprites)
		{
			s.color = c;
		}
	}

	public void PlayerInteract()
	{
		
		CurrentStateId++;
		if (CurrentStateId >= Options.Count) CurrentStateId = 0;
		// TODO
		// TODO
		
		
		SfxInteract.PlayAt(transform.position);

		if (interactEffectPrefab != null)
		{
			GameObject go = Instantiate(interactEffectPrefab, transform.position, Quaternion.identity);
			Destroy(go, 10.0f);
		}

		hasBeenEnabledByPlayer = !hasBeenEnabledByPlayer;
		if (tiedToLight != null)
		{
			tiedToLight.enabled = hasBeenEnabledByPlayer;
		}

		switch (interactionMode)
		{
			case InteractionMode.CrookedPainting:
			{
				float angle = Random.value > 0.5f ? 25f : -25f;
				Quaternion original = transform.GetChild(0).rotation;
				Quaternion target = transform.GetChild(0).rotation =
					Quaternion.Euler(0f, 0f, hasBeenEnabledByPlayer ? 0f : angle);
				this.AddTween(0.7f,
					(a) =>
					{
						transform.GetChild(0).rotation = Quaternion.SlerpUnclamped(original, target, Mathfx.Berp(0, 1, a));
					});
				break;
			}
			
			case InteractionMode.FlipUpsideDown:
			{
				float angle = hasBeenEnabledByPlayer ? 0f : 180f;
				Quaternion original = transform.GetChild(0).rotation;
				Quaternion target = transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, angle);
				this.AddTween(0.7f,
					(a) =>
					{
						transform.GetChild(0).rotation = Quaternion.SlerpUnclamped(original, target, Mathfx.Berp(0, 1, a));
					});
				break;
			}
			
			case InteractionMode.SwapToActivatedSprite when hasBeenEnabledByPlayer:
				mainSprite.sprite = spriteWhenActivated;
				break;
			
			case InteractionMode.SwapToActivatedSprite:
				mainSprite.sprite = originalSprite;
				break;
			
			case InteractionMode.NoneOrCustom:
				break;
			
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}