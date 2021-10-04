using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Negeza : Enemy
{
	Coroutine attackCoroutine;

	protected override void EnemyTurnStart()
	{
		base.EnemyTurnStart();

		// 죽지 않은 경우에만
		if(!isDead)
		{
			MoveAndAttack();    // 이동과 공격패턴
			MoveAnimation();	// 이동 애니메이션
		}
	}

	void MoveAndAttack()
	{
		float distance = Vector3.Distance(player.targetPos, transform.position);

		// Attack - 플레이어의 도착위치가 팬치의 위치 차이가 공격범위 이내일때, 이동하지 않고 공격합니다.
		if(distance <= attackRange)
		{
			if(attackCoroutine != null)
				StopCoroutine(attackCoroutine);
			attackCoroutine = StartCoroutine(AttackCorotuine());

			// 애니메이션 재생 - 공격
			anim.SetTrigger("Attack");
			LookEntity(player, true);
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
		yield return new WaitForSeconds(GameData.instance.turnDelay);

		Debug.Log(transform.name + "에게 공격당함!");
		player.TakeDamage(strength, this);
	}

	private void MoveAnimation()
	{
		// 방향 처리
		if (nav.velocity.x != 0)
		{
			sr.flipX = (nav.velocity.x < 0) ? false : true;
		}
	}
}
