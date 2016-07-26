/*
Written by Peter Larson in 2016
*/

using UnityEngine;
using System.Collections;


/// <summary>
/// This class is an example of a script that can be used with the cve-unity bridge. 
/// This class should be attached to the game object that it will effect, additionally
/// it must implement the AbstractNetworkingChildScript abstract class. 
/// </summary>
public class SphereChildScript : AbstractNetworkingChildScript {

    /// <summary>
    ///  This variable can be used as a multiplier to the location data recieved. 
    /// </summary>
    public float locationMultiplier = 1.0f;
    /// <summary>
    /// This variable can be used as a multiplier to the rotation data recieved. 
    /// </summary>
    public float rotationMultiplier = 1.0f;

    /// <summary>
    /// lastOrientation is used for the rotation transformations implemented in this example. 
    /// </summary>
    private float[] lastOrientation = new float[3];
    /// <summary>
    /// lastLocation is used for the location transformations implemented in this example. 
    /// </summary>
    private float[] lastLocation = new float[3];

    private bool locationHasBeenSet = false;
    private int timerInt = 0;
    public int sendPositionInt = 0;

    /// <summary>
    /// This method is called by unity when the game starts. This is where initalization code should go. 
    /// Here Start() contains initial values for the lastOrientation and lastLocation arrays. 
    /// </summary>
    void Start () {
        lastOrientation[0] = 0.0f;
        lastOrientation[1] = 0.0f;
        lastOrientation[2] = 0.0f;

        lastLocation[0] = 0.0f;
        lastLocation[1] = 0.0f;
        lastLocation[2] = 0.0f;

        
    }
	
	/// <summary>
    /// This method is for operations that should happen once per frame. 
    /// There is no code in this method because all updates are done asynchronously as information from the cve-unity bridge comes in. 
    /// It is possible to change this by storing locations and orientations, then using them in the update() method to perform whatever is desired. 
    /// </summary>
    void Update () {
        if (this.sendPositionInt == 1)
        {

            if (this.timerInt < 60)
            {
                this.timerInt++;
            }
            else
            {
                this.timerInt = 0;
                base.sendLocation(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                UnityEngine.Debug.Log("Tried to send location");
            }
        }
	}

    /// <summary>
    /// This method is an overrided abstract method in the base class. This recieves locations from the CVE server. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public override void setLocation(float x, float y, float z)
    {
        if (!locationHasBeenSet)
        {
            this.lastLocation[0] = x;
            this.lastLocation[1] = y;
            this.lastLocation[2] = z;
            this.locationHasBeenSet = true;
        }
        else
        {
            Vector3 newPos = new Vector3((x- this.lastLocation[0]) * locationMultiplier, (y - this.lastLocation[1]) * locationMultiplier, (z - this.lastLocation[2]) * locationMultiplier);
            this.lastLocation[0] = x;
            this.lastLocation[1] = y;
            this.lastLocation[2] = z;
            transform.localPosition = newPos;
            UnityEngine.Debug.Log("Position set to: " + newPos.ToString());
        }




    }

    /// <summary>
    /// This method is an overrided abstract method in the base class. This recieves orientations from the CVE server. 
    /// </summary>
    /// <param name="roll"></param>
    /// <param name="pitch"></param>
    /// <param name="yaw"></param>
    public override void setOrientation(float roll, float pitch, float yaw)
    {

        UnityEngine.Debug.Log("Orientation recieved was: roll" + roll + " pitch: " + pitch + " yaw: " + yaw);
        //This doesn't work. This code was the start of a transformation from roll-pitch-yaw to 
        //a format used by unity, but I could not get it right. The code below works if there are no location changes, but 
        //The combination of location and orientation was causing incorrect locations to appear. 

        //// roll
        //transform.Rotate(0f, 0f, roll - lastOrientation[0], Space.Self
        //// pitch
        //transform.Rotate(pitch - lastOrientation[1], 0f, 0f, Space.Self
        //// yaw
        //transform.Rotate(0f, yaw - lastOrientation[2], 0f, Space.Self
        //lastOrientation[0] = roll;
        //lastOrientation[1] = pitch;
        //lastOrientation[2] = yaw;
    }

    /// <summary>
    /// This method is an overrided abstract method in the base class. This recieves Positions from the CVE server. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="roll"></param>
    /// <param name="pitch"></param>
    /// <param name="yaw"></param>
    public override void setPosition(float x, float y, float z, float roll, float pitch, float yaw)
    {
        setLocation(x, y, z);
        setOrientation(roll, pitch, yaw);
    }

    /// <summary>
    /// This method is an overrided abstract method in the base class. This recieves ExtraParameters from the CVE server. 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public override void setExtraParam(string name, float value)
    {
        UnityEngine.Debug.LogError("Extra Parameter recieved by object, but not implemented for use. Message is: " + name);
    }
}
