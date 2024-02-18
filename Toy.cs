using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Toy : Interactable {

	private Rigidbody rigidBody;
	[SerializeField] private float kickPower;
	[SerializeField] private float cooldownDuration;
	[SerializeField] private float funValue;
	[SerializeField] private float maxPowerScaleFactor;
	private AudioSource audioSource;

	private void Awake() {
		ItemType = ItemType.Toy;
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}

	public override void Interact(GameObject interactingObject) {
		Debug.Log("now interacting with: " + this.gameObject);
		
		Monster monster = interactingObject.GetComponent<Monster>();
		if (monster != null) {
			SatisfyMonster(monster);
			KickBall(monster);
		}
	
	}

	private void SatisfyMonster(Monster monster) {
		NeedsController needs = monster.needsController;
		needs.GetNeedByType(NeedType.Fun)?.ChangeNeedValue(funValue);
	}

	private void KickBall(Monster monster) {
		Vector3 upwardForce = new Vector3(0, Random.Range(1, 4), 0);
		Vector3 kickDirection = ((this.transform.position -monster.transform.position) + upwardForce).normalized * kickPower * Mathf.Min(monster.Scale, maxPowerScaleFactor);
		rigidBody.AddForce(kickDirection, ForceMode.Impulse);

		monster.monsterBehavior.MonsterSeekNeedInstance.SetHuntedObject(null);
		audioSource.Play();
	}

	private void DisableInteraction() {
		InteractiveObject interactiveObject = GetComponent<InteractiveObject>();
		if (interactiveObject != null) {
			interactiveObject.canPickUp = false;
		}
	}
}
