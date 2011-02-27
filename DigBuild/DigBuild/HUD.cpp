#include "HUD.h"
#include "Screen.h"
#include <sstream>

HUD::HUD()
{
  InitText();
  showFps = false;
}

HUD::~HUD()
{
  FreeText();
}

void HUD::Draw()
{
  if (!font) return;

  glMatrixMode(GL_PROJECTION);
  glPushMatrix();
  glLoadIdentity();
  glOrtho(0, Screen::Width, 0, Screen::Height, -1, 1);

  glMatrixMode(GL_MODELVIEW);
  glPushMatrix();
  glLoadIdentity();

  glEnable(GL_TEXTURE_2D);
  glEnable(GL_BLEND);

  glColor3f(1.0f, 1.0f, 1.0f);

  if (showFps)
  {
    DrawText(5, 5, fps);
  }

  glDisable(GL_BLEND);
  glDisable(GL_TEXTURE_2D);

  glMatrixMode(GL_PROJECTION);
  glPopMatrix();   
  glMatrixMode(GL_MODELVIEW);
  glPopMatrix();
}

void HUD::Update(unsigned int ticks)
{
  if (showFps)
  {
    std::stringstream ss (std::stringstream::in | std::stringstream::out);
    float frames = 1000.0f / ticks;
    ss << frames;
    fps = ss.str();
  }
}

// http://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
static unsigned int nextPowerOfTwo(unsigned int v)
{
  v--;
  v |= v >> 1;
  v |= v >> 2;
  v |= v >> 4;
  v |= v >> 8;
  v |= v >> 16;
  v++;
  return v;
}

void HUD::DrawText(int x, int y, std::string text)
{
  SDL_Color color = { 255, 255, 255 };
  SDL_Surface* textSurface = TTF_RenderText_Blended(font, text.c_str(), color);

  int w = nextPowerOfTwo(textSurface->w);
  int h = nextPowerOfTwo(textSurface->h);

  SDL_Surface* destSurface;
  destSurface = SDL_CreateRGBSurface(0, w, h, 32, 0x00ff0000, 0x0000ff00, 0x000000ff, 0xff000000);

  SDL_BlitSurface(textSurface, 0, destSurface, 0);

  GLuint texture;
  glGenTextures(1, &texture);
  glBindTexture(GL_TEXTURE_2D, texture);
  glTexImage2D(GL_TEXTURE_2D, 0, 4, w, h, 0, GL_BGRA, GL_UNSIGNED_BYTE, destSurface->pixels);

  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

  y = Screen::Height - y - h;

  glBegin(GL_QUADS);
  /* Recall that the origin is in the lower-left corner
  That is why the TexCoords specify different corners
  than the Vertex coors seem to. */
  glTexCoord2f(0.0f, 1.0f); 
  glVertex2f(x, y);
  glTexCoord2f(1.0f, 1.0f); 
  glVertex2f(x + w, y);
  glTexCoord2f(1.0f, 0.0f); 
  glVertex2f(x + w, y + h);
  glTexCoord2f(0.0f, 0.0f); 
  glVertex2f(x, y + h);
  glEnd();

  glFinish();

  SDL_FreeSurface(textSurface);
  SDL_FreeSurface(destSurface);
  glDeleteTextures(1, &texture);
}

void HUD::InitText()
{
  font = 0;
  if (TTF_Init() == 0)
  {
    font = TTF_OpenFont("VeraMono.ttf", 16);
    if (font)
    {
      glBlendFunc(GL_ONE, GL_ONE);
    }
  }
}

void HUD::FreeText()
{
  if (font)
  {
    TTF_CloseFont(font);
  }
  if (TTF_WasInit())
  {
    TTF_Quit();
  }
}

void HUD::HandleInput(const SDL_Event& event)
{
  if (event.type == SDL_KEYDOWN && event.key.keysym.sym == SDLK_u)
  {
   showFps = !showFps;
  }
}