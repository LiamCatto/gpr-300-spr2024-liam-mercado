#version 450

out vec4 FragColor;

in vec2 UV;

uniform sampler2D _ColorBuffer;
uniform float sharpenFactor;
uniform int invertColor;
uniform int sharpen;

void main(){
	vec3 totalColor = vec3(0);
	vec2 texelSize = 1.0 / textureSize(_ColorBuffer, 0).xy;

	if (UV.x < 0.5 && sharpen == 1) {
		// Sharpen
		vec3 up = texture(_ColorBuffer, UV + (vec2(0, -1) / texelSize)).rgb * -1;
		vec3 left = texture(_ColorBuffer, UV + (vec2(-1, 0) / texelSize)).rgb * -1;
		vec3 center = texture(_ColorBuffer, UV + (vec2(0, 0) / texelSize)).rgb;
		vec3 right = texture(_ColorBuffer, UV + (vec2(1, 0) / texelSize)).rgb * -1;
		vec3 down = texture(_ColorBuffer, UV + (vec2(0, 1) / texelSize)).rgb * -1;

		totalColor = (5 * sharpenFactor) - sharpenFactor * (up + left + right + down);
		
		// Edge Detection
		for (int y = -1; y <= 1; y++)
		{
			for (int x = -1; x <= 1; x++)
			{
				vec2 offset = (vec2(x, y) / texelSize);
				if (vec2(x, y) == vec2 (0, 0)) totalColor += texture(_ColorBuffer, UV + offset).rgb * 8;
				else totalColor += texture(_ColorBuffer, UV + offset).rgb * -1;
			}
		}

		// Gamma Correction
		totalColor = pow(totalColor, vec3(10));

	} else totalColor = texture(_ColorBuffer, UV).rgb;
	
	// Color inversion
	if (invertColor == 1) totalColor = 1.0 - totalColor;

	FragColor = vec4(totalColor, 1.0);
}
