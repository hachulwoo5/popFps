using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TYPE {top, bottom }                    // ��ü, ��ü�� ������ Ÿ��
public class Distance2 : MonoBehaviour
{
    float Distance1;                               // ������ �Ÿ�
    
    private float TimeLeft = 0.1f;
    private float nextTime = 0.0f;

    public TYPE myType;                             // ��ü, ��ü�� ������ Ÿ��

    GameManager2 gameManager2;
    public GameObject pt1;                          // ��ġ�� 1
    public GameObject pt2;                          // ��ġ�� 2
    int ttime;                                      // Ÿ�̸� ����

    private void Start()
    {

        gameManager2=  GameObject.Find("GameM").GetComponent<GameManager2>();


        pt1.transform.position = this.transform.position;       // ���۽� �󲮵��� pt1�� ������Ʈ �� ����, ù Vector3.Distance ���� �� ����� �� �� �ο��� ���� 
    }
    void Update()
    {

        CountDistance();


    }


    void CountDistance()
    {
        if (Time.time > nextTime)
        {
            // n�ʸ��� �۵�
            nextTime = Time.time + TimeLeft;       

            // �� �����ǿ� ��ġ�� �־ n�ʸ��� ������ �Ÿ� �ջ� /������ pt2-pt1 
            pt2.transform.position = this.transform.position;                                  
            Distance1 = Vector3.Distance(pt1.transform.position, pt2.transform.position) * 0.01f;       

           // ������ ������ ������ ��ũ��Ʈ�� ���Ǽ� 1���� ���� ���� �� 32���� ���� ��
            #region Ʀ ������ ���� �ջ� ����
            if (Distance1 <= 1.0f)
            {
                pt1.transform.position = pt2.transform.position;
                if (myType == TYPE.top && gameManager2._Sec2Trans>=3.0f)
                {
                    gameManager2.TopPoint += Distance1;     // ����Ʈ�� ������ ��ü��� ��ü ������ �߰�
                }
                if (myType == TYPE.bottom && gameManager2._Sec2Trans >= 3.0f)
                {
                    gameManager2.BottomPoint += Distance1;  // ����Ʈ�� ������ ��ü��� ��ü ������ �߰�
                }
            }
            else if (Distance1 > 1.0f)
            {
                pt1.transform.position = pt2.transform.position;
                
            }
            #endregion


            // Debug.Log(this.gameObject.name+"="+ Distance1); //Ȯ�ο�
          
        }
    }

    public void ReSetTime()
    {
        nextTime = 0f;
    }

}