using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayingIconPrefab : MonoBehaviour
{
    string m_strImgDirPath;
    string m_strSndDirPath;
    Icon m_Icon;

    RectTransform m_RectTrans;

    // Image
    RawImage m_RawImage;
    Texture2D m_Tex;

    // Sound
    AudioClip m_AudioClip;

    // Animation
    List<Vector3> m_AnimPoints;
    float m_Speed = 5.0f;

    // Destroy
    public Material m_DestroyMtrl;
    float m_DestroySpeed = 1.0f;
    float m_DestroyStartTime = 0.0f;
    bool m_bDestroy = false;

    // the responsibility of destruction
    float m_resTime = 0.0f;
    bool m_bFirstDestroy = true;

    // 랜덤 생성 위치

    public int[] randombox = new int[100];                   // 랜덤 확률
    PlayScene playScene;
  
    public int Movelevel;
    

   

    private void Awake()
    {
        m_strImgDirPath = ThemeConfig.ThemesDirectoryPath + ThemeConfig.CurrentThemeName + "/Icons_Images/";
        m_strSndDirPath = ThemeConfig.ThemesDirectoryPath + ThemeConfig.CurrentThemeName + "/Icons_Sounds/";

        m_RectTrans = GetComponent<RectTransform>();
        if (m_Tex == null) m_Tex = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        if (m_RawImage == null)
        {
            m_RawImage = GetComponent<RawImage>();

            Material mtrl = new Material(m_DestroyMtrl);
            m_RawImage.material = mtrl;
            m_RawImage.material.SetFloat("_Destroyer_Value_1", 0.0f);
        }

        m_AnimPoints = new List<Vector3>();
        
        

    }

    // Start is called before the first frame update
    void Start()
    {
        m_bDestroy = false;
        m_bFirstDestroy = true;

      

    }

    // Update is called once per frame
    void Update()
    {
        if (m_bDestroy)
        {
            if (m_bFirstDestroy)
            {
                m_resTime = Time.time - m_DestroyStartTime;
                m_bFirstDestroy = false;
            }
            m_RawImage.material.SetFloat("_Destroyer_Value_1", (Time.time - m_DestroyStartTime) / m_DestroySpeed);
        }

        

    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, h * 4.0f / 100.0f, w, h * 4.0f / 100.0f);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = (int)(h * 4.0f / 100.0f);
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

        if (m_bFirstDestroy == false)
        {
            string text = string.Format("{0:0.0} ms ", m_resTime * 1000.0f);
            GUI.Label(rect, text, style);
        }
        
    }

    public void SetIcon(Icon icon)
    {
        
        m_Icon = icon;

        if (m_Icon.ImageFileName == "Default")
        {
            Texture2D tex = Resources.Load("DefaultIconImage") as Texture2D;
            m_RawImage.texture = tex;
        }
        else
        {
            string strImgFile = m_strImgDirPath + m_Icon.ImageFileName;
            byte[] fileData = File.ReadAllBytes(strImgFile);
            m_Tex.LoadImage(fileData);
            m_RawImage.texture = m_Tex;
        }

         float imgSize = m_Icon.IsImageSizeRandom ? Random.Range(m_Icon.ImgRandMin*2, m_Icon.ImgRandMax*2) : m_Icon.ImageSize;        
       
        m_RectTrans.sizeDelta = new Vector2(imgSize, imgSize);
        GetComponent<BoxCollider2D>().size = m_RectTrans.sizeDelta;

        AudioClip audio_clip = null;
        if (m_Icon.SoundFileName == "Default")
        {
            audio_clip = Resources.Load("DefaultIconSound") as AudioClip;
        }
        else
        {
            Utility.LoadSound(m_strSndDirPath + m_Icon.SoundFileName, ref audio_clip);
        }
        m_AudioClip = audio_clip;

        CurveControlPoints points = ThemeConfig.AnimPoints[icon.AnimIndex];
        m_AnimPoints.Clear();
        int num_pts = points.m_CtrlPoints.Length;
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        for (int i = 0; i < num_pts; i++)
        {
            Vector3 temp = new Vector3(points.m_CtrlPoints[i].x, points.m_CtrlPoints[i].y, points.m_CtrlPoints[i].z);
            temp.x *= Screen.width;
            temp.y *= Screen.height;

            m_AnimPoints.Add(temp);

            if (temp.x < min.x) min.x = temp.x;
            if (temp.x > max.x) max.x = temp.x;
            if (temp.y < min.y) min.y = temp.y;
            if (temp.y > max.y) max.y = temp.y;
        }



        //랜덤성 아이콘 위치 찍어주는 구문
        #region RandonTransIcon
        float x_offset = (Screen.width - (max.x - min.x) - m_Icon.ImageSize) / 2.0f;     // 값  = 910
        float y_offset = (Screen.height - (max.y - min.y) - m_Icon.ImageSize) / 2.0f;

        float x_offset1 = x_offset * 0.5f;                                               // 값  = 455




        for (int i = 0; i < 100; i++)
        {
            randombox[i] = i;
        }

        int randompoint = randombox[Random.Range(0, 100)];      // 랜덤 확률 부여


        playScene = GameObject.Find("Canvas").GetComponent<PlayScene>();            // 플레이 씬에서 MoveLevel 값을 정상젹으로 가져오게 하는 구문
        Movelevel = playScene.MovingLevel;                                          // 플레이 씬에서 MoveLevel 값을 정상젹으로 가져오게 하는 구문


        float x = 0;
        if (Movelevel == 1)
        {
            Debug.Log("사용자 레벨 1 정상 작동 ");
            if (randompoint <= 19)
            {
                x = Random.Range(-x_offset, -x_offset1);          // 1면 20% 
            }

            else if (randompoint <= 49)
            {
                x = Random.Range(-x_offset1, 0);                  // 2면 30% 
            }

            else if (randompoint <= 79)
            {
                x = Random.Range(0, x_offset1);                   // 3면 30% 
            }

            else if (randompoint <= 99)
            {
                x = Random.Range(x_offset1, x_offset);          // 4면 20%
            }


        }
        if (Movelevel == 2)
        {
            Debug.Log("사용자 레벨2 정상 작동 ");
            if (randompoint <= 24)
            {
                x = Random.Range(-x_offset, -x_offset1);          // 1면 25%
            }

            else if (randompoint <= 49)
            {
                x = Random.Range(-x_offset1, 0);                  // 2면 25% 
            }

            else if (randompoint <= 74)
            {
                x = Random.Range(0, x_offset1);                   // 3면 25%
            }

            else if (randompoint <= 99)
            {
                x = Random.Range(x_offset1, x_offset);          // 4면 25%
            }

        }
        if (Movelevel == 3)
        {
            Debug.Log("사용자 레벨 3 정상 작동 ");
            if (randompoint <= 29)
            {
                x = Random.Range(-x_offset, -x_offset1);          // 1면 30% 
            }

            else if (randompoint <= 49)
            {
                x = Random.Range(-x_offset1, 0);                  // 2면 20% 
            }

            else if (randompoint <= 69)
            {
                x = Random.Range(0, x_offset1);                   // 3면 20%
            }

            else if (randompoint <= 99)
            {
                x = Random.Range(x_offset1, x_offset);          // 4면 30% 
            }


        }
        if (Movelevel == 4)
        {
            Debug.Log("사용자 레벨 4 정상 작동 ");
            if (randompoint <= 34)
            {
                x = Random.Range(-x_offset, -x_offset1);          // 1면 35% 
            }

            else if (randompoint <= 49)
            {
                x = Random.Range(-x_offset1, 0);                  // 2면 15%
            }

            else if (randompoint <= 64)
            {
                x = Random.Range(0, x_offset1);                   // 3면 15%
            }

            else if (randompoint <= 99)
            {
                x = Random.Range(x_offset1, x_offset);          // 4면 35%
            }

        }
        if (Movelevel == 5)
        {
            Debug.Log("사용자 레벨5 정상 작동 ");
            if (randompoint <= 39)
            {
                x = Random.Range(-x_offset, -x_offset1);          // 1면 40%
            }

            else if (randompoint <= 49)
            {
                x = Random.Range(-x_offset1, 0);                  // 2면 10%
            }

            else if (randompoint <= 59)
            {
                x = Random.Range(0, x_offset1);                   // 3면 10% 
            }

            else if (randompoint <= 99)
            {
                x = Random.Range(x_offset1, x_offset);          // 4면 40%
            }

        }
        #endregion







        float y = Random.Range(-y_offset, y_offset);



        for (int i = 0; i < num_pts; i++)
        {
            m_AnimPoints[i] += new Vector3(x, y, 0.0f);         // x, y 값을 위에서 얻어내 화면에서 아이콘을 찍어낼 위치를 정해줌
        }

        m_RectTrans.position = m_AnimPoints[0];

        m_Speed = m_Icon.IsAnimRandom ? Random.Range(m_Icon.AnimRandMin, m_Icon.AnimRandMax) : m_Icon.AnimTime;

        RunAnimation();

       
    }
   
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
        {
            if(m_AudioClip) PlayScene.m_AudioSource.PlayOneShot(m_AudioClip);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            iTween.Stop(gameObject);

            m_DestroyStartTime = Time.time;
            m_bDestroy = true;
            Destroy(gameObject, m_DestroySpeed);
        }
    }

    void RunAnimation()
    {
        Hashtable hash = new Hashtable();
        hash.Clear();
        hash.Add("time", m_Speed);
        hash.Add("path", m_AnimPoints.ToArray());
        hash.Add("movetopath", false);
        hash.Add("oncompletetarget", gameObject);
        hash.Add("oncomplete", "Extinct");
        hash.Add("easetype", iTween.EaseType.linear);

        iTween.MoveTo(gameObject, hash);
    }

    void Extinct()
    {
        Destroy(gameObject);
    }

    public void PauseAnimation()
    {
        iTween.Pause(gameObject);
    }

    public void ResumeAnimation()
    {
        iTween.Resume(gameObject);
    }

    
}
