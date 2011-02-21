class Vector3
{
public:
  Vector3();
  Vector3(float xx, float yy, float zz);

  Vector3 normalize();
  float dot(Vector3& v);
  Vector3 cross(Vector3& v);
  Vector3 operator +(Vector3& v);
  Vector3 operator -(Vector3& v);
  float* get3f(float* arr);

	float X()		{ return x; }
	float Y()		{ return y; }
	float Z()		{ return z; }
private:
  float x, y, z;
};
