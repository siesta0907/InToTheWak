using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 보스전 등에서 애니메이션 이벤트로 라이트를 활성화, 비활성화하는 연출입니다.
 * EX) 풍신 라이트
 */
public class LightAnimEvent : MonoBehaviour
{
	[SerializeField] private GameObject[] lights;

	public void LightActive(int index)
	{
		for (int i = 0; i < lights.Length; i++)
			lights[i].SetActive(false);

		lights[index].SetActive(true);
	}

	public void LightDeActive(int index)
	{
		for (int i = 0; i < lights.Length; i++)
			lights[i].SetActive(false);

		lights[index].SetActive(false);
	}
}
