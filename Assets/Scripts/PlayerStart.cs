using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * 이 스크립트를 가진 오브젝트를 씬에 배치하면, 플레이어가 이곳에서 시작합니다.
public class PlayerStart : MonoBehaviour
{
	Player player;

    void Awake()
    {
		player = FindObjectOfType<Player>();
    }

	void Start()
	{
		if(player)
		{
			player.transform.position = transform.position;
			player.UpdateTargetPos();
		}
	}
}
