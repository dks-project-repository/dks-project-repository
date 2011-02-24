#include "Player.h"
#include "Screen.h"
#include "Utils.h"
#include <math.h>

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

	mouseLook = MOUSELOOK_DISABLED;
	SDL_GrabMode(SDL_GRAB_OFF);
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
      //case SDLK_m:
      //  if (event.type == SDL_KEYDOWN)
      //  {
      //    mouseLook = mouseLook ? MOUSELOOK_DISABLED : MOUSELOOK_ENABLED;
      //    SDL_GrabMode(mouseLook ? SDL_GRAB_ON : SDL_GRAB_OFF);
      //  }
      //  break;
    }
  }
  else if (mouseLook && event.type == SDL_MOUSEMOTION)
  {
    if (mouseLook == MOUSELOOK_ENABLED)
    {
      rotation += -0.2f * event.motion.xrel;
      mouseLook = MOUSELOOK_WARPING;
      SDL_WarpMouse(Screen::Width / 2, Screen::Height / 2);
    }
    else
    {
      mouseLook = MOUSELOOK_ENABLED;
    }
  }
}

void Player::Update(unsigned int ticks)
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
	//if (movement.rotateLeft)
	//{
	//	rotation += rotateStep;
	//}
	//if (movement.rotateRight)
	//{
	//	rotation -= rotateStep;
	//}
	Vector3 lookDirection = Vector3(sin(rotation * Utils::PIOVER180), 0, cos(rotation * Utils::PIOVER180));
	if (movement.forward)
	{
		position += lookDirection;
	}
	if (movement.backward)
	{
		position -= lookDirection;
	}

	camera.SetRotation(rotation);
	camera.SetPosition(position);
	
	camera.Update(ticks);

	if (!mouseLook)
	{
		mouseLook = MOUSELOOK_ENABLED;
		SDL_GrabMode(SDL_GRAB_ON);
	}
}

void Player::Draw()
{
	camera.Draw();
}