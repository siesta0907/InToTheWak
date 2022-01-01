using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * 플레이어의 정보를 시각적으로 보여줍니다.
 * InitOwner만 설정해주면 자동으로 갱신됩니다.
 * 
 * 현재는 체력바 설정밖에 없지만 이후에 포만감 등을 추가할 예정입니다.
 */
public class Hud : MonoBehaviour
{
	[SerializeField] private Image healthBar;   // 체력바
	[SerializeField] private Image manaBar;   // 마나바

	Entity owner;								// UI를 갱신할 대상입니다.

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
		if (gap <= 0.03f)
		{
			healthBar.fillAmount = healthPercent;
		}
	}

	public void SetManaBar()
	{
		float manaPercent = owner.curMana / owner.mana;
		float gap = Mathf.Abs(manaBar.fillAmount - manaPercent);

		manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, manaPercent, 0.15f);
		if (gap <= 0.03f)
		{
			manaBar.fillAmount = manaPercent;
		}
	}

	// HUD UI 갱신
	private void UpdateHudUI()
	{
		if(owner)
		{
			SetHealthBar();
			SetManaBar();
		}
	}
}
