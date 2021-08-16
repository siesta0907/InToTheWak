using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
	[SerializeField] private Text text_Health;  // 체력 텍스트
	[SerializeField] private Text text_Satiety; // 포만감 텍스트

	[SerializeField] private Image healthBar;	// 체력바

	Entity owner; // UI를 갱신할 대상입니다.

	void Update()
	{
		UpdateHudUI();
	}

	public void InitOwner(Entity target)
	{
		owner = target;
	}

	public void SetHealthBar(float value)
	{
		healthBar.fillAmount = value / owner.health;
	}

	// HUD UI 갱신
	private void UpdateHudUI()
	{
		if(owner)
		{
			SetHealthBar(owner.curHealth);
		}
	}
}
