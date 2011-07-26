#include <TimerOne.h>
#include <LPD6803.h>
#include "palette.h"

const int numLights = 70;
const int numRows = 6;
int fire[numLights][numRows];

LPD6803 strip = LPD6803(70, 9, 10);

//void setup() {
//  
//  // The Arduino needs to clock out the data to the pixels
//  // this happens in interrupt timer 1, we can change how often
//  // to call the interrupt. setting CPUmax to 100 will take nearly all all the
//  // time to do the pixel updates and a nicer/faster display, 
//  // especially with strands of over 100 dots.
//  // (Note that the max is 'pessimistic', its probably 10% or 20% less in reality)
//  
//  strip.setCPUmax(70);  // start with 50% CPU usage. up this if the strand flickers or is slow
//  
//  // Start up the LED counter
//  strip.begin();
//
//  // Update the strip, to start they are all 'off'
//  strip.show();
//}
void setup() {
  strip.setCPUmax(70);
  strip.begin();
  strip.show();
  randomSeed(analogRead(0));
  
  initPalette();

  for(int x = 0; x < numLights; x++)
    for(int y = 0; y < numRows; y++)
      fire[x][y] = 0;
}


void loop() {
  for(int x = 0; x < numLights; x++)
    fire[x][0] = random(0, 230);
  
  for(int y = 1; y < numRows; y++)
  for(int x = 0; x < numLights; x++) {
    int l = (x - 1 + numLights) % numLights;
    int r = (x + 1) % numLights;
    int sum = fire[l][y-1] + fire[r][y-1] + fire[x][y-1]
            + fire[l][y] + fire[r][y];
    fire[x][y] = sum / 5;
  }
  
  for(int i = 0; i < 70; i++) {
    int val = constrain(fire[i][numRows-1], 0, 255);
    strip.setPixelColor(i, palette[val]);
  }
  strip.show();
  //delay(10);
}
