#pragma once

class Vector3
{
public:
  Vector3();
  Vector3(float xx, float yy, float zz);

  float length();
  Vector3 normalize();
  float dot(const Vector3& v);
  Vector3 cross(const Vector3& v);
  Vector3 operator +(const Vector3& v);
  Vector3 operator -(const Vector3& v);
  void operator +=(const Vector3& v);
  void operator -=(const Vector3& v);
  float* get3f(float* arr);

  float X() { return x; }
  float Y() { return y; }
  float Z() { return z; }

  void set_X(float newx) { x = newx; }
  void set_Y(float newy) { y = newy; }
  void set_Z(float newz) { z = newz; }

private:
  float x, y, z;
};
