/* Encoder Library - Basic Example
 * http://www.pjrc.com/teensy/td_libs_Encoder.html
 *
 * This example code is in the public domain.
 */

#include <Encoder.h>

Encoder myEnc(5, 15);
//   avoid using pins with LEDs attached

void setup() {
  Serial.begin(9600);
  Serial.println("Basic Encoder Test:");
}

long oldPosition  = -999;

void loop() {
  long newPosition = myEnc.read();

  if (newPosition != oldPosition) {
    if(newPosition < oldPosition) {
      Keyboard.println("W");
    }
    else if((newPosition > oldPosition)) {
      Keyboard.println("Q");
    }
    oldPosition = newPosition;
  }
}
