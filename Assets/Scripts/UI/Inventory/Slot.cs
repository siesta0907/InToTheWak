using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SlotType
{
	Default,
	Equipment,
	Food,
	LookOnly,
}

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public SlotType slotType;	// 슬롯 타입
	public Item item;			// Slot이 담고있는 아이템
	public int itemCount;       // Slot이 가지고 있는 아이템 개수

	[SerializeField] Image img_Item;		// 아이템 아이콘을 담을 Image 컴포넌트
	[SerializeField] Text txt_Count;		// 아이템 카운트를 담을 Text 컴포넌트


	public void SetItem(Item item, int itemCount)
	{
		if (item != null)
		{
			this.item = item;
			this.itemCount = itemCount;
			UpdateSlotImage();
		}
	}

	public bool AddItemCount(int count)
	{
		// 1) 최대 보유수량이 가득찬 경우 실패 반환
		if (itemCount + count > item.maxCount)
			return false;

		itemCount += count;
		if (item.maxCount > 1)
			txt_Count.text = itemCount.ToString();
		else
			txt_Count.text = "";

		if (itemCount <= 0)
			ClearSlot();
		return true;
	}

	public void ClearSlot()
	{
		this.item = null;
		this.itemCount = 0;
		UpdateSlotImage();
	}

	void UpdateSlotImage()
	{
		// 1) 아이템이 있는경우
		if (item)
		{
			// Image 갱신
			img_Item.sprite = item.itemImage;
			img_Item.gameObject.SetActive(true);

			// Item Count 텍스트 갱신
			txt_Count.text = itemCount.ToString();
			txt_Count.gameObject.SetActive(true);
		}
		// 2) 아이템이 없는경우
		else
		{
			// Image 갱신
			img_Item.sprite = null;
			img_Item.gameObject.SetActive(false);

			// Item Count 텍스트 갱신
			txt_Count.text = "";
			txt_Count.gameObject.SetActive(false);
		}
	}


	#region DragLogic
	public void OnPointerDown(PointerEventData eventData)
	{
		
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		
	}
	#endregion
}
