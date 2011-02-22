#pragma once
#include "Interfaces.h"

class Screen : public Inputable
{
public:
  Screen();

  static bool Resize();
  void HandleInput(const SDL_Event& event); 

  static int Width;
  static int Height;
  static bool Fullscreen;

private:
  static int FullWidth;
  static int FullHeight;
};
