using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMesh2DVolume : MonoBehaviour
{
	// * Grid의 탐색할 시작점, 끝점
	[Header("맵의 크기")]
	public Vector2Int leftBottomPosition;
	public Vector2Int rightTopPosition;

	// * 모든 타일의 정보(등록된 타일)
	public Tile[,] tileArray;

	int sizeX, sizeY;

	// Start is called before the first frame update
	void Awake()
    {
		InitSetting();
	}

	private void InitSetting()
	{
		// * 탐색 사이즈 지정
		sizeX = rightTopPosition.x - leftBottomPosition.x + 1;
		sizeY = rightTopPosition.y - leftBottomPosition.y + 1;

		// * TileArray 선언과 초기화
		tileArray = new Tile[sizeX, sizeY];

		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				tileArray[i, j] = null;
			}
		}

		// * 씬 이동 등에서 누락된경우 다시 등록
		NavMesh2D[] navs = FindObjectsOfType<NavMesh2D>();
		for(int i = 0; i < navs.Length; i++)
		{
			navs[i].navVolume = this;
		}
	}

	// * 타일을 등록하는 함수
	public void RegisterTile(Tile tile, Vector2Int tilePos)
	{
		// 현재 타일위치를 배열의 상대좌표로 변환하여 저장
		/*
		 * Ex: 시작점(왼쪽 아래)이 (-2, -2)이고, 끝점(오른쪽 위)이 (10, 10)이며, 타일의 위치가 (-1, -2)이면
		 * 배열의 [1, 0] 인덱스에 타일이 저장됨
		 */
		int indexX = tilePos.x - leftBottomPosition.x;
		int indexY = tilePos.y - leftBottomPosition.y;
		tileArray[indexX, indexY] = tile;
	}

	public Tile GetTileAtPosition(Vector3 pos)
	{
		int indexX = (int)pos.x - leftBottomPosition.x;
		int indexY = (int)pos.y - leftBottomPosition.y;

		int maxX = rightTopPosition.x - leftBottomPosition.x;
		int maxY = rightTopPosition.y - leftBottomPosition.y;

		if (indexX >= 0 && indexX <= maxX &&
			indexY >= 0 && indexY <= maxY)
		{
			return tileArray[indexX, indexY];
		}
		return null;
	}

	public void SetWallAtPosition(Vector3 pos, bool wall)
	{
		int indexX = (int)pos.x - leftBottomPosition.x;
		int indexY = (int)pos.y - leftBottomPosition.y;

		int maxX = rightTopPosition.x - leftBottomPosition.x;
		int maxY = rightTopPosition.y - leftBottomPosition.y;

		if (indexX >= 0 && indexX <= maxX &&
			indexY >= 0 && indexY <= maxY)
		{
			tileArray[indexX, indexY].isWall = wall;
		}
	}
}
