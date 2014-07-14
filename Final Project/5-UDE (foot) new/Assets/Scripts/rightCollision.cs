using UnityEngine;
using System.Collections;

public class rightCollision : MonoBehaviour {
	
	static public bool collided,grabbed;
	static public GameObject collidedObject;

	public AudioClip clip;
	private AudioSource source;
	
	void start()
	{
		grabbed = false;
		collided = false;
	}
	
	void update()
	{
		
	}
	
	void OnTriggerEnter(Collider other) {
		// If an object is not already grabbed, check for collisions with another
		
		Debug.Log ("obj - "+gameObject.name);
		if(grabbed == false && other.gameObject.name.IndexOf("rung") > -1) {
			
			collidedObject = other.gameObject;
			collided = true;
			Debug.Log ("with - "+other.gameObject.name);
		}
        if (other.gameObject.name == "Plane")
        {
			source = this.gameObject.AddComponent<AudioSource>();
            other.gameObject.SetActive(false);
            //leftGhost.transform.active = false;
			source.PlayOneShot(clip);
        }
	}
	
	void OnTriggerExit(Collider other) {
		// If an object is not grabbed, forget the collided object
		if(collidedObject != null) {
			collidedObject = null;
			collided = false;
		}
	}
}