using UnityEngine;

public class Ghost : MonoBehaviour
{
	[Header("Config")] 
	public string Name;
	public SpriteRenderer SpriteRenderer;

	[Header("State")]
	public RoomState DesiredRoomState;
	public bool IsRescued;
}