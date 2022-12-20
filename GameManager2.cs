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
    public float TopPoint= 0;   // ��ü �Ÿ� �ջ�
    public float BottomPoint = 0; // ��ü �Ÿ� �ջ�
    public int MovingLevel = 0;  // ������ ����( ������ �Ÿ� , ���� ��Ż )
    public int Userlevel;   // ����� ����

    public GameObject ReBut;
    

    PlayingIconPrefab playingIconPrefab;

    [SerializeField]
    private TextMeshProUGUI textMt;  // ���� ���
    [SerializeField]
    private TextMeshProUGUI textUser; // ���� ���� ���

    PlayScene playScene;


    /// �ð� ��� ����
    float _Sec;
    public  float _Sec2;
    public double _Sec2Trans;
    int _Min;

    /// TXT ���� ��� �غ�, Ư�� �����ȿ� DataSave �̸����� ����
    string fullpth = "Assets/Resources/DataSave";
    
    StreamWriter sw;


    void Start()
    {
        
        
            if (false == File.Exists(fullpth))
            {

                sw = new StreamWriter(fullpth+".txt");
      
            }
         
       
        /// TXT ���� ��� �غ�, ������ �������� ���� ��� ����
       

        
        /// 1�ʸ��� TXT ���� ��� �ڷ�ƾ
        StartCoroutine(textOut(1));

    }

    // Update is called once per frame
    void Update()
    {
        MovingLevelCount();
        textMt.text = " Moving Level : " + MovingLevel;
        textUser.text = " User Level : " + Userlevel;

        /// �ð� ��� ����
        _Sec += Time.deltaTime;
        
        _Sec2 += Time.deltaTime;
        
        // PlayScene Scripts Only
        _Sec2Trans = Math.Truncate(_Sec2);
       
    }
   

    void MovingLevelCount()
    {

        if (Time.time > nextTime)
        {

            nextTime = Time.time + TimeLeft;                                         //1�ʸ���
            float AllPoint = (TopPoint + BottomPoint)/_Sec2;


            //Debug.Log("��ü : " + Math.Truncate(TopPoint * 10) / 10);      // ��ü ������ �Ÿ� �Ҽ��� 1�ڸ�
            //Debug.Log("��ü : " + Math.Truncate(BottomPoint * 10) / 10);           // ��ü ������ �Ÿ� �Ҽ��� 1�ڸ�  
            Debug.Log("��ü : " + Math.Truncate(AllPoint * 10) / 10);            // ��ü ������ �Ÿ� �Ҽ��� 1�ڸ�

            Debug.Log(Math.Truncate( _Sec2)+ "��");
            //  Debug.Log(AllPoint);


            #region MovingLevel Point Trans

            int a = 10;
            // �����ġ 0~10  : 1�ܰ�    AllPoint = �����ġ
            if (AllPoint <= 1*a)       
            {
                MovingLevel = 1;
            }
            // �����ġ 11~19  : 2�ܰ�
            else if (AllPoint < 2*a)
            {
                MovingLevel = 2;
            }
            // �����ġ 20~29  : 3�ܰ�
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
        /// ���� �� �޾ƿ��� ������
        playScene = GameObject.Find("Canvas").GetComponent<PlayScene>();
        Userlevel = playScene.MovingLevel;

        float AllPoint = (TopPoint + BottomPoint);

        if (_Sec2 >= 1)
        {
            AllPoint = AllPoint / _Sec2;
        }


        #region �ð���걸��
        string timeStr;

        // �� : ��  �� ������ ��
        timeStr = string.Format("{0:D2}:{1:D2}", _Min, (int)_Sec);
        if ((int)_Sec > 59)
        {
            _Sec = 0;
            _Min++;                 // 60�ʰ� ���̸� 1������ ȯ�� 
        }
        #endregion


        /// TXT ���� ��� ����
        sw.Flush();
        // txt ���Ͽ� ������ ���� :  ���� �ð� / ��ü ������ / ��ü ������ / (��ü ������ ������ ����� ��) / ������ ���� / ����� ����
        sw.WriteLine(timeStr + ", " + Math.Truncate(TopPoint * 10) / 10 + ", " + Math.Truncate(BottomPoint * 10) / 10 + ", " + Math.Truncate(AllPoint * 10) / 10 + ", " + MovingLevel + ", " + Userlevel);
       

        /// 1�� ���� �۵�
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(textOut(1));
        

    }
    
    // Restart Time Attack After
    public void NextScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
