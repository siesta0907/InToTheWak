using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Create Food Item")]
public class FoodItem : Item
{
	[Header("Setting - Food")]
	[Space(20)]
	public float hp;        // 증가 체력
	public float mp;
}
