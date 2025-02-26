#version 450

layout (location = 0) in vec3 vPos;

uniform mat4 lightSpaceMatrix;
uniform mat4 model;

out vec4 lightSpacePos; //Sent to fragment shader

void main(){
	lightSpacePos = lightSpaceMatrix * model * vec4(vPos, 1);
}
