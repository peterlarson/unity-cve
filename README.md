# Unity-CVE bridge 

__CVE__ stands for collaborative virtual environment, and is a system developed by Michael Cohen's lab at the university of aizu. 
CVE communication takes place through Java serialized objects, so communication can happen easily on platforms that support Java, 
but requires some form of middleware to communicate with non-Java programs.
CVE communicates using four kinds of messages. Positions, Orientations, Locatinos, and one called Extra Paramter. 
* Location is the x, y, z coordinates of an object without any units. 
* Orientation is the roll, pitch, yaw that determine orientation. 
* Position is a message containing both location and orientation, but because each message has only one of these four types a Position message is not
a separate Location message and Orientation message. Instead it is a single message that conveys the same data of parameters x, y, z, roll, pitch, yaw. 
* Extra Parameter is intended to be used to send other things through CVE. This message contains a string parameter and a float parameter to identify and 
provide the extra value.  

__Unity__ is a modern game engine that allows deployment to different platforms including a web player, webgl, Windows, Mac, Ios, Android and more. 
Code for unity is not written in Java, but because it is multiplatform it is a good candidate for inclusion with CVE. 

This project allows multichannel, bidirectional communication between Unity and CVE.  

## Solution: 

CVE uses a client-server model, and so the end goal of this connection is to allow unity games to communicate as clients with the CVE server. 
There are two components to the solution used in this project. 
There is a bridge program written in Java that communicates with the CVE server and with Unity, and there is code written for Unity (scripts)
that let Unity communicate with the bridge. 

### Bridge Program
The bridge program is written in Java to communicate with Unity and with the CVE server. 
The protocols used for communicating with Unity and the CVE server are different, and so they were designed to use separate classes. 

The main class of the bridge program, called Bridge_Controller, starts the program by creating one object for communicating with Unity, 
and an object for each CVE channel that communication will take place on. Currently it opens channels with the names CH0, CH1, ... CH6. 
Each object that is created is given a reference to the parent object, the Bridge_Controller, in order to facilitate communication between the two classes. 

#### The Bridge_Controller 
The Bridge_Controller has 8 methods used for communication between the CVE component and the Unity component. 
4 of these methods are for the 4 kinds of messages recieved by the CVE component to be relayed to Unity, and 
4 of these methods are from Unity to relay to the CVE component. 

#### CVE component
This class handles communication between the Bridge and the CVE server by creating a CVEClient object, and calling methods on that 
object to send messages to the server, and having messages called on it through the CVEClientIF interface from the CVEClient object when
they are recieved from the CVE server. 
The CVE component class implements the CVEClientIF interface that allows it to recieve messages from a CVE client object.
Each CVE component object can communicate on only one channel at a time, so in order to set up multichannel communication the Bridge 
Controller instantiates multiple CVE components. Each for the same server but a different channel. 

#### Unity component. 
Class that communicates with Unity. Sends messages encoded as flatbuffers to the IP address provided. Listens for messages on an open
socket and parses them before sending the data to the parent Bridge_Controller object.
Communication happens using FlatBuffer objects. A discussion of this takes place in the Flabuffers section below. 
    
### Unity scripts. 
Inside unity the solution is set up in the following way. A single networking script attached to any game object (empty objects in the 
samples) handles all of the communication between the bridge and Unity. This script has references to the objects that will be on each
channel, and passes messages to scripts attached to those objects. Objects that will send or recieve data, called Child objects, are 
associated with scripts  


#### Networking Script
This is the main networking class. This script handles communication with scripts attached to other objects in unity, 
Encoding and decoding from the FlatBuffers format, and UDP-based communication with the unity CVE server. This class
should not require modifications to use. 
The code will listen on the provided port for UDP packets with CVE data, and it will send to the specified server at the 
specified port. 

#### Abstract Child Script
The code for communication with the Networking Script is located in an abstract class, so Child Scripts should extend this class. 
This means that the actual code that must be written by the programmer is very small. 

The abstract class requires some methods be overloaded to handle messages sent to that object asynchronously. To send messages to the 
CVE server, a call to base.sendXXXX() can be made where XXXX is location, position, orientation, or extra parameter.    

### Flatbuffers
Communication between the bridge server and Unity is done through UDP packets. The data in the packets is encoded as a FlatBuffer. 
https://google.github.io/flatbuffers/
FlatBuffers is intended to be a fast, cross platform serialization library. Further modification to the Encoding and Decoding should
not be needed, but just in case the schema, named CVE_types.fbs, and a copy of the flatbuffers compiler is provided in the directory FlatBuffers. 
   
### Ant
Ant, http://ant.apache.org/, is used to compile and run the Bridge component of the project. 

## How to use: 

* __Compile and run bridge__ 
    * ant
    * ant Bridge_Controller -Darg0=[CVE hostname] -Darg1=[Unity hostname]

* __Add to a unity game__
    * Add unity networking scripts to the Unity project. 
        * NetworkingScript.cs and AbstractNetworkingChildScript.cs are required. 
        * NetworkingChildScriptScriptExample.cs is an optional start for your child script. 

    * Add FlatBuffers code to the project. 
        * All needed code is in the unity-code/cve-flatbuffers-encoding directory. 

* __Set up custom child script__
    * NetworkingChildScriptScriptExample.cs can be a starting point for your code. You will define when to send messages to the CVE server, and what to do when you recieve them. More detailed instructions are in the example. 

* __Attach custom child scripts to game objets__
    * Attach the script to each game object that you wish to connect to the CVE server. There is a maximum of one object per channel. If you wish to have multiple objects on one channel you can have the connected object be an empty parent of the child objects you want to control. 
* __Configure NetworkingScript__
    * Attach the NetworkingScript to any object in your code. In the examples an empty object is used. Set up the relationship between each game object and each channel by draggint the objects into the script variable in the inspector. Also in the inspector, set up the server IP, and the ports to send to and listen on. 

* __Run and Enjoy__ 
    * As soon as the game starts Unity will begin to listen for incoming packets, and whenever any of your child objects sends a message it will be relayed to the IP of the Bridge Server. 


    

