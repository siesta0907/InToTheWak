using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillCheck : MonoBehaviour
{
	[SerializeField] private GameObject checkCircle;
	[SerializeField] private Image successCircle;
	[SerializeField] private Image perfectCircle;
	[SerializeField] private Image point;
	[SerializeField] private float rotateSpd = 60.0f;

	public event Action OnSuccess;	// 성공시
	public event Action OnPerfect;  // 대성공시
	public event Action OnFailed;   // 실패시


	Vector2 successAngle = Vector2.zero;	// 성공 범위
	Vector2 perfectAngle = Vector2.zero;	// 대성공 범위
	float pointAngle = 0.0f;				// 바늘의 각도
	bool activated = false;					// 활성화 상태인지


    void Update()
    {
		if(activated)
		{
			// 바늘 이동
			pointAngle -= (rotateSpd * Time.deltaTime);
			if (pointAngle <= -360.0f)
			{
				pointAngle = 0.0f;
				OnFailed();
				ClearSkillCheck();
			}
			point.transform.localEulerAngles = new Vector3(0, 0, pointAngle);
			TrySkillCheck();
		}
	}


	public void SetSkillCheck(float successSize, float perfectSize)
	{
		activated = true;

		pointAngle = 0.0f;
		successCircle.fillAmount = successSize;
		perfectCircle.fillAmount = perfectSize;

		// 성공, 대성공 범위 설정
		int newAngleZ = UnityEngine.Random.Range(-270, -60);
		successCircle.transform.localEulerAngles = new Vector3(0, 0, newAngleZ);
		successAngle.y = newAngleZ;
		successAngle.x = newAngleZ - (360.0f * successCircle.fillAmount);

		perfectCircle.transform.localEulerAngles = new Vector3(0, 0, successAngle.x);
		perfectAngle.y = successAngle.x;
		perfectAngle.x = successAngle.x - (360.0f * perfectCircle.fillAmount);

		checkCircle.SetActive(true);
	}

	public void ClearSkillCheck()
	{
		activated = false;
		checkCircle.SetActive(false);
	}


	// 스킬체크 시도
	void TrySkillCheck()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (pointAngle >= perfectAngle.x && pointAngle <= perfectAngle.y)
				OnPerfect();
			else if (pointAngle >= successAngle.x && pointAngle <= successAngle.y)
				OnSuccess();
			else
				OnFailed();

			ClearSkillCheck();
		}
	}
}
