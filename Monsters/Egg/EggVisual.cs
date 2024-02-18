using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EggAnim {
	IsGrowing,
}

public class EggVisual : MonoBehaviour
{
	private Animator animator;
	private Egg egg;

	private void Start() {
		animator = GetComponent<Animator>();
		egg = GetComponentInParent<Egg>();
	}

	private void Update() {
		animator.SetFloat("GrowthProgress", egg.GetHatchProgressNormalized());

		SetAnimBool(EggAnim.IsGrowing, egg.IsGrowing());
	}

	private void SetAnimTrigger(EggAnim animType) {
		animator.SetTrigger(animType.ToString());
	}

	public void SetAnimBool(EggAnim anim, bool set) {
		animator.SetBool(anim.ToString(), set);
	}

}
