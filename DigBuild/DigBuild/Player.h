#pragma once
#include "EventHandler.h"
#include "Camera.h"

struct movement_t {
  bool forward:1;
  bool backward:1;
  bool rotateLeft:1;
  bool rotateRight:1;
  bool up:1;
  bool down:1;
  bool strafeLeft:1;
  bool strafeRight:1;
};

enum MouseLook
{
  MOUSELOOK_DISABLED,
  MOUSELOOK_ENABLED,
  MOUSELOOK_WARPING,
  MOUSELOOK_SUSPENDED
};

class Player : public Inputable, public Movable
{
public:
	Player(void);
	void HandleInput(const SDL_Event& event);
	void Update(unsigned int ticks);
	void Draw();

private:
	Vector3 position;
	float rotation;
	movement_t movement;
	MouseLook mouseLook;

	Camera camera;

  void setMouseLook(MouseLook state);
};