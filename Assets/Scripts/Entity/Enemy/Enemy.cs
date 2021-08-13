using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
	Coroutine moveCoroutine;

	// < 필요한 컴포넌트 >
	Player player;
	EntityHealth healthBar;	// 남은체력을 표시하기 위해 사용
	NavMesh2D nav;

	protected override void Awake()
	{
		base.Awake();

		player = FindObjectOfType<Player>();
		healthBar = GetComponent<EntityHealth>();
		nav = GetComponent<NavMesh2D>();
	}

    void Start()
    {
		player.OnTurnEnd += Move;
		healthBar.UpdateHealthText(health);
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
		
		if(moveCoroutine != null)
		{
			StopCoroutine(moveCoroutine);
		}
		moveCoroutine = StartCoroutine(AttackCorotuine());
	}

	protected override void OnDeath(Entity attacker)
	{
		base.OnDeath(attacker);

		player.OnTurnEnd -= Move;
		Destroy(this.gameObject);
	}

	public override void TakeDamage(float damage, Entity attacker)
	{
		base.TakeDamage(damage, attacker);

		healthBar.UpdateHealthText(health);
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
