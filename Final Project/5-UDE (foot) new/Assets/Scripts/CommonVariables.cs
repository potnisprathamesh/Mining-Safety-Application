using UnityEngine;
using System.Collections;

// This class provides a means of using global variables among other scripts
public class CommonVariables {

	static public Vector3 mappedPosition = new Vector3(0f, 0f, 0f);
	static public Vector3 mappedRotation = new Vector3(0f, 0f, 0f);
	static public float dynamicIPD = 0.064f;
}
