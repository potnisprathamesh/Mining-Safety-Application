using UnityEngine;
using System.Collections;

public class AuditoryInstruction : MonoBehaviour {
	public GameObject truck;
	public AudioClip clip;
	private AudioSource audio;
	private float startTime;
	private float timeCount;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		audio = this.gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance(truck.transform.position, transform.position);
		timeCount = Time.time - startTime;

		if (dist > 20F && timeCount >= 15F){ //Plays every 15 seconds if user is away from area
			audio.Stop();
			startTime = Time.time;
			audio.PlayOneShot(clip);
		}
	
	}
}
