using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
	[SerializeField] private Text text_Health;  // 체력 텍스트
	[SerializeField] private Text text_Hunger;  // 배고픔 텍스트

	Entity owner; // UI를 갱신할 대상입니다.

	void Update()
	{
		UpdateHudUI();
	}

	public void InitOwner(Entity target)
	{
		owner = target;
	}

	public void SetHealthText(int value)
	{
		text_Health.text = "체력: " + value;
	}
	public void SetHungerText(int value)
	{
		text_Hunger.text = "배고픔: " + value;
	}

	// HUD UI 갱신
	private void UpdateHudUI()
	{
		if(owner)
		{
			SetHealthText(owner.health);
			SetHungerText(owner.hunger);
		}
	}
}
