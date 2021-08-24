using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pungsin : Enemy
{
	protected override void EnemyTurnStart()
	{
		base.EnemyTurnStart();

		StartCoroutine(AttackCorotuine());
	}

	IEnumerator AttackCorotuine()
	{
		// 플레이어의 이동을 기다리고 공격
		anim.SetTrigger("Skill" + 3);
		player.currentTurnDelay += 1.5f;
		yield return new WaitForSeconds(1.5f);
		Pattern3();
	}

	private void Pattern3()
	{
		Vector2 Direction = player.transform.position - transform.position;
		if(Mathf.Abs(Direction.x) >= Mathf.Abs(Direction.y))
			Direction.y = 0;
		else
			Direction.x = 0;

		Direction = Direction.normalized;
		player.Push(Direction, 2);
	}
}
