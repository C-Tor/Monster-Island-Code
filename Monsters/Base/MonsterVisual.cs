using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterAnim {
	Idle,
	Eat,
	IsWalking,
	IsSleeping,
	IsInWater,
	JumpInWater,
	JumpOutWater,
}

public class MonsterVisual : MonoBehaviour
{
	private Animator animator;
	private MonsterAudio monsterAudio;
	public Transform eatFoodAttach;
	[SerializeField] private MonsterMovement monsterMovement;
	[SerializeField] private string isWalkingString;
	[SerializeField] private string isSleepingString;
	[SerializeField] private bool isInWater;
	public bool IsSleeping;

	private float baseLayerWeight = 1f;
	private float swimmingLayerWeight = 0f;

	private void Start() {
		animator = GetComponent<Animator>();
		monsterAudio = GetComponentInParent<Monster>().GetComponentInChildren<MonsterAudio>();

		GetComponentInParent<AgentLinkMover>().OnJumpInWater += AgentLinkMover_OnJumpInWater;
		GetComponentInParent<AgentLinkMover>().OnJumpOutWater += AgentLinkMover_OnJumpOutWater;

	}


	private void Update() {
		bool isMoving = monsterMovement.IsMoving();

		SetAnimBool(MonsterAnim.IsWalking, isMoving);

		isInWater = monsterMovement.CheckIsInWater();

		// Adjust layer weights based on the swimming state
		if (isInWater) {
			// Increase the weight of the swimming layer
			swimmingLayerWeight = 1f; //Mathf.Lerp(swimmingLayerWeight, 1f, Time.deltaTime * 5f);
			baseLayerWeight = 0f;  Mathf.Lerp(baseLayerWeight, 0f, Time.deltaTime * 5f);
			SetAnimBool(MonsterAnim.IsInWater, true);
		} else {
			// Increase the weight of the base layer
			swimmingLayerWeight = 0f;// Mathf.Lerp(swimmingLayerWeight, 0f, Time.deltaTime * 5f);
			baseLayerWeight = 1f; // Mathf.Lerp(baseLayerWeight, 1f, Time.deltaTime * 5f);
			SetAnimBool(MonsterAnim.IsInWater, false);

		}

		// Set the layer weights in the Animator
		animator.SetLayerWeight(0, baseLayerWeight); // Base layer index (0)
		animator.SetLayerWeight(1, swimmingLayerWeight); // Swimming layer index (1)
	}

	private void AgentLinkMover_OnJumpInWater() {
		SetAnimTrigger(MonsterAnim.JumpInWater);
	}

	private void AgentLinkMover_OnJumpOutWater() {
		SetAnimTrigger(MonsterAnim.JumpOutWater);
	}


	#region animations
	private void SetAnimTrigger(MonsterAnim animType) {
		animator.SetTrigger(animType.ToString());
	}

	private Food interactedFood;

	public void SetAnimBool(MonsterAnim anim, bool set) {
		animator.SetBool(anim.ToString(), set);
	}

	public void EatFood(Food foodObject) {
		SetAnimTrigger(MonsterAnim.Eat);
		interactedFood = foodObject;
	}

	public void EatFoodAnimEvent() {
		interactedFood.VisualAttach(eatFoodAttach);
	}

	public void EatFoodAnimEventFinish() {
		interactedFood.VisualConsume();
	}
	
	//Resumes after a monster was stopped for animation purposes.
	public void ResumeAgent() {
		GetComponentInParent<MonsterMovement>().ResumeAgent();
	}
	#endregion

	#region Audio

	public void PlayFootStep() {
		monsterAudio.PlayFootstep();
	}

	public void PlayEatSound() {
		monsterAudio.PlayEatSound();
	}

	public void PlayWaterSplashSound() {
		monsterAudio.PlayWaterSplashSound();
	}

	#endregion
}
