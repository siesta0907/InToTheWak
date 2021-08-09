using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
	// < 필요한 컴포넌트 >
	Player player;
	NavMesh2D nav;

	void Awake()
	{
		player = FindObjectOfType<Player>();
		nav = GetComponent<NavMesh2D>();
	}

    void Start()
    {
		player.OnTurnEnd += Move;
    }

	// 적 이동
	void Move()
	{
		float distance = Vector3.Distance(player.targetPos, transform.position);
		if (distance > 1)
		{
			Vector2Int playerPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
			nav.MoveTo(playerPos, moveCount);
		}
		
		StopAllCoroutines();
		StartCoroutine(AttackCorotuine());
	}

	protected override void OnDeath(Entity attacker)
	{
		base.OnDeath(attacker);
		player.OnTurnEnd -= Move;
	}

	// 적 공격 - 이동까지 대기 후 데미지
	IEnumerator AttackCorotuine()
	{
		yield return new WaitForSeconds(GameData.instance.turnDelay);
		float distance = Vector3.Distance(player.targetPos, transform.position);
		if (distance <= 1.1)
		{
			Debug.Log(transform.name + "에게 공격당함!");
			player.TakeDamage(strength, this);
		}
	}
}
