using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	[SerializeField] private Slot[] slots;
	[SerializeField] private GameObject body;

	[Header("Equipment Slots")]
	[SerializeField] private Slot weaponSlot;
	[SerializeField] private Slot helmetSlot;
	[SerializeField] private Slot chestplateSlot;
	[SerializeField] private Slot bootsSlot;
	[SerializeField] private Slot jewelySlot;

	[Header("Food Slots")]
	[SerializeField] private Slot[] foodSlots;

	public Item[] testItems;

	Player player;
	bool isOpen;

	void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
			AddInventory(testItems[Random.Range(0, testItems.Length)], 1);

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (isOpen)
				CloseInventory();
			else
				OpenInventory();
		}
	}

	public void AddInventory(Item item, int count)
	{
		// 1) 먼저 겹칠 수 있는 아이템이 있으면 겹침
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].item == item && slots[i].itemCount + count <= slots[i].item.maxCount)
			{
				slots[i].AddItemCount(count);
				return;
			}
		}

		// 2) 하나도 겹칠 수 없으면 비어있는 인벤토리에 아이템을 추가
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].item == null)
			{
				slots[i].SetItem(item, count);
				return;
			}
		}
	}

	public void OpenInventory()
	{
		GameData.instance.uiMode = true;
		body.SetActive(true);
		isOpen = true;
	}

	public void CloseInventory()
	{
		GameData.instance.uiMode = false;
		body.SetActive(false);
		DragOperation.instance.SetDragSlot(null);
		isOpen = false;
	}

	public bool IsFull()
	{
		for (int i = 0; i < slots.Length; i++)
			if (slots[i].item == null)
				return false;
		return true;
	}

	// 장비 아이템 착용시
	public void OnEquipmentChange(EquipmentPart part, Item item)
	{
		// 무기 아이템의 경우 애니메이션을 추가로 변경
		if (part == EquipmentPart.Weapon)
		{
			// 장착한 아이템이 있다면 해당 아이템으로 애니메이션 변경
			if (item != null)
			{
				player.SetWeaponAnimation(item.weaponAnim);

				// 스텟 변경
				player.SetMinMaxDamage(item.minDamage, item.maxDamage);
				player.attackRange = item.attackRange;
			}
			// 없다면 기본 애니메이션(주먹)으로 변경
			else
			{
				player.SetWeaponAnimation(null);

				// 스텟 변경
				player.SetMinMaxDamage(1, 1);
				player.attackRange = 1;
			}
		}

	}

	// 음식(소비) 아이템 착용시
	public void OnFoodChange()
	{

	}
}
