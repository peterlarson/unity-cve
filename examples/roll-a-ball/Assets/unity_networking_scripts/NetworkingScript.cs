/*
Written by Peter Larson in 2016
*/
using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Text;
using FlatBuffers;
using Unity_CVE;


/// <summary>
/// This is the main networking class. This script handles communication with scripts attached to other objects in unity, 
/// Encoding and decoding from the FlatBuffers format, and UDP-based communication with the unity CVE server. This class
/// should not require modifications to use. 
/// 
/// To add this to your project, put this script on some empty object, and fill in the Server IP, Server Send Port, and 
/// Server Recieve Ports. 
/// To add game objects to channels, first add a script that extends the AbstractNetworkingChildScript to some game object, 
/// Then drag that object to the CH*_Game Obj public variable in the unity inspector. They you're good to go. 
/// 
/// The code will listen on the provided port for UDP packets with CVE data, and it will send to the specified server at the 
/// specified port. 
/// </summary>
public class NetworkingScript : MonoBehaviour {

    /// <summary>
    /// This class stores data for callbacks from listening to the open port. 
    /// </summary>
    private class UdpData {
        public IPEndPoint callbackEndpoint;
        public UdpClient callbackClient;
    }

    /// <summary>
    /// This class is a layer in between the NetworkingScript and a Game Object or a Game Object's child script
    /// Some error messages for incorrect initialization are included here. 
    /// </summary>
    private class ChildUpdateHolder
    {
        bool Active;
        GameObject childObject;
        AbstractNetworkingChildScript childScript;

        public ChildUpdateHolder()
        {
            this.Active = false;
        }
        public ChildUpdateHolder(GameObject passedObject)
        {
            if (passedObject != null)
            {
                this.Active = true;
                this.childObject = passedObject;
                this.childScript = (AbstractNetworkingChildScript)this.childObject.GetComponent<AbstractNetworkingChildScript>();
            }
            else
            {
                this.Active = false;
            }
        }

        public void configureToSendOnChannel(String channel, NetworkingScript sender)
        {
            if(this.childScript != null)
            {
                this.childScript.setupToSend(sender, channel);
            }
        }

        public void setExtraParam(string name, float value)
        {
            if (this.Active)
            {
                if (this.childScript != null)
                    this.childScript.setExtraParam(name, value);
                else
                    UnityEngine.Debug.LogWarning("Extra Param printed in NetworkingScript. AbstractNetworkingChildScript component missing in child. name: " + name);
            }
        }

        public void setLocation(float x, float y, float z)
        {
            UnityEngine.Debug.Log("CUH: Loc: " + x + y + z);
            if (this.Active)
            {
                if (this.childScript != null)
                    this.childScript.setLocation(x, y, z);
                else
                    this.childObject.transform.localPosition.Set(x, y, z);
            }
        }

        public void setOrientation(float roll, float pitch, float yaw)
        {
            if (this.Active)
            {
                if (this.childScript != null)
                    this.childScript.setOrientation(roll, pitch, yaw);
                else
                    UnityEngine.Debug.LogWarning("Orientation dropped because a child object was missing an AbstractNetworkingChildScript component.");
            }
        }

        public void setPosition(float x, float y, float z, float roll, float pitch, float yaw)
        {
            if (this.Active)
            {
                if (this.childScript != null)
                    this.childScript.setPosition(x, y, z, roll, pitch, yaw);
                else
                {
                    UnityEngine.Debug.LogWarning("Orientation in Position message dropped because a child object was missing an AbstractNetworkingChildScript component.");
                    this.childObject.transform.localPosition.Set(x, y, z);
                }
            }
        }
    }

    private UdpClient udpClientReceiver;
    private IPEndPoint RemoteIpEndPoint;
    private UdpClient udpClientSender;

    
    /// <summary>
    /// messageRecieved is used as a flag that lastMessage has a new message. 
    /// </summary>
    private static bool messageReceived = false;
    private Message lastMessage; 
    private static bool orientationReceived = false;
    private static bool locationReceived = false;
    private float[] orientationData = new float[3];
    private float[] locationData = new float[3];

    public String serverIP = "127.0.0.1";
    public int serverSendPort = 11000;
    public int serverRecievePort = 11001;

   // These should not be changed during runtime because in start() they are used to construct ChildUpdateHolder
   // Objects. 
    public GameObject CH0_GameObj;
    public GameObject CH1_GameObj;
    public GameObject CH2_GameObj;
    public GameObject CH3_GameObj;
    public GameObject CH4_GameObj;
    public GameObject CH5_GameObj;
    public GameObject CH6_GameObj;

    //These objects are created in the start() function. 
    private ChildUpdateHolder CH0_ChildUpdateHolder;
    private ChildUpdateHolder CH1_ChildUpdateHolder;
    private ChildUpdateHolder CH2_ChildUpdateHolder;
    private ChildUpdateHolder CH3_ChildUpdateHolder;
    private ChildUpdateHolder CH4_ChildUpdateHolder;
    private ChildUpdateHolder CH5_ChildUpdateHolder;
    private ChildUpdateHolder CH6_ChildUpdateHolder;

    void Start () {

        try
        {
            udpClientReceiver = new UdpClient(this.serverRecievePort);
            udpClientSender = new UdpClient();
            //TODO: Change IPAddress.Any to use this.serverIP
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            listenForMessage();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }

        //ForTest
        Byte[] test1 = new Byte[1];
        test1[0] = 1;

        this.CH0_ChildUpdateHolder = new ChildUpdateHolder(CH0_GameObj);
        this.CH1_ChildUpdateHolder = new ChildUpdateHolder(CH1_GameObj);
        this.CH2_ChildUpdateHolder = new ChildUpdateHolder(CH2_GameObj);
        this.CH3_ChildUpdateHolder = new ChildUpdateHolder(CH3_GameObj);
        this.CH4_ChildUpdateHolder = new ChildUpdateHolder(CH4_GameObj);
        this.CH5_ChildUpdateHolder = new ChildUpdateHolder(CH5_GameObj);
        this.CH6_ChildUpdateHolder = new ChildUpdateHolder(CH6_GameObj);

        this.CH0_ChildUpdateHolder.configureToSendOnChannel("CH0", this);
        this.CH1_ChildUpdateHolder.configureToSendOnChannel("CH1", this);
        this.CH2_ChildUpdateHolder.configureToSendOnChannel("CH2", this);
        this.CH3_ChildUpdateHolder.configureToSendOnChannel("CH3", this);
        this.CH4_ChildUpdateHolder.configureToSendOnChannel("CH4", this);
        this.CH5_ChildUpdateHolder.configureToSendOnChannel("CH5", this);
        this.CH6_ChildUpdateHolder.configureToSendOnChannel("CH6", this);


        //TIP: Below are examples of how you can manually send messages to the Unity CVE bridge. 
        //sendLocation(1, 2, 3, "CH0");
        //sendPosition(4, 5, 6, 7, 0, 9, "CH0");
        //sendOrientation(10, 11, 12, "CH0");
        //sendExtraParam("test", 13, "CH0");
    }

    /// <summary>
    /// This callback from when a message is recieved from the CVE Unity bridge. 
    /// </summary>
    /// <param name="result"></param>
    private void OnReceive(System.IAsyncResult result)
    {

        //string returnData = Encoding.ASCII.GetString(//receiveBytes);
        UdpClient client = (UdpClient)((UdpData)(result.AsyncState)).callbackClient;
        IPEndPoint endpoint = (IPEndPoint)((UdpData)(result.AsyncState)).callbackEndpoint;

        Byte[] receiveBytes = client.EndReceive(result, ref endpoint);

        //TODO: find what happens when an invalid message is recieved. 
        this.lastMessage = Unity_CVE.Message.GetRootAsMessage(new ByteBuffer(receiveBytes));


        messageReceived = true;

        listenForMessage();
    }


    /// <summary>
    /// This method begins listening for packets from the server. 
    /// </summary>
    private void listenForMessage()
    {
        UdpData callbackData = new UdpData();
        callbackData.callbackClient = udpClientReceiver;
        callbackData.callbackEndpoint = RemoteIpEndPoint;

        //UnityEngine.Debug.Log("Listening");
        udpClientReceiver.BeginReceive(new System.AsyncCallback(OnReceive), callbackData);
    }

    /// <summary>
    /// Called once per frame. 
    /// 
    /// Currently designed so that if messages are recieved more frequently than Update is called 
    /// (i.e. at a rate higher than the framerate) then only the last message received is kept. 
    /// This can be changed by extending the OnRecieve method, but for now there is no downside to this approach. 
    /// </summary>
    private void Update()
    {
        if (messageReceived == true)
        {
            //Message message = Unity_CVE.Message.GetRootAsMessage(new ByteBuffer(receiveBytes));
            ChildUpdateHolder ObjectToUpdate = null;
            switch (this.lastMessage.Channel)
            { 
                case "CH0":
                    ObjectToUpdate = this.CH0_ChildUpdateHolder;
                    break;
                case "CH1":
                    ObjectToUpdate = this.CH1_ChildUpdateHolder;
                    break;
                case "CH2":
                    ObjectToUpdate = this.CH2_ChildUpdateHolder;
                    break;
                case "CH3":
                    ObjectToUpdate = this.CH3_ChildUpdateHolder;
                    break;
                case "CH4":
                    ObjectToUpdate = this.CH4_ChildUpdateHolder;
                    break;
                case "CH5":
                    ObjectToUpdate = this.CH5_ChildUpdateHolder;
                    break;
                case "CH6":
                    ObjectToUpdate = this.CH6_ChildUpdateHolder;
                    break;
                default:
                    //Channel is not one that is monitored. 
                    UnityEngine.Debug.Log("Received message on unwatched channel. Channel: "+this.lastMessage.Channel);
                    break;
                    
            }
            if (ObjectToUpdate != null)
            {

             //This switch is based on message type. 
             switch (this.lastMessage.MessageDataType)
                {
                    case Data.Location:
                        //this.lastMessage.GetMessageData<>();
                        Location loc = this.lastMessage.GetMessageData<Location>(new Location());
                        ObjectToUpdate.setLocation(loc.X, loc.Y, loc.Z);
                        UnityEngine.Debug.Log("Recieved loc: " + loc.X + loc.Y + loc.Z);
                        break;
                    case Data.Position:
                        Position pos = this.lastMessage.GetMessageData<Position>(new Position());
                        ObjectToUpdate.setPosition(pos.X, pos.Y, pos.Z, pos.Roll, pos.Pitch, pos.Yaw);
                        UnityEngine.Debug.Log("Recieved pos: " + pos.X + pos.Y + pos.Z + pos.Roll + pos.Pitch + pos.Yaw);
                        break;
                    case Data.Orientation:
                        Orientation ori = this.lastMessage.GetMessageData<Orientation>(new Orientation());
                        ObjectToUpdate.setOrientation(ori.Roll, ori.Pitch, ori.Yaw);
                        UnityEngine.Debug.Log("Recieved ori: " + ori.ToString());
                        break;
                    case Data.ExtraParam:
                        ExtraParam ext = this.lastMessage.GetMessageData<ExtraParam>(new ExtraParam());
                        ObjectToUpdate.setExtraParam(ext.Name, ext.Value);
                        UnityEngine.Debug.Log("Recieved ext: " + ext.ToString());
                        break;
                    default:
                        //Empty Message or Error. 
                        UnityEngine.Debug.Log("Networking Script unable to determine the type of a recieved message.");
                        break;
                }   
            }
            messageReceived = false;
        }
    }

    /// <summary>
    /// Called when the program quits. Closes the open socket. 
    /// </summary>
    private void OnApplicationQuit()
    {
        if(udpClientReceiver != null)
            udpClientReceiver.Close();
    }


    /// <summary>
    /// This method is called from child scripts. It sends a message to the CVE server on the specified channel.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="channel"></param>
    public void sendLocation(float x, float y, float z, String channel)
    {
        FlatBufferBuilder builder = new FlatBufferBuilder(1);

        var loc = Location.CreateLocation(builder, x, y, z);
        var channelInt = builder.CreateString(channel);

        Message.StartMessage(builder);
        Message.AddMessageDataType(builder, Data.Location);
        Message.AddMessageData(builder, loc.Value);
        Message.AddChannel(builder, channelInt);

        var message = Message.EndMessage(builder);
        builder.Finish(message.Value);
        Byte[] byteA = builder.SizedByteArray();
        sendBytes(byteA);
    }

    /// <summary>
    /// This method is called from child scripts. It sends a message to the CVE server on the specified channel.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="roll"></param>
    /// <param name="pitch"></param>
    /// <param name="yaw"></param>
    /// <param name="channel"></param>
    public void sendPosition(float x, float y, float z, float roll, float pitch, float yaw, string channel)
    {
        FlatBufferBuilder builder = new FlatBufferBuilder(1);

        var pos = Position.CreatePosition(builder, x, y, z, roll, pitch, yaw);
        var channelInt = builder.CreateString(channel);

        Message.StartMessage(builder);
        Message.AddMessageDataType(builder, Data.Position);
        Message.AddMessageData(builder, pos.Value);
        Message.AddChannel(builder, channelInt);

        var message = Message.EndMessage(builder);
        builder.Finish(message.Value);
        Byte[] byteA = builder.SizedByteArray();
        sendBytes(byteA);
    }

    /// <summary>
    /// This method is called from child scripts. It sends a message to the CVE server on the specified channel.
    /// </summary>
    /// <param name="roll"></param>
    /// <param name="pitch"></param>
    /// <param name="yaw"></param>
    /// <param name="channel"></param>
    public void sendOrientation(float roll, float pitch, float yaw, String channel)
    {
        FlatBufferBuilder builder = new FlatBufferBuilder(1);

        var ori = Orientation.CreateOrientation(builder, roll, pitch, yaw);
        var channelInt = builder.CreateString(channel);

        Message.StartMessage(builder);
        Message.AddMessageDataType(builder, Data.Orientation);
        Message.AddMessageData(builder, ori.Value);
        Message.AddChannel(builder, channelInt);

        var message = Message.EndMessage(builder);
        builder.Finish(message.Value);
        Byte[] byteA = builder.SizedByteArray();
        sendBytes(byteA);
    }

    /// <summary>
    /// This method is called from child scripts. It sends a message to the CVE server on the specified channel.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="channel"></param>
    public void sendExtraParam(String name, float value, String channel)
    {
        FlatBufferBuilder builder = new FlatBufferBuilder(1);

        var channelInt = builder.CreateString(channel);

        var nameInt = builder.CreateString(name);
        var ext = ExtraParam.CreateExtraParam(builder, nameInt, value);

        Message.StartMessage(builder);
        Message.AddMessageDataType(builder, Data.ExtraParam);
        Message.AddMessageData(builder, ext.Value);
        Message.AddChannel(builder, channelInt);

        var message = Message.EndMessage(builder);
        builder.Finish(message.Value);
        Byte[] byteA = builder.SizedByteArray();
        sendBytes(byteA);
    }

    /// <summary>
    /// This method sends a byte array to the spefied server and port. 
    /// </summary>
    /// <param name="toSend"></param>
    private void sendBytes(Byte[] toSend)
    {
        
        //TODO: maybe making this a static method would be better form, but for now there are no problems, 
        //and we want to use only one instance of the udpCLientSender Object. 
        udpClientSender.Connect(this.serverIP, this.serverSendPort);
        udpClientSender.Send(toSend, toSend.Length);

        }


}
