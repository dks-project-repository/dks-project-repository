#include "Camera.h"
#include "Cube.h"
#include "Interfaces.h"

#include <math.h>
#include <vector>

const float PI = 3.141592653589793238f;

bool isQuit = false;
bool isPaused = false;

int screenWidth = 800;
int screenHeight = 600;
int screenAltWidth = 0;
int screenAltHeight = 0;
bool screenIsFull = false;

std::vector<Drawable*> drawables;
std::vector<Movable*> movables;
std::vector<Inputable*> inputables;

void draw()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();
	
  int size = drawables.size();
  for (int i = 0; i < size; i++)
  {
    drawables[i]->Draw();
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

  drawables = std::vector<Drawable*>();
  movables = std::vector<Movable*>();
  inputables = std::vector<Inputable*>();

	return true;
}

void update(unsigned int numTicks)
{
  int size = movables.size();
  for (int i = 0; i < size; i++)
  {
    movables[i]->Update(numTicks);
  }
}

void handleInput()
{
	SDL_Event event;
  int size = inputables.size();

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

    for (int i = 0; i < size; i++)
    {
      inputables[i]->HandleInput(event);
    }
  }
}

void buildScene()
{
	Camera* camera = new Camera();
  inputables.push_back(camera);
  movables.push_back(camera);
  drawables.push_back(camera);
  
  Cube* cube = new Cube(Vector3(-sqrt(2.0f), 0, 0), 0xff0000);
  cube->RotateDirection = 1;
  inputables.push_back(cube);
  movables.push_back(cube);
  drawables.push_back(cube);

	cube = new Cube(Vector3(sqrt(2.0f), 0, 0), 0x0000ff);
  cube->RotateDirection = -1;
  inputables.push_back(cube);
  movables.push_back(cube);
  drawables.push_back(cube);
}

int main(int argc, char* args[])
{
	if (!init())
		return 1;

  buildScene();

	const Uint32 frameRateDelay = 1000 / 60; //capped at 60fps

  Uint32 numTicks;
  Uint32 lastTicks;
  Uint32 nowTicks = 0;

	while (!isQuit)
	{
    // Step 1: handle input
    handleInput();

    // Step 2: handle pause
		if (isPaused)
		{
			SDL_Delay(250);
      nowTicks = SDL_GetTicks();
			continue;
		}

    // Step 3: handle animation
    lastTicks = nowTicks; 
    while ((nowTicks = SDL_GetTicks()) - lastTicks < frameRateDelay) {}
    numTicks = nowTicks - lastTicks;
    update(numTicks);

    // Step 4: handle drawing
    draw();
	}

	SDL_Quit();

  //TODO: clean up the data in our vectors? This would require a change to shared pointers

	return 0;
}


