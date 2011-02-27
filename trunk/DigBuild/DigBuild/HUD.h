#pragma once
#include "EventHandler.h"
#include <SDL_ttf.h>
#include <string>

class HUD : public Movable, public Inputable
{
public:
  HUD();
  ~HUD();
  void Draw();
  void Update(unsigned int ticks);
  void HandleInput(const SDL_Event& event);
  
  void InitText();
  void FreeText();
private:
  void DrawText(int x, int y, std::string text);

  TTF_Font* font;
  std::string fps;
  bool showFps;
};
