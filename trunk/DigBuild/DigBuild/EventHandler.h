#pragma once
#include <SDL.h>
#include <SDL_opengl.h>
#include <vector>
#include "Vector3.h"

enum RemoveState
{
  HANDLER_ON,
  HANDLER_REMOVING,
  HANDLER_OFF
};

class EventHandler
{
public:
  EventHandler();
  void Add();
  void Remove();
  void CompleteRemove();
  RemoveState HandlerState() { return handlerState; }
protected:
  virtual void OneFunctionMustBeVirtual();
private:
  RemoveState handlerState;
};

class Drawable : public virtual EventHandler
{
public:
  virtual void Draw() = 0;
protected:
  //TODO: add matrix,or position + quaternion, here
  //http://gpwiki.org/index.php/OpenGL:Tutorials:Using_Quaternions_to_represent_rotation
};

class Movable : public Drawable
{
public:
  virtual void Update(unsigned int ticks) = 0;
};

class Inputable : public virtual EventHandler
{
public:
  virtual void HandleInput(const SDL_Event& event) = 0;
};

namespace ObjectLists
{
  extern std::vector<Drawable*> drawables;
  extern std::vector<Movable*> movables;
  extern std::vector<Inputable*> inputables;
  extern std::vector<EventHandler*> toBeRemoved;
};
