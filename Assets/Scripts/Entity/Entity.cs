using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[Header("엔티티 스탯")]
	public string entityName = "Entity";
	public int minDamage = 1;         // 최소 데미지
	public int maxDamage = 1;         // 최대 데미지
	public float health = 1;			// 체력 - 몬스터와 전투시 사용
	public float satiety = 100;			// 포만감 - 일정량 이상일시 체력 감소 등..
	public float mana = 0;				// 마나
	public int moveCount = 1;			// 이동 가능한 거리(칸수)
	public int attackRange = 1;			// 공격가능 거리(칸수)
	public float attackDelay = 0.25f;	// 공격 딜레이(공격을 맞기까지의 시간)
	public bool invincible = false;

	[HideInInspector] public float curHealth = 0;	// 현재 체력입니다.
	[HideInInspector] public float curSatiety = 0;	// 현재 포만감입니다.
	[HideInInspector] public float curMana = 0;		// 현재 마나입니다.

	[HideInInspector] public bool isDead = false;	// 죽었는지 체크하는 상태변수

	[Header("피격 효과")]
	protected Material originMat;
	[SerializeField] protected Material hitMat;

	// < 필요한 컴포넌트 >
	[SerializeField] protected SpriteRenderer sr;   // Sprite Renderer
	protected Animator anim;                        // Entity의 애니메이터


	protected virtual void Awake()
	{
		originMat = GetComponentInChildren<SpriteRenderer>().material;
		anim = GetComponentInChildren<Animator>();
	}

	protected virtual void Start()
	{
		curHealth = health;
		curSatiety = satiety;
		curMana = mana;
	}

	// * 스탯 관련 함수
	public void SetMinMaxDamage(int minDmg, int maxDmg)
	{
		minDamage = minDmg;
		maxDamage = maxDmg;
	}

	public void AddMinDamage(int value)
	{
		minDamage += value;
		minDamage = Mathf.Clamp(minDamage, 0, maxDamage);
	}

	public void AddMaxDamage(int value)
	{
		maxDamage += value;
		maxDamage = Mathf.Clamp(maxDamage, minDamage, maxDamage);
	}

	public void AddHealth(float value)
	{
		curHealth += value;
		curHealth = Mathf.Clamp(curHealth, 0, health);
	}

	public void AddSatiety(float value)
	{
		curSatiety += value;
		curSatiety = Mathf.Clamp(curSatiety, 0, satiety);
	}

	public void AddMana(float value)
	{
		curMana += value;
		curMana = Mathf.Clamp(curMana, 0, mana);
	}

	public void AddMoveCount(int value)
	{
		moveCount += value;
		moveCount = Mathf.Clamp(moveCount, 0, moveCount);
	}

	public void AddAttackRange(int value)
	{
		attackRange += value;
		attackRange = Mathf.Clamp(attackRange, 0, attackRange);
	}

	public int GetRandomDamage()
	{
		int rDamage = Random.Range(minDamage, maxDamage + 1);
		return rDamage;
	}

	IEnumerator HitEffectCoroutine()
	{
		sr.material = hitMat;
		yield return new WaitForSeconds(0.1f);

		// 원래 색으로 되돌림
		sr.material = originMat;
	}

	// * 공격을 받으면 호출되는 함수 (데미지, 공격을 가한 객체)
	public virtual void TakeDamage(int damage, Entity attacker)
	{
		if(!isDead)
		{
			AddHealth(-damage);
			attacker.OnHit(this);
			StartCoroutine(HitEffectCoroutine());

			if (curHealth <= 0)
			{
				OnDeath(attacker);
			}
		}
	}

	// * 공격에 성공하면 호출되는 함수 (피해를 입은 객체)
	protected virtual void OnHit(Entity victim) { }

	// * 죽은겨우 호출되는 함수 (나를 죽인 객체)
	protected virtual void OnDeath(Entity attacker)
	{
		isDead = true;
	}

	// * 대상을 바라보게 만드는 함수 (reverse는 각 스프라이트마다 바라보는 방향이 달라 넣었습니다.)
	protected virtual void LookEntity(Entity enemy, bool reverse)
	{
		int x1 = (int)transform.position.x;
		int x2 = (int)enemy.gameObject.transform.position.x;

		if (x1 - x2 > 0)
			sr.flipX = !reverse;
		else if (x1 - x2 < 0)
			sr.flipX = reverse;
	}
}
