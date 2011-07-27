const int PALETTE_SIZE = 256;
unsigned int palette[PALETTE_SIZE];

unsigned int color(byte r, byte g, byte b) {
  r /= 8;
  g /= 8;
  b /= 8;
  //Take the lowest 5 bits of each value and append them end to end
  return( ((unsigned int)r & 0x1F )<<10 | ((unsigned int)g & 0x1F)<<5 | (unsigned int)b & 0x1F);
}

unsigned int colorf(float r, float g, float b) {
  return color(r*255, g*255, b*255);
}

// h -> 0..360
// s, v -> 0..1
unsigned int hsv(float h, float s, float v) {  
  if(s == 0) {
    return color(v*255, v*255, v*255);
  }

  h /= 60;
  int sector = (int)(h);
  float f = h - sector;
  float p = v * (1-s);
  float q = v * (1-s*f);
  float t = v * (1-s * (1-f));
   
  switch(sector) {
    case 0:
      return colorf(v, t, p);
    case 1:
      return colorf(q, v, p);
    case 2:
      return colorf(p, v, t);
    case 3:
      return colorf(p, q, v);
    case 4:
      return colorf(t, p, v);
    case 5:
      return colorf(v, p, q);
    default:
      return color(0, 0, 0);
  }
}

void initPalette() {
  for(int i=0; i<PALETTE_SIZE; i++) {
    palette[i] = hsv(i, 1.0, 1.0);
  }
}
