using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : Entity
{
	// < 이벤트 >
	// [HideInInspector] public UnityEvent OnTurnEnd; // 플레이어의 턴이 종료되면 호출됨
	public delegate void TurnEndHandler();
	public event TurnEndHandler OnTurnEnd;

	// < 설정 >
	public GameObject previewTile;  // 마우스를 가져다 댔을 때 보여주는 오브젝트

	// < 필요한 컴포넌트 >
	PlayerInput playerInput;		// 플레이어가 입력한 키값을 받아오기 위해 사용
	TileChecker tileChecker;		// 마우스 위치 타일을 표시하기 위해 사용, 실제 움직임과 관련없음
	TargetChecker targetChecker;    // 마우스로 선택한 대상 (공격에서 사용)
	CameraShake cameraShake;		// 카메라 흔들림 효과를 위해 사용
	NavMesh2D nav;					// 2D 네비게이션
	Hud hud;						// 플레이어의 상태를 표시할 HUD

	// < 그 외 >
	public Vector3 targetPos { get; private set; }  // 이동할 위치를 미리 저장해주는 변수입니다. (Enemy 스크립트에서 사용됨)
	float currentTurnDelay = 0.0f;							// 턴 딜레이 변수입니다. (이 시간이 모두 소모되면 턴이 돌아옵니다.)
	bool playerTurn = true;							// 플레이어 턴 체크 변수입니다.


	void Awake()
    {
		playerInput = GetComponent<PlayerInput>();
		tileChecker = GetComponent<TileChecker>();
		targetChecker = GetComponent<TargetChecker>();
		cameraShake = GetComponent<CameraShake>();
		nav = GetComponent<NavMesh2D>();
		hud = FindObjectOfType<Hud>();

		DontDestroyOnLoad(this);
    }


	void Start()
	{
		OnTurnEnd += PlayerTurnEnd;
		hud.InitOwner(this);
	}


    void Update()
    {
		TurnCheck();	// 일정시간 뒤 돌아오는 턴을 체크
		ShowTile();		// 타일에 마우스를 올렸을 때 효과를 보여줌

		// * Player Action부분(입력을 받아 턴을 소비)
		TryAttack();
		TryMove();
    }


	// 스테이지 이동시 기존 Delegate를 기본값으로 되돌립니다.
	public void ResetDelegate()
	{
		OnTurnEnd = null;
		OnTurnEnd += PlayerTurnEnd;
	}


	// 설정된 딜레이에 따른 턴 체크
	private void TurnCheck()
	{
		currentTurnDelay -= Time.deltaTime;
		if (currentTurnDelay <= 0 && playerTurn == false)
		{
			PlayerTurnStart();
		}
	}


	// 클릭하려는 타일을 보여줌 (벽이 아닌경우에만)
	private void ShowTile()
	{
		if(!tileChecker.TileIsWall() && playerTurn && nav.velocity == Vector3.zero)
		{
			previewTile.SetActive(true);
			previewTile.transform.position = tileChecker.GetTilePosition();
		}
		else
		{
			previewTile.SetActive(false);
		}
	}


	// 대상을 공격함 (턴 소비)
	private void TryAttack()
	{
		if(playerInput.LButtonClick && playerTurn && targetChecker.selectedEntity
			&& targetChecker.GetDistance() <= attackRange)
		{
			// TODO: 이후에 지울 Debug.Log
			Debug.Log(targetChecker.selectedEntity.transform.name + "을 공격함!");

			// 피해를 입히고 플레이어 턴을 끝냅니다.
			targetChecker.selectedEntity.TakeDamage(strength, this);
			OnTurnEnd();
		}
	}


	// 클릭시 이동 (턴 소비)
	private void TryMove()
	{
		if (playerInput.LButtonClick && playerTurn) // 왼쪽 버튼을 클릭한 경우
		{
			// 벽이 아니고, 거리가 움직일수 있는 범위보다 작고, 움직이는 상태가 아니면
			if (!tileChecker.TileIsWall() && tileChecker.GetDistance() <= moveCount && nav.velocity == Vector3.zero)
			{
				// 마우스 좌표를 불러옴
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				// 타일 좌표에 맞게 Int로 변환후 저장
				int x = Mathf.RoundToInt(mousePos.x);
				int y = Mathf.RoundToInt(mousePos.y);

				mousePos = new Vector3(x, y, 0);

				// 움직이려는 위치가 현재 위치와 다르다면
				if(mousePos != transform.position)
				{
					// 이동위치 저장, 이동, 턴종료 알림
					targetPos = new Vector3(x, y, 0);
					nav.MoveTo(new Vector2Int(x, y), moveCount);
					OnTurnEnd();
				}
			}
		}
	}


	// 플레이어의 턴이 돌아왔을 때
	private void PlayerTurnStart()
	{
		playerTurn = true;
	}


	// 턴 종료시 무슨 행동을 할것인지 (배고픔 감소... 등)
	private void PlayerTurnEnd()
	{
		// 턴 증가, 딜레이 리셋, 배고픔 증가
		GameData.instance.turn += 1;
		currentTurnDelay = GameData.instance.turnDelay;
		playerTurn = false;
		AddHunger(GameData.instance.increaseHunger);
	}


	// Override Method
	protected override void OnDeath(Entity attacker)
	{
		base.OnDeath(attacker);
		// TODO: 이후에 지울 Debug.Log
		Debug.Log("플레이어가 죽었습니다!");
	}

	protected override void OnHit(Entity victim)
	{
		base.OnHit(victim);
		cameraShake.Play(Camera.main, 0.16f, 0.1f);
	}
}
