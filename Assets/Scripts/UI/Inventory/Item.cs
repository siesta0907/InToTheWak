using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
	Equipment,
	Food,
	Etc,
}

public class Item : ScriptableObject
{
	public ItemType itemType;   // 아이템 타입
	public string itemName;     // 아이템 이름
	[TextArea]
	public string itemDesc;     // 아이템 설명
	public Sprite itemImage;    // 아이템 이미지
	public int maxCount = 10;	// 최대 보유개수
}
