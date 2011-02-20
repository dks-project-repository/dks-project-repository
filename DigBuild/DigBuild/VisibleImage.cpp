#include "VisibleImage.h"
#include <SDL_image.h>
#include <SDL_video.h>

#define COLORKEY 255, 0, 255 //Your Transparent colour


VisibleImage::VisibleImage(char* imageFilename)
{

}

VisibleImage::~VisibleImage()
{
}

void VisibleImage::Tick()
{
}

void VisibleImage::Draw()
{
}

SDL_Surface* VisibleImage::LoadImage(char* imageFilename)
{
	SDL_Surface *tmp;
	tmp = IMG_Load(imageFilename);

	if (!tmp)
	{
		fprintf(stderr, "Error: '%s' could not be opened: %s\n", imageFilename, IMG_GetError());
		return NULL;
	} 
	else
	{
		if(SDL_SetColorKey(tmp, SDL_SRCCOLORKEY | SDL_RLEACCEL, SDL_MapRGB(tmp->format, COLORKEY)) == -1)
		{
			fprintf(stderr, "Warning: colorkey will not be used, reason: %s\n", SDL_GetError());
		}
	}
	return tmp;
}
