# Process Documentation

## 02.01.22
Worked out the secondary display for the device. There are a few issues with the screen not resizing properly. To fix, I have been changing the window style (windowed, maximum vs. windowed) and that seems to be fixing the problem, but it's not a permanent solution.

The Window_Launch scene launches a secondary scene on the secondary display, which then is responsible for switching the "applications." 
I might spend some time thinking on how that could integrated to the main Game Manager for ease of later coding.... maybe.

Also of note, if the secondary display is an iPad, it can be run wireless, which leads to some super interesting future possibilities.