using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
	[SerializeField] private GameObject timerBar;
	[SerializeField] private Image timerBar_Full;
	public event Action OnTimerEnd;

	float timeRemain;	// 남은 타이머 시간
	float time;			// 설정된 타이머 시간

	void Update()
	{
		UpdateTimer();
	}

	void UpdateTimer()
	{
		if (timeRemain > 0)
		{
			timeRemain -= Time.deltaTime;

			// 타이머 종료
			if(timeRemain <= 0)
			{
				timeRemain = 0;
				timerBar.SetActive(false);
				if (OnTimerEnd != null) OnTimerEnd();
			}

			timerBar_Full.fillAmount = timeRemain / time;
		}

	}

	public void SetTimer(float time)
	{
		// 타이머 설정
		this.time = time;
		timeRemain = this.time;
		timerBar.SetActive(true);
	}

	public void ClearTimer()
	{
		// 타이머 초기화
		time = 0;
		timeRemain = 0;
		timerBar.SetActive(false);
	}
}
