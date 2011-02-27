#pragma once
#include "EventHandler.h"

class Screen : public Inputable
{
public:
  Screen();

  static bool Resize();
  void HandleInput(const SDL_Event& event); 

  static int Width;
  static int Height;
  static bool Fullscreen;
  static Uint32 MaxFpsInverse;

private:
  void ToggleVsync();

  static int FullWidth;
  static int FullHeight;
};
