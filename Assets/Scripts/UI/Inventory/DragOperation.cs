using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragOperation : MonoBehaviour
{
	public static DragOperation instance;
	[HideInInspector] public Slot dragSlot;
	[SerializeField] Image itemImg;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void SetDragSlot(Slot slot)
	{
		if (slot != null)
		{
			dragSlot = slot;
			itemImg.sprite = dragSlot.item.itemImage;
			itemImg.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}
		else
		{
			dragSlot = null;
			itemImg.sprite = null;
			itemImg.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}
	}
}
