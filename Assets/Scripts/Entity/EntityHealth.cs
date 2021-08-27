using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
	[SerializeField] private Image healthBar;
	[SerializeField] private Image healthBar_white;

	Entity owner; // UI를 갱신할 대상입니다.

	void Update()
	{
		if(owner != null)
		{
			UpdateHealthBar();
		}
	}

	public void InitOwner(Entity target)
	{
		owner = target;
	}

	private void UpdateHealthBar()
	{
		float whiteAmount = healthBar_white.fillAmount;
		float barAmount = healthBar.fillAmount;

		healthBar.fillAmount = owner.curHealth / owner.health;

		// 체력바 애니메이션 (흰색 부분)
		if (whiteAmount == barAmount) return;

		float gap = whiteAmount - barAmount;
		if (gap <= 0.05f)
		{
			healthBar_white.fillAmount = barAmount;
			return;
		}
		healthBar_white.fillAmount = Mathf.Lerp(healthBar_white.fillAmount, barAmount, 0.05f);
	}
}
