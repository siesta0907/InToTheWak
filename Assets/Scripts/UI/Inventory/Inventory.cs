using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	[SerializeField] private Slot[] slots;
	[SerializeField] private GameObject body;

	public Item[] testItems;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
			AddInventory(testItems[Random.Range(0, testItems.Length)], 1);

		if (Input.GetKeyDown(KeyCode.E))
			OpenInventory();

		if (Input.GetKeyDown(KeyCode.R))
			CloseInventory();
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
		body.SetActive(true);
	}

	public void CloseInventory()
	{
		body.SetActive(false);
		DragOperation.instance.SetDragSlot(null);
	}

	private bool IsFull()
	{
		for (int i = 0; i < slots.Length; i++)
			if (slots[i].item == null)
				return false;
		return true;
	}
}
