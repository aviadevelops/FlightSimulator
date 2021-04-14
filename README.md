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
- [x] You will see his real-time updated graph and a real-time updated graph of his most correlated feature.
- [x] Choose an anomaly detection algorithm
- [x] Load a linear regression algorithm, and you will see a the regression line of the chosen feature and his most correlated feature.
- [x] Load a minimum enclosing circle algorithm, and you will see him.
- [x] In the graph below you will see gray points belonging to the last 30 seconds of the flight.
- [x] Points indicating an anomaly will be painted red.

### Folder Structure


### Required installations
* Microsoft .NET 5.0.104 (x64)
* FlightFear 2020.3.8 (For windows 7,8,10)
* Visual Studio 2019

### Compiling and Running
1. Download this repository.
2. Make sure the settings file ("playback_small.xml") is located in the correct directory (flightGear/data/protocols).
3. Open "FlightSimulator.sln" in Visual studio and build the project.
4. Run the app and you will see the home screen - load the .exe file which propably locate at FlightSimulator/bin/Debug/FlightSimulator.exe
5. Load the chosen anomaly detection algorithm, for exmaple a Linear regression or a minimum enclosing circle. This algorithm must be a .dll file!
6. Make sure that the uploaded algorithm has a learn-normal() + detect() + returnShape() + getGraphName() methods.
7. Make sure that the .dll file name is exactly the same as the class name of the uploaded algorithm.
9. Load the csv train file.
10. Load the csv test file.
11. Press the play button .

### Additional Links
* [UML diagram](UML.pdf)
* [tutorial video](http://google.com) 







