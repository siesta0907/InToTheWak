using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
	Equipment,
	Food,
	Etc,
}

public enum EquipmentPart
{
	Weapon,
	Helmet,
	ChestPlate,
	Boots,
	Jewely,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Create Item")]
public class Item : ScriptableObject
{
	public ItemType itemType;   // 아이템 타입
	public string itemName;     // 아이템 이름
	[TextArea]
	public string itemDesc;     // 아이템 설명
	public Sprite itemImage;    // 아이템 이미지
	public int maxCount = 10;   // 최대 보유개수

	// Equipment 타입 세팅
	[Header("[ Setting - Equipment ]")]
	public EquipmentPart itemPart;  // 아이템 부위
	public float def;               // 추가 방어력
	public float atk;               // 추가 공격력

	[Header("* if ItemPart is Weapon")]
	public Animator weaponAnim;     // 무기 장착시 변경되는 애니메이터


	// Food 타입 세팅
	[Header("[ Setting - Food ]")]
	public float hp;        // 증가 체력
	public float mp;

}
