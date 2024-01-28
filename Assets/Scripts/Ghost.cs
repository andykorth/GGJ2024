using UnityEngine;

public class Ghost : MonoBehaviour
{
	[Header("Config")] 
	public string Name;
	public SpriteRenderer SpriteRenderer;
	public Animer Animer;

	[Header("State")]
	public RoomState DesiredRoomState;
	public bool IsRescued;
}