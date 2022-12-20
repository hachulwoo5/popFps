using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TYPE {top, bottom }                    // 상체, 하체를 나누는 타입
public class Distance2 : MonoBehaviour
{
    float Distance1;                               // 움직인 거리
    
    private float TimeLeft = 0.1f;
    private float nextTime = 0.0f;

    public TYPE myType;                             // 상체, 하체를 나누는 타입

    GameManager2 gameManager2;
    public GameObject pt1;                          // 위치값 1
    public GameObject pt2;                          // 위치값 2
    int ttime;                                      // 타이머 변수

    private void Start()
    {

        gameManager2=  GameObject.Find("GameM").GetComponent<GameManager2>();


        pt1.transform.position = this.transform.position;       // 시작시 빈껍데기 pt1에 오브젝트 값 대입, 첫 Vector3.Distance 실행 시 제대로 된 값 부여를 위해 
    }
    void Update()
    {

        CountDistance();


    }


    void CountDistance()
    {
        if (Time.time > nextTime)
        {
            // n초마다 작동
            nextTime = Time.time + TimeLeft;       

            // 빈 포지션에 위치값 넣어서 n초마다 움직인 거리 합산 /메인은 pt2-pt1 
            pt2.transform.position = this.transform.position;                                  
            Distance1 = Vector3.Distance(pt1.transform.position, pt2.transform.position) * 0.01f;       

           // 각각의 점에서 각각의 스크립트가 계산되서 1개의 점수 합은 곧 32개의 점수 합
            #region 튐 방지용 점수 합산 구문
            if (Distance1 <= 1.0f)
            {
                pt1.transform.position = pt2.transform.position;
                if (myType == TYPE.top && gameManager2._Sec2Trans>=3.0f)
                {
                    gameManager2.TopPoint += Distance1;     // 포인트의 부위가 상체라면 상체 점수에 추가
                }
                if (myType == TYPE.bottom && gameManager2._Sec2Trans >= 3.0f)
                {
                    gameManager2.BottomPoint += Distance1;  // 포인트의 부위가 하체라면 하체 점수에 추가
                }
            }
            else if (Distance1 > 1.0f)
            {
                pt1.transform.position = pt2.transform.position;
                
            }
            #endregion


            // Debug.Log(this.gameObject.name+"="+ Distance1); //확인용
          
        }
    }

    public void ReSetTime()
    {
        nextTime = 0f;
    }

}