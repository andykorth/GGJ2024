using UnityEngine;

public class Animer : MonoBehaviour
{
	public Animator Animator;

	public void SetWalking(bool isWalking)
	{
		Animator.SetBool("Walking", isWalking);
	}
	
}