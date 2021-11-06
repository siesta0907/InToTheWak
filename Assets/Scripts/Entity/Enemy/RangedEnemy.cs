using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
 * 투사체 타입의 적 객체입니다.
 * 탐지거리 내에고 공격범위 내에 있고, 공격범위 밖에있는 플레이어에게 투사체를 발사합니다.
 */
public class RangedEnemy : Enemy
{
	[SerializeField] private float projectileChance = 40f;  // 투사체 소환확률 (공격범위 밖 투사체)
	[SerializeField] private float projectileSpd = 5f;		// 투사체 속도
	[SerializeField] private GameObject projectile;			// 소환할 투사체
	Coroutine attackCoroutine;


	// TODO: 밸런스를 파일로 수정할 수 있게 해두었으므로 밸런스 조절후 Awake 메소드는 삭제됩니다.
	protected override void Awake()
	{
		base.Awake();

		// Load JSON
		string PATH = Application.dataPath + "/Data/Entity/" + gameObject.name + ".json";
		if (File.Exists(PATH))
		{
			string loadjson = File.ReadAllText(PATH);
			RangedEnemyData data = JsonUtility.FromJson<RangedEnemyData>(loadjson);
			strength = data.strength;
			health = data.health;
			moveCount = data.moveCount;
			attackRange = data.attackRange;
			detectRange = data.detectRange;
			attackChance = data.attackChance;
			projectileChance = data.projectileChance;
			projectileSpd = data.projectileSpd;
		}
	}


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


		// 탐지거리 범위안에 있고 공격범위 밖에있으면 있으면 투사체를 발사합니다.
		if (distance <= detectRange && distance > attackRange)
		{
			if (Random.Range(0, 100) < projectileChance)
			{
				if (attackCoroutine != null)
					StopCoroutine(attackCoroutine);
				attackCoroutine = StartCoroutine(AttackCorotuine_Projectile());

				// 애니메이션 재생 - 공격
				anim.SetTrigger("Attack");
				LookEntity(player, true);
				return;
			}
		}

		// Attack - 플레이어의 도착위치가 적의 위치 차이가 공격범위 이내일때, 이동하지 않고 공격합니다.
		if (distance <= detectRange && distance <= attackRange)
		{
			if (Random.Range(0, 100) < attackChance)
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
		else if(distance <= detectRange)
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


	IEnumerator AttackCorotuine_Projectile()
	{
		// 플레이어의 이동을 기다리고 공격
		yield return new WaitForSeconds(attackDelay);

		// 투사체 생성
		GameObject storm = Instantiate(projectile, transform.position, Quaternion.identity);
		storm.GetComponent<Projectile>().SetData(this, strength, projectileSpd, player.transform.position - transform.position);
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
