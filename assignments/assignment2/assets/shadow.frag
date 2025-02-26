/*#version 450

in vec4 lightSpacePos;

uniform sampler2D shadowMap;

void main(){
	//Homogeneous Clip space to NDC [-w,w] to [-1,1]
	vec3 sampleCoord = lightSpacePos.xyz / lightSpacePos.w;
	
	//Convert from [-1,1] to [0,1]
	sampleCoord = sampleCoord * 0.5 + 0.5;

	float myDepth = sampleCoord.z; 
	float shadowMapDepth = texture(shadowMap, sampleCoord.xy).r; 
	if (myDepth > shadowMapDepth)
	{
		//In shadow!
		//Ignore direct light contribution
	}
}*/

#version 330 core
out vec4 FragColor;
  
in vec2 TexCoords;

uniform sampler2D depthMap;

void main()
{             
    float depthValue = texture(depthMap, TexCoords).r;
    FragColor = vec4(vec3(depthValue), 1.0);
}