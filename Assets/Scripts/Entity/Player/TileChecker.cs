using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 스테이지의 타일을 체크하는 스크립트입니다.
 * 플레이어와 함께 사용되며 마우스를 타일 위에 가져다 댄 경우 등에서 타일 정보를 얻기위해 쓰입니다.
 */
public class TileChecker : MonoBehaviour
{
	NavMesh2DVolume navVolume; // 저장된 타일의 정보를 가져오기 위해 사용
	Tile selectedTile;
	Vector3 mousePos;

	void Update()
	{
		// 만약 Scene 이동 등으로 navVolume을 잃은경우, 다시 찾아줌
		if(navVolume == null) navVolume = FindObjectOfType<NavMesh2DVolume>();
		SetMousePos();
		SelectTileAtPosition(mousePos);
	}

	// 현재 선택된 타일이 벽인지 반환하는 함수 (벽타일은 이동할 수 없는 공간을 말합니다)
	public bool TileIsWall()
	{
		if (selectedTile == null) return true;
		return selectedTile.isWall;
	}

	// 플레이어 위치와 선택한 타일과 위치계산
	public float GetDistance()
	{
		if (selectedTile == null) return 999999;

		Vector3 pos1 = transform.position;
		Vector3 pos2 = selectedTile.GetTilePosition();

		float dist = Mathf.Round(Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y));
		return dist;
	}

	// 현재 선택된 타일의 좌표를 반환
	public Vector3 GetTilePosition()
	{
		if (selectedTile == null) return Vector3.zero;
		return selectedTile.GetTilePosition();
	}

	// 마우스 좌표를 타일에 맞게 변환(정수)
	private void SetMousePos()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		int x = Mathf.RoundToInt(mousePos.x);
		int y = Mathf.RoundToInt(mousePos.y);

		mousePos.x = x;
		mousePos.y = y;
	}

	// 좌표의 타일을 선택
	private void SelectTileAtPosition(Vector3 pos)
	{
		Tile tile = navVolume.GetTileAtPosition(pos);
		if(tile != null)
		{
			selectedTile = tile;
		}
	}
}
