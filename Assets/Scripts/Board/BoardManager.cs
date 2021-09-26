using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
  //  public GameObject floorTile;
 //  public GameObject corridorTile;
    //private GameObject[,] boardPositionsFloor;

    public int boardRows, boardColumns;
    public int minRoomSize, maxRoomSize;

    public Tilemap GroundTilemap;
    private Tile[,] boardPositionsFloor;
    /* public Tile[] Ground_center, Ground_LeftTop, Ground_Left, Ground_LeftBottom, 
        Ground_Top, Ground_RightTop, Ground_Right, Ground_RightBottom, Ground_Bottom,
        Ground_Corrider; */
    public TileBase[] Ground_center, Ground_LeftTop, Ground_Left, Ground_LeftBottom,
        Ground_Top, Ground_RightTop, Ground_Right, Ground_RightBottom, Ground_Bottom,
        Ground_Corrider;



    public class SubDungeon
    {
        public List<Rect> corridors = new List<Rect>();
        public SubDungeon left, right; //왼쪽 자식 트리, 오른쪽 자식 트리
        public Rect rect; //공간
        public Rect room = new Rect(-1, -1, 0, 0); //룸, Rect (처음 x ,y 위치, 너비, 높이) 
        public int debugId; 

        private static int debugCounter = 0;
        

        public SubDungeon(Rect mrect)
        {
            rect = mrect; 
            debugId = debugCounter; 
            debugCounter++; 
        }

        
        public Rect GetRoom() //룸을 반환하는 함수
        {
            //만약 리프라면, 현재 룸 반환, 
            //왼쪽 자식이 있다면 왼쪽 자식의 GetRoom()을 호출하고 결과값 반환
            //오른쪽 자식이 있다면 오른쪽 자식의 GetRoom()을 호출하고 결과값 반환
            if (IAmLeaf())
            {
                return room;
            }
            if (left != null)
            {
                Rect lroom = left.GetRoom();
                if (lroom.x != -1)
                {
                    return lroom;
                }
            }
            if (right != null)
            {
                Rect rroom = right.GetRoom();
                if (rroom.x != -1)
                {
                    return rroom;
                }
            }

           
            return new Rect(-1, -1, 0, 0);
        } 

    
        public void CreateRoom() //룸을 만드는 함수
        {
            //왼쪽 자식이 있다면, CreateRoom() 호출
            if (left != null)
            {
                left.CreateRoom();
            }
            // 오른쪽 자식이 있다면, CreateRoom() 호출
            if (right != null)
            {
                right.CreateRoom();
            }
            if (IAmLeaf())
            {
                //룸 사이즈를 랜덤하게 결정 
                //너비/2~너비-1
                //높이/2 ~ 높이-1 
                int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
                int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);

                //룸 위치 랜덤하게 결정
                int roomX = (int)Random.Range(1, rect.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, rect.height - roomHeight - 1);

                room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
                Debug.Log("Created room " + room + " in sub-dungeon " + debugId + " " + rect);
            }
        }

        public bool IAmLeaf() //왼쪽 자식과 오른쪽 자식 둘다 없으면 리프를 true로 리턴
        {
            return left == null && right == null;
        }

        public bool Split(int minRoomSize, int maxRoomSize)  //가장작은 사이즈와 가장 큰 사이즈를 입력받는다. 
        {
            //리프가 아니면 false를 리턴한다.
            if (!IAmLeaf())
            {
                return false;
            }

            bool splitH; //horizontal 기준으로 방을 나눌것인가? true일 경우에는 horizontal로, false일 경우에는 vertical로 나눈다.
            //너비가 높이에 비해 1.25배 이상 길 때 vertical로 나눈다.
            if (rect.width / rect.height >= 1.25)
            {
                splitH = false;
            }

            //높이가 너비에 비해 1.25배 이상 길 때, horizontal로 나눈다.
            else if (rect.height / rect.width >= 1.25)
            {
                splitH = true;
            }
            // 둘다 충분히 길지 않을 경우, 랜덤하게 결정
            else
            {
                splitH = Random.Range(0.0f, 1.0f) > 0.5;
            }


            //Math.Min은 두 개이상 값중에 가장 작은 값 반환
            //가로와 세로 둘 중 작은 쪽을 2로 나눈 값이 최소 방 사이즈보다 작을 때
            if (Mathf.Min(rect.height, rect.width) / 2 < minRoomSize)
            {
                Debug.Log("Sub-dungeon " + debugId + " will be a leaf");
                return false;
            }

            if (splitH) //horizontal 기준으로 방을 나눌때
            {
              
                int split = Random.Range(minRoomSize, (int)(rect.width - minRoomSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, rect.width, split));
                right = new SubDungeon(new Rect(rect.x, rect.y + split, rect.width, rect.height - split));
            }
            else //vertical 기준으로 방을 나눌 때
            {
                int split = Random.Range(minRoomSize, (int)(rect.height - minRoomSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, split, rect.height));
                right = new SubDungeon(
                  new Rect(rect.x + split, rect.y, rect.width - split, rect.height));
            }

            return true;
        }
        
        public void CreateCorridorBetween(SubDungeon left, SubDungeon right)//복도만드는 함수, 서브던전의 왼쪽 자식과 오른쪽 자식을 입력받는다.
        {
            Rect lroom = left.GetRoom();
            Rect rroom = right.GetRoom();

            // lroom에서 랜덤하게 포인트를 잡음 //rroom에서 랜덤하게 포인트를 잡음
            Vector2 lpoint = new Vector2((int)Random.Range(lroom.x + 1, lroom.xMax - 1), (int)Random.Range(lroom.y + 1, lroom.yMax - 1));
            Vector2 rpoint = new Vector2((int)Random.Range(rroom.x + 1, rroom.xMax - 1), (int)Random.Range(rroom.y + 1, rroom.yMax - 1));

            // lpoint가 항상왼쪽에 있도록 
            if (lpoint.x > rpoint.x)
            {
                Vector2 temp = lpoint;
                lpoint = rpoint;
                rpoint = temp;
            }
            //통로의 너비와 높이
            int w = (int)(lpoint.x - rpoint.x);
            int h = (int)(lpoint.y - rpoint.y);

            Debug.Log("lpoint: " + lpoint + ", rpoint: " + rpoint + ", w: " + w + ", h: " + h);

   
            if (w != 0) // 통로가 위아래로 일자가 아닌 경우, 
            {

                //가로부터 그리고 세로 그리기
                if (Random.Range(0, 1) > 2)
                {
                    corridors.Add(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w), 1));
              
                    corridors.Add(new Rect(lpoint.x, lpoint.y, 1, 1));
                    

                    if (h < 0)
                    {
                         corridors.Add(new Rect(rpoint.x, lpoint.y, 1, Mathf.Abs(h)));

                    }
                    else
                    {

                        corridors.Add(new Rect(rpoint.x, lpoint.y, 1, -Mathf.Abs(h)));

                    }
                }
                //세로부터 그리고 가로 그리기
                else
                {

                    if (h < 0)
                    {
                        corridors.Add(new Rect(lpoint.x, lpoint.y, 1, Mathf.Abs(h)));

                    }
                    else
                    {
                        corridors.Add(new Rect(lpoint.x, rpoint.y, 1, Mathf.Abs(h)));
                    }


                    corridors.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w), 1));
                    corridors.Add(new Rect(lpoint.x, rpoint.y, 1, 1));
                }
            }
            
            else //통로가 위아래로 일자일 경우
            {

                if (h < 0) //rpoint가 아래에 있을 경우
                {
                    corridors.Add(new Rect((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs(h)));

                }
                else //lpoint가 아래에 있을 경우
                {
                    corridors.Add(new Rect((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs(h)));

                }
            }
        }

    }





    void DrawCorridors(SubDungeon subDungeon) //통로를 그리는 함수
    {
        if (subDungeon == null)
        {
            return;
        }

        DrawCorridors(subDungeon.left);
        DrawCorridors(subDungeon.right);


        foreach (Rect corridor in subDungeon.corridors)
        {
            for (int i = (int)corridor.x; i < corridor.xMax; i++)
            {
                for (int j = (int)corridor.y; j < corridor.yMax; j++)
                {
                    if (boardPositionsFloor[i, j] == null)
                    {

                        GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_center[Random.Range(0, Ground_center.Length - 1)]);
                    }
                }
            }
        }
    }


    public void DrawRooms(SubDungeon subDungeon) //룸을 그리는 함수
    {
        if (subDungeon == null)
        {
            return;
        }
        //입력받은 서브던전이 리프일때, 
        if (subDungeon.IAmLeaf())
        {
            for (int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++)
            {
                for (int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++)
                {
                   //왼쪽
                    if(i == (int)subDungeon.room.x)
                    {
                        //왼쪽 위일때
                        if(j == (int)subDungeon.room.y) GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_LeftBottom[Random.Range(0, Ground_LeftBottom.Length - 1)]);
                        //왼쪽 아래일때
                        else if(j == subDungeon.room.yMax-1) GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_LeftTop[Random.Range(0, Ground_LeftTop.Length - 1)]);
                        
                        //그 외의 경우
                        else
                        {
                            GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_Left[Random.Range(0, Ground_Left.Length - 1)]);
                        }

                    }
                    //오른쪽
                    else if (i == subDungeon.room.xMax-1)
                  {
                        //오른쪽 위일때
                        if (j == (int)subDungeon.room.y) GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_RightBottom[Random.Range(0, Ground_RightBottom.Length - 1)]);
                        //오른쪽 아래일때
                        else if (j == subDungeon.room.yMax - 1) GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_RightTop[Random.Range(0, Ground_RightTop.Length - 1)]);
                        //그 외의 경우
                        else
                        {
                            GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_Right[Random.Range(0, Ground_Right.Length - 1)]);
                        }
                    }
                    //중간
                    else
                    {
                        //가운데 위일때
                        if (j == (int)subDungeon.room.y)  GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_Bottom[Random.Range(0, Ground_Bottom.Length - 1)]);
                        //가운데 아래일때
                        else if (j == subDungeon.room.yMax - 1) GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_Top[Random.Range(0, Ground_Top.Length - 1)]);
                        //그 외의 경우
                        else
                        {
                            GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_center[Random.Range(0, Ground_center.Length - 1)]);
                        }
                    }


                 

                }
            }
        }
        else
        {
            DrawRooms(subDungeon.left);
            DrawRooms(subDungeon.right);
        }
    }


    public void CreateBSP(SubDungeon subDungeon)
    {
        Debug.Log("Splitting sub-dungeon " + subDungeon.debugId + ": " + subDungeon.rect);
        if (subDungeon.IAmLeaf())
        {
            // 서브던전이 너무 클 경우
            if (subDungeon.rect.width > maxRoomSize
              || subDungeon.rect.height > maxRoomSize
              || Random.Range(0.0f, 1.0f) > 0.25)
            {

                if (subDungeon.Split(minRoomSize, maxRoomSize))
                {
                    Debug.Log("Splitted sub-dungeon " + subDungeon.debugId + " in "
                      + subDungeon.left.debugId + ": " + subDungeon.left.rect + ", "
                      + subDungeon.right.debugId + ": " + subDungeon.right.rect);

                    CreateBSP(subDungeon.left);
                    CreateBSP(subDungeon.right);
                }
            }
        }
    }

    public void CreateColl(SubDungeon sub) //통로만드는 함수
    {
        if (sub.IAmLeaf() == true) return;
        sub.CreateCorridorBetween(sub.left, sub.right);
        CreateColl(sub.left);
        CreateColl(sub.right);
    }


    void Start()
    {
        SubDungeon rootSubDungeon = new SubDungeon(new Rect(0, 0, boardRows, boardColumns));
        CreateBSP(rootSubDungeon);
        rootSubDungeon.CreateRoom();

        boardPositionsFloor = new Tile [boardRows, boardColumns];

        CreateColl(rootSubDungeon);

        DrawCorridors(rootSubDungeon);
        DrawRooms(rootSubDungeon);
    }
}


