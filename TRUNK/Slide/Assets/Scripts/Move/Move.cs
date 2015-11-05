using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Move : MonoBehaviour {

    Rigidbody _Rigidbody;
    Collider _Collider;

    public float _Speed = 5;
    public float _SpeedIncrease = 0;

    public List<int> _Palliers = new List<int>();
    public int _CurrentPalier = 0;
    public int _MaxPallier = 0;

    public float _SpeedDegradation = 1;
    public float _SpeedDegradationTimer = 2;

    public float _DegradationTimer = 0;

    public float _PrevHeight = 0;
    public float _AscendValue = 0;

    public float _ArtificialGravity = 10;

    private List<ObstaclesBehaviour> _AllObstacles = new List<ObstaclesBehaviour>();


    // Use this for initialization
    void Awake () 
	{
		_Rigidbody = GetComponent<Rigidbody> ();
        _Collider = GetComponent<Collider>();
        _PrevHeight = transform.position.y;
        GetAllObstacles();
    }

    // Update is called once per frame
    void Update () 
	{
        PallierBehaviour();
        Debug();
        StickToGround();

        

    }

    void FixedUpdate()
    {
        Vector3 _Direction;
        Vector3 _Forward;

        _Forward = new Vector3(transform.forward.x, 0, transform.forward.z);

        //_Direction = _Forward * Input.GetAxis ("Vertical") + transform.right * Input.GetAxis("Horizontal");
        _Direction = _Forward * Mathf.Abs(Input.GetAxis("Acceleration"));
        _Rigidbody.AddForce(_Direction.normalized * (_Speed + _SpeedIncrease), ForceMode.Acceleration);

        _Rigidbody.AddForce(new Vector3(0, -_ArtificialGravity, 0));

        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal"), 0));

        StickToGround();

        _AscendValue = transform.position.y - _PrevHeight;

        _PrevHeight = transform.position.y;
    }

    void LateUpdate()
    {
        Reveal();
    }



    void PallierBehaviour()
    {
        if (_MaxPallier == _Palliers.Count - 3)
        {
            _Palliers.Add(_Palliers[_Palliers.Count - 1] + 10);
        }

        GetPallier();
        SpeedDegradation();

        if (_Speed + _SpeedIncrease > _Palliers[_MaxPallier + 1])
        {
            _MaxPallier++;
        }
    }

    void SpeedDegradation()
    {

        _DegradationTimer += Time.deltaTime;


        if (_DegradationTimer >= _SpeedDegradationTimer && _SpeedIncrease > 0)
        {
            _SpeedIncrease -= (_SpeedDegradation / Mathf.Pow(2, _MaxPallier - _CurrentPalier)) * Time.deltaTime;
        }

    }
    

    void GetPallier()
    {
        _CurrentPalier = -1;
        foreach(int _i in _Palliers)
        {

            if (_Speed + _SpeedIncrease >= _i)
            {
                _CurrentPalier++;
            }
            else
            {
                break;
            }

        }

        _CurrentPalier = Mathf.Clamp(_CurrentPalier, 0, 99999999);

    }

    void Debug()
    {
        GameObject.Find("Debug").GetComponent<Text>().text = "Speed : " + (_Speed + _SpeedIncrease).ToString() +
            "\nPalier : " + _CurrentPalier + " " + _Palliers[_CurrentPalier] + "-" + _Palliers[_CurrentPalier + 1] +
            "\nDegradation : " + _DegradationTimer + "/" + _SpeedDegradationTimer;
    }


    



    public void StickToGround()
    {
        RaycastHit _hit;

        if (Physics.Raycast(transform.position, -transform.up, out _hit))
        {
            if (_hit.transform.tag == "Ground")
            {
               
                transform.position -= new Vector3(0, _hit.distance - _Collider.bounds.extents.y, 0);

            }
        }
    }

    public void GetAllObstacles()
    {
        ObstaclesBehaviour[] _O = FindObjectsOfType<ObstaclesBehaviour>();
        _AllObstacles = new List<ObstaclesBehaviour>();
        _AllObstacles = _O.ToList();
    }

    public void Reveal()
    {

        foreach(ObstaclesBehaviour _obs in _AllObstacles)
        {
            if (Vector3.Distance(transform.position, _obs.transform.position) < (_CurrentPalier + 1) * 100)
            {
                _obs._IsInRange = true;
            }
        }


    }



}
