using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * 모든 적의 기반이되는 스크립트입니다.
 * 기본적인 네비게이션, 체력바, 스테이지와 플레이어 정보들을 담고있습니다
 * Enemy 스크립트를 상속받아 원하시는 객체를 만드시면 됩니다.
 * EX) 보스, 근거리를 공격하는 적, 투사체를 발사하는 적, 자폭하는 적...
 */
public class Enemy : Entity
{
	[Header("Enemy Setting")]
	[SerializeField] protected int detectRange = 8;           // 탐지 거리 (탐지거리 내에 들어와야 행동)
	[SerializeField] protected float attackChance = 70f;      // 공격확률 (공격범위 내에 있을경우)

	// < 필요한 컴포넌트 >
	protected Player player;
	protected EntityHealth healthBar;	// 남은체력을 표시하기 위해 사용
	protected NavMesh2D nav;
	protected GameManager gm;
	protected BoxCollider2D col;

	protected override void Awake()
	{
		base.Awake();

		player = FindObjectOfType<Player>();
		healthBar = GetComponent<EntityHealth>();
		nav = GetComponent<NavMesh2D>();
		gm = FindObjectOfType<GameManager>();
		col = GetComponent<BoxCollider2D>();
	}

    protected override void Start()
    {
		base.Start();
		player.OnTurnEnd += EnemyTurnStart;

		// healthBar를 사용중인 경우에만 체력표시
		if(healthBar) healthBar.InitOwner(this);

		// 적 개체수 증가
		gm.AddEnemyCount(1);
	}

	// 적의 턴이 시작되었을 때
	protected virtual void EnemyTurnStart() { }

	protected override void OnDeath(Entity attacker)
	{
		base.OnDeath(attacker);

		player.OnTurnEnd -= EnemyTurnStart;

		// 애니메이션 재생 - 죽음
		anim.SetTrigger("Dead");

		// 죽었을 때 설정 (충돌 해제, 개체수 감소...)
		gm.AddEnemyCount(-1);
		col.enabled = false;
		if (nav != null) nav.navVolume.SetWallAtPosition(transform.position, false);

		Destroy(this.gameObject, 6.0f);
	}
}
