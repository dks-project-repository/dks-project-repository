#pragma once
#include <SDL.h>
#include <SDL_opengl.h>
#include "Vector3.h"

class Drawable
{
public:
  virtual void Draw() = 0;
protected:
  //TODO: add matrix, or position + quaternion, here
  //http://gpwiki.org/index.php/OpenGL:Tutorials:Using_Quaternions_to_represent_rotation
};

class Movable : public Drawable
{
public:
  virtual void Update(unsigned int ticks) = 0;
};

class Inputable
{
public:
  virtual void HandleInput(const SDL_Event& event) = 0;
};
