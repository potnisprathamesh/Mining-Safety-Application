using UnityEngine;
using System.Collections;

// This class provides an easy method of controlling a GameObject with a Vicon tracker
// Directions:
// 		1. Attach to the GameObject that will be tracked
//		2. Change ViconName to the name of the Vicon object that is being tracked
public class Tracker : MonoBehaviour {
	
	public string ViconName = "RightWiimote";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// Set rotation and position of object to match Vicon tracker
		transform.localRotation = InputBroker.GetRotation(ViconName);
		transform.localPosition = InputBroker.GetPosition(ViconName);

	}
}
