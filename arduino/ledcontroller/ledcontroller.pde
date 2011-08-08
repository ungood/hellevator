#include <TimerOne.h>
#include <LPD6803.h>

const byte address = 'a';
const int clockOut = 5;
const int dataOut  = 6;

const int numLights = 70;

LPD6803 strip = LPD6803(numLights, 2, 3);

void setup() {
  strip.setCPUmax(60);
  strip.begin();
  strip.show();
  
  Serial.begin(9600);
  Serial.println("HI");
}

boolean isReading = false;
int index = 0;
int state = 0;
int buf[3] = { 0, 0, 0};

void loop() {
  if(!Serial.available())
    return;
    
  byte data = Serial.read();
  if(data == 0x80) {
    strip.show();
    isReading = false;
    return;
  }
  
  if((data & 0x80) != 0) {
    isReading = (data & 0x7f) == address;
    index = state = 0;
    Serial.println(isReading ? "READ" : "IGNORE");
    return;
  }
  
  if(!isReading)
    return;
    
  buf[state] = data * 2;
  state++;
  if(state > 2) {
    state = 0;
    strip.setPixelColor(index, color(buf[0], buf[1], buf[2]));
    index++;
    if(index >= numLights)
      return;
  }
}

unsigned int color(byte r, byte g, byte b) {
  r /= 8;
  g /= 8;
  b /= 8;
  //Take the lowest 5 bits of each value and append them end to end
  return( ((unsigned int)r & 0x1F )<<10 | ((unsigned int)g & 0x1F)<<5 | (unsigned int)b & 0x1F);
}
