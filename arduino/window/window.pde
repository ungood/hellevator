#include <TimerOne.h>
#include <LPD6803.h>
#include "palette.h"

const int numLights = 70;

int src[numLights];
int dest[numLights];

LPD6803 strip = LPD6803(numLights, 2, 3);

void setup() {
  strip.setCPUmax(60);
  strip.begin();
  strip.show();
  randomSeed(analogRead(0));
  initPalette();
  for(int i = 0; i < numLights; i++)
    src[i] = 0;
  randomDest();
}

int randomize(int prev, int amount) {
  return (prev + random(1, amount)) % PALETTE_SIZE;
}

void randomDest() {
  int prev = random(0, PALETTE_SIZE);
  for(int i = 0; i < numLights / 2; i++) {
    prev = randomize(prev, 5);
    dest[i] = dest[numLights-i-1] = prev;
  }
}

float tween = 0;
float velocity = 0.01;

void reset() {
  tween = 0;
  velocity = random(10) == 0 ? 0.01 : 0.005;
  
  for(int i = 0; i < numLights; i++)
    src[i] = dest[i];
    
  randomDest();
}

int interpolate(int from, int to, float ratio) {
  return constrain(from + (to - from) * ratio, 0, PALETTE_SIZE-1);
}

void loop() {
  if(tween > 1.0)
    reset();
    
  for(int i = 0; i < numLights; i++) {
    int index = interpolate(src[i], dest[i], tween);
    unsigned int color = palette[index];
    strip.setPixelColor(i, color);
  }
  
  tween += velocity;
  strip.show();
  delay(40);
}
