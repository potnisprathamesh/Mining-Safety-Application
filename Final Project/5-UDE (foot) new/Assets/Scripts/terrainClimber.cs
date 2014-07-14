using UnityEngine;
using System.Collections;

public class terrainClimber : MonoBehaviour {
	
	public GameObject head,terrain,truck,RMote,placeHolder,user,plane;
	public RaycastHit objInfo;
	public Vector3 direction;
	public float firstx,firstz,secondx,secondz;


	void Start () {
		firstx = RMote.transform.position.x;
		firstz = RMote.transform.position.z;
		secondx = RMote.transform.position.x;
		secondz = RMote.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {

		secondx = RMote.transform.position.x;
		secondz = RMote.transform.position.z;
		if (firstx != secondx || firstz != secondz) 
		{
			direction.x = secondx - firstx;
			direction.z = secondz - firstz;
			direction.y = 0;

			Debug.DrawRay(head.transform.position,-direction,Color.green);
			if (Physics.Raycast (RMote.transform.position, direction, out objInfo, 1f)) 
			{
				if(objInfo.transform.name.IndexOf ("Plane") > -1)	;
				else
				{
					terrain.transform.position += direction;
					truck.transform.position += direction;
					placeHolder.transform.position += direction;
					plane.transform.position += direction;
				}
			}
		}
		firstx = secondx;
		firstz = secondz;
	}
}