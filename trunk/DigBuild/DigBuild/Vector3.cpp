#include "Vector3.h"

Vector3::Vector3()
{
	xyz[0] = xyz[1] =xyz[2] = 0;
}

Vector3::Vector3(float x, float y, float z)
{
	xyz[0] = x;
	xyz[1] = y;
	xyz[2] = z;
}

Vector3::~Vector3()
{
}