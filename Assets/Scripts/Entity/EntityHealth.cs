using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
	[SerializeField] private Text text_Health;

	public void UpdateHealthText(float amount)
	{
		text_Health.text = amount.ToString();
	}
}
