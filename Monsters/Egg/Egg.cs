using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	[SerializeField] private MonsterDataSO monsterData;
	[SerializeField] protected Monster hatchedMonsterPrefab;
	protected float hatchTimer;
	public bool isGrowing;

	private void Start() {
		hatchTimer = monsterData.eggHatchTime;
		isGrowing = false;
	}

	protected void HatchMonster() {
		CreateMonster();
		Destroy(gameObject);
	}

	protected void CreateMonster() {
		Monster newMonster = Instantiate(hatchedMonsterPrefab, transform.position, Quaternion.identity);
		newMonster.InitializeMonsterFromTemplate();
		MonsterManager.Instance.EggHatched(newMonster);

	}

	public float GetHatchProgressNormalized() {
		return 1 - (hatchTimer/monsterData.eggHatchTime);
	}

	public bool IsGrowing() {
		return isGrowing;
	}
}
