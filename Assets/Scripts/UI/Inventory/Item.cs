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
	public ItemType itemType;				// 아이템 타입
	public string itemName;					// 아이템 이름
	public Color nameColor = Color.white;	// 아이템 이름 색깔
	[TextArea]
	public string itemDesc;					// 아이템 설명
	public Sprite itemImage;				// 아이템 이미지
	public int maxCount = 10;				// 최대 보유개수

	// Equipment 타입 세팅
	[Header("[ Setting - Equipment ]")]
	public EquipmentPart itemPart;  // 아이템 부위
	public float def;               // 추가 방어력

	[Header("* if ItemPart is Weapon")]
	public RuntimeAnimatorController weaponAnim;    // 무기 장착시 변경되는 애니메이터
	public int minDamage;	// 최소 공격력
	public int maxDamage;   // 최대 공격력
	public int attackRange;	// 공격 범위

	// Food 타입 세팅
	[Header("[ Setting - Food ]")]
	public float health;        // 증가 체력
	public float satiety;		// 증가 포만감
	public float mana;			// 증가 마나

}
