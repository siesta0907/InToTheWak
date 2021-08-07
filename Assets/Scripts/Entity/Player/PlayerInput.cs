using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public bool LButtonClick { get; private set; } // 좌클릭
	public bool RButtonClick { get; private set; } // 우클릭

	void Update()
    {
		LButtonClick = Input.GetButtonDown("Fire1");
		RButtonClick = Input.GetButtonDown("Fire2");
	}
}
