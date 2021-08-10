using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	Vector3 originPos;
	Camera cam;
	float duration;
	float scale;

	bool isPlaying = false;

	void Update()
	{
		Shake();
	}

	// 대상 카메라, 지속시간, 흔들림 강도, 흔들림 속도
	public void Play(Camera cam, float duration, float scale)
	{
		if (isPlaying == false)   // 코루틴 중복실행 방지
		{
			isPlaying = true;

			this.originPos = cam.transform.position;
			this.cam = cam;
			this.duration = duration;
			this.scale = scale;
		}
	}

	void Shake()
	{
		if (duration > 0)
		{
			duration -= Time.deltaTime;
			float randX = Random.Range(-scale, scale);
			float randY = Random.Range(-scale, scale);

			cam.transform.position = new Vector3(originPos.x + randX, originPos.y + randY, cam.transform.position.z);

			if (duration <= 0)
			{
				cam.transform.position = originPos;

				originPos = Vector3.zero;
				cam = null;
				duration = 0;
				scale = 0;
				isPlaying = false;
			}
		}
	}
}
