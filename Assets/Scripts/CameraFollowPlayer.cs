using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Player player;
    public Vector3 offset;

    public void LateUpdate()
    {
        this.transform.position = player.transform.position + offset;
        this.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
    }
}
