using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using UnityEngine;

public class Food : Interactable {
	[SerializeField] private FoodDataSO foodData;
	private bool isEaten;
	private bool isAttached;
	private Transform attachedPosition;
	private float destroyTimer = 3f;

	

	private void Awake() {
		ItemType = ItemType.Food;
		isEaten = false;
	}

	private void Update() {
		if (isAttached) {
			transform.position = attachedPosition.position;
		}
		if (isEaten) {
			destroyTimer -= Time.deltaTime;
			if (destroyTimer < 0f) {
				Debug.Log("Destroying from time");
				Destroy(gameObject);
			}
		}
	}

	public override void Interact(GameObject interactingObject) {
		if (!isEaten) {
			Monster monster = interactingObject.GetComponent<Monster>();
			if (monster != null) {
				FeedMonster(monster);
			}
		}
	}

	private void FeedMonster(Monster monster) {
		NeedsController needs = monster.needsController;
		needs.GetNeedByType(NeedType.Food)?.ChangeNeedValue(foodData.foodValue);
		needs.GetNeedByType(NeedType.Water)?.ChangeNeedValue(foodData.waterValue);


		isEaten = true;

		monster.GetComponentInChildren<MonsterVisual>().EatFood(this);

		monster.monsterBehavior.RemoveFromDetected(this);
		MonsterManager.Instance.RemoveInteractableFromAllMonsters(this);

		DisableInteraction();
	}

	private void DisableInteraction() {
		InteractiveObject interactiveObject = GetComponent<InteractiveObject>();
		if (interactiveObject != null) {
			interactiveObject.canPickUp = false;
		}
	}

	public void VisualConsume() {
		Destroy(gameObject);
	}
	
	public void VisualAttach(Transform attachment) {
		if (attachment != null) {
			isAttached = true;
			attachedPosition = attachment;
		}
	}

	private void DestroyFood() {
		
	}

	private void OnDestroy() {
		
	}

}