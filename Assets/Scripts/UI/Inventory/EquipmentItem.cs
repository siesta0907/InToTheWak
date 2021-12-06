using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipmentPart
{
	Weapon,
	Helmet,
	ChestPlate,
	Boots,
	Jewely,
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Create Equipment Item")]
public class EquipmentItem : Item
{
	[Header("Setting - Equipment")]
	[Space(20)]
	public EquipmentPart itemPart;  // 아이템 부위
	public float def;               // 추가 방어력
	public float atk;               // 추가 공격력

	[Header("Setting - Weapon")]
	public Animator weaponAnim;		// 무기 장착시 변경되는 애니메이터
}
