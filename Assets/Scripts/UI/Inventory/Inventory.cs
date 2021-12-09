using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	[SerializeField] Slot[] slots;

	public Item[] testItems;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
			AddInventory();
	}

	public void AddInventory()
	{
		slots[0].SetItem(testItems[Random.Range(0, testItems.Length)], 1);
	}
}
