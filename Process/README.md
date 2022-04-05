# Process Documentation

## 04.02.22 - Window Activated!

Implemented the window kiosk overlay. We've also got the voice recognition working with wit.io, and have the whole core game loop running. Lastly, we created a <a href="https://docs.google.com/spreadsheets/d/1tGc6KPbSOPDsjxxMaBgKS6yINaMUH0e9Hox9kDeORi4/edit#gid=1067057013">spreadsheet</a> to start filling out for all of the character interactions. <a href="https://www.youtube.com/watch?v=x_7wtEIPI0w">Everything is coming up Space Mall!</a>

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/window.png" width="60%">

## 03.29.22 - Wit: Great Power...

After trying out coqui for a few weeks, we tried out a new Speech to Text implementation. Facebook's Wit.io seems like a great resource, easy to implement and easy to train. But we shall see!

## 03.06.22 - More Character Explorations!

Seeing as we probably need more objects to occupy the mall, we wanted to continue to explore the possible characters that could be in the game, as well as continue to crank out assets for them. The crowd favorite, a mom stroller with baby strollers had to be digitized, so we have that update this week. As well as another character exploration sheet and a fun set of silverware characters! 

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Character-Stroller01.png" width="40%"> <img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/CharacterExploration-MiscCharacters.png" width="40%">

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Character-Fork01.png" width="30%"> <img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Character-Spoon01.png" width="30%"> <img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Character-Knife01.png" width="30%">


## 03.03.22 - More Interface Display Graphic Mockups
The next set of interface display scenes that are moving forward in the visual design process are the music manipulation and where's waldo scenes. 

The Waldo scene needed some framing and visual symmetry with the over arching space mall style, so we tried to get a sort of lense view of the store with a blueprint like background to frame the top down view of the mini-game.

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/WaldoInterface-MockUp.png" width="60%">

The music manipulation scene gets it's visual style from the sort of retro music tech style. It made sense to mirror the physical equivalent when designing its virtual representation.

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Scene-MusicManipulation-MockUp.png" width="60%">



## 02.28.22 - First Character Assets!
Building on the character sketches from last week, we wanted to push a few of them to the level of actual game assets so below we have the segway mall cop, regular and on alert, a stressed chair, and the first buff teddy bear. There are still more characters to both sketch and polish, but it feels good to finally see some of the crazy characters that will be inhabiting our space mall!

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Character-SegwayMallCop00.png" width="40%"> <img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Character-SegwayMallCop01.png" width="40%">

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Character-Chair01.png" width="40%"> <img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Character-StronkTeddyBear01.png" width="40%">



## 02.22.22 - Twos-day Character Sketches!
Now that we are starting to get towards the main game loop, we need to start thinking about the kinds of characters that will come up to your mall info kiosk. We had previously talked about a few ideas including a mall cop segway, a stressed mom stroller, and a buff teddy bear that might attend the build-a-bear gym. Along with these more specific characters we wanted to flesh out some of the other more general ideas including furniture objects like chairs, house hold objects big and small, and a few other ideas we had been tossing around. There is still more sketching to do as far as the range of character that might appear in the mall asking for assistance, but this should provide a decent place to start

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/CharacterExploration-Chair.png" width="60%">

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/CharacterExploration-HouseholdObjects.png" width="45%"> <img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/CharacterExploration-OtherCharacters.png" width="45%">

## 02.18.22 - Pure Data Soundtrack Finished
The soundtrack for the game consists of two parts. The first was Mallsoft tracks using Ableton and the second is a generative composition created in Pure Data. Both are implemented into Unity via Pure Data. This allows the player to manipulate the music in various ways. 1. Timeshift, slowing down or speeding up the track 2. Crossfade, adjusting volume between Mallsoft and generative music. 3. Filter, changing the frequency of the sounds 4. Scale, cycling through different keys and tracks.

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/38cd54cbf220b02d3f9224f085d0d75774749289/Process/Media/Ambient%20Generative%20Patch.png" width="45%"> 
<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/96ecf5ccf4fee68e7fcf9f5f3ca239688f31eb7f/Process/Media/Mallsoft%20Tracks%20Patch.png" width="45%"> 



## 02.12.22 - Mall Directory Scene Implementation
Another scene we want to have on the display interface is a directory list of all the stores in the mall and their location code. For now, the text in the scene is place holder, snagged from the directory list of an actual mall in Denver. We want to store the directory information in a text file that we can edit and pull from, so all the places and codes are being stored there, and linked into the text visual through a simple script. The location codes will reference the actual location on the mall map which is still in development. We want the crank to control the directory so you are cranking all the way up and down the list. 

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Scene-MallDirectory-Interaction.gif" width="60%">



## 02.03.22 - Interface Display Graphic Mockups
Moving forward with the interface mockups, we have digitized the ideas for the plant care and audio tuning scenes.

The plant care scene has an empty spot for a slider interaction, a draining water meter that can be partially replenished with a button press, a rotating light source that you have to line up to the direction of your plant, a growth meter, and a variety of other less functional interface bibs and bobs. 

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Scene-PlantCare-Interaction.gif" width="60%">

The audio tuner scene will allow the player to tune to the right frequency in order to understand the being that has appeared at the information booth. There are three frequencies that need to be tuned into and there is color feedback for each about how close you are to the target.

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Scene-AudioTunerInterface-MockUp.png" width="60%">



## 02.01.22 - Multi-Display Work
Worked out the secondary display for the device. There are a few issues with the screen not resizing properly. To fix, we have been changing the window style (windowed, maximum vs. windowed) and that seems to be fixing the problem, but it's not a permanent solution.

The Window_Launch scene launches a secondary scene on the secondary display, which then is responsible for switching the "applications." 
We might spend some time thinking on how that could integrated to the main Game Manager for ease of later coding.... maybe.

Also of note, if the secondary display is an iPad, it can be run wireless, which leads to some super interesting future possibilities.



## 01.28.22 - Interface Ideation
Since the game will be featuring two displays: the main info booth looking into the mall, and the mall interface options screen, we need to start thinking about how the interface screen will be laid out. One of the scenes for the interface display will be controlling the wellbeing of your desk plant. This is a nice simple place to start as far as interface, since the controls and options are a little more straight forward. Below is an idea of what the various interface options might look like.

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Sketch-PlantInterface.jpg" width="60%">


## 01.25.22 - Mall Scene Concepts
The next step in defining the visual style of the mall, is probably to ya know... try drawing what the mall might look like! So the next set of concept pieces look to what the mall spaces might look like on the inside. Referencing actual mall interiors, our concepts are shown below. 

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/MallScene-1.png" width="60%">

These concepts feel in line with the vibe we are going for and have a lot of fun retro elements to build on. While the final mall scenes will be much busier and full of miscelaneous sentient furniture and objects, these give a nice idea of the internal spaces and theme.

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/MallScene-2.png" width="45%"> <img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/MallScene-3.png" width="52%"> 

## 1.19.22 - Mall Sounds
We decided to incoporate a contemporary style of hypnogogic pop music called Mallsoft. It incorporates muzak played throughout malls in the 80s, 90s, and early 2000s. The idea is to recreate something in this style for our game.

<a href="https://www.youtube.com/watch?v=JELt1jxJsHQ">Welcome To The Lobby!</a>


## 01.19.22 - Concept Art Exploration
Building on the retro-futurism style, we wanted to explore what that might look like to define the direction of the visual assets. Below is a set of both machine and visual explorations to work towards that consistant look and feel. Our favorite by far is the fourth option which has a nice balance of soft colors and vintage _Jetsons_ feel, so the art assets will try to head in that direction for the future. 

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/MachineStyleExploration01.PNG" width="45%"> <img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/MachineStyleExploration02.PNG" width="45%">

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/MachineStyleExploration03.PNG" width="45%"> <img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/MachineStyleExploration04.PNG" width="45%">



## 01.19.22 - Retro Futurism Inspo
Out of our initial brainstorming came this idea of theming the visual direction of the game toward a sort of retro-futurism vibe. Inspired by the wacky ways in which people imagined the future straight out of the _Jetsons_, we took a little time to draw from these styles and create some initial mood boards. 

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Ideation-RetroFuturismImagery.png" width="60%">

And some fun fonts too!

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Ideation-RetroFontOptions.png" width="60%">

There are a ton of funky retro machines that will probably influence the physical interface of the game design, here is sort of what we are thinking for that vibe:

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Ideation-RetroMachine.png" width="40%">



## 01.11.22 - The Birth of the Space Mall
**Brainstorm Session Notes:**
- Multiple streams of info
- Infinite dungeon mall? -> Being separate and in a managereal role
- Combination of macro and micro management
- Dialing into the language translator
- Cash register or something for many transfers and exchanges
- Retro futurism vibe

**Future + Mall + Management suff** -> Maybe it's in space

<img src="https://github.com/mouseandthebillionaire/whaaatStudio_s22/blob/master/Process/Media/Sketch-PhysicalGame.jpg" width="50%">

**Mall Things!**
- Info booth
- Carts
- Fountains
- Kids play areas
- Small trains
- Santa photo booth
- Large box office stores
- Movie theatre
- Food court
  -  Fro-yo that is constantly changing to a new thing
  -  Competing similar stores (next door pretzle places)
- Art Galleyr?
- Mini-golf
- Escalators
- Arcade
- Parking garage
- Build-a-bear

**Design Goals**
1) Assisting People
2) Communication
