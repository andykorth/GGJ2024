using Foundational;
using futz.ActGhost;
using Lumberjack;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEngine;

public class Player : Singleton<Player>
{
	public Animer Animer;
	public Transform DirFlipper;
	public float WalkThreshold = .1f;
	public float CharacterScale = .2f;
	public float AnimWalkSpeedMin = .1f;
	public float AnimWalkSpeedMax = 3f;
	public float AnimWalkVelocityMin = 0f;
	public float AnimWalkVelocityMax = 5f;
	public float speed = 3f;

	public Rigidbody rb;

	void LateUpdate()
	{
		var target = new Vector2(
				Input.GetAxis("Horizontal"),
				Input.GetAxis("Vertical"))
			.Clamp();
		rb.AddForce(new Vector3(target.x, target.y) * speed * Time.deltaTime);
	}

	void Update()
	{
		var act = GameSysClip.I.GhostAct.Current;
		if (!act) return;

		if (act.Phase.Current != GhostActivity.PhaseEnum.PLAYING_ROOM) return;


		var velocityMag = rb.velocity.magnitude;

		var animSpeed = velocityMag.Map(
			(AnimWalkVelocityMin, AnimWalkVelocityMax),
			(AnimWalkSpeedMin, AnimWalkSpeedMax)
		);
		Animer.SetWalking(velocityMag >= WalkThreshold, animSpeed);
		// $"{velocityMag}".LgOrange0();

		DirFlipper.localScale = rb.velocity.x <= 0
			? new Vector3(1, 1, 1) * CharacterScale
			: new Vector3(-1, 1, 1) * CharacterScale;

		// rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(target.x, 0f, target.y) * speed, accel);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (mostRecentTouch != null)
			{
				// Debug.Log("Interact with " + mostRecentTouch.name);
				mostRecentTouch.PlayerInteract();

				GhostPlayerManager.i.AssessRoomStates();
			}
			else
			{
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
		if (mostRecentTouch == interactableObject)
		{
			mostRecentTouch = null;
		}
	}
}