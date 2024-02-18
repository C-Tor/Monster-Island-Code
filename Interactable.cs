using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	public ItemType ItemType { get; protected set; }

	public virtual void Interact(GameObject interactingObject) {

	}


}
