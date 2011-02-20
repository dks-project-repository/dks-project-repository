#include "Cube.h"

#include <SDL.h>
#include <SDL_opengl.h>

#include <math.h>
#include <stdio.h>

const float PI = 3.141592653589793238f;

bool isQuit = false;
bool isPaused = false;

int screenWidth = 800;
int screenHeight = 600;
int screenAltWidth = 0;
int screenAltHeight = 0;
bool screenIsFull = false;

int test = 0;
int stepNum = 0;

const float numCubes = 2;
Cube* Cubes[2];

const float cameraRotStep = PI / 180.0f;
float cameraRot = 0;
Vector3 cameraPos = Vector3(0, 0, -12);

unsigned int drawMode;

void draw()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();
	
	glTranslatef(cameraPos.X(), cameraPos.Y(), cameraPos.Z());
	glRotatef(cameraRot, 0, 1, 0);

	for (int i = 0; i < numCubes; i++)
	{
		Cubes[i]->Draw(drawMode);
	}

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

	SDL_WM_SetCaption("Dig Build", 0);

	if (SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1) != 0)
		return false;

	if (!resize(screenWidth, screenHeight, false))
		return false;

	SDL_ShowCursor(SDL_DISABLE);
	SDL_EnableKeyRepeat(10, 10);

	return true;
}



void tick()
{
	const int step = 1;
	test += step;
	test %= 360;

	Cubes[0]->SetRot(Vector3(0, test, 0));
	Cubes[1]->SetRot(Vector3(0, -test, 0));
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
			case SDLK_r:
				if (drawMode == GL_QUADS)
				{
					drawMode = GL_LINE_LOOP;
				}
				else
				{
					drawMode = GL_QUADS;
				}
			break;
			// Camera Controls
			case SDLK_w:
				cameraPos = Vector3(cameraPos.X() + 1 * sin(cameraRot), cameraPos.Y(), cameraPos.Z() + 1 * cos(cameraRot));
				break;
			case SDLK_s:
				cameraPos = Vector3(cameraPos.X() - 1 * sin(cameraRot), cameraPos.Y(), cameraPos.Z() - 1 * cos(cameraRot));
				break;
			case SDLK_a:
				cameraRot -= cameraRotStep;
				break;
			case SDLK_d:
				cameraRot += cameraRotStep;
				break;
			case SDLK_q:
				cameraPos = Vector3(cameraPos.X(), cameraPos.Y() - 1, cameraPos.Z());
				break;
			case SDLK_e:
				cameraPos = Vector3(cameraPos.X(), cameraPos.Y() + 1, cameraPos.Z());
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

	drawMode = GL_QUADS;

	Cubes[0] = new Cube(Vector3(-sqrt(2.0f), 0, 0), 0xff0000);
	Cubes[1] = new Cube(Vector3(sqrt(2.0f), 0, 0), 0x0000ff);

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

		while (next > SDL_GetTicks());
	}

	SDL_Quit();

	return 0;
}


