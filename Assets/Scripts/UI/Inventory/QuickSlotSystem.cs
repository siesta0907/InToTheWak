using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotSystem : MonoBehaviour
{
	[SerializeField] private Slot[] foodSlots;

	Player player;

	void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	void Update()
	{
		if (player.playerTurn)
		{
			PressQuickSlot();
		}
	}

	private void PressQuickSlot()
	{
		int pressBtnIdx = -1;

		if (Input.GetKeyDown(KeyCode.Alpha1))
			pressBtnIdx = 0;
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			pressBtnIdx = 1;
		else if (Input.GetKeyDown(KeyCode.Alpha3))
			pressBtnIdx = 2;
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			pressBtnIdx = 3;
		else if (Input.GetKeyDown(KeyCode.Alpha5))
			pressBtnIdx = 4;
		else if (Input.GetKeyDown(KeyCode.Alpha6))
			pressBtnIdx = 5;

		// 아이템이 있는 경우에만 사용 가능
		if (pressBtnIdx >= 0 && foodSlots[pressBtnIdx].item)
		{
			UseFood(foodSlots[pressBtnIdx].item);
			foodSlots[pressBtnIdx].AddItemCount(-1);
			player.PlayerTurnEnd();
		}
	}

	private void UseFood(Item item)
	{
		player.AddHealth(item.health);
		player.AddSatiety(item.satiety);
		player.AddMana(item.mana);
	}
}
