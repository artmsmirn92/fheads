using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Utils;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    #region constants

    private const float CameraBottomOffset = -5f;

    #endregion
    
    #region serialized fields

    public Scripts       scr;
    public GameObject[]  obj_CamButtons;
    public Image[]       im_GameButtons;
    public LineRenderer  ballLineR;
    public GameObject    obj_FireTrail;
    public RectTransform rTr_Circle;
    public Transform     roofTr, roof1Tr;
    public Transform     stadiumsTr;

    public Vector3 camDefPos;
    public float   lerpX;
    public float   tim;
    
    [Range(.0f, .1f)] public float angCoeff;

    #endregion

    #region nonpublic members
    
    private float     m_DefY;
    private Camera    m_Cam;
    private Transform m_FollowTr;
    
    private int     m_Graph;
    private Vector3 m_CamDefRot;
    private float   m_ResMy0,      m_ResMy01;
    private float   m_LeftCamEdge, m_RightCamEdge, m_TopCamEdge, m_BottomCamEdge;
    private float   m_NewAng0,     m_NewAng;
    private bool    m_IsFromButton;

    #endregion

    #region engine methods

    private void Awake()
    {
        SetCameraDefaultPosition();
        scr.alPrScr._camera = 1;
        Camera = GetComponent<Camera>();

        m_Graph = PlayerPrefs.GetInt("Graph");
        m_Cam   = GetComponent<Camera>();

        m_FollowTr = scr.pMov.transform;
        m_ResMy01 = GraphicUtils.AspectRatio;
        m_ResMy0   = 14f / 10f;

        SetCameraSize(scr.alPrScr._camera);
        SetGraphics(1);

        m_CamDefRot = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        tim += Time.deltaTime;
        if (MathUtils.IsInRange(tim, PlayerMovement.restartDelay2, PlayerMovement.restartDelay1))
            transform.position = camDefPos;
        else if ((!scr.pMov.restart && tim > PlayerMovement.restartDelay1) ||
                scr.pMov.restart)
        {
            TiltCamera();
            CameraTransform();
        }
    }

    #endregion

    #region api
    
    public Camera Camera { get; set; }
    
    public void SetGraphics(int _FromAwake)
    {
        if (_FromAwake == 0)
        {
            m_Graph = m_Graph == 2 ? 0 : m_Graph + 1;
            PlayerPrefs.SetInt("Graph", m_Graph);
            int graph1 = Animator.StringToHash(m_Graph.ToString());
            rTr_Circle.GetComponent<Animator>().SetTrigger(graph1);
        }
        else
        {
            rTr_Circle.anchoredPosition = new Vector2(
                0f,
                rTr_Circle.anchoredPosition.y);
        }
        SetGraphicsCore(m_Graph);
        if (scr.rainMan.isRain)
            scr.rainMan.SetRain_On();
    }
    
    public void SetCameraDefaultPosition()
    {
        transform.position = camDefPos;
        transform.rotation = Quaternion.Euler(m_CamDefRot);
    }

    public void SetCameraPositionForCongratulationPanel()
    {
        transform.SetPosX(-20f);
        transform.rotation = Quaternion.Euler(m_CamDefRot);
    }

    public void SetCameraSize(int _Size)
    {
        for (int i = 0; i < obj_CamButtons.Length; i++)
            obj_CamButtons[i].SetActive(i == _Size);
        switch (_Size)
        {
            case 0: SetCameraSizeBig();   break;
            case 1: SetCameraSizeSmall(); break;
        }
        SetCameraNewPositionY(m_BottomCamEdge);
        m_IsFromButton      = true;
        scr.alPrScr._camera = _Size;
        scr.alPrScr.doCh    = true;
    }

    #endregion

    #region nonpublic methods

    private void SetCameraSizeBig()
    {
        m_Cam.orthographicSize = 28.42f;
        m_LeftCamEdge  = 27.78f  * m_ResMy01 - 99.22f;
        m_RightCamEdge = -27.78f * m_ResMy01 + 59.17f;
        m_BottomCamEdge = 13.6f;
    }

    private void SetCameraSizeSmall()
    {
        m_Cam.orthographicSize = 25f;
        m_LeftCamEdge  = 25.56f  * m_ResMy01 - 100.3f;
        m_RightCamEdge = -25.56f * m_ResMy01 + 60.3f;
        m_TopCamEdge   = 2.78f     * m_ResMy01 + 15f;
        m_BottomCamEdge = 4.5f;
    }

    private void SetCameraNewPositionY(float _PosY)
    {
        transform.SetPosY(_PosY);
        camDefPos = m_IsFromButton ? camDefPos.SetY(_PosY) : transform.position;
    }
    
    private void CameraTransform()
    {
        float newPosX = GetCameraPositionX();
        float newPosY = GetCameraPositionY();
        transform.SetLocalPosXY(newPosX, newPosY);
    }

    private float GetCameraPositionX()
    {
        (Vector2 bPos, float camOrtSize) = (scr.ballScr.transform.position, m_Cam.orthographicSize);
        float newPosX = 0.5f * (m_FollowTr.position.x + bPos.x);
        newPosX = Mathf.Clamp(newPosX, bPos.x - camOrtSize, bPos.x + camOrtSize);
        newPosX = Mathf.Clamp(newPosX, m_LeftCamEdge, m_RightCamEdge);
        newPosX = Mathf.Lerp(transform.position.x, newPosX, lerpX * Time.deltaTime);
        return newPosX;
    }

    private float GetCameraPositionY()
    {
        float newPosY;
        if (scr.alPrScr._camera == 0)           //Big Camera
            newPosY = -14.78f * m_ResMy0 + 34.3f;
        else                                    //Small Camera
        {
            newPosY = Mathf.Lerp(m_FollowTr.position.y, scr.ballScr.transform.position.y, 0.8f);
            newPosY = Mathf.Clamp(newPosY, m_BottomCamEdge, m_TopCamEdge);
            // newPosY = Mathf.Lerp(transform.position.y, newPosY, lerpX * Time.deltaTime * 5f);
        }
        // newPosY += CameraBottomOffset;
        return newPosY;
    }

    private void TiltCamera()
    {
        if (!scr.objLev.isTiltOn) 
            return;
        angCoeff = scr.objLev.isTiltOn ? 0.03f : 0f;
        m_NewAng   = scr.pMov._rb.velocity.x * angCoeff;

        if (scr.pMov.transform.position.x > scr.marks.rightTiltEdgeTr.position.x ||
            scr.pMov.transform.position.x < scr.marks.leftTiltEdgeTr.position.x)
            m_NewAng = 0;

        transform.rotation = Quaternion.AngleAxis(
            m_NewAng,
            Vector3.forward);
    }
    
    private void SetGraphicsCore(int _Case)
    {
        ballLineR.enabled = false;
        obj_FireTrail.SetActive(true);
    }

    #endregion
}