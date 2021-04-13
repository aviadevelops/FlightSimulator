# FlightSimulator

### Summary
This is a desktop app for anomalies detection. We display an animation of flight through a flight simulator called FlightGear.
During the project we implement MVVM architecture, and we using .Net Framework called WPF.
We actually implement a TCP client which sends data in real-time to the FlightGear. 
The app supports real-time updates of the flight and updates on anomaly detections.

### App Features
- [x] Jump quickly for every selected moment during the flight, by moving the slider back and forward.
- [x] Change the flight speed as desired.
- [x] joystick animation.
- [x] Real-time update flight statistics such as altimeter, airspeed, heading_deg, pitch, roll ,yaw.
- [x] Select a flight feature for research
- [x] You will see his a real-time updated graph of the feature and a real-time updated graph of his most correlated feature.
- [x] Chosen anomaly detection algorithm
- [x] Loading a linear regression algorithm you will see a the regression line of the chosen feature and his most correlated feature.
- [x] Loading a minimum enclosing circle algorithm you will him.
- [x] In the graph below you will see gray points belonging to the last 30 seconds of the flight.
- [x] Points indicating an anomaly will be painted red.

### Folder Structure


### Required installations
* Microsoft .NET 5.0.104 (x64)
* FlightFear 2020.3.8 (For windows 7,8,10)
* Visual Studio 2019

### Compiling and Running
1. download this repository
2. open "FlightSimulator.sln" in Visual studio and build the project.
3. run the app and you will see the home screen - load the .exe file which propably locate at FlightSimulator/bin/Debug/FlightSimulator.exe
4. load the chosen anomaly detection algorithm, for exmaple a Linear regression or a minimum enclosing circle. This algorithm you must be a .dll file!
5. load the csv tain file
6. load the test csv file
7. press the play button 

### Additional Links
* [UML diagram](http://google.com)
* [tutorial video](http://google.com) 







