#pragma once
#include "Interfaces.h"

struct movement_t {
  bool forward:1;
  bool backward:1;
  bool rotateLeft:1;
  bool rotateRight:1;
  bool up:1;
  bool down:1;
};

class Camera : public Inputable, public Movable
{
public:
  Camera(void);
  void HandleInput(const SDL_Event& event);
  void Update(unsigned int ticks);
  void Draw();
private:
  //TODO: use a matrix instead.
  Vector3 position;
  float rotation;
  movement_t movement;
};
