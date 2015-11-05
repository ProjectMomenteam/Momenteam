using UnityEngine;
using System.Collections;

public class CharacterSound : MonoBehaviour {

	private Rigidbody _rigidbody;

	public FMODAsset Asset;

	private FMOD.Studio.EventInstance _event;
	private FMOD.Studio.EventInstance _musique;
	private FMOD.Studio.ParameterInstance _Change;
	private FMOD.Studio.ParameterInstance _IAirZic;
	private FMOD.Studio.ParameterInstance _InAir;
	private FMOD.Studio.ParameterInstance _Vitesse;
	private FMOD.Studio.ParameterInstance _Braquage;
	private float _braq = 1;

	private float AirTime;

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody>();

		_musique = FMOD_StudioSystem.instance.GetEvent ("event:/MusicFinal");
		_musique.getParameter ("Change", out _Change);
		_musique.getParameter ("Air", out _IAirZic);
		_musique.start();

		_event = FMOD_StudioSystem.instance.GetEvent (Asset);
		_event.getParameter ("InAir", out _InAir);
		_event.getParameter ("Vitesse", out _Vitesse);
		_event.getParameter ("Braquage", out _Braquage);
		_event.start();
	}
	
	// Update is called once per frame
	void Update () {
		_Vitesse.setValue(_rigidbody.velocity.magnitude * 0.01f);

		if(Input.GetAxis("Horizontal") < 0){
			_braq -= Input.GetAxis("Horizontal") * Time.deltaTime * 0.5f;
		}
		if(Input.GetAxis("Horizontal") > 0){
			_braq += Input.GetAxis("Horizontal") * Time.deltaTime * 0.5f;
		}
		if(Input.GetAxis("Horizontal") == 0){
			if(_braq > 1){_braq -= Time.deltaTime * 2f;}
			if(_braq < 1){_braq += Time.deltaTime * 2f;}
		}
		_Braquage.setValue(_braq);

		if(Physics.Raycast(transform.position, Vector3.down, 1.1f)){
			_InAir.setValue(0);
			_IAirZic.setValue(0);
			if(AirTime > 0.25f){
				AirTime -= Time.deltaTime;
			}
			else{
				AirTime = 0;
			}
		}
		else{
			if(AirTime > 0.25f){
				_InAir.setValue(1);
				_IAirZic.setValue(1);
				AirTime = 0.5f;
			}
			else{
				AirTime += Time.deltaTime;
			}
		}
	}
}
