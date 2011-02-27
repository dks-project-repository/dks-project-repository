#include "EventHandler.h"

std::vector<Drawable*> ObjectLists::drawables;
std::vector<Movable*> ObjectLists::movables;
std::vector<Inputable*> ObjectLists::inputables;
std::vector<EventHandler*> ObjectLists::toBeRemoved;

EventHandler::EventHandler()
{
  handlerState = HANDLER_OFF;
}

void EventHandler::Add()
{
  Drawable* d = dynamic_cast<Drawable*>(this);
  Movable* m = dynamic_cast<Movable*>(this);
  Inputable* i = dynamic_cast<Inputable*>(this);
  if (d)
  {
    ObjectLists::drawables.push_back(d);
  }
  if (m)
  {
    ObjectLists::movables.push_back(m);
  }
  if (i)
  {
    ObjectLists::inputables.push_back(i);
  }

  handlerState = HANDLER_ON;
}

void EventHandler::Remove()
{
  ObjectLists::toBeRemoved.push_back(this);
  handlerState = HANDLER_REMOVING;
}

void EventHandler::CompleteRemove()
{
  if (HandlerState() != HANDLER_REMOVING)
  {
    return;
  }
  
  Drawable* d = dynamic_cast<Drawable*>(this);
  Movable* m = dynamic_cast<Movable*>(this);
  Inputable* i = dynamic_cast<Inputable*>(this);
  if (d)
  {
    std::vector<Drawable*>::iterator it;
    for (it=ObjectLists::drawables.begin(); it < ObjectLists::drawables.end(); it++)
    {
      if (*it == d)
      {
        ObjectLists::drawables.erase(it);
        break;
      }
    }
  }
  if (m)
  {
    std::vector<Movable*>::iterator it;
    for (it=ObjectLists::movables.begin(); it < ObjectLists::movables.end(); it++)
    {
      if (*it == m)
      {
        ObjectLists::movables.erase(it);
        break;
      }
    }
  }
  if (i)
  {
    std::vector<Inputable*>::iterator it;
    for (it=ObjectLists::inputables.begin(); it < ObjectLists::inputables.end(); it++)
    {
      if (*it == i)
      {
        ObjectLists::inputables.erase(it);
        break;
      }
    }
  }

  handlerState = HANDLER_OFF;
}

void EventHandler::OneFunctionMustBeVirtual()
{
}
