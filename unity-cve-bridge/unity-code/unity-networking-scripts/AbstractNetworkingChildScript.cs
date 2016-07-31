/*
Written by Peter Larson in 2016
*/

using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// This class is used by Networking Child Scripts. One example is provided named "NetworkingChildScriptExample", 
/// but the programmer is encouraged to implement more for different games. 
/// 
/// This class contains some code for communication with the NetworkingScript, as well as method signatures. 
/// 
/// The code below should not need to be changed. All modifications should be made in classes that implement this
/// method, but there are comments here so that it is easy to understand. 
/// </summary>
public abstract class AbstractNetworkingChildScript : MonoBehaviour {

    /// <summary>
    /// This is a reference to the parent NetworkingScript object. This object will make calls to this class when
    /// new data comes from the server, and messages to be sent out go through this as well. 
    /// </summary>
    private NetworkingScript parentScript = null;

    /// <summary>
    /// This string is a local copy of the channel that is assigned to this class. This variable should not be changed. 
    /// </summary>
    private String channel;

    /// <summary>
    /// This method is used to provide the channel, and a refernece to the parent NetworkingScript. This probably 
    /// should not be overridden in child classes. 
    /// This method is used by NetworkingScript. 
    /// </summary>
    /// <param name="script"> Parent Script</param>
    /// <param name="channel">Channel that this object is on. </param>
    public void setupToSend(NetworkingScript script, String channel)
    {
        this.parentScript = script;
        this.channel = channel;
    }

    /// <summary>
    /// This method allows this object to send location information to the CVE server. 
    /// Classes that extend this class should call this method through: base.sendLocation(x,y,z)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void sendLocation(float x, float y, float z)
    {
        if(this.parentScript == null)
        {
            UnityEngine.Debug.LogWarning("Location not sent in AbstractNetworkingChildScript because of Initalization problem");
        }
        else
        {
            this.parentScript.sendLocation(x, y, z, this.channel);
        }

    }

    /// <summary>
    /// This method allows this object to send orientation information to the CVE server. 
    /// Classes that extend this class should call this method through: base.sendOrientation(roll,pitch,yaw)
    /// </summary>
    /// <param name="roll"></param>
    /// <param name="pitch"></param>
    /// <param name="yaw"></param>
    public void sendOrientation(float roll, float pitch, float yaw)
    {
        if (this.parentScript == null)
        {
            UnityEngine.Debug.LogWarning("Orientation not sent in AbstractNetworkingChildScript because of Initalization problem");
        }
        else
        {
            this.parentScript.sendOrientation(roll,pitch,yaw, this.channel);
        }
    }

    /// <summary>
    /// This method allows this object to send position information to the CVE server. 
    /// Classes that extend this class should call this method through: base.sendPosition(x,y,z,roll,pitch,yaw)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="roll"></param>
    /// <param name="pitch"></param>
    /// <param name="yaw"></param>
    public void sendPosition(float x, float y, float z, float roll, float pitch, float yaw)
    {
        if (this.parentScript == null)
        {
            UnityEngine.Debug.LogWarning("Position not sent in AbstractNetworkingChildScript because of Initalization problem");
        }
        else
        {
            this.parentScript.sendPosition(x, y, z, roll, pitch, yaw, this.channel);
        }
    }

    /// <summary>
    /// This method allows this object to send Extra Parameters to the CVE server. 
    /// Classes that extend this class should call this method through: base.sendExtraParam(name,value)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void sendExtraParam(string name, float value)
    {
        if (this.parentScript == null)
        {
            UnityEngine.Debug.LogWarning("Extra Param not sent in AbstractNetworkingChildScript because of Initalization problem");
        }
        else
        {
            this.parentScript.sendExtraParam(name, value, this.channel);
        }
    }

    /// <summary>
    /// This method must be implemented in the extending class. It is used to recieve this information from the server. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public abstract void setLocation(float x, float y, float z);

    /// <summary>
    /// This method must be implemented in the extending class. It is used to recieve this information from the server. 
    /// </summary>
    /// <param name="roll"></param>
    /// <param name="pitch"></param>
    /// <param name="yaw"></param>
    public abstract void setOrientation(float roll, float pitch, float yaw);

    /// <summary>
    /// This method must be implemented in the extending class. It is used to recieve this information from the server. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="roll"></param>
    /// <param name="pitch"></param>
    /// <param name="yaw"></param>
    public abstract void setPosition(float x, float y, float z, float roll, float pitch, float yaw);

    /// <summary>
    /// This method must be implemented in the extending class. It is used to recieve this information from the server. 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public abstract void setExtraParam(string name, float value);
}
