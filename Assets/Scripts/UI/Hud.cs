using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
	[SerializeField] private Image healthBar;   // 체력바

	Entity owner; // UI를 갱신할 대상입니다.

	void Update()
	{
		UpdateHudUI();
	}

	public void InitOwner(Entity target)
	{
		owner = target;
	}

	public void SetHealthBar()
	{
		float healthPercent = owner.curHealth / owner.health;
		float gap = Mathf.Abs(healthBar.fillAmount - healthPercent);

		healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, healthPercent, 0.15f);
		if(gap <= 0.03f)
		{
			healthBar.fillAmount = healthPercent;
		}
	}

	// HUD UI 갱신
	private void UpdateHudUI()
	{
		if(owner)
		{
			SetHealthBar();
		}
	}
}
