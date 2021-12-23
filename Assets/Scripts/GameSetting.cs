using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
	public GameObject back;

	public Dropdown dd_resolution;
	public Toggle tg_fullscreen;

	public Slider sl_framerate;
	public Text txt_framerate;

	public Slider sl_bgm;
	public Text txt_bgm;

	public Slider sl_sfx;
	public Text txt_sfx;

	Resolution[] allResolutions;

	bool isOpen = false;

	void Start()
	{
		UpdateUI();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			if (isOpen)
				CloseSetting();
			else
				OpenSetting();
		}
	}

	void UpdateUI()
	{
		// 해상도 세팅
		allResolutions = Screen.resolutions;
		int curResIdx = 0;
		List<string> options = new List<string>();

		for (int i = 0; i < allResolutions.Length; i++)
		{
			Resolution res = allResolutions[i];
			Vector2Int resolution = new Vector2Int(res.width, res.height);
			int refreshRate = res.refreshRate;
			options.Add(resolution.x + " × " + resolution.y + " (" + refreshRate + "hz)");

			if (Screen.currentResolution.width == resolution.x &&
				Screen.currentResolution.height == resolution.y)
				curResIdx = i;
		}
		dd_resolution.ClearOptions();
		dd_resolution.AddOptions(options);
		dd_resolution.value = curResIdx;

		// FullScreen 세팅
		tg_fullscreen.isOn = Screen.fullScreen;

		// Framerate 세팅
		sl_framerate.value = Application.targetFrameRate;
		txt_framerate.text = Application.targetFrameRate.ToString();

		// BGM, 효과음 세팅
		sl_bgm.value = GameData.instance.curSettingData.bgmVolume;
		txt_bgm.text = GameData.instance.curSettingData.bgmVolume.ToString();

		sl_sfx.value = GameData.instance.curSettingData.sfxVolume;
		txt_sfx.text = GameData.instance.curSettingData.sfxVolume.ToString();
	}

	public void OpenSetting()
	{
		GameData.instance.uiMode = true;
		UpdateUI();
		back.SetActive(true);
		isOpen = true;
	}

	public void CloseSetting()
	{
		GameData.instance.uiMode = false;
		back.SetActive(false);
		isOpen = false;
	}

	public void UpdateText()
	{
		txt_framerate.text = sl_framerate.value.ToString();
		txt_bgm.text = sl_bgm.value.ToString();
		txt_sfx.text = sl_sfx.value.ToString();
	}

	public void ApplySetting()
	{
		SettingData data = new SettingData();
		data.resolution = new Vector2Int(allResolutions[dd_resolution.value].width, allResolutions[dd_resolution.value].height);
		data.refreshRate = allResolutions[dd_resolution.value].refreshRate;
		data.framerate = (int)sl_framerate.value;
		data.bgmVolume = (int)sl_bgm.value;
		data.sfxVolume = (int)sl_sfx.value;
		data.fullScreen = tg_fullscreen.isOn;

		GameData.instance.ApplySettingData(data);
		CloseSetting();
	}
}
