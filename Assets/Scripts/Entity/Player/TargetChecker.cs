using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChecker : MonoBehaviour
{
	[SerializeField] LayerMask targetLayer;  // 선택할 수 있는 레이어입니다. (이 레이어에 포함된 객체들만 선택됨)
	public Entity selectedEntity { get; private set; }
	Vector3 mousePos;

    // Update is called once per frame
    void Update()
    {
		SelectEntityAtMousePos();
	}

	void SelectEntityAtMousePos()
	{
		Vector2 startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.zero, Mathf.Infinity, targetLayer);
		if (hit)
		{
			Entity tmpEntity = hit.transform.GetComponent<Entity>();
			selectedEntity = (!tmpEntity.isDead) ? tmpEntity : null;
		}
		else
		{
			selectedEntity = null;
		}
	}

	// 선택된 객체와의 거리를 반환합니다.
	public float GetDistance()
	{
		if (selectedEntity == null) return 999999;

		Vector3 pos1 = transform.position;
		Vector3 pos2 = selectedEntity.transform.position;

		float dist = Mathf.Round(Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y));
		return dist;
	}
}
