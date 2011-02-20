#include <SDL.h>

// Visible Class is the base drawing class. All visible objects inherit off of this class
class VisibleImage
{
	VisibleImage(char* imageFilename);
	~VisibleImage();

public:
	void Tick();
	void Draw();

private:
	SDL_Surface* LoadImage(char* imageFilename);
};