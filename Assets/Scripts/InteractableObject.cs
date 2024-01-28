using System;
using System.Collections.Generic;
using Idealist;
using Lumberjack;
using Sonic;
using Swoonity.Collections;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;


[Serializable]
public class StateOption
{
	[Header("Config")] public string Label = "?";

	[Tooltip("verb used while in this state (indicating the NEXT state)")]
	public string VerbToNext = "poke";
	// public string Verb = "poke"; // TODO?

	[Header("Ghosts")]
	[Tooltip("if this state is chosen to be 'wanted' (meaning the object MUST be in this state)")]
	[TextArea]
	public List<string> Wants = new();

	[Tooltip("if this state is chosen to be 'hated' (meaning the object CANNOT be in this state)")] [TextArea]
	public List<string> Hates = new();

	[Header("Exit")]
	[Tooltip("if this state is chosen to be 'wanted' (meaning the object MUST be in this state)")]
	[TextArea]
	public List<string> ExitWants = new();

	[Tooltip("if this state is chosen to be 'hated' (meaning the object CANNOT be in this state)")] [TextArea]
	public List<string> ExitHates = new();
}

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

public enum InteractionMode
{
	NoneOrCustom,
	CrookedPainting,
	FlipUpsideDown,
	SwapToActivatedSprite
}


[SelectionBase]
public class InteractableObject : MonoBehaviour
{
	[Header("Interact config")] public string Name = "";
	public GameObject interactEffectPrefab;
	public InteractableType interactableType;
	public InteractableColor interactableColor;
	public InteractionMode interactionMode;
	public float flipAngle;

	[Header("Misc config")] public GameObject tiedToChild;
	public Light2D tiedToLight;
	public Sprite spriteWhenActivated;
	private Sprite originalSprite;
	private SpriteRenderer[] allSprites;
	public SpriteRenderer mainSprite;
	private Color originalSpriteColor;

	public bool hasBeenEnabledByPlayer = false;

	[Header("Sounds")] public SonicSfx SfxInteract;


	[Header("Room State")] [Btn(nameof(GeneratePlaceholders))]
	public bool RandomStart = true;

	public List<StateOption> Options = new();
	public int CurrentStateId;


	public static List<InteractableObject> allInteractables;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ReloadAssembly()
	{
		allInteractables = new List<InteractableObject>();
	}

	public void Start()
	{
		if (Name == "") Name = name;

		allSprites = GetComponentsInChildren<SpriteRenderer>();
		allInteractables.Add(this);
		// Debug.Log("Interactable count: " + allInteractables.Count);
		originalSprite = mainSprite.sprite;
		originalSpriteColor = mainSprite.color;

		SetState(RandomStart ? Random.Range(0, Options.Count) : 0);
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
		SetState(CurrentStateId + 1);

		SfxInteract.PlayAt(transform.position);

		// TODO
		// TODO: update below
		// TODO

		if (interactEffectPrefab != null)
		{
			GameObject go = Instantiate(interactEffectPrefab, transform.position, Quaternion.identity);
			Destroy(go, 10.0f);
		}
	}

	/// will wrap
	public void SetState(int stateId)
	{
		if (stateId >= Options.Count) stateId = 0;

		$"{name} set: {stateId}, {Options[stateId].Label}".LgOrange0(this);

		var spriteTf = mainSprite.transform;


		switch (interactionMode)
		{
			case InteractionMode.CrookedPainting:
			{
				var angle = stateId switch
				{
					0 => 0f, // straight
					1 => Random.value > 0.5f ? flipAngle : -flipAngle // crooked
				};

				var original = spriteTf.rotation;
				var target = Quaternion.Euler(0f, 0f, angle);
				this.AddTween(0.7f,
					a => spriteTf.rotation =
						Quaternion.SlerpUnclamped(original, target, Mathfx.Berp(0, 1, a)));
				// spriteTf.rotation = target;
				break;
			}

			case InteractionMode.FlipUpsideDown:
			{
				var angle = stateId switch
				{
					0 => 0f, // normal
					1 => flipAngle // flipped
				};
				var original = spriteTf.rotation;
				var target = Quaternion.Euler(0f, 0f, angle);
				this.AddTween(0.7f,
					(a) =>
					{
						spriteTf.rotation =
							Quaternion.SlerpUnclamped(original, target, Mathfx.Berp(0, 1, a));
					});
				// spriteTf.rotation = target;
				break;
			}

			case InteractionMode.SwapToActivatedSprite:
				mainSprite.sprite = stateId switch
				{
					0 => originalSprite, // default
					1 => spriteWhenActivated // activated
				};
				break;

			case InteractionMode.NoneOrCustom:
				break;

			default:
				throw new ArgumentOutOfRangeException();
		}

		if (tiedToChild != null) tiedToChild.SetActive(stateId == 1);
		if (tiedToLight != null) tiedToLight.enabled = stateId == 1;

		CurrentStateId = stateId;
	}


	public void GeneratePlaceholders()
	{
		if (Name == "") Name = name;

		if (Options.Count == 0)
		{
			Options.Add(new StateOption { Label = "normal" });
			Options.Add(new StateOption { Label = "activated" });
		}

		foreach (var option in Options)
		{
			if (option.Wants.Count == 0) option.Wants.Add("TODO Ghost want: {color} {name} {label}");
			if (option.Hates.Count == 0) option.Hates.Add("TODO Ghost want: {color} {name} NOT {label}");
			if (option.ExitWants.Count == 0) option.ExitWants.Add("TODO Exit want: {color} {name} {label}");
			if (option.ExitHates.Count == 0) option.ExitHates.Add("TODO Exit want: {color} {name} NOT {label}");
		}
	}
}