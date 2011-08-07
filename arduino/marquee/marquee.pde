#include <TimerOne.h>
#include <LPD6803.h>
#include "palette.h"

const int numLights = 70;
const int numRows = 6;
int fire[numLights][numRows];

LPD6803 strip = LPD6803(numLights, 2, 3);

void setup() {
  Serial.begin(9600);
  strip.setCPUmax(60);
  strip.begin();
  strip.show();
  randomSeed(analogRead(0));
  
  initPalette();

  for(int x = 0; x < numLights; x++)
    for(int y = 0; y < numRows; y++)
      fire[x][y] = 0;
}


float deg = 0;
void loop() {
  fire[0][0] = random(0, 256);
  for(int x = 1; x < numLights; x++)
    //fire[x][0] = (fire[x-1][0] + random(0, 10)) % 256;
    fire[x][0] = random(0, 256);
  
  for(int y = 1; y < numRows; y++)
  for(int x = 0; x < numLights; x++) {
    int l = (x - 1 + numLights) % numLights;
    int r = (x + 1) % numLights;
    int sum = fire[l][y-1] + fire[r][y-1] + fire[x][y-1]
            + fire[l][y] + fire[r][y];
    fire[x][y] = sum / 5;
  }
  
  int burn = sin(deg) * 100;
  deg += 0.025;
  
  for(int i = 0; i < 70; i++) {
    int val = constrain(fire[i][numRows-1] + burn, 0, 255);
    strip.setPixelColor(i, palette[val]);
  }
  strip.show();
  delay(50);
}
