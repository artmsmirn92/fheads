using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Utils;
using UnityEngine;
using UnityEngine.UI;

public class CameraSize : MonoBehaviour
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
    
    private float     m_defY;
    private Camera    m_cam;
    private Transform m_followTr;
    
    private int     m_graph;
    private Vector3 m_camDefRot;
    private float   m_resMy0,      m_resMy01;
    private float   m_leftCamEdge, m_rightCamEdge, m_topCamEdge;
    private float   m_newAng0,     m_newAng;
    private bool    m_isFromButton;

    #endregion

    #region engine methods

    private void Awake()
    {
        Camera = GetComponent<Camera>();

        m_graph = PlayerPrefs.GetInt("Graph");
        m_cam   = GetComponent<Camera>();

        m_followTr = scr.pMov.transform;
        m_resMy01 = GraphicUtils.AspectRatio;
        m_resMy0   = 14f / 10f;

        SetCameraSize(scr.alPrScr._camera);
        SetGraphics(1);

        m_camDefRot = transform.localRotation.eulerAngles;
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
            m_graph = m_graph == 2 ? 0 : m_graph + 1;
            PlayerPrefs.SetInt("Graph", m_graph);
            int graph1 = Animator.StringToHash(m_graph.ToString());
            rTr_Circle.GetComponent<Animator>().SetTrigger(graph1);
        }
        else
        {
            rTr_Circle.anchoredPosition = new Vector2(
                0f,
                rTr_Circle.anchoredPosition.y);
        }
        SetGraphicsCore(m_graph);
        if (scr.rainMan.isRain)
            scr.rainMan.SetRain_On();
    }
    
    public void SetCameraDefaultPosition()
    {
        transform.position = camDefPos;
        transform.rotation = Quaternion.Euler(m_camDefRot);
    }

    public void SetCameraPositionForCongratulationPanel()
    {
        transform.SetPosX(-20f);
        transform.rotation = Quaternion.Euler(m_camDefRot);
    }

    public void SetCameraSize(int _Size)
    {
        for (int i = 0; i < obj_CamButtons.Length; i++)
            obj_CamButtons[i].SetActive(i == _Size);
        float newY0 = 0f;
        switch (_Size)
        {
            case 0:
                newY0                  = 13.6f;
                m_cam.orthographicSize = 28.42f;

                m_leftCamEdge  = 27.78f  * m_resMy01 - 99.22f;
                m_rightCamEdge = -27.78f * m_resMy01 + 59.17f;
                break;
            case 1:
                newY0                  = 10.4f;
                m_cam.orthographicSize = 25f;

                m_leftCamEdge  = 25.56f  * m_resMy01 - 100.3f;
                m_rightCamEdge = -25.56f * m_resMy01 + 60.3f;
                m_topCamEdge   = 2.78f   * m_resMy01 + 12.28f;
                break;
        }

        transform.SetPosY(newY0);
        camDefPos = !m_isFromButton ? transform.position : new Vector3(camDefPos.x, newY0, camDefPos.z);

        m_isFromButton = true;

        scr.alPrScr._camera = _Size;
        scr.alPrScr.doCh    = true;
    }

    #endregion

    #region nonpublic methods
    
    private void CameraTransform()
    {
        float newPosX = GetCameraPositionX();
        float newPosY = GetCameraPositionY();
        transform.SetLocalPosXY(newPosX, newPosY);
    }

    private float GetCameraPositionX()
    {
        (Vector2 bPos, float camOrtSize) = (scr.ballScr.transform.position, m_cam.orthographicSize);
        float newPosX = 0.5f * (m_followTr.position.x + bPos.x);
        newPosX = Mathf.Clamp(newPosX, bPos.x - camOrtSize, bPos.x + camOrtSize);
        newPosX = Mathf.Clamp(newPosX, m_leftCamEdge, m_rightCamEdge);
        newPosX = Mathf.Lerp(transform.position.x, newPosX, lerpX * Time.deltaTime);
        return newPosX;
    }

    private float GetCameraPositionY()
    {
        float newPosY;
        if (scr.alPrScr._camera == 0)           //Big Camera
            newPosY = -14.78f * m_resMy0 + 34.3f;
        else                                    //Small Camera
        {
            newPosY = 0.5f * (m_followTr.position.y + scr.ballScr.transform.position.y);
            // newPosY = Mathf.Clamp(newPosY, 10f, m_topCamEdge);
            newPosY = Mathf.Lerp(transform.position.y, newPosY, lerpX * Time.deltaTime * 5f);
        }
        newPosY += CameraBottomOffset;
        return newPosY;
    }

    private void TiltCamera()
    {
        if (!scr.objLev.isTiltOn) 
            return;
        angCoeff = scr.objLev.isTiltOn ? 0.03f : 0f;
        m_newAng   = scr.pMov._rb.velocity.x * angCoeff;

        if (scr.pMov.transform.position.x > scr.marks.rightTiltEdgeTr.position.x ||
            scr.pMov.transform.position.x < scr.marks.leftTiltEdgeTr.position.x)
            m_newAng = 0;

        transform.rotation = Quaternion.AngleAxis(
            m_newAng,
            Vector3.forward);
    }
    
    private void SetGraphicsCore(int _Case)
    {
        ballLineR.enabled = false;
        obj_FireTrail.SetActive(true);
    }

    #endregion
}