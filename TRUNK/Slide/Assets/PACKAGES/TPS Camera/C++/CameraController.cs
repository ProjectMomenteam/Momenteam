using UnityEngine;
using System.Collections;


public class CameraController : MonoBehaviour {

    GameObject  m_pCameraContainer;
    GameObject  m_pMyPawn;
    Camera      m_pCameraComponent;

    //CAMERA ROTATION PARAMETERS
    [Range(0f, 100f)]
    public float m_fCameraYRotationSpeed;
    [Range(0f, 10f)]
    public float m_fCameraXRotationSpeed;
    [Range(0f, 10f)]
    public float m_fResetCameraPosSpeed;

    //CAMERA SMOOTH FOLLOW PARAMETERS
    [Range(0f, 20f)]
    public float m_fDesiredCamHeight =          0f;
    [Range(0f, 20f)]
    public float m_fDesiredCamDistance =        0f;
    [Range(0f, 20f)]
    public float m_fDesiredHeightDamping =      2f;
    [Range(0f, 200f)]
    public float m_fDesiredRotationDamping =    2f;
    [Range(0f, 50f)]
    public float m_fDesiredFOVSpeedChange = 10f;

    float m_fInputValue;

    public bool m_bIsRotatingBackToInitialPos = false;



    void Awake()
    {
        SetPawn();
        SetCamera();
    }

    void Start () {
       
    }

    void Update () {
        RightStickCameraUpdate();
        UpdateCameraPos();
    }


    void SetCamera()
    {
        m_pCameraContainer =    this.gameObject;
        m_pCameraComponent =             GetComponent<Camera>();
    }

    void SetPawn()
    {
        m_pMyPawn =             GameObject.Find("CharacterController");
    }

    void UpdateCameraPos()
    {
        if (!m_pMyPawn) return;
        if (!m_pCameraContainer) return;

        float _fWantedHeight =          m_pMyPawn.transform.position.y + m_fDesiredCamHeight;
        float _fWantedRotationYAngle =  m_pMyPawn.transform.eulerAngles.y;
        float _fWantedRotationXAngle =  m_pMyPawn.transform.eulerAngles.x;
        float _fWantedRotationZAngle =  m_pMyPawn.transform.eulerAngles.z;

        float _fCurrentRotationYAngle = transform.eulerAngles.y;
        float _fCurrentRotationXAngle = transform.eulerAngles.x;
        float _fCurrentRotationZAngle = transform.eulerAngles.z;
        float _fCurrentHeight = transform.position.y;

        _fCurrentRotationYAngle =       Mathf.LerpAngle(_fCurrentRotationYAngle, _fWantedRotationYAngle, m_fDesiredRotationDamping * Time.deltaTime);
        _fCurrentRotationXAngle =       Mathf.LerpAngle(_fCurrentRotationXAngle, _fWantedRotationXAngle, m_fDesiredRotationDamping * Time.deltaTime);
        _fCurrentRotationZAngle =       Mathf.LerpAngle(_fCurrentRotationZAngle, _fWantedRotationZAngle, m_fDesiredRotationDamping * Time.deltaTime);
        _fCurrentHeight =               Mathf.Lerp(_fCurrentHeight, _fWantedHeight, m_fDesiredHeightDamping * Time.deltaTime);

        Quaternion _qCurrentRotation =  Quaternion.Euler(_fCurrentRotationXAngle, _fCurrentRotationYAngle, _fCurrentRotationZAngle);

        transform.position =            m_pMyPawn.transform.position;
        transform.position -=           _qCurrentRotation * Vector3.forward * m_fDesiredCamDistance;
        transform.position =            new Vector3(transform.position.x, _fCurrentHeight, transform.position.z);

        
        m_pCameraContainer.transform.LookAt(m_pMyPawn.transform);
    }

    void UpdateCameraRotation(float _fHorizontalValue, float _fVerticalValue)
    {
        if (!m_pCameraContainer)    return;
        if (!m_pCameraComponent)    return;

        if (Input.GetKey(KeyCode.Keypad4))
           m_pCameraContainer.transform.RotateAround(m_pMyPawn.transform.position,Vector3.up,m_fCameraYRotationSpeed*Time.deltaTime);
        
        else if (Input.GetKey(KeyCode.Keypad6))
           m_pCameraContainer.transform.RotateAround(m_pMyPawn.transform.position, -Vector3.up, m_fCameraXRotationSpeed * Time.deltaTime);

        //Debug.Log(m_pMyPawn.GetComponent<Rigidbody>().velocity.magnitude);

        if (true)
        {
            if (m_pCameraComponent.fieldOfView <= 100f)
            {
                m_pCameraComponent.fieldOfView += Time.deltaTime * m_fDesiredFOVSpeedChange;
                m_bIsRotatingBackToInitialPos = true;
            }

            else if (m_pCameraComponent.fieldOfView >= 100f)
            {
                m_pCameraComponent.fieldOfView = 100f;
            }
        }
        else if (m_pMyPawn.GetComponent<Rigidbody>().velocity.magnitude > 4f && true)
        {
            if(m_pCameraComponent.fieldOfView <= 80.5f)
            {
                m_pCameraComponent.fieldOfView += Time.deltaTime* m_fDesiredFOVSpeedChange;
                m_bIsRotatingBackToInitialPos = true;
            }

            else if (m_pCameraComponent.fieldOfView >= 80.5f)
            {
                m_pCameraComponent.fieldOfView -= Time.deltaTime* m_fDesiredFOVSpeedChange;
            }
        }
        else if (true && m_pMyPawn.GetComponent<Rigidbody>().velocity.magnitude < 4f)
        {
            if (m_pCameraComponent.fieldOfView >= 60f)
            {
                m_pCameraComponent.fieldOfView -= Time.deltaTime* m_fDesiredFOVSpeedChange;
                m_bIsRotatingBackToInitialPos = false;
            }

            else if (m_pCameraComponent.fieldOfView < 60f)
            {
                m_pCameraComponent.fieldOfView = 60f;
            }
        }
        


        m_pCameraContainer.transform.RotateAround(m_pMyPawn.transform.position,Vector3.up, _fVerticalValue);
        m_pCameraContainer.transform.RotateAround(m_pMyPawn.transform.position, Vector3.right, _fHorizontalValue);

           
    }


    void RightStickCameraUpdate()
    {
        float _fYAxisValue = Input.GetAxis("Right Horizontal") * m_fCameraXRotationSpeed;
        float _fXAxisValue = Input.GetAxis("Right Vertical") * m_fCameraYRotationSpeed;

        UpdateCameraRotation(_fYAxisValue, _fXAxisValue);
    }
}


