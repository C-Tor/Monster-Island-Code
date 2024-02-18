using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Responsible for managing the behavior and state transitions
/// of a monster. Utilizes a state machine to control the monster's
/// actions, and it handles interactions with the environment, such as
/// detecting nearby interactable objects
/// </summary>
public class MonsterBehaviorController : MonoBehaviour {
	private Monster monster;

	// A list to hold all interactable objects in vicinity
	public List<Interactable> detectedInteractables = new List<Interactable>();
	
	#region State Machine Variables

	public MonsterStateMachine StateMachine { get; set; }

	public MonsterIdleState IdleState { get; set; }
	public MonsterSeekNeedState SeekNeedState { get; set; }
	public MonsterSleepState SleepState { get; set; }
	public MonsterLookForState LookForState { get; set; }

	#endregion

	#region Scriptable Object Variables

	[SerializeField] private MonsterIdleSOBase MonsterIdleBehavior;
	[SerializeField] private MonsterSeekNeedSOBase MonsterSeekNeedBehavior;
	[SerializeField] private MonsterSleepingSOBase MonsterSleepingBehavior;
	[SerializeField] private MonsterLookForSOBase MonsterLookForBehavior;

	public MonsterIdleSOBase MonsterIdleInstance { get; set; }
	public MonsterSeekNeedSOBase MonsterSeekNeedInstance { get; set; }
	public MonsterSleepingSOBase MonsterSleepInstance { get; set; }
	public MonsterLookForSOBase MonsterLookForInstance { get; set; }
	#endregion
	
	private void Awake() {

		monster = GetComponent<Monster>();

		MonsterIdleInstance = Instantiate(MonsterIdleBehavior);
		MonsterSeekNeedInstance = Instantiate(MonsterSeekNeedBehavior);
		MonsterSleepInstance = Instantiate(MonsterSleepingBehavior);
		MonsterLookForInstance = Instantiate(MonsterLookForBehavior);

		StateMachine = new MonsterStateMachine();

		IdleState = new MonsterIdleState(this, StateMachine);
		SeekNeedState = new MonsterSeekNeedState(this, StateMachine);
		SleepState = new MonsterSleepState(this, StateMachine);
		LookForState = new MonsterLookForState(this, StateMachine);

	}


	// Start is called before the first frame update
	private void Start()
    {

		TimingManager.Instance.OnTick += TimingManager_OnTick;

		MonsterIdleInstance.Initialize(gameObject, monster);
		MonsterSeekNeedInstance.Initialize(gameObject, monster);
		MonsterSleepInstance.Initialize(gameObject, monster);
		MonsterLookForInstance.Initialize(gameObject, monster);

		StateMachine.Initialize(IdleState);
	}

	private void FixedUpdate() {
		StateMachine.CurrentMonsterState.FrequentUpdate();
	}

	// Update is called once per frame
	private void Update()
    {
		StateMachine.CurrentMonsterState.FrameUpdate();

		if (Input.GetKeyDown(KeyCode.P)) {
			PrintDetectedInteractables();
		}
	}

	public void Tick() {
		StateMachine.CurrentMonsterState.TickUpdate();
	}

	public void SeekFulfillment(ItemType itemType) {
		if (GetNearbyItem(itemType) != null) {
			MonsterSeekNeedInstance.SetHuntedObject(GetNearbyItem(itemType));
			StateMachine.ChangeState(SeekNeedState);
		} else if (monster.memory.GetItemRememberedLocation(itemType) != Vector3.zero) {
			// If no nearby object and remembered a location for object
			MonsterLookForInstance.LookFor(itemType);
			StateMachine.ChangeState(LookForState);
		}
	}


	#region Detected Interactables
	public void DetectedObjectEntered(Interactable detectedObject) {
		if (detectedObject != null && !detectedInteractables.Contains(detectedObject)) {
			detectedInteractables.Add(detectedObject);

			// Remember object location
			monster.memory.SetRememberedLocation(detectedObject.ItemType, detectedObject.transform.position);
		}
	}

	public void DetectedObjectExited(Interactable detectedObject) {
		if (detectedObject != null && detectedInteractables.Contains(detectedObject)) {
			detectedInteractables.Remove(detectedObject);
		}
	}

	public void RemoveFromDetected(Interactable detectedObject) {
		detectedInteractables.Remove(detectedObject);

	}
	#endregion



	public Interactable GetNearbyItem(ItemType itemType) {
		foreach (Interactable interactable in detectedInteractables) {
			if (interactable.ItemType == itemType) {
				if (interactable != null && interactable != null) {
					return interactable;
				}
			}
		}
		//no match is found
		return null;
	}

	private void PrintDetectedInteractables() {
		Debug.Log("detectedInteractables: ");
		if (detectedInteractables.Count == 0) {
			Debug.Log("No interactables in vicinity");
		} else {
			foreach (var interactable in detectedInteractables) {
				Debug.Log("Detected Interactable: " + interactable);
			}
		}
		Debug.Log("====================================");
	}

	private void TimingManager_OnTick() {
		Tick();
	}


}
