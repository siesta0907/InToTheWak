using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[Header("엔티티 스탯")]
	public float strength = 1;		// 힘(공격력) - 데미지와 관련됨
	public float health = 1;		// 체력 - 몬스터와 전투시 사용
	public float satiety = 100;		// 포만감 - 일정량 이상일시 행동 느려짐
	public float stress = 0;		// 스트레스 - 수치에 따라 공격력, 배고픔 등 영향
	public int moveCount = 1;		// 이동 가능한 거리(칸수)
	public int attackRange = 1;		// 공격가능 거리(칸수)

	bool isDead = false;		// 죽었는지 체크하는 상태변수

	private Material originMat;
	[SerializeField] private Material hitMat;


	protected virtual void Awake()
	{
		originMat = GetComponent<SpriteRenderer>().material;
	}

	// * 스탯 관련 함수
	public void AddStrength(float value)
	{
		strength += value;
		strength = Mathf.Clamp(strength, 0, strength);
	}

	public void AddHealth(float value)
	{
		health += value;
		health = Mathf.Clamp(health, 0, health);
	}

	public void AddSatiety(float value)
	{
		satiety += value;
		satiety = Mathf.Clamp(satiety, 0, 100);
	}

	public void AddStress(float value)
	{
		stress += value;
		stress = Mathf.Clamp(stress, 0, 100);
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

	IEnumerator HitEffectCoroutine()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.material = hitMat;

		yield return new WaitForSeconds(0.1f);

		// 원래 색으로 되돌림
		renderer.material = originMat;
	}

	// * 공격을 받으면 호출되는 함수 (데미지, 공격을 가한 객체)
	public virtual void TakeDamage(float damage, Entity attacker)
	{
		AddHealth(-damage);
		attacker.OnHit(this);
		StartCoroutine(HitEffectCoroutine());

		if (health <= 0)
		{
			OnDeath(attacker);
		}
	}

	// * 공격에 성공하면 호출되는 함수 (피해를 입은 객체)
	protected virtual void OnHit(Entity victim) { }

	// * 죽은겨우 호출되는 함수 (나를 죽인 객체)
	protected virtual void OnDeath(Entity attacker)
	{
		isDead = true;
	}
}
