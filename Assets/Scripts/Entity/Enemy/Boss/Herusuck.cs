using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Herusuck : Enemy
{

	[SerializeField] private float[] comboDamage;	// 해루석 보스는 데미지가 Strength로 관리되지 않습니다.
	[SerializeField] private float[] comboDamage_Plus;

	[Header("Pattern - Skill1")]
	[SerializeField] private int upgradeCnt = 4;    // 공격강화 횟수	(5 = 기본공격 5번이 강화상태로 발동)

	[Header("Pattern - Event")]
	[SerializeField] private Material unlitMat;     // 연출을 위한 메테리얼, 라이트
	[SerializeField] private Light2D gl;


	bool skillMode = false;							// 공격 강화상태인지			(true = 기본공격이 강화된 공격으로)
	bool eventPattern = false;						// 이벤트 패턴을 사용했는지	(50% 이하일시 스킬강화 + QTE)
	int comboCnt = 1;								// 콤보카운트				(1 ~ 4)
	int remainCnt = 0;								// 남은 공격강화 횟수
	Coroutine attackCoroutine;
	SkillCheck skillCheck;


	protected override void Start()
	{
		base.Start();

		// 스킬체크 콜백함수 등록
		skillCheck = FindObjectOfType<SkillCheck>();
		skillCheck.OnFailed += QTE_Fail;
		skillCheck.OnSuccess += QTE_Success;
		skillCheck.OnPerfect += QTE_Perfect;

		player.SetPlayerTurn(false, 2.5f);
		anim.SetTrigger("StageStart");
	}


	protected override void EnemyTurnStart()
	{
		base.EnemyTurnStart();

		if(!isDead)
		{
			if (!eventPattern && curHealth <= (health / 2))
				StartCoroutine(Pattern_EventCoroutine());
			else
			{
				// 패턴들....
				int r = Random.Range(0, 100);

				// 강화공격 상태가 아니고, 체력이 50% 이하가 아니라면 - 20% 확률로 스킬강화
				if (r <= 20 && !skillMode && !eventPattern)
				{
					Pattern_SkillUpgrade();
					return;
				}

				// 체력이 50% 이하, 이벤트가 발생한 경우라면 (상시 강화공격) - 20% 확률로 QTE 발생
				float distance = Vector3.Distance(player.targetPos, transform.position);
				if (r <= 40 && eventPattern && distance <= attackRange)
				{
					Pattern_QTE();
					return;
				}

				// 기본공격, 이동
				MoveAndAttack();
			}
		}
	}


	void MoveAndAttack()
	{
		float distance = Vector3.Distance(player.targetPos, transform.position);
		// Attack - 플레이어의 도착위치가 보스의 위치 차이가 공격범위 이내일때, 이동하지 않고 공격합니다.
		if (distance <= attackRange)
		{
			if (attackCoroutine != null)
				StopCoroutine(attackCoroutine);

			// 콤보 데미지 계산
			float damage = (skillMode) ? comboDamage_Plus[comboCnt - 1] : comboDamage[comboCnt - 1];


			// 애니메이션 재생 - 공격
			anim.SetTrigger("Attack_" + comboCnt);
			attackCoroutine = StartCoroutine(AttackCorotuine(damage));
		}
		else
		{
			Vector2Int tmp = new Vector2Int(Random.Range(-1, 1), Random.Range(-1, 1));
			Vector2Int playerPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y) + tmp;
			nav.MoveTo(playerPos, moveCount);
		}
	}


	IEnumerator AttackCorotuine(float damage)
	{
		// 플레이어의 이동을 기다리고 공격
		player.SetPlayerTurn(false, attackDelay + 0.3f);
		yield return new WaitForSeconds(GameData.instance.turnDelay + attackDelay);

		comboCnt = comboCnt % 4 + 1;
		// 강화공격 상태에서 공격시 카운트 차감, 모두 소진시 기본공격으로 전환
		if (!eventPattern && remainCnt > 0)
		{
			remainCnt--;
			if (remainCnt == 0) SkillMode(false);
		}

		player.TakeDamage(damage, this);
	}



	// 스킬 강화모드 설정
	void SkillMode(bool flag)
	{
		skillMode = flag;
		anim.SetBool("Event", skillMode);
	}


	// 이벤트 패턴 - 기본공격 상시강화, QTE
	IEnumerator Pattern_EventCoroutine()
	{
		player.SetPlayerTurn(false, 5.0f);

		// 1. 대사출력


		// 2. 연출시작, 기본공격 상시강화
		yield return new WaitForSeconds(0.5f);
		sr.material = unlitMat;

		// 점점 어둡게
		while(gl.intensity > 0.05f)
		{
			gl.intensity -= Time.deltaTime;
			yield return null;
		}
		gl.intensity = 0.05f;

		// 스킬강화, 애니메이션 재생
		SkillMode(true);
		eventPattern = true;
		anim.SetTrigger("Skill_Event");

		yield return new WaitForSeconds(3.0f);

		// 점점 밝게
		while (gl.intensity < 1.0f)
		{
			gl.intensity += Time.deltaTime;
			yield return null;
		}
		gl.intensity = 1.0f;
		sr.material = originMat;

		// 3. QTE 실행
		Pattern_QTE();
	}


	// 패턴1 - 강화공격
	void Pattern_SkillUpgrade()
	{
		player.SetPlayerTurn(false, 2f);
		anim.SetTrigger("Skill1");
		SkillMode(true);
		remainCnt = upgradeCnt;
	}


	// 패턴2 - QTE
	void Pattern_QTE()
	{
		Debug.Log("QTE 재생");
		player.SetPlayerTurn(false, 9999999.0f);
		skillCheck.SetSkillCheck(0.1f, 0.03f);
	}


	// QTE 실패
	void QTE_Fail()
	{
		StartCoroutine(QTE_Combo());
	}

	// QTE 성공
	void QTE_Success()
	{
		player.SetPlayerTurn(false, GameData.instance.turnDelay);
	}

	// QTE 대성공
	void QTE_Perfect()
	{
		player.SetPlayerTurn(false, GameData.instance.turnDelay);
	}

	IEnumerator QTE_Combo()
	{
		for (int i = 1; i <= 4; i++)
		{
			yield return new WaitForSeconds(0.35f);
			anim.SetTrigger("Attack_" + i);
			yield return new WaitForSeconds(GameData.instance.turnDelay + attackDelay);
			player.TakeDamage(comboDamage_Plus[i-1], this);
		}

		yield return new WaitForSeconds(1.0f);
		player.SetPlayerTurn(false, GameData.instance.turnDelay);
	}
}
