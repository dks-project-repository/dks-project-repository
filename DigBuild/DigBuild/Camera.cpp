#include "Camera.h"
#include <math.h>

const float PIOVER180 = 0.01745329251994329576923690768489f;

Camera::Camera()
{
  position = Vector3(0, 0, -20);
  rotation = 0.0f;

  movement.forward = false;
  movement.backward = false;
  movement.rotateLeft = false;
  movement.rotateRight = false;
  movement.up = false;
  movement.down = false;
}

void Camera::HandleInput(const SDL_Event& event)
{
  if (event.type == SDL_KEYDOWN || event.type == SDL_KEYUP)
  {
    switch (event.key.keysym.sym)
    {
      case SDLK_w:
        movement.forward = event.type == SDL_KEYDOWN ? 1 : 0;
        break;
      case SDLK_s:
        movement.backward = event.type == SDL_KEYDOWN ? 1 : 0;
        break;
      case SDLK_a:
        movement.rotateLeft = event.type == SDL_KEYDOWN ? 1 : 0;
        break;
      case SDLK_d:
        movement.rotateRight = event.type == SDL_KEYDOWN ? 1 : 0;
        break;
      case SDLK_q:
        movement.up = event.type == SDL_KEYDOWN ? 1 : 0;
        break;
      case SDLK_e:
        movement.down = event.type == SDL_KEYDOWN ? 1 : 0;
        break;
    }
  }
}

void Camera::Update(unsigned int ticks)
{
  float rotateStep = 0.08f * ticks;
  float translateStep = 0.04f * ticks;

  if (movement.up)
  {
    position.set_Y(position.Y() + translateStep);
  }
  if (movement.down)
  {
    position.set_Y(position.Y() - translateStep);
  }
  if (movement.rotateLeft)
  {
    rotation += rotateStep;
  }
  if (movement.rotateRight)
  {
    rotation -= rotateStep;
  }
  Vector3 lookDirection = Vector3(sin(rotation * PIOVER180), 0, cos(rotation * PIOVER180));
  if (movement.forward)
  {
    position += lookDirection;
  }
  if (movement.backward)
  {
    position -= lookDirection;
  }
}

void Camera::Draw()
{
  Vector3 lookDirection = Vector3(sin(rotation * PIOVER180), 0, cos(rotation * PIOVER180));
  lookDirection += position;
  gluLookAt(position.X(), position.Y(), position.Z(),
            lookDirection.X(), lookDirection.Y(), lookDirection.Z(),
            0.0, 1.0, 0.0);
}
