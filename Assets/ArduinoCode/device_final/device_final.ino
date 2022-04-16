/* Space Mall device code
 *  Whaaat!? Lab 2022 Spring Project
 */
 
#include <Encoder.h>
//   avoid using pins with LEDs attached


// knob 1
Encoder myEncKnob1(4, 14);

// knob 2
Encoder myEncKnob2(5, 15);

// knob 3
Encoder myEncKnob3(6, 16);

int channelChanger = A3;
int channelInput = 0;
int channelThresholds[] = {0, 5, 15, 25, 30, 40, 50, 60, 70, 80, 90, 95, 105};
char channelCodes[] = {'q','w','e','r','t','y','a','s','d','f','g','h'};
int currChannel = -999;


void setup() {
  Serial.begin(9600);
  Serial.println("Basic Encoder Test:");
}


long oldPosition1  = -999;
long oldPosition2  = -999;
long oldPosition3  = -999;


void loop() {
  

  // channel
  channelInput = analogRead(channelChanger); 
  Serial.println(channelInput);

  for(int i=0; i<sizeof(channelCodes); i++){
     if((channelInput >= channelThresholds[i]) && (channelInput < channelThresholds[i+1])){
        if(currChannel != i){
          Keyboard.write(channelCodes[i]);
          currChannel = i;
        }
     }
  }

  
  
  // knob 1 update
  long newPosition1 = myEncKnob1.read();
  if (newPosition1 != oldPosition1) {
    if(newPosition1 < oldPosition1) {
      Keyboard.write('m');
      //Keyboard.println(KEY_RIGHT);
      //Keyboard.press(KEY_LEFT);
    }
    else if((newPosition1 > oldPosition1)) {
      Keyboard.write('n');
      //Keyboard.println(KEY_LEFT);
    }
    oldPosition1 = newPosition1;
  }

  // knob 2 update
  long newPosition2 = myEncKnob2.read();
  if (newPosition2 != oldPosition2) {
    if(newPosition2 < oldPosition2) {
      //Keyboard.println(KEY_UP);
      Keyboard.write('l');
    }
    else if((newPosition2 > oldPosition2)) {
      //Keyboard.println(KEY_DOWN);
      Keyboard.write('k');
    }
    oldPosition2 = newPosition2;
  }

  // knob 3 update
  long newPosition3 = myEncKnob3.read();
  if (newPosition3 != oldPosition3) {
    if(newPosition3 < oldPosition3) {
      Keyboard.write('p');
    }
    else if((newPosition3 > oldPosition3)) {
      Keyboard.write('o');
    }
    oldPosition3 = newPosition3;
  }

  delay(5);
  
}
