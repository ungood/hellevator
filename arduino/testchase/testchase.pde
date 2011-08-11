#include <TimerOne.h>
#include <LPD6803.h>
#include "palette.h"

const int numLights = 70;

LPD6803 strip = LPD6803(numLights, 2, 3);

void setup() {
  Serial.begin(9600);
  strip.setCPUmax(60);
  strip.begin();
  strip.show();
  randomSeed(analogRead(0));
}

void loop() {
  chase(255, 0, 0);
  delay(5000);
  chase(0, 255, 0);
  delay(5000);
  chase(0, 0, 255);
  delay(5000);
}

void chase(int red, int green, int blue) {
  for(int i = 0; i < numLights; i++) {
    strip.setPixelColor(i, red, green, blue);
    strip.show();
    delay(50);
  }
}
