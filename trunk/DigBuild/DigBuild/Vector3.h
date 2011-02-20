class Vector3
{
public:
	Vector3();
	Vector3(float x, float y, float z);
	~Vector3();
	
	float X()		{ return xyz[0]; }
	float Y()		{ return xyz[1]; }
	float Z()		{ return xyz[2]; }

private:
	float xyz[3];
};