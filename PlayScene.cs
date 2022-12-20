using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayScene : MonoBehaviour
{
    [Tooltip("Camera used to estimate the overlay positions of 3D-objects over the background. By default it is the main camera.")]
    public Camera m_ForegroundCamera;

    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;

    [Tooltip("Game object used to overlay the left hand.")]
    public Transform leftHandOverlay;

    [Tooltip("Game object used to overlay the right hand.")]
    public Transform rightHandOverlay;

    public GameObject m_ExitMessageBoxPrefab;
    public GameObject m_PlayingIconPrefab;

    public RawImage m_BgImage;
    Texture2D m_BgTex;

    // reference to KinectManager
    KinectManager m_KinectManager;

    Theme m_Theme;
    List<Icon> m_Icons;
    bool m_bPlaying = false;
    bool m_bStop = false;

    // Sound
    public static AudioSource m_AudioSource;
    AudioSource m_BgAudioSource;

    // Animation
    UnityEvent m_AnimPauseEvent;
    UnityEvent m_AnimResumeEvent;

    // Debug Information
    ulong m_numDetected;
    ulong m_numNotDetected;

    bool m_StartCountDetection;


    public int MovingLevel = 1;             // MoveLevel 값을 정해 아이콘 생성 확률을 조작함

    GameManager2 gameManager2;
    

   // string fullpth = "Assets/Resources/Data/test1";
   // StreamWriter sw;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 90;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //
        if (m_ForegroundCamera == null)
        {
            // by default use the main camera
            m_ForegroundCamera = Camera.main;
        }

        if (m_KinectManager == null)
        {
            m_KinectManager = KinectManager.Instance;
        }

        m_AudioSource = gameObject.AddComponent<AudioSource>();
        m_Theme = ThemeConfig.Themes[ThemeConfig.CurrentThemeName];

        SetupBackground();

        m_Icons = new List<Icon>();
        foreach(KeyValuePair<string, Icon> pair in m_Theme.Icons)
        {
            Icon icon = pair.Value;
            if (icon.IsChecked)
                m_Icons.Add(icon);
        }

        m_bPlaying = true;
        m_bStop = false;

        if (m_AnimPauseEvent == null) m_AnimPauseEvent = new UnityEvent();
        if (m_AnimResumeEvent == null) m_AnimResumeEvent = new UnityEvent();

        if (m_Icons.Count > 0)
            StartCoroutine(GenerateIcon());

        m_numDetected = m_numNotDetected = 0;
        m_StartCountDetection = false;

       

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject exit_panel = Instantiate(m_ExitMessageBoxPrefab);
            exit_panel.GetComponent<ExitPrefab>().m_PlayScene = gameObject;

            RectTransform rt = exit_panel.GetComponent<RectTransform>();
            rt.SetParent(gameObject.GetComponent<RectTransform>(), false);
            //m_bPlaying = false;
            m_bStop = true;
            if (m_BgAudioSource) m_BgAudioSource.Pause();

            m_AnimPauseEvent.Invoke();
            rt.SetAsLastSibling();

            
            
        }

        /// 키보드로 레벨 조정 구문 
        #region 레벨 조정 for Keyboard
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MovingLevel = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MovingLevel = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MovingLevel = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MovingLevel = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            MovingLevel = 5;
        }
        #endregion


        #region TimeAttack########## (Time Second Pick here) + Gamemanager2 스크립트 149줄

        gameManager2 = GameObject.Find("GameM").GetComponent<GameManager2>(); 
        if (gameManager2._Sec2Trans == 28f)
        {
           // m_bStop = true;

        }
        if (gameManager2._Sec2Trans == 31f)
        {
          //  m_bStop = true;
           // Time.timeScale = 0;
        }
        #endregion
      


    }

    private void SetupBackground()
    {
        // Image
        m_BgTex = new Texture2D(1920, 1080);
        if (m_Theme.BackgroundImageName == "Default")
        {
            m_BgTex = Resources.Load("DefaultBgImage") as Texture2D;
        }
        else
        {
            string strBgImage = ThemeConfig.ThemesDirectoryPath + m_Theme.ThemeName + "/" + m_Theme.BackgroundImageName;
            byte[] fileData = File.ReadAllBytes(strBgImage);
            m_BgTex.LoadImage(fileData);
        }
        m_BgImage.texture = m_BgTex;



        // Sound
        if (m_Theme.IsMute) return;

        m_BgAudioSource = gameObject.AddComponent<AudioSource>();
    
        AudioClip bgAudioClip = null;
        if (m_Theme.BackgroundSoundName == "Default")
        {
            bgAudioClip = Resources.Load("DefaultIconSound") as AudioClip;
        }
        else
        {
            string strBgSound = ThemeConfig.ThemesDirectoryPath + m_Theme.ThemeName + "/" + m_Theme.BackgroundSoundName;
            Utility.LoadSound(strBgSound, ref bgAudioClip);
        }
        m_BgAudioSource.clip = bgAudioClip;
        m_BgAudioSource.loop = true;
        m_BgAudioSource.Play();
    }

    private void OnGUI()
    {
        // get the background rectangle (use the portrait background, if available)
        Rect backgroundRect = m_ForegroundCamera.pixelRect;
        PortraitBackground portraitBack = PortraitBackground.Instance;

        if (portraitBack && portraitBack.enabled)
        {
            backgroundRect = portraitBack.GetBackgroundRect();
        }

        // overlay the joints
        if (m_KinectManager.IsUserDetected(playerIndex))
        {
            long userId = m_KinectManager.GetUserIdByIndex(playerIndex);

            OverlayJoint(userId, (int)KinectInterop.JointType.HandLeft, leftHandOverlay, backgroundRect);
            OverlayJoint(userId, (int)KinectInterop.JointType.HandRight, rightHandOverlay, backgroundRect);

            if (m_StartCountDetection == false) m_StartCountDetection = true;

            if (m_StartCountDetection) m_numDetected++;
        }
        else
        {
            if (m_StartCountDetection) m_numNotDetected++;
        }

        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, h * 8.0f / 100.0f, w, h * 4.0f / 100.0f);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = (int)(h * 4.0f / 100.0f);
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

        float fTotal = m_numDetected + m_numNotDetected;
        string text = string.Format("", m_numDetected, fTotal, m_numDetected/fTotal*100.0f);
      //  string text = string.Format("Detected Rate = {0}/{1} ({2:0.0} %))", m_numDetected, fTotal, m_numDetected / fTotal * 100.0f);  This origin text
        GUI.Label(rect, text, style);
    }

    // overlays the given object over the given user joint
    private void OverlayJoint(long userId, int jointIndex, Transform overlayObj, Rect imageRect)
    {
        if (m_KinectManager.IsJointTracked(userId, jointIndex))
        {
            Vector3 posJoint = m_KinectManager.GetJointKinectPosition(userId, jointIndex);

            MoveOnCanvas(posJoint, overlayObj, imageRect);
        }
    }

    void MoveOnCanvas(Vector3 posJoint, Transform overlayObj, Rect imageRect)
    {
        if (posJoint != Vector3.zero)
        {
            // 3d position to depth 이미지좌표9depth
            Vector2 posDepth = m_KinectManager.MapSpacePointToDepthCoords(posJoint);
            ushort depthValue = m_KinectManager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);

            if (posDepth != Vector2.zero && depthValue > 0)
            {
                // depth pos to color pos
                Vector2 posColor = m_KinectManager.MapDepthPointToColorCoords(posDepth, depthValue);

                if (!float.IsInfinity(posColor.x) && !float.IsInfinity(posColor.y))
                {
                    RectTransform rt = (RectTransform)overlayObj;

                    float x = imageRect.width * posColor.x / m_KinectManager.GetColorImageWidth();
                    float y = imageRect.height * (1.0f - posColor.y / m_KinectManager.GetColorImageHeight());
                    rt.position = new Vector3(x, y, 0.0f);
                }
            }
        }
    }

    void ResumePlay()
    {
        if (m_BgAudioSource) m_BgAudioSource.UnPause();
        m_AnimResumeEvent.Invoke();
        //m_bPlaying = true;
        //StartCoroutine(GenerateIcon());
        m_bStop = false;
    }

    IEnumerator GenerateIcon()
    {
        while(m_bPlaying)
        {
            yield return new WaitForSeconds(1f);         // Set amount Object Here!

            //if (!m_bPlaying) continue;
            if (m_bStop) continue;

            int idx = Random.Range(0, m_Icons.Count);             // 선택된 아이콘 출력
            
            Icon icon = m_Icons[idx];

            GameObject new_icon = Instantiate(m_PlayingIconPrefab);
            new_icon.GetComponent<RectTransform>().SetParent(gameObject.GetComponent<RectTransform>(), false);

            new_icon.layer = LayerMask.NameToLayer("Icon");

            PlayingIconPrefab script = new_icon.GetComponent<PlayingIconPrefab>();
            script.SetIcon(icon);
            

            m_AnimPauseEvent.AddListener(script.PauseAnimation);
            m_AnimResumeEvent.AddListener(script.ResumeAnimation);
        }
    }

   

  
}
