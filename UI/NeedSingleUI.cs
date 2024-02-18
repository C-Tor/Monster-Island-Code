using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedSingleUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI needNameText;
	[SerializeField] private Image needFillBar;

	public void SetNeedUI(Need need) {
		needNameText.text = need.name;
		needFillBar.fillAmount = (need.value / need.maxValue);
		if (need.value <= need.neededValue) {
			needFillBar.color = Color.yellow;
		} else {
			needFillBar.color = Color.green;
		}
		if (need.value <= 0) {
			needFillBar.fillAmount = 0.01f;
			needFillBar.color = Color.red;
		} 
	}
}
