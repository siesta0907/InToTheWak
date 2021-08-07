using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
	Player player;

	void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	void Start()
	{
		player.OnTurnEnd += Pickup;
	}

	void Pickup()
	{
		StopAllCoroutines();
		StartCoroutine(PickupCoroutine());
	}

	IEnumerator PickupCoroutine()
	{
		yield return new WaitForSeconds(GameData.instance.moveDelay);
		float distance = Vector2.Distance(transform.position, player.targetPos);

		if (distance < 0.1f)
		{
			// 아이템을 주움
			player.OnTurnEnd -= Pickup;
			Destroy(this.gameObject);
		}
	}
}
