#include "Camera.h"
#include "Screen.h"
#include "Utils.h"
#include <math.h>

Camera::Camera()
{
  position = Vector3();
  rotation = 0.0f;
}


void Camera::Update(unsigned int ticks)
{

}

void Camera::Draw()
{
  Vector3 lookDirection = Vector3(sin(rotation * Utils::PIOVER180), 0, cos(rotation * Utils::PIOVER180));
  lookDirection += position;
  gluLookAt(position.X(), position.Y(), position.Z(),
            lookDirection.X(), lookDirection.Y(), lookDirection.Z(),
            0.0, 1.0, 0.0);
}

void Camera::SetPosition(Vector3& pos)
{
	position = pos;
}

Vector3& Camera::GetPosition()
{
	return position;
}

void Camera::SetRotation(float rot)
{
	rotation = rot;
}

float Camera::GetRotation()
{
	return rotation;
}
