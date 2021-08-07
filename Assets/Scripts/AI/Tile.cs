using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile
{
	public bool isWall;
	public Tile preTile; // 이전 타일(지나온)

	/* 
	 * G: 이동거리
	 * H: 목표까지 거리 (장애물 무시)
	 * F: G + H
	 */
	public int x, y, G, H;
	public int F { get { return G + H; } }

	public Tile(int x, int y, bool isWall)
	{
		this.x = x;
		this.y = y;
		this.isWall = isWall;
	}

	// 정수로 변환하여 포지션 반환
	public Vector3 GetTilePosition()
	{
		Vector3 tilePos = new Vector3(x, y, 0);
		return tilePos;
	}

	// 정수로 변환하여 포지션 반환
	public Vector2Int GetTilePositionInt()
	{
		Vector2Int tilePos = new Vector2Int(x, y);
		return tilePos;
	}
}
