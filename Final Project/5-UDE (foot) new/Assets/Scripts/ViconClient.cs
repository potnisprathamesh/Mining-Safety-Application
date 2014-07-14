/**************************************************************************************/
/**************************************************************************************/
/**                                                                                  **/
/**                         DO NOT EDIT THIS SCRIPT!!                                **/
/**                                                                                  **/
/**************************************************************************************/
/**************************************************************************************/

// This class provides a method for obtaining Vicon data
// Directions:
// 		1. Attach to the GameObject representing the User

using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class ViconClient : MonoBehaviour {

	private string ViconServerIP = "127.0.0.1";
	private int ViconServerPort = 11001;
	
	public struct Translation_
	{
	    public float x;
	    public float y;
	    public float z;
	}
	
	public struct Quaternion_
	{
	    public float x;
	    public float y;
	    public float z;
	    public float w;
	}

	public struct Euler_ {
		public float h;
		public float p;
		public float r;
	}
	
	public class Body_
        {
            public string name;
            public Translation_ translation;
            public Quaternion_ quaternion;
			public Euler_ euler;

            public Body_() {
                name = "";
                translation = new Translation_();
                quaternion = new Quaternion_();
				euler = new Euler_();
            }

            public const int BufferSize = sizeof(int) + 32 + (sizeof(double)*7);
            public byte[] GetBytes()
            {
                byte[] buff = new byte[BufferSize];
                int pos = 0;
                char[] n = new char[32];
                name.CopyTo(0, n, 0, Math.Min(32, name.Length));
                for (pos = 0; pos < Math.Min(32, name.Length); pos++)
                {
                    byte[] chars = System.BitConverter.GetBytes(name[pos]);
                    if (chars.Length > 0)
                        buff[pos] = System.BitConverter.GetBytes(name[pos])[0];
                    else
                        buff[pos] = 0;
                }
                System.BitConverter.GetBytes((double)translation.x).CopyTo(buff, pos);
                pos += sizeof(double);
                System.BitConverter.GetBytes((double)translation.y).CopyTo(buff, pos);
                pos += sizeof(double);
                System.BitConverter.GetBytes((double)translation.z).CopyTo(buff, pos);
                pos += sizeof(double);
                System.BitConverter.GetBytes((double)quaternion.x).CopyTo(buff, pos);
                pos += sizeof(double);
                System.BitConverter.GetBytes((double)quaternion.y).CopyTo(buff, pos);
                pos += sizeof(double);
                System.BitConverter.GetBytes((double)quaternion.z).CopyTo(buff, pos);
                pos += sizeof(double);
                System.BitConverter.GetBytes((double)quaternion.w).CopyTo(buff, pos);
                pos += sizeof(double);

                return buff;
            }

            public void FromBytes(byte[] buf)
            {
                int pos = 0;
                int length = (int)System.BitConverter.ToInt32(buf, pos);
                pos += sizeof(int);
                name = System.Text.Encoding.ASCII.GetString(buf, pos, length);
                pos += 32;
                translation.x = (float)System.BitConverter.ToDouble(buf, pos);
                pos += sizeof(double);
                translation.y = (float)System.BitConverter.ToDouble(buf, pos);
                pos += sizeof(double);
                translation.z = (float)System.BitConverter.ToDouble(buf, pos);
                pos += sizeof(double);
                quaternion.x = (float)System.BitConverter.ToDouble(buf, pos);
                pos += sizeof(double);
                quaternion.y = (float)System.BitConverter.ToDouble(buf, pos);
                pos += sizeof(double);
                quaternion.z = (float)System.BitConverter.ToDouble(buf, pos);
                pos += sizeof(double);
                quaternion.w = (float)System.BitConverter.ToDouble(buf, pos);
                pos += sizeof(double);
            }
        }
	
	private Socket viconSocket;
	private System.Collections.Generic.Dictionary<string, Body_> Bodies_ = new System.Collections.Generic.Dictionary<string, Body_>();
	private System.Collections.Generic.Dictionary<string, Body_> BodyAdjustments_ = new System.Collections.Generic.Dictionary<string, Body_>();
	
	public class ReceiveStateObject
	{
		public Socket sock = null;
		public int BufferSize;
		public byte[] buffer;
	}
		
	void Start() {

		// START EDITABLE SECTION: Here we declare a body adjustment for each Vicon tracker to address Blade's limitations for creating objects

		// Adjustment for Oculus Rift
		Body_ OculusRift = new Body_();
		OculusRift.name = "OculusRift";
		OculusRift.translation.x = -0.022f;
		OculusRift.translation.y = -0.095f;
		OculusRift.translation.z = -0.018f;
		OculusRift.euler.h = 342.5f;
		OculusRift.euler.p = 347.0f;
		OculusRift.euler.r = 196.5f;
		BodyAdjustments_[OculusRift.name] = OculusRift;

		// Adjustment for Right Wiimote
		Body_ RightWiimote = new Body_();
		RightWiimote.name = "RightWiimote";
		RightWiimote.translation.x = 0f;
		RightWiimote.translation.y = 0f;
		RightWiimote.translation.z = 0f;
		RightWiimote.euler.h = 90f;
		RightWiimote.euler.p = 5f;
		RightWiimote.euler.r = 90f;
		BodyAdjustments_[RightWiimote.name] = RightWiimote;
		
		// Adjustment for Left Wiimote
		Body_ LeftWiimote = new Body_();
		LeftWiimote.name = "LeftWiimote";
		LeftWiimote.translation.x = 0f;
		LeftWiimote.translation.y = 0f;
		LeftWiimote.translation.z = 0f;
		LeftWiimote.euler.h = 90f;
		LeftWiimote.euler.p = -10f;
		LeftWiimote.euler.r = 74f;
		BodyAdjustments_[LeftWiimote.name] = LeftWiimote;

		// Adjustment for Right Shoe (Men's size 9)
		Body_ RightShoe_M9 = new Body_();
		RightShoe_M9.name = "RightShoe_M9";
		RightShoe_M9.translation.x = -0.02f;//-0.19f;//-0.002f;
		RightShoe_M9.translation.y = -0.047f;//-0.047f;//0f;	//-0.45
		RightShoe_M9.translation.z = -0.010f;//-0.25f;//0.015f;
		//*
		RightShoe_M9.euler.h = 89f;		//95
		RightShoe_M9.euler.p = 359f;		//0
		RightShoe_M9.euler.r = 241f;	//240
		/**/
		/*
		RightShoe_M9.euler.h = 0f;		//95
		RightShoe_M9.euler.p = 0f;		//0
		RightShoe_M9.euler.r = 0f;	//240
		/**/
		BodyAdjustments_[RightShoe_M9.name] = RightShoe_M9;

		// Adjustment for Left Shoe (Men's size 9)
		Body_ LeftShoe_M9 = new Body_();
		LeftShoe_M9.name = "LeftShoe_M9";
		LeftShoe_M9.translation.x = -0.013f;
		LeftShoe_M9.translation.y = -0.048f;
		LeftShoe_M9.translation.z = -0.013f;
		//*
		LeftShoe_M9.euler.h = 92.5f;			//89
		LeftShoe_M9.euler.p = 0f;			//0
		LeftShoe_M9.euler.r = 117.5f;			//118
		/**/
		/*
		LeftShoe_M9.euler.h = 0f;			//89
		LeftShoe_M9.euler.p = 0f;			//0
		LeftShoe_M9.euler.r = 0f;			//118
		/**/
		BodyAdjustments_[LeftShoe_M9.name] = LeftShoe_M9;
		
		// Adjustment for BaseballGlove
		Body_ BaseballGlove = new Body_();
		BaseballGlove.name = "BaseballGlove";
		BaseballGlove.translation.x = 0.055f;
		BaseballGlove.translation.y = -0.023f;
		BaseballGlove.translation.z = 0f;
		BaseballGlove.euler.h = 106f;
		BaseballGlove.euler.p = 354f;
		BaseballGlove.euler.r = 40f;
		BodyAdjustments_[BaseballGlove.name] = BaseballGlove;

		// END EDITABLE SECTION

		viconSocket = new Socket(AddressFamily.InterNetwork, 
                    SocketType.Stream, ProtocolType.Tcp );
		//Debug.Log ("Trying to open a connection to: " + ViconServerIP);
        IPAddress ipAddress;
		IPAddress.TryParse (ViconServerIP, out ipAddress);
        
		System.IAsyncResult result = viconSocket.BeginConnect(ipAddress, ViconServerPort, null, null);
		result.AsyncWaitHandle.WaitOne(1000, true);
		if(!viconSocket.Connected)
		{
			//Debug.Log ("Closing the socket");
			viconSocket.Close ();
			Destroy (this);
			return;
		}
		
		ReceiveStateObject state = new ReceiveStateObject();
		state.sock = viconSocket;
		state.BufferSize = Body_.BufferSize;
		state.buffer = new byte[state.BufferSize];
		viconSocket.BeginReceive(state.buffer, 0, state.BufferSize, 0, new System.AsyncCallback(ReceiveCallback), state);
	}
	
	System.Threading.Mutex StateMutex = new System.Threading.Mutex();
	
	void Update() {
		StateMutex.WaitOne();
		if(Bodies_ != null && BodyAdjustments_ != null)
		{
			foreach(System.Collections.Generic.KeyValuePair<string,Body_> kv in Bodies_)
			{
				bool adjustmentMade = false;
				foreach(System.Collections.Generic.KeyValuePair<string,Body_> adjustment in BodyAdjustments_) {
					if(kv.Key == adjustment.Key) {
						InputBroker.SetPosition(kv.Key, new Vector3(
							kv.Value.translation.x + adjustment.Value.translation.x,
							kv.Value.translation.y + adjustment.Value.translation.y,
							kv.Value.translation.z + adjustment.Value.translation.z));
						Quaternion q = new Quaternion(kv.Value.quaternion.x, kv.Value.quaternion.y, kv.Value.quaternion.z, kv.Value.quaternion.w) *
							Quaternion.Euler(adjustment.Value.euler.p, adjustment.Value.euler.h, adjustment.Value.euler.r);
						InputBroker.SetRotation(kv.Key, q);
						adjustmentMade = true;
					}
					if(!adjustmentMade) {
						InputBroker.SetPosition(kv.Key, new Vector3(kv.Value.translation.x, kv.Value.translation.y, kv.Value.translation.z));
						Quaternion q = new Quaternion(kv.Value.quaternion.x, kv.Value.quaternion.y, kv.Value.quaternion.z, kv.Value.quaternion.w);
						InputBroker.SetRotation(kv.Key, q);
					}
				}
			}
		}
		StateMutex.ReleaseMutex();
	}
	
	private void ReceiveCallback( System.IAsyncResult ar )
	{
		ReceiveStateObject state = (ReceiveStateObject) ar.AsyncState;
		byte[] buff = state.buffer;
		
		try{
			Body_ b = new Body_();
			b.FromBytes(buff);
			//Debug.Log ("Received " + b.name + " translation, (x,y,z)=" + b.translation.x + "," + b.translation.y + "," + b.translation.z + "\n");
			StateMutex.WaitOne();
			Bodies_[b.name] = b;
			StateMutex.ReleaseMutex();
		}
		catch(Exception ex)
		{
			Debug.Log ("Caught: " + ex.Message);
		}
		viconSocket.BeginReceive(state.buffer, 0, state.BufferSize, 0, new System.AsyncCallback(ReceiveCallback), state);
	}
}
