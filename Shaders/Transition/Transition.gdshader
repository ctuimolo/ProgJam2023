shader_type canvas_item;

// An input into the shader from our game code.
// Ranges from 0 to 1 over the course of the transition.
// We use this to actually animate the shader.
uniform float progress : hint_range(0, 1);

// Size of each diamond, in pixels.
uniform float diamondPixelSize = 32.0;

uniform bool inverseDraw = false;
uniform bool directionX = false;
uniform bool directionY = false;

void fragment() {
	float _scale = 1.0;
	float _progress = progress;
	float y = FRAGCOORD.y;
	float x = FRAGCOORD.x;
	
    float xFraction = fract(x / diamondPixelSize);
    float yFraction = fract(y / diamondPixelSize);
    
    float xDistance = abs(xFraction - 0.5);
    float yDistance = abs(yFraction - 0.5);
    
	float UVxy = 0.0;
	
	if (directionX || directionY) _scale = 2.0;
	
	if (directionX) UVxy += UV.x;
	if (directionY) UVxy += UV.y;
	
	if (inverseDraw) {
		_progress = 1.0 - _progress;
	    if (xDistance + yDistance + UVxy < _progress * _scale) {
	        discard;
	    }
	} else {
	    if (xDistance + yDistance + UVxy > progress * _scale) {
	        discard;
	    }
	}
}