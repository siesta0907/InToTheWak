using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class EnemyData
{
	public int minDamage;
	public int maxDamage;
	public float health;
	public int moveCount;
	public int attackRange;
	public int detectRange;
	public float attackChance;
}

[System.Serializable]
public class RangedEnemyData : EnemyData
{
	public float projectileChance;
	public float projectileSpd;
}

[System.Serializable]
public class PungsinData : EnemyData
{
	public int windCnt = 3;
	public float windAngle = 45.0f;
	public int windDamage = 3;
	public float windSpeed = 5.0f;

	public int lightningCnt = 8;
	public int lightningRange = 3;
	public int lightningDamage = 5;

	public int pushAmount = 2;
}

[System.Serializable]
public class HerusuckData : EnemyData
{
	public int upgradeCnt = 3;
	public float power = 0f;
	public int[] damage_QTE = new int[3];
}


public class EntityJson : MonoBehaviour
{
	string SAVE_DIRECTORY = "";

	void Start()
    {
		// 폴더가 없으면 폴더를 생성하고 파일들을 생성함
		SAVE_DIRECTORY = Application.dataPath + "/Data/Entity/";

		if (!Directory.Exists(SAVE_DIRECTORY))
		{
			Directory.CreateDirectory(SAVE_DIRECTORY);
			CreateJson();
		}
    }

	void CreateJson()
	{
		string json;

		// JSON - 팬치
		EnemyData data_panch = new EnemyData();
		data_panch.minDamage = 1;
		data_panch.maxDamage = 1;
		data_panch.health = 5.0f;
		data_panch.moveCount = 1;
		data_panch.attackRange = 1;
		data_panch.detectRange = 8;
		data_panch.attackChance = 70;
		json = JsonUtility.ToJson(data_panch, true);
		File.WriteAllText(SAVE_DIRECTORY + "Panch.json", json);

		// JSON - 느그자
		EnemyData data_negeza = new EnemyData();
		data_negeza.minDamage = 1;
		data_negeza.maxDamage = 1;
		data_negeza.health = 10.0f;
		data_negeza.moveCount = 1;
		data_negeza.attackRange = 1;
		data_negeza.detectRange = 8;
		data_negeza.attackChance = 70;
		json = JsonUtility.ToJson(data_negeza, true);
		File.WriteAllText(SAVE_DIRECTORY + "Negeza.json", json);

		// JSON - 왁무새
		RangedEnemyData data_wakbird = new RangedEnemyData();
		data_wakbird.minDamage = 5;
		data_wakbird.maxDamage = 5;
		data_wakbird.health = 4.0f;
		data_wakbird.moveCount = 1;
		data_wakbird.attackRange = 1;
		data_wakbird.detectRange = 8;
		data_wakbird.attackChance = 70;
		data_wakbird.projectileChance = 30;
		data_wakbird.projectileSpd = 5;
		json = JsonUtility.ToJson(data_wakbird, true);
		File.WriteAllText(SAVE_DIRECTORY + "Wakbird.json", json);

		// JSON - 아메바
		EnemyData data_amoeba = new EnemyData();
		data_amoeba.minDamage = 8;
		data_amoeba.maxDamage = 8;
		data_amoeba.health = 1.0f;
		data_amoeba.moveCount = -1;
		data_amoeba.attackRange = 6;
		data_amoeba.detectRange = -1;
		data_amoeba.attackChance = 100;
		json = JsonUtility.ToJson(data_amoeba, true);
		File.WriteAllText(SAVE_DIRECTORY + "Amoeba.json", json);

		// JSON - 풍신
		PungsinData data_pungsin = new PungsinData();
		data_pungsin.minDamage = 10;
		data_pungsin.maxDamage = 10;
		data_pungsin.health = 50.0f;
		data_pungsin.moveCount = 1;
		data_pungsin.attackRange = 1;
		data_pungsin.detectRange = -1;
		data_pungsin.attackChance = 25;
		data_pungsin.windCnt = 3;
		data_pungsin.windAngle = 45.0f;
		data_pungsin.windDamage = 3;
		data_pungsin.windSpeed = 5.0f;
		data_pungsin.lightningCnt = 8;
		data_pungsin.lightningRange = 3;
		data_pungsin.lightningDamage = 5;
		data_pungsin.pushAmount = 2;
		json = JsonUtility.ToJson(data_pungsin, true);
		File.WriteAllText(SAVE_DIRECTORY + "Pungsin.json", json);


		// JSON - 해루석
		HerusuckData data_herusuck = new HerusuckData();
		data_herusuck.minDamage = 10;
		data_herusuck.maxDamage = 10;
		data_herusuck.health = 100.0f;
		data_herusuck.moveCount = 1;
		data_herusuck.attackRange = 1;
		data_herusuck.detectRange = -1;
		data_herusuck.attackChance = 100;
		data_herusuck.upgradeCnt = 3;
		data_herusuck.power = 100.0f;
		data_herusuck.damage_QTE[0] = 20;
		data_herusuck.damage_QTE[1] = 100;
		data_herusuck.damage_QTE[2] = 250;
		json = JsonUtility.ToJson(data_herusuck, true);
		File.WriteAllText(SAVE_DIRECTORY + "Herusuck.json", json);
	}
}
