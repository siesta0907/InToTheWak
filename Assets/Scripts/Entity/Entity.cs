using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[Header("엔티티 스탯")]
	public int strength = 1;    // 힘(공격력) - 데미지와 관련됨
	public int health = 1;      // 체력 - 몬스터와 전투시 사용
	public int hunger = 0;      // 배고픔 - 일정량 이상일시 행동 느려짐
	public int moveCount = 1;   // 이동 가능한 거리(칸수)
	public int attackRange = 1; // 공격가능 거리(칸수)

	private Material originMat;
	[SerializeField] private Material hitMat;


	protected virtual void Awake()
	{
		originMat = GetComponent<SpriteRenderer>().material;
	}

	// * 스탯 관련 함수
	public void AddStrength(int value)
	{
		strength += value;
		strength = Mathf.Clamp(strength, 0, strength);
	}

	public void AddHealth(int value)
	{
		health += value;
		health = Mathf.Clamp(health, 0, health);
	}

	public void AddHunger(int value)
	{
		hunger += value;
		hunger = Mathf.Clamp(hunger, 0, hunger);
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
	public virtual void TakeDamage(int damage, Entity attacker)
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
	protected virtual void OnDeath(Entity attacker) { }
}
