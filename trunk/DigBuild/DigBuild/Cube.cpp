#include "Cube.h"

Cube::Cube(Vector3 pos, unsigned int color)
{
	m_uColor = color;

	m_rPos = pos;
	m_rRot = Vector3();

  drawMode = GL_QUADS;
  RotateDirection = 1;
  rotation = 0;
}

Cube::~Cube()
{
}

void Cube::HandleInput(const SDL_Event& event)
{
  if (event.type == SDL_KEYDOWN && event.key.keysym.sym == SDLK_r)
  {
		if (drawMode == GL_QUADS)
		{
			drawMode = GL_LINE_LOOP;
		}
		else
		{
			drawMode = GL_QUADS;
		}
  }
}

void Cube::Update(unsigned int ticks)
{
  float rotateStep = RotateDirection * 0.1f * ticks;

	rotation += rotateStep;
  while (rotation >= 360)
	  rotation -= 360;

	SetRot(Vector3(0, rotation, 0));
}

void Cube::Draw()
{
	glPushMatrix();

	glTranslatef(m_rPos.X(), m_rPos.Y(), m_rPos.Z());
	glRotatef(m_rRot.X(), 1, 0, 0);
	glRotatef(m_rRot.Y(), 0, 1, 0);
	glRotatef(m_rRot.Z(), 0, 0, 1);

	float red = ((m_uColor & 0xff0000) >> 16) / 255;
	float green = ((m_uColor & 0x00ff00) >> 8) / 255;
	float blue = (m_uColor & 0x0000ff) / 255;

	// Top Face
	glBegin(drawMode);
	glColor3f(red, green, blue);
	glVertex3f( 1.0f, 1.0f, -1.0f);
	glVertex3f(-1.0f, 1.0f, -1.0f);
	glVertex3f(-1.0f, 1.0f, 1.0f);
	glVertex3f( 1.0f, 1.0f, 1.0f);
	glEnd();

	// Bottom Face
	glBegin(drawMode);
	glColor3f(red, green, blue);
	glVertex3f( 1.0f, -1.0f, -1.0f);
	glVertex3f(-1.0f, -1.0f, -1.0f);
	glVertex3f(-1.0f, -1.0f, 1.0f);
	glVertex3f( 1.0f, -1.0f, 1.0f);
	glEnd();

	// Front Face
	glBegin(drawMode);
	glColor3f(red, green, blue);
	glVertex3f( 1.0f, 1.0f, 1.0f);
	glVertex3f(-1.0f, 1.0f, 1.0f);
	glVertex3f(-1.0f, -1.0f, 1.0f);
	glVertex3f( 1.0f, -1.0f, 1.0f);
	glEnd();

	// Back Face
	glBegin(drawMode);
	glColor3f(red, green, blue);
	glVertex3f( 1.0f, 1.0f, -1.0f);
	glVertex3f(-1.0f, 1.0f, -1.0f);
	glVertex3f(-1.0f, -1.0f, -1.0f);
	glVertex3f( 1.0f, -1.0f, -1.0f);
	glEnd();

	//Left Face
	glBegin(drawMode);
	glColor3f(red, green, blue);
	glVertex3f(-1.0f, 1.0f, 1.0f);
	glVertex3f(-1.0f, 1.0f, -1.0f);
	glVertex3f(-1.0f, -1.0f, -1.0f);
	glVertex3f(-1.0f, -1.0f, 1.0f);
	glEnd();

	// Right Face
	glBegin(drawMode);
	glColor3f(red, green, blue);
	glVertex3f( 1.0f, 1.0f, 1.0f);
	glVertex3f( 1.0f, 1.0f, -1.0f);
	glVertex3f( 1.0f, -1.0f, -1.0f);
	glVertex3f( 1.0f, -1.0f, 1.0f);
	glEnd();

	glPopMatrix();
}
