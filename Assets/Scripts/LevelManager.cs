using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject amoebaPrefab;
    public GameObject NegezaPrefab;
    public GameObject PanchPrefab;
    public GameObject WakBirdPrefab;
    public GameObject DoorPrefab;
    GameObject MonPrefab;

    void Start()
    {


    }
    /*
        NORMAL, //몬스터 0~2, 단순하게 피할 수 있는 함정 0~1
        TRAP, //약간의 퍼즐적 요소? 피할 수 있는 함정방? 
        MONSTER, //몬스터 3~6, 높은 등급의 몬스터 0~1, 아이템 상자 
        NPC, //특정 npc가 존재하는 방 
        BONUS, //아이템 상자 
        STAIR, //(필수)계단이 있는 방 

     */


    public void makeMonster(RoomManager.RoomType roomType, int x, int y)
    {
        if (roomType == RoomManager.RoomType.STAIR)
        {
            Vector3 position = new Vector3(Random.Range(x * (RoomManager.roomsize + 1), x * (RoomManager.roomsize + 1) + RoomManager.roomsize), Random.Range(y * (RoomManager.roomsize + 1), y * (RoomManager.roomsize + 1) + RoomManager.roomsize), 0);
            Instantiate(DoorPrefab, position, Quaternion.identity);
        }

        else if (roomType == RoomManager.RoomType.NORMAL)
        {
            int monType = Random.Range(0, 3);
            if (monType == 0) MonPrefab = WakBirdPrefab;
            else if(monType == 1) MonPrefab = PanchPrefab;
            else if (monType == 2) MonPrefab = amoebaPrefab;


            int monNum = Random.Range(1, 4);
            for (int i =0; i<monNum; i++)
            {
                Vector3 Monposition = new Vector3( Random.Range( x*(RoomManager.roomsize+1) , x * (RoomManager.roomsize + 1) + RoomManager.roomsize ), Random.Range(y * (RoomManager.roomsize + 1), y * (RoomManager.roomsize + 1) + RoomManager.roomsize), 0);
                Instantiate(MonPrefab, Monposition, Quaternion.identity);
            }
        }

        else if (roomType == RoomManager.RoomType.MONSTER)
        {
            
        }

        else if (roomType == RoomManager.RoomType.NPC)
        {

        }

        else if (roomType == RoomManager.RoomType.BONUS)
        {

        }

        else if (roomType == RoomManager.RoomType.TRAP)
        {

        }
        
    }


}
