using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
