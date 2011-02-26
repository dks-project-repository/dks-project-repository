#pragma once
#include "EventHandler.h"

class Camera : public Movable
{
public:
	Camera(void);
	void Update(unsigned int ticks);
	void Draw();

	void SetYaw(float rot);
	void SetPosition(const Vector3& pos);
  void SetPitch(float pit);

	float GetYaw();	
	const Vector3& GetPosition();
  float GetPitch();

private:
	//TODO: use a matrix instead.
	Vector3 position;
	float yaw;
  float pitch;
	//movement_t movement;
	//Uint8 mouseLook;
};
