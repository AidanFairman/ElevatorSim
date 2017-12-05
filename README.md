# Welcome to Elevator Simulation

![ElevatorScreenshot](/ElevatorScreen.PNG/)

### Summary

This project was for my Modeling and Simulation class. It is a Discreet event simulation that simulates people walking down a hallway on the first or second floor, queueing for, and riding the elevator. The simulation itself outputs simulation events, which are then read in by the animation, and the simulation is then animated.

People spawn on a random floor every second. The people walk down the hall and queue for the elevator. The elevator has a small AI that determines its behavior. It will wait a maximum amount of time if nobody has gotten on, or has loaded a maximum of 10 people. The elevator looks like it takes off without people, but this is just because the people rendered in the back are the ones being loaded. when the elevator gets to the other floor, it offloads all the people and loads up to 10 more.

The simulation runs, generating a specified number of people, and then terminates after the last person has walked the length of the hallway and off the right hand side of the screen.

### Try it out

There is a runnable executable in the [runnable folder](https://github.com/AidanFairman/ElevatorSim/tree/master/Runnable) where you can watch the animation. It does require the surrounding files to run, so please download the entire folder. If you watch multiple times, the simulation will not be exactly the same. This is because it _does_ actually run the simulation every time. The results are not hard-coded in.
