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


public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public SlotType slotType;   // 슬롯 타입
	[HideInInspector] public Item item;			// Slot이 담고있는 아이템
	[HideInInspector] public int itemCount;     // Slot이 가지고 있는 아이템 개수

	[SerializeField] private Color highlightColor;  // 마우스를 올렸을때 색깔
	private Color originColor;
	private Image slotImage;

	[SerializeField] private Image img_Item;		// 아이템 아이콘을 담을 Image 컴포넌트
	[SerializeField] private Text txt_Count;        // 아이템 카운트를 담을 Text 컴포넌트

	[Header("[ Equipment Slot Setting ]")]
	public EquipmentPart slotParts;			// 해당 슬롯에 착용 가능한 파츠

	void Awake()
	{
		slotImage = GetComponent<Image>();
		originColor = slotImage.color;
	}

	void OnEnable()
	{
		slotImage.color = originColor;	// 슬롯 색상을 기존으로 되돌림
	}

	public void SetItem(Item item, int itemCount)
	{
		// 아이템이 있는경우
		if (item != null)
		{
			this.item = item;
			this.itemCount = itemCount;
			UpdateSlotImage();
		}
		// 2) 아이템이 없는경우 슬롯을 비움
		else
		{
			ClearSlot();
		}
	}

	public void SwapSlot(Slot slot)
	{
		if (slot != null)
		{
			Item tmpItem = this.item;
			int tmpItemCount = this.itemCount;

			SetItem(slot.item, slot.itemCount);
			slot.SetItem(tmpItem, tmpItemCount);
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
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (item != null && DragOperation.instance.dragSlot == null)
		{
			Tooltip.instance.HideTooltip();
			DragOperation.instance.SetDragSlot(this);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (DragOperation.instance.dragSlot != null)
		{
			DragOperation.instance.transform.position = eventData.position;
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (DragOperation.instance.dragSlot != null)
		{
			Slot dragSlot = DragOperation.instance.dragSlot;

			// 1) 옮기려는 슬롯이 비어있는경우
			if (item == null)
			{
				// 1-A) 인벤토리 슬롯에서 타입만 맞으면 어느슬롯으로 옮겨도 OK
				if (dragSlot.slotType == SlotType.Default)
				{
					if (slotType == SlotType.Food && dragSlot.item.itemType == ItemType.Food)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
					else if (slotType == SlotType.Equipment && dragSlot.item.itemType == ItemType.Equipment
							&& dragSlot.item.itemPart == slotParts)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
					else if (slotType == SlotType.Default)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
				}
				// 1-B) 장비 슬롯에서는 인벤토리로만 이동가능
				else if (dragSlot.slotType == SlotType.Equipment)
				{
					if (slotType == SlotType.Default)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
				}
				// 1-C) 음식 슬롯에서는 음식슬롯간 이동 또는 인벤토리로 이동가능
				else if (dragSlot.slotType == SlotType.Food)
				{
					if (slotType == SlotType.Default)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
					else if (slotType == SlotType.Food)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
				}
			}
			// 2) 옮기려는 슬롯이 비어있지 않은경우 (아이템끼리의 스왑이 발생함)
			else
			{
				// 2-A) 인벤토리끼리는 언제든 스왑이 가능, 다른 슬롯으로 옮기는 경우
				//      옮기려는 슬롯의 아이템타입과 일치하면 옮길수있음
				if (dragSlot.slotType == SlotType.Default)
				{
					if (slotType == SlotType.Default)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
					else if (slotType == SlotType.Food && dragSlot.item.itemType == item.itemType)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
					else if (slotType == SlotType.Equipment && dragSlot.item.itemType == item.itemType
							 && dragSlot.item.itemPart == item.itemPart)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
				}
				// 2-B) 음식슬롯끼리 서로 스왑가능, 그 외 인벤토리에 있는 아이템과 타입이 일치하면 스왑가능
				else if (dragSlot.slotType == SlotType.Food)
				{
					if (slotType == SlotType.Food)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
					else if (slotType == SlotType.Default && dragSlot.item.itemType == item.itemType)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
				}
				// 3-A) 오직 인벤토리와 장비슬롯만 스왑이 가능함, 타입과 파츠가 일치한 경우에만
				else if (dragSlot.slotType == SlotType.Equipment)
				{
					if (slotType == SlotType.Equipment && dragSlot.item.itemType == item.itemType
						&& dragSlot.item.itemPart == item.itemPart)
					{
						SwapSlot(DragOperation.instance.dragSlot);
					}
				}
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (DragOperation.instance.dragSlot != null)
		{
			DragOperation.instance.SetDragSlot(null);
		}
	}
	#endregion

	public void OnPointerEnter(PointerEventData eventData)
	{
		slotImage.color = highlightColor;
		if (item != null && DragOperation.instance.dragSlot == null)
			Tooltip.instance.ShowTooltip(item, eventData);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		slotImage.color = originColor;
		if (item != null)
			Tooltip.instance.HideTooltip();
	}
}
