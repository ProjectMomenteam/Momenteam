using UnityEngine;
using System.Collections;

public class ObstaclesBehaviour : MonoBehaviour {

    public int _PallierLimite = 0;

    [HideInInspector] public bool _IsActivate;

    public bool _IsInRange = false;

    private Vector3 _InitialPos;

	// Use this for initialization
	void Awake ()
    {
        _InitialPos = transform.position;
        transform.position -= new Vector3(0, GetComponent<Collider>().bounds.size.y * 1.5f, 0);

	}
	
	// Update is called once per frame
	void Update ()
    {

        if (_IsInRange)
        {
            transform.position = Vector3.Lerp(transform.position, _InitialPos, Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _InitialPos - new Vector3(0, GetComponent<Collider>().bounds.size.y * 1.5f, 0), Time.deltaTime * 2);
        }



        _IsInRange = false;
	}


}
