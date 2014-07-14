using UnityEngine;
using System.Collections;

// This class provides an interface for simulating Vicon and Wiimote input
// Directions:
// 		1. Attach to the GameObject representing the User
//		2. Manipulate the public variables in the Inspector before or during Play
// Note:
//		In the MOCAP Lab, you must disable the InputSimulator or it will override the 
//		actual Vicon and Wiimote data.
public class InputSimulator : MonoBehaviour {

	// Transformation values for Vicon OculusRift
	private string OculusRiftName = "OculusRift";
	public float OculusRiftX = 0.0f;
	public float bothHandsY = 0.0f;
	public float OculusRiftY = 1.75f;
	public float OculusRiftZ = 0.0f;
	public float OculusRiftH = 0.0f;
	public float OculusRiftP = 15.0f;
	public float OculusRiftR = 0.0f;

	// Transformation values for Vicon RightWiimote
	private string RightWiimoteName = "RightWiimote";
	public float RightWiimoteX = 0.25f;
	public float RightWiimoteY = 1.0f;
	public float RightWiimoteZ = 0.5f;
	public float RightWiimoteH = 0.0f;
	public float RightWiimoteP = 0.0f;
	public float RightWiimoteR = 0.0f;
	public bool RightWiimoteUp = false;
	public bool RightWiimoteDown = false;
	public bool RightWiimoteLeft = false;
	public bool RightWiimoteRight = false;
	public bool RightWiimoteA = false;
	public bool RightWiimoteB = false;
	public bool RightWiimoteMinus = false;
	public bool RightWiimotePlus = false;
	public bool RightWiimote1 = false;
	public bool RightWiimote2 = false;
	
	// Transformation values for Vicon LeftWiimote
	private string LeftWiimoteName = "LeftWiimote";
	public float LeftWiimoteX = -0.25f;
	public float LeftWiimoteY = 1.0f;
	public float LeftWiimoteZ = 0.5f;
	public float LeftWiimoteH = 0.0f;
	public float LeftWiimoteP = 0.0f;
	public float LeftWiimoteR = 0.0f;
	public bool LeftWiimoteUp = false;
	public bool LeftWiimoteDown = false;
	public bool LeftWiimoteLeft = false;
	public bool LeftWiimoteRight = false;
	public bool LeftWiimoteA = false;
	public bool LeftWiimoteB = false;
	public bool LeftWiimoteMinus = false;
	public bool LeftWiimotePlus = false;
	public bool LeftWiimote1 = false;
	public bool LeftWiimote2 = false;

	// Transformation values for Vicon RightShoe
	private string RightShoeName = "RightShoe_M9";
	public float RightShoeX = 0.25f;
	public float RightShoeY = 0.0f;
	public float RightShoeZ = 0.0f;
	public float RightShoeH = 0.0f;
	public float RightShoeP = 0.0f;
	public float RightShoeR = 0.0f;

	// Transformation values for Vicon LeftShoe
	private string LeftShoeName = "LeftShoe_M9";
	public float LeftShoeX = -0.25f;
	public float LeftShoeY = 0.0f;
	public float LeftShoeZ = 0.0f;
	public float LeftShoeH = 0.0f;
	public float LeftShoeP = 0.0f;
	public float LeftShoeR = 0.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		// Update InputBroker's OculusRift
		InputBroker.SetPosition(OculusRiftName, new Vector3(OculusRiftX, OculusRiftY, OculusRiftZ));
		Quaternion OculusRiftQuaternion = Quaternion.Euler(OculusRiftP, OculusRiftH, OculusRiftR);
		InputBroker.SetRotation(OculusRiftName, OculusRiftQuaternion);


		RightWiimoteY += bothHandsY;
		// Update InputBroker's RightWiimote
		InputBroker.SetPosition(RightWiimoteName, new Vector3(RightWiimoteX, RightWiimoteY, RightWiimoteZ));
		Quaternion RightWiimoteQuaternion = Quaternion.Euler(RightWiimoteP, RightWiimoteH, RightWiimoteR);
		InputBroker.SetRotation(RightWiimoteName, RightWiimoteQuaternion);
		// And buttons
		InputBroker.SetKey(RightWiimoteName + ":Up", RightWiimoteUp);
		InputBroker.SetKey(RightWiimoteName + ":Down", RightWiimoteDown);
		InputBroker.SetKey(RightWiimoteName + ":Left", RightWiimoteLeft);
		InputBroker.SetKey(RightWiimoteName + ":Right", RightWiimoteRight);
		InputBroker.SetKey(RightWiimoteName + ":A", RightWiimoteA);
		InputBroker.SetKey(RightWiimoteName + ":B", RightWiimoteB);
		InputBroker.SetKey(RightWiimoteName + ":Minus", RightWiimoteMinus);
		InputBroker.SetKey(RightWiimoteName + ":Plus", RightWiimotePlus);
		InputBroker.SetKey(RightWiimoteName + ":One", RightWiimote1);
		InputBroker.SetKey(RightWiimoteName + ":Two", RightWiimote2);

		LeftWiimoteY += bothHandsY;

		bothHandsY = 0;
		// Update InputBroker's LeftWiimote
		InputBroker.SetPosition(LeftWiimoteName, new Vector3(LeftWiimoteX, LeftWiimoteY, LeftWiimoteZ));
		Quaternion LeftWiimoteQuaternion = Quaternion.Euler(LeftWiimoteP, LeftWiimoteH, LeftWiimoteR);
		InputBroker.SetRotation(LeftWiimoteName, LeftWiimoteQuaternion);
		// And buttons
		InputBroker.SetKey(LeftWiimoteName + ":Up", LeftWiimoteUp);
		InputBroker.SetKey(LeftWiimoteName + ":Down", LeftWiimoteDown);
		InputBroker.SetKey(LeftWiimoteName + ":Left", LeftWiimoteLeft);
		InputBroker.SetKey(LeftWiimoteName + ":Right", LeftWiimoteRight);
		InputBroker.SetKey(LeftWiimoteName + ":A", LeftWiimoteA);
		InputBroker.SetKey(LeftWiimoteName + ":B", LeftWiimoteB);
		InputBroker.SetKey(LeftWiimoteName + ":Minus", LeftWiimoteMinus);
		InputBroker.SetKey(LeftWiimoteName + ":Plus", LeftWiimotePlus);
		InputBroker.SetKey(LeftWiimoteName + ":One", LeftWiimote1);
		InputBroker.SetKey(LeftWiimoteName + ":Two", LeftWiimote2);
		
		// Update InputBroker's RightShoe
		InputBroker.SetPosition(RightShoeName, new Vector3(RightShoeX, RightShoeY, RightShoeZ));
		Quaternion RightShoeQuaternion = Quaternion.Euler(RightShoeP, RightShoeH, RightShoeR);
		InputBroker.SetRotation(RightShoeName, RightShoeQuaternion);
		
		// Update InputBroker's LeftShoe
		InputBroker.SetPosition(LeftShoeName, new Vector3(LeftShoeX, LeftShoeY, LeftShoeZ));
		Quaternion LeftShoeQuaternion = Quaternion.Euler(LeftShoeP, LeftShoeH, LeftShoeR);
		InputBroker.SetRotation(LeftShoeName, LeftShoeQuaternion);

	}
}
