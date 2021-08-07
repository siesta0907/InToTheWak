using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
	[Header("하위 UI")]
	[SerializeField] private Hud hud;

    void Awake()
    {
		DontDestroyOnLoad(this.gameObject);
    }
}
