using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 자폭 타입의 적 객체입니다.
 * 탐지거리 내에 있는 플레이어를 쫓아가 자폭합니다.
 */

public class TrapEnemy : Enemy
{
	Coroutine attackCoroutine;

	// 턴이 시작될때
	protected override void EnemyTurnStart()
	{
		base.EnemyTurnStart();

		// 죽지 않은 경우에만
		if (!isDead)
		{
			Bomb();    // 이동과 공격패턴
		}
	}


	void Bomb()
	{
		float distance = Vector3.Distance(player.targetPos, transform.position);

		// Attack - 플레이어의 도착위치가 적의 위치 차이가 공격범위 이내일때, 공격합니다.
		if (distance <= attackRange && !invincible)
		{
			// 애니메이션 재생 - 이동
			anim.SetBool("IsMove", true);
			invincible = true;
			if (attackCoroutine != null)
				StopCoroutine(attackCoroutine);
			attackCoroutine = StartCoroutine(AttackCorotuine());
		}
	}

	IEnumerator AttackCorotuine()
	{

		Vector3 originPos = transform.position;
		float lerpScale = 0.0f;

		// 플레이어에게 이동한 뒤 공격
		while(lerpScale < 1.0f)
		{
			transform.position = Vector3.Lerp(originPos, player.transform.position, lerpScale);
			lerpScale += Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(attackDelay);

		// 애니메이션 재생 - 공격
		anim.SetTrigger("Attack");

		player.TakeDamage(strength, this);

		OnDeath(null);
	}
}
