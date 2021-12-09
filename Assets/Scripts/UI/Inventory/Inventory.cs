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
			AddInventory();

		if (Input.GetKeyDown(KeyCode.E))
			OpenInventory();

		if (Input.GetKeyDown(KeyCode.R))
			CloseInventory();
	}

	public void AddInventory()
	{
		slots[0].SetItem(testItems[Random.Range(0, testItems.Length)], 1);
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
}
