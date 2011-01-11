#include <SDL.h>
#include <SDL_opengl.h>

#include <math.h>
#define PI (3.141592653589793238)

bool isQuit = false;
bool isPaused = false;

int screenWidth = 800;
int screenHeight = 600;
int screenAltWidth = 0;
int screenAltHeight = 0;
bool screenIsFull = false;

float test = 0.0f;
int stepNum = 0;


float smoothStepC2(float min, float max, float x)
{
	x = (x - min) / (max - min);
	float PI2 = 2 * PI;
	return x - sin(x * PI2) / PI2;
}

float smoothStepC1(float min, float max, float x)
{
	x = (x - min) / (max - min);
	return x * x * (3 - 2 * x);
}

float lerp(float min, float max, float x)
{
	return (x - min) / (max - min);
}

void draw()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();

	glTranslatef(-1.20710678f, test, -6.0f);

	glBegin(GL_TRIANGLES);
	glColor3f ( 1.0f, 0.0f, 0.0f);
	glVertex3f( 0.0f, 1.0f, 0.0f);
	glColor3f ( 0.0f, 1.0f, 0.0f);
	glVertex3f(-1.20710678f, -1.0f, 0.0f);
	glColor3f ( 0.0f, 0.0f, 1.0f);
	glVertex3f( 1.20710678f, -1.0f, 0.0f);
	glEnd();							

	glTranslatef(2.41421356f, -2 * test, 0.0f);
	glColor3f(0.5f, 0.5f, 0.5f);

	glBegin(GL_TRIANGLES);
	glColor3f ( 1.0f, 0.0f, 0.0f);
	glVertex3f( 0.0f, -1.0f, 0.0f);
	glColor3f ( 0.0f, 0.0f, 1.0f);
	glVertex3f(-1.20710678f, 1.0f, 0.0f);
	glColor3f ( 0.0f, 1.0f, 0.0f);
	glVertex3f( 1.20710678f, 1.0f, 0.0f);
	glEnd();										

	SDL_GL_SwapBuffers();
}



bool resize(int width, int height, bool isFullscreen)
{
	if (SDL_SetVideoMode(width, height, 32, SDL_OPENGL | (isFullscreen ? SDL_FULLSCREEN : 0)) == NULL)
		return false;

	glViewport(0, 0, width, height);

	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	gluPerspective(45.0, static_cast<double>(width) / static_cast<double>(height), 0.1, 100.0);

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	glEnable(GL_DEPTH_TEST);

	if (isPaused)
		draw();

	if(glGetError() != GL_NO_ERROR)
		return false;

	return true;
}



bool init()
{
	if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER) != 0)
		return false;

	SDL_Rect** modes = SDL_ListModes(NULL, SDL_OPENGL | SDL_FULLSCREEN);
	for (int i = 0; modes[i] != 0; i++)
	{
		if (modes[i]->w > screenAltWidth)
			screenAltWidth = modes[i]->w;
		if (modes[i]->h > screenAltHeight)
			screenAltHeight = modes[i]->h;
	}

	SDL_WM_SetCaption("Toroid Race", 0);

	if (SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1) != 0)
		return false;

	if (!resize(screenWidth, screenHeight, false))
		return false;

	SDL_ShowCursor(SDL_DISABLE);

	return true;
}



void tick()
{
	const double step = PI / 100;
	test = sin(step * stepNum);
	stepNum++;
}



void handleInput()
{
	SDL_Event event;

	while (SDL_PollEvent(&event))
	{
		if (event.type == SDL_QUIT)
		{
			isQuit = true; 
		}
		else if (event.type == SDL_KEYDOWN)
		{
			switch (event.key.keysym.sym)
			{
			case SDLK_ESCAPE:
				isQuit = true;
				break;
			case SDLK_f:
				screenIsFull = !screenIsFull;
				if (screenIsFull)
					resize(screenAltWidth, screenAltHeight, screenIsFull);
				else
					resize(screenWidth, screenHeight, screenIsFull);
				break;
			case SDLK_p:
				isPaused = !isPaused;
				break;
			}
		}
	}
}

int main(int argc, char* args[])
{
	if (!init())
		return 1;

	const Uint32 tickMs = 1000 / 60;
	Uint32 next = SDL_GetTicks() + tickMs;

	while (!isQuit)
	{
		handleInput();

		if (isPaused)
		{
			next += 250;
			SDL_Delay(250);
			continue;
		}

		while (next <= SDL_GetTicks())
		{
			tick();
			next += tickMs;
		}

		draw();

		while (next > SDL_GetTicks())
			;
	}

	SDL_Quit();

	return 0;
}


