using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUIManager : MonoBehaviour
{
	[SerializeField] private Text text_Health;  // 체력 텍스트
	[SerializeField] private Text text_Hunger;  // 배고픔 텍스트

	Player owner; // UI를 갱신할 대상입니다.

	void Awake()
	{
		owner = FindObjectOfType<Player>();
	}

	void Update()
	{
		UpdateHudUI();
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
		SetHealthText(owner.health);
		SetHungerText(owner.hunger);
	}
}
