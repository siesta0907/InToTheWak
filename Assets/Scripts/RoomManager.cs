using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class RoomManager : MonoBehaviour
{ //레벨이랑 방생성 알고리즘 스크립트

    public static int roomN = 8; //방개수
    public int[,] RoomPosition = new int[roomN, roomN];
   
    List<string> direction = new List<string>();
    string DIR;
    public Tilemap GroundTilemap;
    public TileBase[] Ground_center, Ground_LeftTop, Ground_Left, Ground_LeftBottom,
        Ground_Top, Ground_RightTop, Ground_Right, Ground_RightBottom, Ground_Bottom;
    public TileBase Door;
    public static int roomsize = 9; //방크기(정사각형 9*9)
    public Room[] DunRooms = new Room[roomN];

    public enum RoomType
    {
        NORMAL, //몬스터 0~2, 단순하게 피할 수 있는 함정 0~1
        TRAP, //약간의 퍼즐적 요소? 피할 수 있는 함정방? 
        MONSTER, //몬스터 3~6, 높은 등급의 몬스터 0~1, 아이템 상자 
        NPC, //특정 npc가 존재하는 방 
        BONUS, //아이템 상자 
        STAIR, //(필수)계단이 있는 방 
    }

   public  class Room
    {
        public RoomType roomType;
        public int x, y;
        

    }


    void Start()
    {
        
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        RandomRooms();
        for (int i =0; i< roomN; i++)
        {
            for (int j =0; j< roomN; j++)
            {
               //Debug.Log("room["+ i+","+j+"] :"+ RoomPosition[i, j]);
                if (RoomPosition[i, j] == 1)
                {

                    DrawRoom(i, j);
                }
            }
        }
        for(int i =0; i<roomN; i++)
        {
            levelManager.makeMonster(DunRooms[i].roomType, DunRooms[i].x, DunRooms[i].y);
        }


    }

    void clearL()
    {
        direction.Clear();
        direction.Add("North");
        direction.Add("South");
        direction.Add("West");
        direction.Add("East");

    }
    public void makeRoom(int i, int _x, int _y)
    {
        //노멀 50퍼센트 몬스터 30퍼센트, npc 10퍼센트, 보너스 10퍼센트
        DunRooms[i] = new Room();
        DunRooms[i].x = _x;
        DunRooms[i].y = _y;
        int Percent = Random.Range(0, 100);
        if(Percent <= 50) DunRooms[i].roomType = RoomType.NORMAL;
        else if (Percent <= 90) DunRooms[i].roomType = RoomType.MONSTER;
        else if (Percent <= 95) DunRooms[i].roomType = RoomType.NPC;
        else DunRooms[i].roomType = RoomType.BONUS;
    }
    public void makeRoom(int i, int _x, int _y, string str)
    {
        DunRooms[i] = new Room();
        DunRooms[i].x = _x;
        DunRooms[i].y = _y;
        if (str == "STAIR") DunRooms[i].roomType = RoomType.STAIR;
    }

    public void RandomRooms()
     {
        (int x, int y) now = (3, 3);
        (int x, int y) pre;

        for (int i=0; i<roomN; i++) //룸크기만큼 반복                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        {
            //룸 설정값 
            if(i == roomN - 1)  makeRoom(i, now.x, now.y, "STAIR");
            else  makeRoom(i, now.x, now.y);

            Debug.Log(i+"번째 방: "+ DunRooms[i].roomType);

            pre = (now.x, now.y);
            RoomPosition[now.x, now.y] = 1;

            clearL();
            if (now.x>0 && now.x<6 && now.y > 0 && now.y < 6) //현재위치가 0보다 크고 6보다 작을때
            {
                if(RoomPosition[now.x-1, now.y] == 1) //West
                {
                    direction.Remove("West");
                }
                if (RoomPosition[now.x + 1, now.y] == 1) //East
                {
                    direction.Remove("East");
                }
                if (RoomPosition[now.x, now.y-1] == 1) //South
                {
                    direction.Remove("South");
                }
                if (RoomPosition[now.x, now.y+1] == 1) //North
                {
                    direction.Remove("North");
                }
                DIR = direction[Random.Range(0, direction.Count)];

                if (DIR == "North") now = (now.x, now.y + 1);
                else if (DIR == "South") now = (now.x, now.y - 1);
                else if (DIR == "West") now = (now.x-1, now.y);
                else if (DIR == "East") now = (now.x+1, now.y);
                else Debug.Log("사방이 다 벽임;");

            }
            else if(now.x == 0 && now.y == 6)
            {
                direction.Remove("West");
                direction.Remove("North");
                if (RoomPosition[now.x + 1, now.y] == 1) //East
                {
                    direction.Remove("East");
                }
                if (RoomPosition[now.x, now.y - 1] == 1) //South
                {
                    direction.Remove("South");
                }
                DIR = direction[Random.Range(0, direction.Count)];

                if (DIR == "North") now = (now.x, now.y + 1);
                else if (DIR == "South") now = (now.x, now.y - 1);
                else if (DIR == "West") now = (now.x - 1, now.y);
                else if (DIR == "East") now = (now.x + 1, now.y);
            }
            else if (now.x == 0 && now.y == 0)
            {
                direction.Remove("West");
                direction.Remove("South");
                if (RoomPosition[now.x + 1, now.y] == 1) //East
                {
                    direction.Remove("East");
                }
                if (RoomPosition[now.x, now.y + 1] == 1) //North
                {
                    direction.Remove("North");
                }
                DIR = direction[Random.Range(0, direction.Count)];

                if (DIR == "North") now = (now.x, now.y + 1);
                else if (DIR == "South") now = (now.x, now.y - 1);
                else if (DIR == "West") now = (now.x - 1, now.y);
                else if (DIR == "East") now = (now.x + 1, now.y);
            }
            else if (now.x == 6 && now.y == 0)
            {
                direction.Remove("East");
                direction.Remove("South");
                if (RoomPosition[now.x - 1, now.y] == 1) //West
                {
                    direction.Remove("West");
                }
                if (RoomPosition[now.x, now.y + 1] == 1) //North
                {
                    direction.Remove("North");
                }
                DIR = direction[Random.Range(0, direction.Count)];

                if (DIR == "North") now = (now.x, now.y + 1);
                else if (DIR == "South") now = (now.x, now.y - 1);
                else if (DIR == "West") now = (now.x - 1, now.y);
                else if (DIR == "East") now = (now.x + 1, now.y);
            }
            else if (now.x == 6 && now.y == 6)
            {
                direction.Remove("East");
                direction.Remove("North");
                if (RoomPosition[now.x - 1, now.y] == 1) //West
                {
                    direction.Remove("West");
                }
                if (RoomPosition[now.x, now.y - 1] == 1) //South
                {
                    direction.Remove("South");
                }
                DIR = direction[Random.Range(0, direction.Count)];

                if (DIR == "North") now = (now.x, now.y + 1);
                else if (DIR == "South") now = (now.x, now.y - 1);
                else if (DIR == "West") now = (now.x - 1, now.y);
                else if (DIR == "East") now = (now.x + 1, now.y);
            }
            else if (now.x == 0)
             {
                 direction.Remove("West");
                if (RoomPosition[now.x + 1, now.y] == 1) //East
                {
                    direction.Remove("East");
                }
                if (RoomPosition[now.x, now.y - 1] == 1) //South
                {
                    direction.Remove("South");
                }
                if (RoomPosition[now.x, now.y + 1] == 1) //North
                {
                    direction.Remove("North");
                }
                DIR = direction[Random.Range(0, direction.Count)];

                if (DIR == "North") now = (now.x, now.y + 1);
                else if (DIR == "South") now = (now.x, now.y - 1);
                else if (DIR == "West") now = (now.x - 1, now.y);
                else if (DIR == "East") now = (now.x + 1, now.y);
            }
            else if (now.x == 6)
            {
                direction.Remove("East");
                if (RoomPosition[now.x - 1, now.y] == 1) //West
                {
                    direction.Remove("West");
                }
                if (RoomPosition[now.x, now.y - 1] == 1) //South
                {
                    direction.Remove("South");
                }
                if (RoomPosition[now.x, now.y + 1] == 1) //North
                {
                    direction.Remove("North");
                }
                DIR = direction[Random.Range(0, direction.Count)];

                if (DIR == "North") now = (now.x, now.y + 1);
                else if (DIR == "South") now = (now.x, now.y - 1);
                else if (DIR == "West") now = (now.x - 1, now.y);
                else if (DIR == "East") now = (now.x + 1, now.y);
                else Debug.Log("사방이 다 벽임;");
            }
            else if (now.y == 0)
            {
                direction.Remove("South");

                if (RoomPosition[now.x - 1, now.y] == 1) //West
                {
                    direction.Remove("West");
                }
                if (RoomPosition[now.x + 1, now.y] == 1) //East
                {
                    direction.Remove("East");
                }
                if (RoomPosition[now.x, now.y + 1] == 1) //North
                {
                    direction.Remove("North");
                }

                DIR = direction[Random.Range(0, direction.Count)];

                if (DIR == "North") now = (now.x, now.y + 1);
                else if (DIR == "South") now = (now.x, now.y - 1);
                else if (DIR == "West") now = (now.x - 1, now.y);
                else if (DIR == "East") now = (now.x + 1, now.y);
                else Debug.Log("사방이 다 벽임;");
            }
            else if (now.y == 6)
            {
                direction.Remove("North");
                if (RoomPosition[now.x - 1, now.y] == 1) //West
                {
                    direction.Remove("West");
                }
                if (RoomPosition[now.x + 1, now.y] == 1) //East
                {
                    direction.Remove("East");
                }
                
                if (RoomPosition[now.x, now.y - 1] == 1) //South
                {
                    direction.Remove("South");
                }
                DIR = direction[Random.Range(0, direction.Count)];

                if (DIR == "North") now = (now.x, now.y + 1);
                else if (DIR == "South") now = (now.x, now.y - 1);
                else if (DIR == "West") now = (now.x - 1, now.y);
                else if (DIR == "East") now = (now.x + 1, now.y);
                else Debug.Log("사방이 다 벽임;");
            }
            else //
            {
                Debug.Log("오류발생 ㅋㅋㄹㅃㅃ");
            }
            if(i != roomN-1) CalDoor(now, pre);

        }
     }

    public void DrawRoom(int x, int y)
    {
        roomsize = 9;
        
        for (int i = x * (roomsize + 1); i < x * (roomsize + 1) + roomsize; i++)
        {
            for (int j = y * (roomsize + 1); j < y * (roomsize + 1) + roomsize; j++)
            {
                int Percent = Random.Range(0, 100);
                if (Percent <= 50) GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_center[0]);
                else GroundTilemap.SetTile(new Vector3Int(i, j, 0), Ground_center[Random.Range(0, Ground_center.Length)]);
            }
        }
    }

    public void DrawDoor(int x, int y)
    {
        GroundTilemap.SetTile(new Vector3Int(x, y, 0), Door);
    }

    public void CalDoor((int x, int y)now, (int x, int y)pre)
    {
        bool isvertical;
        (int x, int y) room = (0,0);
        if (now.x == pre.x) isvertical = true;
        else isvertical = false;

        if (isvertical) //방이 위아래로 있을 경우
        {
            if (now.y > pre.y) //현재 방이 더 위에 있을 경우
            {
                room.y = now.y * (roomsize + 1)-1;
                room.x = pre.x * (roomsize + 1) + (int)(roomsize/2);

            }
            else if (now. y < pre.y) //현재 방이 더 아래에 있을 경우
            {
                room.y = pre.y * (roomsize + 1)-1;
                room.x = now.x * (roomsize + 1) + (int)(roomsize / 2);
            }
        }
        else //방이 양옆으로 있을 경우
        {
            if(now.x > pre.x) //현재 방이 오른쪽에 있을 경우
            {
                room.x = now.x * (roomsize + 1) - 1;
                room.y = now.y * (roomsize + 1) + (int)(roomsize / 2);
            }

            else if (now.x < pre.x) //현재 방이 왼쪽에 있을 경우
            {
                room.x = pre.x * (roomsize + 1) - 1;
                room.y = now.y * (roomsize + 1) + (int)(roomsize / 2);
            }
        }

        DrawDoor(room.x, room.y);
    }
}

