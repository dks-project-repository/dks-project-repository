#include "Camera.h"
#include "Screen.h"
#include "Utils.h"
#include <math.h>

Camera::Camera()
{
  position = Vector3();
  yaw = 0.0f;
  pitch = 0.0f;
}


void Camera::Update(unsigned int ticks)
{

}

void Camera::Draw()
{
  Vector3 lookDirection = Vector3(sin(yaw * Utils::PIOVER180) * cos(pitch * Utils::PIOVER180),
                                  sin(pitch * Utils::PIOVER180),
                                  cos(yaw * Utils::PIOVER180) * cos(pitch * Utils::PIOVER180));
  lookDirection += position;
  gluLookAt(position.X(), position.Y(), position.Z(),
            lookDirection.X(), lookDirection.Y(), lookDirection.Z(),
            0.0, 1.0, 0.0);
}

void Camera::SetPosition(const Vector3& pos)
{
	position = pos;
}

const Vector3& Camera::GetPosition()
{
	return position;
}

void Camera::SetYaw(float rot)
{
	yaw = rot;
}

float Camera::GetYaw()
{
	return yaw;
}

void Camera::SetPitch(float pit)
{
	pitch = pit;
}

float Camera::GetPitch()
{
	return pitch;
}
