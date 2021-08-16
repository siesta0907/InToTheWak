using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
	[SerializeField] private Text text_Health;

	Entity owner; // UI를 갱신할 대상입니다.

	void Update()
	{
		if(owner != null)
		{
			UpdateHealthText(owner.curHealth);
		}
	}

	public void InitOwner(Entity target)
	{
		owner = target;
	}

	public void UpdateHealthText(float amount)
	{
		text_Health.text = amount.ToString();
	}
}
