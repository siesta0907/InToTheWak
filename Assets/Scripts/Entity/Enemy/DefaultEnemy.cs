using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 기본 타입의 적 객체입니다.
 * 탐지거리 내에 있는 플레이어를 추격하고, 공격범위 내의 플레이어를 공격합니다
 */
public class DefaultEnemy : Enemy
{
	Coroutine attackCoroutine;

	// 턴이 시작될때
	protected override void EnemyTurnStart()
	{
		base.EnemyTurnStart();

		// 죽지 않은 경우에만
		if(!isDead)
		{
			MoveAndAttack();    // 이동과 공격패턴
		}
	}

	void Update()
	{
		if (!isDead)
		{
			MoveAnimation();    // 이동 애니메이션
		}
	}

	void MoveAndAttack()
	{
		float distance = Vector3.Distance(player.targetPos, transform.position);

		// 탐지거리 범위내에 있지 않으면 행동하지 않습니다.
		if (distance > detectRange) return;

		// Attack - 플레이어의 도착위치가 적의 위치 차이가 공격범위 이내일때, 이동하지 않고 공격합니다.
		if(distance <= attackRange)
		{
			// 정해진 확률에 따라 공격함
			if(Random.Range(0, 100) < attackChance)
			{
				if (attackCoroutine != null)
					StopCoroutine(attackCoroutine);
				attackCoroutine = StartCoroutine(AttackCorotuine());

				// 애니메이션 재생 - 공격
				anim.SetTrigger("Attack");
				LookEntity(player, true);
			}
		}

		// Chase - 차이가 난다면 플레이어를 추격합니다.
		else
		{
			Vector2Int playerPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
			nav.MoveTo(playerPos, moveCount);
		}
	}

	IEnumerator AttackCorotuine()
	{
		// 플레이어의 이동을 기다리고 공격
		yield return new WaitForSeconds(attackDelay);

		// TODO: 이후에 지울 Debug.Log
		Debug.Log(transform.name + "에게 공격당함!");
		player.TakeDamage(strength, this);
	}

	private void MoveAnimation()
	{
		// 애니메이션 처리
		if (nav.velocity.magnitude > 0)
			anim.SetBool("IsMove", true);
		else
			anim.SetBool("IsMove", false);

		// 방향 처리
		if (nav.velocity.x != 0)
			sr.flipX = (nav.velocity.x < 0) ? false : true;
	}
}
