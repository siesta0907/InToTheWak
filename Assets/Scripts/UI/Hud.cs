using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
	[SerializeField] private Text text_Health;  // 체력 텍스트
	[SerializeField] private Text text_Satiety; // 포만감 텍스트

	Entity owner; // UI를 갱신할 대상입니다.

	void Update()
	{
		UpdateHudUI();
	}

	public void InitOwner(Entity target)
	{
		owner = target;
	}

	public void SetHealthText(float value)
	{
		text_Health.text = "체력: " + value;
	}
	public void SetSatietyText(float value)
	{
		text_Satiety.text = "포만감: " + value;
	}

	// HUD UI 갱신
	private void UpdateHudUI()
	{
		if(owner)
		{
			SetHealthText(owner.health);
			SetSatietyText(owner.satiety);
		}
	}
}
