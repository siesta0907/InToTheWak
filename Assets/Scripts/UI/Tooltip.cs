using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
	public static Tooltip instance;

	[SerializeField] private GameObject body;
	[SerializeField] private Text title;
	[SerializeField] private Text desc;

	RectTransform tf;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}

		tf = GetComponent<RectTransform>();
	}

	void Update()
	{
		//tf.position = Input.mousePosition;
	}

	public void ShowTooltip(Item item, PointerEventData eventData)
	{
		title.text = item.itemName;
		title.color = item.nameColor;
		desc.text = item.itemDesc;

		tf.position = eventData.position;
		body.SetActive(true);

		if (item.itemType == ItemType.Equipment)
		{
			if (item.itemPart == EquipmentPart.Weapon)
			{
				desc.text += "\n\n";
				desc.text += "<color=yellow>공격력:</color> " + item.minDamage + " ~ " + item.maxDamage + "          \n";
				desc.text += "<color=yellow>공격범위:</color> " + item.attackRange;
			}
		}
	}

	public void HideTooltip()
	{
		title.text = "";
		desc.text = "";
		body.SetActive(false);
	}
}
