#include "Player.h"
#include "Cube.h"
#include "EventHandler.h"
#include "Screen.h"

#include <math.h>
#include <vector>

bool isQuit = false;
bool isPaused = false;

void draw()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();
	
  std::vector<Drawable*>::iterator it;
  for (it=ObjectLists::drawables.begin(); it < ObjectLists::drawables.end(); it++)
  {
    (*it)->Draw();
  }

	SDL_GL_SwapBuffers();
}

bool init()
{
	if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER) != 0)
		return false;

	SDL_WM_SetCaption("Dig Build", 0);

	if (SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1) != 0)
		return false;

  Screen* screen = new Screen();
  screen->Add();

  if (!Screen::Resize())
		return false;

	SDL_ShowCursor(SDL_DISABLE);

	return true;
}

void update(unsigned int numTicks)
{
  int size = ObjectLists::movables.size();
  for (int i = 0; i < size; i++)
  {
    ObjectLists::movables[i]->Update(numTicks);
  }
  
  std::vector<Movable*>::iterator it;
  for (it=ObjectLists::movables.begin(); it < ObjectLists::movables.end(); it++)
  {
    (*it)->Update(numTicks);
  }
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
      case SDLK_p:
        isPaused = !isPaused;
        break;
      }
    }

    std::vector<Inputable*>::iterator it;
    for (it=ObjectLists::inputables.begin(); it < ObjectLists::inputables.end(); it++)
    {
      (*it)->HandleInput(event);
    }
  }
}

void buildScene()
{
  Player* player = new Player();
  player->Add();
  
  Cube* cube = new Cube(Vector3(-sqrt(2.0f), 0, 0), 0xff0000);
  cube->RotateDirection = 1;
  cube->Add();

  cube = new Cube(Vector3(sqrt(2.0f), 0, 0), 0x0000ff);
  cube->RotateDirection = -1;
  cube->Add();
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


