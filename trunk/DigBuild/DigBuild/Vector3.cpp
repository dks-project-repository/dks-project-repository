#include "Vector3.h"
#include <math.h>

Vector3::Vector3()
{
  x = y = z = 0;
}

Vector3::Vector3(float xx, float yy, float zz)
{
  x = xx;
  y = yy;
  z = zz;
}

float Vector3::length()
{
  return sqrtf(x * x + y * y + z * z);
}

Vector3 Vector3::normalize()
{
  float length = sqrtf(x * x + y * y + z * z);
  if (length == 0)
  {
    return Vector3(0, 0, 0);
  }
  return Vector3(x / length, y / length, z / length);
}

float Vector3::dot(const Vector3& v)
{
  return x * v.x + y * v.y + z * v.z;
}

Vector3 Vector3::cross(const Vector3& v)
{
  return Vector3(
    y * v.z - z * v.y,
    z * v.x - x * v.z,
    x * v.y - y * v.x
    );
}

Vector3 Vector3::operator +(const Vector3& v)
{
  return Vector3(
    x + v.x,
    y + v.y,
    z + v.z
    );
}

Vector3 Vector3::operator -(const Vector3& v)
{
  return Vector3(
    x - v.x,
    y - v.y,
    z - v.z
    );
}

void Vector3::operator +=(const Vector3& v)
{
    x += v.x;
    y += v.y;
    z += v.z;
}

void Vector3::operator -=(const Vector3& v)
{
    x -= v.x;
    y -= v.y;
    z -= v.z;
}

float* Vector3::get3f(float* arr)
{
  arr[0] = x;
  arr[1] = y;
  arr[2] = z;
  return arr;
}
