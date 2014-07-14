using UnityEngine;
using System.Collections;

public class placeLocater : MonoBehaviour {

	public GameObject user, RMote, LMote,leftLeg,rightLeg;
	public GameObject terrain,truck,placeHolder,plane;
	public GameObject lgrabbedRung,rgrabbedRung;
	public RaycastHit objInfo;
	public Vector3 direction,handDirection;
	public bool ready,rightUp,leftUp;
	public string RWiimoteName = "RightWiimote";
	public string LWiimoteName = "LeftWiimote";
	public float leftfirsty,leftsecondy,rightfirsty,rightsecondy, leftDisplacement, rightDisplacement;

	void Start () 
	{
		direction.x = 0;
		direction.z = 0;
		direction.y = -1;

		leftfirsty = LMote.transform.position.y;
		leftsecondy = LMote.transform.position.y;
		rightfirsty = RMote.transform.position.y;
		rightsecondy = RMote.transform.position.y;

		rightUp = false;
		leftUp = false;

		RMote.transform.collider.isTrigger = true;
		LMote.transform.collider.isTrigger = true;
		RMote.transform.rigidbody.isKinematic = true;
		LMote.transform.rigidbody.isKinematic = true;

		rightLeg.gameObject.SetActive(false);
		leftLeg.gameObject.SetActive(false);
	}

	void Update () 
	{
		leftfirsty = leftsecondy;
		leftsecondy = LMote.transform.position.y;
		rightfirsty = rightsecondy;
		rightsecondy = RMote.transform.position.y;

		if (leftCollision.collided) 
		{
			if (InputBroker.GetKeyDown (LWiimoteName + ":A") && InputBroker.GetKeyDown (LWiimoteName + ":B"))
			{
				lgrabbedRung = leftCollision.collidedObject;
				Debug.Log ("left grabbed - " + leftCollision.collidedObject.name);
				leftCollision.grabbed = true;
				leftLeg.gameObject.SetActive(true);
				handDirection = leftLeg.transform.position;
				handDirection.y = leftsecondy - 0.6102f;
				leftLeg.transform.position = handDirection;

				if(leftsecondy != leftfirsty)
					leftUp = true;
			}
			else
			{
				leftCollision.grabbed = false;
				lgrabbedRung = null;
				leftLeg.gameObject.SetActive(false);
			}
		}
		else
		{
			leftCollision.grabbed = false;
			lgrabbedRung = null;
			leftLeg.gameObject.SetActive(false);
		}


		if (rightCollision.collided) 
		{
			if (InputBroker.GetKeyDown (RWiimoteName + ":A") && InputBroker.GetKeyDown (RWiimoteName + ":B"))
			{
				rgrabbedRung = rightCollision.collidedObject;
				Debug.Log ("right grabbed - " + rightCollision.collidedObject.name);
				rightCollision.grabbed = true;
				rightLeg.gameObject.SetActive(true);

				handDirection = rightLeg.transform.position;
				handDirection.y = rightsecondy - 0.6102f;
				rightLeg.transform.position = handDirection;

				if(rightsecondy != rightfirsty)
					rightUp = true;
			}
			else
			{
				rightCollision.grabbed = false;
				rgrabbedRung = null;
				rightLeg.gameObject.SetActive(false);
			}
		}
		else
		{
			rightCollision.grabbed = false;
			rgrabbedRung = null;
			rightLeg.gameObject.SetActive(false);
		}

		if (rightUp && leftUp) 
		{
			leftDisplacement = leftfirsty - leftsecondy;
			rightDisplacement = rightfirsty - rightsecondy;
			if ((leftDisplacement - rightDisplacement) >= 1 || (leftDisplacement - rightDisplacement) <= -1) 
			{

				Debug.Log ("falling here");

				rgrabbedRung = null;
				rightCollision.grabbed = false;
				lgrabbedRung = null;
				leftCollision.grabbed = false;

				handDirection = placeHolder.transform.position;
				handDirection.y = -0.98f;
				placeHolder.transform.position = handDirection;
				
				handDirection = terrain.transform.position;
				handDirection.y = 0;
				terrain.transform.position = handDirection;
				
				handDirection = truck.transform.position;
				handDirection.y = -0.06663513f;
				truck.transform.position = handDirection;
				
				plane.gameObject.SetActive(true);
				
				handDirection = user.transform.position;
				handDirection.z -= 0.98f;
				handDirection.z = 1.816118f;

				plane.transform.position = handDirection;

				rightLeg.gameObject.SetActive(false);
				leftLeg.gameObject.SetActive(false);
			} 
			else 
			{
				handDirection.y = leftDisplacement;
				handDirection.x = 0;
				handDirection.z = 0;

				placeHolder.transform.position -= handDirection;
				terrain.transform.position -= handDirection;
				truck.transform.position -= handDirection;
			}
		} 
		else if(!rightCollision.grabbed && !leftCollision.grabbed)
		{
			Debug.Log ("falling here big time");

			if(terrain.transform.position.y != 0)
			{
				plane.gameObject.SetActive(true);

				handDirection = user.transform.position;
				handDirection.z -= 0.98f;
				handDirection.z = 1.816118f;
				plane.transform.position = handDirection;
			}
			rgrabbedRung = null;
			lgrabbedRung = null;

			handDirection = placeHolder.transform.position;
			handDirection.y = -0.98f;
			placeHolder.transform.position = handDirection;
			
			handDirection = terrain.transform.position;
			handDirection.y = 0;
			terrain.transform.position = handDirection;
			
			handDirection = truck.transform.position;
			handDirection.y = -0.06663513f;
			truck.transform.position = handDirection;

			rightLeg.gameObject.SetActive(false);
			leftLeg.gameObject.SetActive(false);
		}
		rightUp = false;
		leftUp = false;
	}
}