using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Items/Food")]
public class FoodDataSO : ScriptableObject
{
	public string foodName;
	public float foodValue;
	public float waterValue;
}
