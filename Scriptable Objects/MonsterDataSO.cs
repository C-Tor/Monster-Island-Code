using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "MonsterData")]
public class MonsterDataSO : ScriptableObject
{
	public MonsterType monsterType;
	public float interactRadius;
	public float daysUntilMaxSize;
	public float eggHatchTime;

	public List<Need> Needs;

}
