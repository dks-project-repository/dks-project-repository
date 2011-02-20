#include "Vector3.h"

#include <SDL_opengl.h>

class Cube
{
public:
	Cube(Vector3 pos, unsigned int color = 0xffffff);
	~Cube();

	unsigned int GetColor()					{ return m_uColor; }
	void SetColor(unsigned int color)		{ m_uColor = color; }

	virtual void Tick();
	virtual void Draw(unsigned int drawMode);

	Vector3 Pos()							{ return m_rPos; }
	Vector3 Rot()							{ return m_rRot; }

	void SetPos(Vector3 pos)				{ m_rPos = pos; }
	void SetRot(Vector3 rot)				{ m_rRot = rot; }

private:
	unsigned int m_uColor;

	Vector3 m_rPos;
	Vector3 m_rRot;

	void Rotate();
};