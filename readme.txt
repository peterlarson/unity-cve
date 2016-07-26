Unity-CVE bridge. 

Table of Contents: 



CVE and Unity:

CVE stands for collaborative virtual environment, and is a system developed by Michael Cohen's lab at the university of aizu. 
CVE communication takes place through Java serialized objects, so communication can happen easily on platforms that support Java, 
but requires some form of middleware to communicate with non-Java programs. 
CVE communicates using four kinds of messages. Positions, Orientations, Locatinos, and one called Extra Paramter. 
Location is the x, y, z coordinates of an object without any units. 
Orientation is the roll, pitch, yaw that determine orientation. 
Position is a message containing both location and orientation, but because each message has only one of these four types a Position message is not
a separate Location message and Orientation message. Instead it is a single message that conveys the same data of parameters x, y, z, roll, pitch, yaw. 
Extra Parameter is intended to be used to send other things through CVE. This message contains a string parameter and a float parameter to identify and 
provide the extra value.  

Unity is a modern game engine that allows deployment to different platforms including a web player, webgl, Windows, Mac, Ios, Android and more. 
Code for unity is not written in Java, but because it is multiplatform it is a good candidate for inclusion with CVE. 

This project allows multichannel, bidirectional communication between Unity and CVE.  

Solution: 

    CVE uses a client-server model, and so the end goal of this connection is to allow unity games to communicate as clients with the CVE server. 
    There are two components to the solution used in this project. 
    There is a bridge program written in Java that communicates with the CVE server and with Unity, and there is code written for Unity (scripts)
    that let Unity communicate with the bridge. 

    Bridge Program
        The bridge program is written in Java to communicate with Unity and with the CVE server. 
        The protocols used for communicating with Unity and the CVE server are different, and so they were designed to use separate classes. 

        The main class of the bridge program, called Bridge_Controller, starts the program by creating one object for communicating with Unity, 
        and an object for each CVE channel that communication will take place on. Currently it opens channels with the names CH0, CH1, ... CH6. 
        Each object that is created is given a reference to the parent object, the Bridge_Controller, in order to facilitate communication between 
        the two classes. 

        The Bridge_Controller has 8 methods used for communication between the CVE component and the Unity component. 
        4 of these methods are for the 4 kinds of messages recieved by the CVE component to be relayed to Unity, and 
        4 of these methods are from Unity to relay to the CVE component. 

        CVE component

        Unity component. 

    Unity scripts. 

        Networking component

        Gameobject communication

    Flatbuffers

    Ant


How to use. 

    How to add to unity game
        Single scene limitation

    

