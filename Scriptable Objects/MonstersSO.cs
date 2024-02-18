using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterList", menuName = "MonsterList")]
public class MonstersSO : ScriptableObject
{
	[SerializeField] public Monster MudSkipper;
	[SerializeField] public Egg MudSkipperEgg;

}
