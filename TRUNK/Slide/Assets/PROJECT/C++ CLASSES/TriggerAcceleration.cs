using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class TriggerAcceleration : MonoBehaviour {

    public float _VitesseAugment = 1;


    private GameObject _Player;
    private Rigidbody _PlayerRigidbody;




	// Use this for initialization
	void Awake ()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _PlayerRigidbody = _Player.GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void FixedUpdate()
    {
        
    }

    void OnTriggerStay(Collider _Coll)
    {

        if(_Player.GetComponent<Move>()._AscendValue < 0)
        {
            _Player.GetComponent<Move>()._DegradationTimer = 0;
            _Player.GetComponent<Move>()._SpeedIncrease += _VitesseAugment * Time.deltaTime  * -_Player.GetComponent<Move>()._AscendValue * 5;
        }
    }


    void OnSceneGUI()
    {
        Handles.Label(transform.position, "YOLO");
    }

}
