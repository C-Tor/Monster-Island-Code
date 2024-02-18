using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MemoryData {
	public MemoryData(Vector3 Bed, Vector3 Food, Vector3 Toy, Vector3 Water) {
		lastUsedBed = Bed;
		lastUsedToy = Toy;
		lastUsedFood = Food;
		lastUsedWater = Water;
	}
	public MemoryData(MemoryData data) {
		lastUsedBed = data.lastUsedBed;
		lastUsedToy = data.lastUsedToy;
		lastUsedFood = data.lastUsedFood;
		lastUsedWater = data.lastUsedWater;
	}
	public Vector3 lastUsedBed;
	public Vector3 lastUsedFood;
	public Vector3 lastUsedWater;
	public Vector3 lastUsedToy;
}

/// <summary>
/// Serves as a reference to items in which the monster remembers the location of.
/// Remembers locations of object when last interacted with them.
/// IE monster will remember the last spot it was fed and goes there when he's hungry,
/// remembers where their bed is.
/// </summary>
public class MonsterMemory : MonoBehaviour
{
	//private Queue<Transform> rememberedObjects;
	public MemoryData memoryData;

	private void Start() {
		//memoryData.lastUsedBed = Vector3.zero;
		//memoryData.lastUsedFood = Vector3.zero;
		//memoryData.lastUsedWater = Vector3.zero;
		//memoryData.lastUsedToy = Vector3.zero;

		// set a bed to remember by default. Not very good code. Change after demo
		SetRememberedLocation(ItemType.Bed, FindAnyObjectByType<Bed>().transform.position);
	}

	public Vector3 GetItemRememberedLocation(ItemType itemType) {
		switch (itemType) {
			case ItemType.Bed: return memoryData.lastUsedBed;
			case ItemType.Food: return memoryData.lastUsedFood;
			case ItemType.Water: return memoryData.lastUsedWater;
			default:
				return Vector3.zero;
		}
	}

	//there gotta be a better way to do this shit
	public void SetRememberedLocation(ItemType itemType, Vector3 location) {
		switch (itemType) {
			case ItemType.Bed:
				memoryData.lastUsedBed = location;
				break;
			case ItemType.Food:
				memoryData.lastUsedFood = location;
				break;
			case ItemType.Water:
				memoryData.lastUsedWater = location;
				break;
			case ItemType.Toy:
				memoryData.lastUsedToy = location;
				break;
			default:
				break;
		}
	}

	public void UnsetRememberedLocation(ItemType itemType) {
		switch (itemType) {
			case ItemType.Bed: 
				memoryData.lastUsedBed = Vector3.zero;
				break;
			case ItemType.Food:
				memoryData.lastUsedFood = Vector3.zero;
				break;
			case ItemType.Water:
				memoryData.lastUsedBed = Vector3.zero;
				break;
			default:
				break;
		}
	}

}
