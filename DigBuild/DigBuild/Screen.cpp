#include "Screen.h"

int Screen::Width = 800;
int Screen::Height = 600;
int Screen::FullWidth;
int Screen::FullHeight;
bool Screen::Fullscreen = false;

Screen::Screen()
{
	SDL_Rect** modes = SDL_ListModes(NULL, SDL_OPENGL | SDL_FULLSCREEN);
	for (int i = 0; modes[i] != 0; i++)
	{
		if (modes[i]->w > FullWidth)
			FullWidth = modes[i]->w;
		if (modes[i]->h > FullHeight)
			FullHeight = modes[i]->h;
	}
}

bool Screen::Resize()
{
  int w = Fullscreen ? FullWidth : Width;
  int h = Fullscreen ? FullHeight : Height;
  
  if (SDL_SetVideoMode(w, h, 32, SDL_OPENGL | SDL_HWSURFACE | SDL_RESIZABLE | (Fullscreen ? SDL_FULLSCREEN : 0)) == NULL)
		return false;

	glViewport(0, 0, w, h);

	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	gluPerspective(45.0, static_cast<double>(w) / static_cast<double>(h), 0.1, 100.0);

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	glEnable(GL_DEPTH_TEST);

	if(glGetError() != GL_NO_ERROR)
		return false;

	return true;
}

void Screen::HandleInput(const SDL_Event& event)
{
  if (event.type == SDL_RESIZABLE)
  {
    Width = event.resize.w;
    Height = event.resize.h;
    Resize();
  }
	else if (event.type == SDL_KEYDOWN)
  {
    switch (event.key.keysym.sym)
    {
    case SDLK_f:
      Fullscreen = !Fullscreen;
      Resize();
    }
  }
}
