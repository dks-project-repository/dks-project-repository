#pragma once
#include "Interfaces.h"

class Camera : public Movable
{
public:
	Camera(void);
	void Update(unsigned int ticks);
	void Draw();

	void SetRotation(float rot);
	void SetPosition(Vector3& pos);

	float GetRotation();	
	Vector3& GetPosition();

private:
	//TODO: use a matrix instead.
	Vector3 position;
	float rotation;
	//movement_t movement;
	//Uint8 mouseLook;
};
