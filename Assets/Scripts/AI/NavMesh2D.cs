using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavMesh2D : MonoBehaviour
{
	[Header("이동속도")]
	public float moveSpeed = 10.0f;

	// 참조만 가능한 현재 이동중인 방향
	public Vector3 velocity { get; private set; }

	// * 시작, 타겟, 현재 타일
	Tile startTile, targetTile, currentTile;

	// * 열린타일, 닫힌타일, 찾은 경로
	List<Tile> openList;
	List<Tile> closedList;
	List<Tile> findTileList;

	[HideInInspector] public NavMesh2DVolume navVolume;

	
	void Awake()
	{
		navVolume = FindObjectOfType<NavMesh2DVolume>();
		InitSetting();
	}

	void OnDisable()
	{
		navVolume.GetTileAtPosition(transform.position).isWall = false;
	}

	// * 시작시 값 초기화를 위한 함수
	void InitSetting()
	{
		openList = new List<Tile>();
		closedList = new List<Tile>();
		findTileList = new List<Tile>();
	}

	void OpenListAdd(int checkIndexX, int checkIndexY)
	{
		/*
		 * 조건1: 이웃 타일이 범위 안에 들경우
		 * 조건2: 이웃 타일이 벽이 아닌경우
		 * 조건3: 이미 탐색하지 않은 경우(닫힌 타일리스트에 없는경우)
		 */
		int maxX = Mathf.Abs(navVolume.leftBottomPosition.x - navVolume.rightTopPosition.x);
		int maxY = Mathf.Abs(navVolume.leftBottomPosition.y - navVolume.rightTopPosition.y);
		if (checkIndexX >= 0 && checkIndexX <= maxX &&
			checkIndexY >= 0 && checkIndexY <= maxY &&
			navVolume.tileArray[checkIndexX, checkIndexY] != null &&
			navVolume.tileArray[checkIndexX, checkIndexY].isWall == false &&
			closedList.Contains(navVolume.tileArray[checkIndexX, checkIndexY]) == false)
		{
			Tile NeighborTile = navVolume.tileArray[checkIndexX, checkIndexY];
			int MoveCost = currentTile.G + 1; // 현재 위치의 타일 이동비용 계산

			// 이동비용이 이웃타일G보다 작거나 열린 리스트에 타일이 없으면 G, H, ParentTile 설정
			if(MoveCost < NeighborTile.G || openList.Contains(NeighborTile) == false)
			{
				NeighborTile.G = MoveCost;
				NeighborTile.H = (Mathf.Abs(NeighborTile.x - targetTile.x)) + (Mathf.Abs(NeighborTile.y - targetTile.y));
				NeighborTile.preTile = currentTile;

				openList.Add(NeighborTile);
			}
		}
	}

	// * 이동을 위한 함수 (NavMesh3D의 MoveTo)
	public void MoveTo(Vector2Int targetPos, int moveCount)
	{
		// 경로까지 최적의 길 선택
		PathFinding(targetPos);
		if (findTileList.Count <= 0) return;

		// 실제 캐릭터 이동 코루틴 실행
		StopAllCoroutines();
		StartCoroutine(MoveCoroutine(moveCount));
	}

	IEnumerator MoveCoroutine(int moveCount)
	{
		int currentMoveCnt = 0;

		// * 이동 전에 현재 위치를 Wall에서 해제
		int cIndexX = (int)transform.position.x - navVolume.leftBottomPosition.x;
		int cIndexY = (int)transform.position.y - navVolume.leftBottomPosition.y;
		navVolume.tileArray[cIndexX, cIndexY].isWall = false;

		// * 도착 위치를 미리 Wall로 지정 (캐릭터 겹침을 막기 위해)
		int tx = (moveCount >= findTileList.Count) ? findTileList[findTileList.Count - 1].x : findTileList[moveCount].x;
		int ty = (moveCount >= findTileList.Count) ? findTileList[findTileList.Count - 1].y : findTileList[moveCount].y;

		int tIndexX = tx - navVolume.leftBottomPosition.x;
		int tIndexY = ty - navVolume.leftBottomPosition.y;
		navVolume.tileArray[tIndexX, tIndexY].isWall = true;

		for (int i = 0; i < findTileList.Count; i++)
		{
			Vector3 tilePosition = findTileList[i].GetTilePosition();
			while (true)
			{
				// 거리 차이가 0.05 이하면 while문을 빠져나감
				float distance = Vector2.Distance(transform.position, tilePosition);
				if (distance <= 0.05f) break;

				// 이동해야 할 방향 계산, 이동
				velocity = tilePosition - transform.position;
				velocity = velocity.normalized;
				velocity = new Vector3(Mathf.Round(velocity.x), Mathf.Round(velocity.y), Mathf.Round(velocity.z));

				transform.position += velocity * Time.deltaTime * moveSpeed;
				yield return null;
			}
			transform.position = tilePosition;

			// moveCount 칸만큼 움직일수 있음
			currentMoveCnt++;
			if (currentMoveCnt > moveCount) break;
		}
		velocity = Vector3.zero;
	}

	// * 길찾기 함수 (호출시 시작좌표, 끝좌표 기준으로 찾음)
	void PathFinding(Vector2Int targetPos)
	{
		openList.Clear();
		closedList.Clear();
		findTileList.Clear();

		// * 배열 범위 체크
		// 1. 목표 위치를 배열의 상대적인 좌표로 변환
		int targetIndexX = targetPos.x - navVolume.leftBottomPosition.x;
		int targetIndexY = targetPos.y - navVolume.leftBottomPosition.y;

		// 2. 배열의 x, y 인덱스 크기를 구함 (0 ~ )
		int sizeX = navVolume.rightTopPosition.x - navVolume.leftBottomPosition.x;
		int sizeY = navVolume.rightTopPosition.y - navVolume.leftBottomPosition.y;

		// 3. 변환한 배열의 상대적인 좌표(인덱스)가 저장된 배열의 x, y 인덱스에 속해있는지 판단
		if (targetIndexX < 0 || targetIndexX > sizeX || targetIndexY < 0 || targetIndexY > sizeY) return;


		// * 시작 위치와 시작타일, 목표타일 저장
		Vector2Int startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

		startTile = navVolume.tileArray[startPos.x - navVolume.leftBottomPosition.x, startPos.y - navVolume.leftBottomPosition.y];
		targetTile = navVolume.tileArray[targetPos.x - navVolume.leftBottomPosition.x, targetPos.y - navVolume.leftBottomPosition.y];

		if (startTile == null || targetTile == null) return;

		openList.Add(startTile);
		// * 길찾기 로직
		while (openList.Count > 0)
		{
			currentTile = openList[0]; // 현재 타일을 열려있는 타일중 0번째로 임시 설정

			// F가 가장 적은 타일을 찾음
			for (int i = 1; i < openList.Count; i++)
			{
				/* 
				 * 조건1: 열린타일의 F가 현재 타일의 F보다 작으면
				 * 조건2: 만약 F가 같으면 H가 더 작은쪽 선택
				 */
				if ((openList[i].F < currentTile.F) ||
					(openList[i].F == currentTile.F && openList[i].H < currentTile.H))
				{
					currentTile = openList[i];
				}
			}

			// 타일을 찾은뒤 OpenList에서 삭제, CloseList에 추가
			openList.Remove(currentTile);
			closedList.Add(currentTile);

			// 도착했으면 이동 타일들을 저장하고 종료
			if (currentTile == targetTile)
			{
				Tile FindEndTile = targetTile;
				while (FindEndTile != startTile)
				{
					findTileList.Add(FindEndTile);
					FindEndTile = FindEndTile.preTile;
				}
				findTileList.Add(startTile);
				findTileList.Reverse();

				// 이곳에 이동로직 작성

				return;
			}

			// 이웃타일 검색 (오른쪽, 위, 왼쪽, 아래 순서)
			int relativePosX = currentTile.GetTilePositionInt().x - navVolume.leftBottomPosition.x;
			int relativePosY = currentTile.GetTilePositionInt().y - navVolume.leftBottomPosition.y;

			OpenListAdd(relativePosX + 1, relativePosY);
			OpenListAdd(relativePosX - 1, relativePosY);
			OpenListAdd(relativePosX, relativePosY + 1);
			OpenListAdd(relativePosX, relativePosY - 1);
		}
	}

}
