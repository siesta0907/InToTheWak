using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pungsin : Enemy
{
	[Header("Pattern - Wind")]
	[SerializeField] private GameObject go_Wind;
	[SerializeField] private float windAngle;
	[SerializeField] private float windDamage;
	[SerializeField] private float windSpeed;

	[Header("Pattern - Lighting")]
	[SerializeField] private GameObject go_DangerMark;
	[SerializeField] private GameObject go_Lighting;
	[SerializeField] private int lightningRange;
	[SerializeField] private float lightningDamage;

	[Header("Pattern - Push")]
	[SerializeField] private int pushAmount = 2;

	List<Vector3> lightningPos = new List<Vector3>();

	protected override void Start()
	{
		base.Start();

		player.SetPlayerTurn(false);
		player.currentTurnDelay += 3.0f;
		anim.SetTrigger("StageStart");
	}


	protected override void EnemyTurnStart()
	{
		base.EnemyTurnStart();

		if(!isDead)
		{
			StartCoroutine(SpawnLighting());

			int r = Random.Range(0, 100);
			if(r <= 35)
			{
				switch(r % 3)
				{
					case 0:
						StartCoroutine(Pattern_WindCoroutine());
						break;
					case 1:
						StartCoroutine(Pattern_PushCoroutine());
						break;
					case 2:
						StartCoroutine(Pattern_LightingCoroutine());
						break;
				}
			}
			else
			{
				Move();
			}
		}
	}


	void Move()
	{
		Vector2Int playerPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
		nav.MoveTo(playerPos, moveCount);
	}


	// 패턴1 - 바람 방출
	IEnumerator Pattern_WindCoroutine()
	{
		anim.SetTrigger("Skill1");
		player.currentTurnDelay += 1.2f;
		yield return new WaitForSeconds(1.5f);
		Pattern_Wind();
	}


	private void Pattern_Wind()
	{
		float curAngle = -windAngle;

		for(int i = 0; i < 3; i++)
		{
			// 각도 계산
			Vector3 dir = player.transform.position - transform.position;
			float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

			// 투사체 생성
			GameObject projectile = Instantiate(go_Wind, transform.position, Quaternion.identity);
			projectile.transform.rotation = Quaternion.Euler(0, 0, zAngle + curAngle);
			projectile.GetComponent<Projectile>().SetData(this, windDamage, windSpeed);

			curAngle += windAngle;
		}
	}


	// 패턴2 - 번개
	IEnumerator Pattern_LightingCoroutine()
	{
		anim.SetTrigger("Skill2");
		player.currentTurnDelay += 1.5f;
		yield return new WaitForSeconds(1.5f);
		Pattern_Lighting();
	}


	private void Pattern_Lighting()
	{
		for(int i = 0; i < 5; i++)
		{
			int randX = Random.Range(-lightningRange, lightningRange + 1);
			int randY = Random.Range(-lightningRange, lightningRange + 1);

			Vector3 randPos = transform.position + new Vector3(randX, randY, 0);
			GameObject mark = Instantiate(go_DangerMark, randPos, Quaternion.identity);

			lightningPos.Add(mark.transform.position);
			Destroy(mark.gameObject, 1.0f);
		}
	}

	IEnumerator SpawnLighting()
	{
		if (lightningPos.Count > 0)
		{
			yield return new WaitForSeconds(GameData.instance.turnDelay);
			for (int i = 0; i < lightningPos.Count; i++)
			{
				Vector3 spawnPos = lightningPos[i];
				GameObject lightning = Instantiate(go_Lighting, spawnPos, Quaternion.identity);
				lightning.GetComponent<HitBox>().SetData(this, lightningDamage, 1.0f);
			}

			lightningPos.Clear();
		}
	}


	// 패턴3 - 플레이어 밀기
	IEnumerator Pattern_PushCoroutine()
	{
		anim.SetTrigger("Skill" + 3);
		player.currentTurnDelay += 1.5f;
		yield return new WaitForSeconds(1.5f);
		Pattern_Push();
	}


	private void Pattern_Push()
	{
		Vector2 direction = player.transform.position - transform.position;
		if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
			direction.y = 0;
		else
			direction.x = 0;

		direction = direction.normalized;
		player.Push(direction, pushAmount);
	}
}
