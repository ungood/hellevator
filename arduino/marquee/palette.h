unsigned int palette[256];

unsigned int color(byte r, byte g, byte b) {
  r /= 8;
  g /= 8;
  b /= 8;
  //Take the lowest 5 bits of each value and append them end to end
  return( ((unsigned int)r & 0x1F )<<10 | ((unsigned int)g & 0x1F)<<5 | (unsigned int)b & 0x1F);
}

void initPalette() {
  for(int i=0; i<128; i++) {
    palette[i]     = color(i * 2, 0, 0);
    palette[i+128] = color(255, constrain(i*2 - 64, 0, 255), 0);
//    palette[i]     = color(i<<2, 0, 0);
//    palette[i+64]  = color(255, i<<2, 0);
//    palette[i+128] = color(255, 255, i << 2);
//    palette[i+192] = color(255, 255, 255);
  }
}
