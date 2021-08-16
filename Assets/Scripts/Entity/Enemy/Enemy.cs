using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
	// < 필요한 컴포넌트 >
	protected Player player;
	protected EntityHealth healthBar;	// 남은체력을 표시하기 위해 사용
	protected NavMesh2D nav;

	protected override void Awake()
	{
		base.Awake();

		player = FindObjectOfType<Player>();
		healthBar = GetComponent<EntityHealth>();
		nav = GetComponent<NavMesh2D>();
	}

    protected override void Start()
    {
		base.Start();
		player.OnTurnEnd += EnemyTurnStart;

		// healthBar를 사용중인 경우에만 체력표시
		if(healthBar) healthBar.InitOwner(this);
	}

	// 적의 턴이 시작되었을 때
	protected virtual void EnemyTurnStart() { }

	protected override void OnDeath(Entity attacker)
	{
		base.OnDeath(attacker);

		player.OnTurnEnd -= EnemyTurnStart;

		// TODO: 나중에 죽는모션으로 변경
		Destroy(this.gameObject);
	}
}
