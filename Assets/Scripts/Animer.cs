using UnityEngine;

public class Animer : MonoBehaviour
{
	public Animator Animator;

	public void SetWalking(bool isWalking, float speed)
	{
		Animator.SetBool("Walking", isWalking);
		Animator.SetFloat("Speed", speed);
	}
	
}