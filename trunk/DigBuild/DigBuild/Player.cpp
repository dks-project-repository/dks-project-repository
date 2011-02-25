#include "Player.h"
#include "Screen.h"
#include "Utils.h"
#include <math.h>

#ifdef _WIN32
#include <SDL_syswm.h>
#endif

Player::Player() : 
camera()
{
  position = Vector3(0, 0, -20);
  rotation = 0.0f;

  movement.forward = false;
  movement.backward = false;
  movement.rotateLeft = false;
  movement.rotateRight = false;
  movement.up = false;
  movement.down = false;
  movement.strafeLeft = false;
  movement.strafeRight = false;

  mouseLook = MOUSELOOK_WARPING;
  SDL_WM_GrabInput(SDL_GRAB_ON);
  SDL_ShowCursor(SDL_DISABLE);

#ifdef _WIN32
  SDL_EventState(SDL_SYSWMEVENT, SDL_ENABLE);
#endif
}

void Player::HandleInput(const SDL_Event& event)
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
      movement.strafeLeft = event.type == SDL_KEYDOWN ? 1 : 0;
      break;
    case SDLK_d:
      movement.strafeRight = event.type == SDL_KEYDOWN ? 1 : 0;
      break;
    case SDLK_q:
      movement.up = event.type == SDL_KEYDOWN ? 1 : 0;
      break;
    case SDLK_e:
      movement.down = event.type == SDL_KEYDOWN ? 1 : 0;
      break;
    case SDLK_m:
      if (event.type == SDL_KEYDOWN)
      {
        setMouseLook(mouseLook ? MOUSELOOK_DISABLED : MOUSELOOK_WARPING);
      }
      break;
    }
  }
  else if (mouseLook && event.type == SDL_MOUSEMOTION)
  {
    if (mouseLook == MOUSELOOK_ENABLED)
    {
      rotation += -0.2f * event.motion.xrel;
    }
    else if (mouseLook == MOUSELOOK_WARPING)
    {
      mouseLook = MOUSELOOK_ENABLED;
    }
  }
#ifdef _WIN32
  else if (event.type == SDL_SYSWMEVENT)
  {
    SDL_SysWMmsg* msg = event.syswm.msg;
    if (msg->msg == WM_NCACTIVATE)
    {
      int value = msg->wParam;
      if (value)
      {
        if (mouseLook == MOUSELOOK_SUSPENDED)
        {
          setMouseLook(MOUSELOOK_WARPING);
        }
      }
      else if (mouseLook)
      {
        setMouseLook(MOUSELOOK_SUSPENDED);
      }
    }
  }
#endif
}

void Player::setMouseLook(MouseLook state)
{
  mouseLook = state;
  bool enable = state == MOUSELOOK_ENABLED || state == MOUSELOOK_WARPING;
  SDL_WM_GrabInput(enable ? SDL_GRAB_ON : SDL_GRAB_OFF);
  SDL_ShowCursor(enable ? SDL_DISABLE : SDL_ENABLE);
}

void Player::Update(unsigned int ticks)
{
  float rotateStep = 0.08f * ticks;
  float translateStep = 0.03f * ticks;

  if (movement.up)
  {
    position.set_Y(position.Y() + translateStep);
  }
  if (movement.down)
  {
    position.set_Y(position.Y() - translateStep);
  }
  //if (movement.rotateLeft)
  //{
  //	rotation += rotateStep;
  //}
  //if (movement.rotateRight)
  //{
  //	rotation -= rotateStep;
  //}
  Vector3 lookDirection = Vector3(sin(rotation * Utils::PIOVER180), 0, cos(rotation * Utils::PIOVER180));
  Vector3 sideDirection = lookDirection.cross(Vector3(0, 1, 0));
  if (movement.forward)
  {
    position += lookDirection;
  }
  if (movement.backward)
  {
    position -= lookDirection;
  }
  if (movement.strafeRight)
  {
    position += sideDirection;
  }
  if (movement.strafeLeft)
  {
    position -= sideDirection;
  }

  camera.SetRotation(rotation);
  camera.SetPosition(position);

  camera.Update(ticks);
}

void Player::Draw()
{
  camera.Draw();
}