using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


[System.Serializable]
public class SettingData
{
	public Vector2Int resolution;
	public int refreshRate;
	public bool fullScreen;
	public int framerate;
	public int bgmVolume;
	public int sfxVolume;
}

public class GameData : MonoBehaviour
{
	public static GameData instance;

	// < 게임관련 데이터 > - (EX: 클릭 딜레이, 재화 ...)
	public float turnDelay = 0.35f;     // 턴 딜레이, 턴 종료후 0.25초 뒤에 다시 턴 돌아옴
	public int decreaseSatiety = 1;     // 턴 소모시 포만감이 얼마나 줄어들지
	public bool uiMode = false;			// UI모드인지, UI모드면 이동등의 행동이 불가

	public int turn;    // 현재까지 흐른 턴


	// < 세팅관련 데이터 >
	[HideInInspector] public SettingData curSettingData;
	string SAVE_DIRECTORY;


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
			Destroy(this.gameObject);
		}
		#endregion

		CreateOrLoadGameSetting();
	}

	void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}


	// 설정 저장부분
	public void ApplySettingData(SettingData data)
	{
		curSettingData = data;

		// 해상도 설정
		Screen.SetResolution(data.resolution.x, data.resolution.y, data.fullScreen, data.refreshRate);

		if (data.framerate <= 0)
			Application.targetFrameRate = 300;
		else
			Application.targetFrameRate = data.framerate;

		// 사운드 조절부분
		UpdateAudioSource();
	}

	// 게임이 실행되었을때 파일이 존재하지 않으면 설정파일을 만듬
	// 파일이 존재하면 세팅값을 기반으로 조정해줌
	public void CreateOrLoadGameSetting()
	{
		SAVE_DIRECTORY = Application.dataPath + "/Setting/";

		// 파일이 존재하지 않으면
		if (!Directory.Exists(SAVE_DIRECTORY))
		{
			Directory.CreateDirectory(SAVE_DIRECTORY);

			// Setting Json 생성
			string json;

			SettingData settingData = new SettingData();
			settingData.resolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
			settingData.fullScreen = true;
			settingData.refreshRate = Screen.currentResolution.refreshRate;
			settingData.framerate = Application.targetFrameRate;
			settingData.bgmVolume = 100;
			settingData.sfxVolume = 100;

			json = JsonUtility.ToJson(settingData, true);
			File.WriteAllText(SAVE_DIRECTORY + "GameSetting.json", json);

			curSettingData = settingData;
			ApplySettingData(curSettingData);
		}
		else
		{
			string loadjson = File.ReadAllText(SAVE_DIRECTORY + "GameSetting.json");
			curSettingData = JsonUtility.FromJson<SettingData>(loadjson);
			if (curSettingData != null)
			{
				ApplySettingData(curSettingData);
			}
		}
	}

	public void SaveSettingData(SettingData data)
	{
		// Setting Json 생성
		string json;

		SettingData settingData = new SettingData();
		settingData.resolution = new Vector2Int(data.resolution.x, data.resolution.y);
		settingData.refreshRate = data.refreshRate;
		settingData.framerate = data.framerate;
		settingData.bgmVolume = data.bgmVolume;
		settingData.sfxVolume = data.sfxVolume;

		json = JsonUtility.ToJson(settingData, true);
		File.WriteAllText(SAVE_DIRECTORY + "GameSetting.json", json);
	}

	public void UpdateAudioSource()
	{
		AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
		foreach (AudioSource a in audioSources)
		{
			a.volume = (float)GameData.instance.curSettingData.bgmVolume / 100.0f;
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		UpdateAudioSource();
	}
}
