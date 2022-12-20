using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager2 : MonoBehaviour
{
    private float TimeLeft = 1.0f;
    public float nextTime = 0.0f;
    public float TopPoint= 0;   // 상체 거리 합산
    public float BottomPoint = 0; // 하체 거리 합산
    public int MovingLevel = 0;  // 움직임 레벨( 움직임 거리 , 무브 토탈 )
    public int Userlevel;   // 사용자 레벨

    public GameObject ReBut;
    

    PlayingIconPrefab playingIconPrefab;

    [SerializeField]
    private TextMeshProUGUI textMt;  // 점수 출력
    [SerializeField]
    private TextMeshProUGUI textUser; // 유저 레벨 출력

    PlayScene playScene;


    /// 시간 계산 변수
    float _Sec;
    public  float _Sec2;
    public double _Sec2Trans;
    int _Min;

    /// TXT 파일 출력 준비, 특정 폴더안에 DataSave 이름으로 저장
    string fullpth = "Assets/Resources/DataSave";
    
    StreamWriter sw;


    void Start()
    {
        
        
            if (false == File.Exists(fullpth))
            {

                sw = new StreamWriter(fullpth+".txt");
      
            }
         
       
        /// TXT 파일 출력 준비, 파일이 존재하지 않을 경우 생성
       

        
        /// 1초마다 TXT 파일 출력 코루틴
        StartCoroutine(textOut(1));

    }

    // Update is called once per frame
    void Update()
    {
        MovingLevelCount();
        textMt.text = " Moving Level : " + MovingLevel;
        textUser.text = " User Level : " + Userlevel;

        /// 시간 계산 구문
        _Sec += Time.deltaTime;
        
        _Sec2 += Time.deltaTime;
        
        // PlayScene Scripts Only
        _Sec2Trans = Math.Truncate(_Sec2);
       
    }
   

    void MovingLevelCount()
    {

        if (Time.time > nextTime)
        {

            nextTime = Time.time + TimeLeft;                                         //1초마다
            float AllPoint = (TopPoint + BottomPoint)/_Sec2;


            //Debug.Log("상체 : " + Math.Truncate(TopPoint * 10) / 10);      // 상체 움직인 거리 소숫점 1자리
            //Debug.Log("하체 : " + Math.Truncate(BottomPoint * 10) / 10);           // 하체 움직인 거리 소숫점 1자리  
            Debug.Log("전체 : " + Math.Truncate(AllPoint * 10) / 10);            // 전체 움직인 거리 소숫점 1자리

            Debug.Log(Math.Truncate( _Sec2)+ "초");
            //  Debug.Log(AllPoint);


            #region MovingLevel Point Trans

            int a = 10;
            // 운동량수치 0~10  : 1단계    AllPoint = 운동량수치
            if (AllPoint <= 1*a)       
            {
                MovingLevel = 1;
            }
            // 운동량수치 11~19  : 2단계
            else if (AllPoint < 2*a)
            {
                MovingLevel = 2;
            }
            // 운동량수치 20~29  : 3단계
            else if (AllPoint < 3*a)
            {
                MovingLevel = 3;
            }
            else if (AllPoint < 4*a)
            {
                MovingLevel = 4;
            }
            else if (AllPoint < 5*a)
            {
                MovingLevel = 5;
            }
            else if (AllPoint < 6*a)
            {
                MovingLevel = 6;
            }
            else if (AllPoint < 7*a)
            {
                MovingLevel = 7;
            }
            else if (AllPoint < 8*a)
            {
                MovingLevel = 8;
            }
            else if (AllPoint < 9*a)
            {
                MovingLevel = 9;
            }
            else if (AllPoint >= 10*a)
            {
                MovingLevel = 10;
            }
            #endregion


            // Time Attack Button
            if (Math.Truncate(_Sec2) == 30)
            {
             //   ReBut.gameObject.SetActive(true);
              
            }
           
        }
    }

    

    IEnumerator textOut(float delayTime)
    {
        /// 변수 값 받아오는 구문들
        playScene = GameObject.Find("Canvas").GetComponent<PlayScene>();
        Userlevel = playScene.MovingLevel;

        float AllPoint = (TopPoint + BottomPoint);

        if (_Sec2 >= 1)
        {
            AllPoint = AllPoint / _Sec2;
        }


        #region 시간계산구문
        string timeStr;

        // 분 : 초  로 나오게 함
        timeStr = string.Format("{0:D2}:{1:D2}", _Min, (int)_Sec);
        if ((int)_Sec > 59)
        {
            _Sec = 0;
            _Min++;                 // 60초가 모이면 1분으로 환산 
        }
        #endregion


        /// TXT 파일 출력 구문
        sw.Flush();
        // txt 파일에 적히는 내용 :  누적 시간 / 상체 움직임 / 하체 움직임 / (전체 움직임 나누기 진행된 초) / 움직임 레벨 / 사용자 레벨
        sw.WriteLine(timeStr + ", " + Math.Truncate(TopPoint * 10) / 10 + ", " + Math.Truncate(BottomPoint * 10) / 10 + ", " + Math.Truncate(AllPoint * 10) / 10 + ", " + MovingLevel + ", " + Userlevel);
       

        /// 1초 마다 작동
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(textOut(1));
        

    }
    
    // Restart Time Attack After
    public void NextScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
