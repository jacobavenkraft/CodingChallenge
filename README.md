# Coding Challenge
This repository contains code for a coding challenge for an interview.  The code is written in C# using the WPF framework.

# Actual Prompt
The following is the description of the coding prompt:

Using WPF, build a custom control to do the following:
* Display a window with a button labeled "Record"
* When that button is clicked, a countdown timer should appear (either in the same window or another) that gives the user a visual clue to when the recording starts. The countdown should be 3, 2, 1 and take however long you think makes sense.
* Additionally, a stop-recording button or other widget should be shown at this point. By clicking this button, the countdown should be terminated (if it's still ongoing) or the recording should stop.
* When the recording does start (at the end of the countdown):
    * Indicate the recording is live by showing something to the user to illustrate this state.
    * Display the time elapsed recording in some way.
    * Either in the same window or another, display the provided image, centered within the window area. The window should be large enough that there is space around the centered image approximately the same width or height as the image.
* Every 1/10 of a second, you should move the image up, down, right, or left by a small amount (e.g. a pixel). The direction of the movement should be determined by where the mouse is with respect to the center of the window. For example, if the mouse is directly to the right of the window center, the image should shift toward the right. Moving the mouse to the top center of the screen should have the image move upward. If the mouse is in the top right corner of the screen, the image should move both upward and rightward.  How you want to determine the portion of up vs. right is up to you. As you move the mouse around the screen, you should see the image movement follow it.
* Add a way to change the speed of the movement.
* Clicking on the stop recording widget should stop the movement of the image and display the total amount of time the recording took.
* Feel free to embellish any of this as you'd like. For example, you could have the image bounce off the sides of the window instead of just moving toward the appropriate side.
* Finally, please have the app respect the Window's setting for light and dark modes.

# Areas of difficulty with this challenge
1. The animation of both the clock and the image moving around the screen was choppy when running in the debugger.  I experimented with changing between the use of System.Timer and a background thread in order to refresh the timecode and the image position but this made no difference in the choppy movement.  Running standalone seemed to display smoothly so I left things as-is with the assumption that Visual Studio debugging was interfering with my timings.
2. I started by using code-behind events to trigger the roaming image when the window gets loaded and to pass the mouse move parameters to the underlying model.  Since this wasn't the best approach when using MVVM I modified my solution to use attached properties.

# Things I could improve on or do differently
1. Add an animation to the record indicator to make it look more like a flashing led lamp.
2. Improve the styling for dark and light modes.
3. Actually record the movement of the image and allow for playing it back.
4. Actually record the movement of the image and allow saving it to an actual movie file.
5. Make the image movement smoother (it is effectively only 10 frames per second)
6. Improve code quality with introduction of more discrete interfaces and classes that provide a better separation of concerns.
