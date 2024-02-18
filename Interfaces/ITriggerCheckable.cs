using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable
{
	bool IsFoodInVicinity { get; set; }
	bool IsWithinInteractDistance { get; set; }

	void SetFoodInVicinityStatus(bool isChasing);
	void SetInteractDistanceBool(bool isWithinInteractDistance);
}
