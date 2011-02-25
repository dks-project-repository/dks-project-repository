#pragma once
#include "EventHandler.h"
#include <SDL_opengl.h>

class Cube : public Movable, public Inputable
{
public:
	Cube(Vector3 pos, unsigned int color = 0xffffff);
	~Cube();

	unsigned int GetColor()					{ return m_uColor; }
	void SetColor(unsigned int color)		{ m_uColor = color; }

  void HandleInput(const SDL_Event& event);
  void Update(unsigned int ticks);
	void Draw();

	Vector3 Pos()							{ return m_rPos; }
	Vector3 Rot()							{ return m_rRot; }

	void SetPos(Vector3 pos)				{ m_rPos = pos; }
	void SetRot(Vector3 rot)				{ m_rRot = rot; }

  int RotateDirection;

private:
	unsigned int m_uColor;
  unsigned int drawMode;
  float rotation;

	Vector3 m_rPos;
	Vector3 m_rRot;


	void Rotate();
};