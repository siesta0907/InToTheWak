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

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void ShowTooltip(Item item, PointerEventData eventData)
	{
		title.text = item.itemName;
		desc.text = item.itemDesc;

		GetComponent<RectTransform>().position = eventData.position;
		body.SetActive(true);
	}

	public void HideTooltip()
	{
		title.text = "";
		desc.text = "";
		body.SetActive(false);
	}
}
