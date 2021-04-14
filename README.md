# FlightSimulator

### Summary
This is walkthrough of a desktop application for detection of anomalies in various flights. It displays an animation of a flight utilizing a flight simulator called FlightGear. During the project we implement the MVVM architecture, while using the .Net Framework called WPF. The application implements a TCP client which sends data to the FlightGear simulator server. The flight inspector supports real-time updates of anomalies through graphs and animations that represent the current state of the flight.

### App Features
- [x] Jump to every selected moment during the flight, by moving the slider back and forth.
- [x] Change the flight speed as desired or temporarily pause it.
- [x] Joystick animation that represent real time visualization of the plane's rudder.
- [x] Real-time update of flight statistics such as altimeter, airspeed, heading degree, pitch, roll and yaw.
- [x] Select a flight feature to inspect it's current state.
- [x] Display of the current chosen feature's graph and it's most correlated counterpart, both of which update in real-time.
- [x] Loading an anomaly detection algorithm to your desire in runtime.
- [x] Use a linear regression anomaly detection algorithm, that displays the regression line of the chosen feature and its most correlated feature.
- [x] Use a minimum enclosing circle anomaly detection algorithm, that displays the minimum enclosing circle of the chosen feature and its most correlated feature.
- [x] Points indicating an anomaly will be painted red, while points belonging to the last 30 seconds of the flight will appear in gray in real-time.
- [x] Display a list of anomalous times, and jump to each anomaly as you see fit to inspect it more efficently.
- [x] Further documentation in the code.

![Image of Minimal Enclosing Circle](https://github.com/aviadevelops/FlightSimulator/blob/main/min%20circle.PNG)

![Image of Linear Regression](https://github.com/aviadevelops/FlightSimulator/blob/main/linear%20reg.PNG)

### Folder Structure


### Required installations
* Microsoft .NET 5.0.104 (x64)
* FlightFear 2020.3.8 (For windows 7,8,10)
* Visual Studio 2019

### Compiling and Running
1. Download this repository.
2. Make sure the settings file ("playback_small.xml") is located in the proper directory (flightGear/data/protocols).
3. Open "FlightSimulator.sln" in Visual studio and build the project.
4. Open the FlightSimulator.exe file which is located at FlightSimulator/bin/Debug folder to see the home screen. 
5. Load the chosen anomaly detection algorithm's dll, for exmaple a Linear Regression dll or a Minimum Enclosing Circle dll.
6. Make sure that the uploaded algorithm has a learn-normal(), detect(), returnShape() and getGraphName() methods.
7. Make sure that the .dll file name is exactly the same as the class name of the uploaded algorithm.
8. Load the csv train file.
9. Load the csv test file.
10. Press the play button and enjoy the flight!

### Additional Links
* [UML Diagram](UML.pdf)
* [Walkthrough video](https://youtu.be/UIf6bcRP8hU) 







