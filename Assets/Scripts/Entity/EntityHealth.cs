using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * 적의 체력바를 보여주는 스크립트입니다.
 * InitOwner함수에 Entity 타입의 객체를 넣으면 활성화됩니다. (없으면 사용되지 않음)
 * Enemy를 상속받은 객체들은 InitOwner가 불려지기 때문에,
 * 체력바 사용을 위해서 EntityHealth 컴포넌트만 추가하면 됩니다.
 */
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
