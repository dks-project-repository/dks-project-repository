#include "Player.h"
#include "Cube.h"
#include "EventHandler.h"
#include "HUD.h"
#include "Screen.h"

#include <math.h>
#include <vector>

#ifdef _WIN32
#include <SDL_syswm.h>
#endif

bool isQuit = false;
bool isPaused = false;
HUD* hud = 0;

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

int myEventFilter(const SDL_Event* event)
{
#ifdef _WIN32
  if (event->type == SDL_SYSWMEVENT && event->syswm.msg->msg != WM_NCACTIVATE)
  {
    return 0;
  }
#endif

  return 1;
}

bool init()
{
	if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER) != 0)
		return false;

	SDL_WM_SetCaption("Dig Build", 0);

	if (SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1) != 0)
		return false;

  SDL_SetEventFilter(myEventFilter);

  Screen* screen = new Screen();
  screen->Add();

  if (!Screen::Resize())
		return false;

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

void handleEvent(SDL_Event const& event)
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
  else if (isPaused && event.type == SDL_MOUSEMOTION)
  {
    return;
  }

  std::vector<Inputable*>::iterator it;
  for (it=ObjectLists::inputables.begin(); it < ObjectLists::inputables.end(); it++)
  {
    (*it)->HandleInput(event);
  }

  for (int i = ObjectLists::toBeRemoved.size() - 1; i >= 0; i--)
  {
    ObjectLists::toBeRemoved[i]->CompleteRemove();
  }
  ObjectLists::toBeRemoved.clear();
}

void handleInput()
{
	SDL_Event event;

  while(isPaused && !isQuit)
  {
    if (SDL_WaitEvent(&event))
    {
      handleEvent(event);
    }
  }

	while (SDL_PollEvent(&event))
	{
    handleEvent(event);
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

  hud = new HUD();
  hud->Add();
}

int main(int argc, char* args[])
{
	if (!init())
		return 1;

  buildScene();

  Uint32 numTicks;
  Uint32 lastTicks;
  Uint32 nowTicks = 0;

	while (!isQuit)
	{
    // Step 1: handle input
    handleInput();

    // Step 2: handle animation
    lastTicks = nowTicks; 
    while ((nowTicks = SDL_GetTicks()) - lastTicks < Screen::MaxFpsInverse) {}
    numTicks = nowTicks - lastTicks;
    update(numTicks);

    // Step 3: handle drawing
    draw();
	}

  if (hud)
  {
    delete hud;
  }
	SDL_Quit();

  //TODO: clean up the data in our vectors? This would require a change to shared pointers

	return 0;
}


