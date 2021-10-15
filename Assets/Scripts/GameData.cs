using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: 밸런스 조절 후 삭제합니다.
#region DevData
public class EnemyData
{
	public float strength;
	public float health;
	public int moveCount;
	public int attackRange;
	public int detectRange;
	public float attackChance;
}


public class RangedEnemyData : EnemyData
{
	public float projectileChance;
	public float projectileSpd;
}


public class PungsinData : EnemyData
{
	public int windCnt = 3;
	public float windAngle = 45.0f;
	public float windDamage = 3.0f;
	public float windSpeed = 5.0f;

	public int lightningCnt = 8;
	public int lightningRange = 3;
	public float lightningDamage = 5.0f;

	public int pushAmount = 2;
}


public class HerusuckData : EnemyData
{
	public int upgradeCnt = 3;
	public float power = 0f;
	public float[] damage_QTE = new float[3];
}
#endregion



public class GameData : MonoBehaviour
{
	public static GameData instance;

	// < 게임관련 데이터 > - (EX: 클릭 딜레이, 재화 ...)
	public float turnDelay = 0.35f;     // 턴 딜레이, 턴 종료후 0.25초 뒤에 다시 턴 돌아옴
	public int decreaseSatiety = 1;     // 턴 소모시 포만감이 얼마나 줄어들지

	public int turn;	// 현재까지 흐른 턴

	void Awake()
	{
		#region Singleton
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
		}
		#endregion
	}
}
