using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSetup : MonoBehaviour
{
	public bool tilemapIsWall; // 타일맵이 벽인지 (이동불가)

	Tilemap tilemap;
	NavMesh2DVolume navVolume;

	void Awake()
	{
		tilemap = GetComponent<Tilemap>();
		navVolume = FindObjectOfType<NavMesh2DVolume>();
	}

	void Start()
	{
		RegisterTiles();
	}

	public void RegisterTiles()
	{
		for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
		{
			for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
			{
				TileBase tileBase = tilemap.GetTile(new Vector3Int(x, y, 0));
				if(tileBase)
				{
					Tile regTile = new Tile(x, y, tilemapIsWall);
					// 현재 좌표를 기준으로 노드를 NavMeshVolume에 등록함
					navVolume.RegisterTile(regTile, new Vector2Int(x, y));
				}
			}
		}
	}
}
