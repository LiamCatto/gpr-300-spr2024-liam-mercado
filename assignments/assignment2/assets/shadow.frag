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

/*#version 330 core
out vec4 FragColor;

in VS_OUT {
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
    vec4 FragPosLightSpace;
} fs_in;

uniform sampler2D diffuseTexture;
uniform sampler2D shadowMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

float ShadowCalculation(vec4 fragPosLightSpace)
{
    //Homogeneous Clip space to NDC [-w,w] to [-1,1]
	vec3 sampleCoord = lightSpacePos.xyz / lightSpacePos.w;
	
	//Convert from [-1,1] to [0,1]
	sampleCoord = sampleCoord * 0.5 + 0.5;

	float myDepth = sampleCoord.z; 
	float shadowMapDepth = texture(shadowMap, sampleCoord.xy).r; 
	if (myDepth > shadowMapDepth)
	{
		//In shadow!
        return 1.0;
	}
    else return 0.0;
}

void main()
{           
    vec3 color = texture(diffuseTexture, fs_in.TexCoords).rgb;
    vec3 normal = normalize(fs_in.Normal);
    vec3 lightColor = vec3(1.0);
    // ambient
    vec3 ambient = 0.15 * lightColor;
    // diffuse
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    float diff = max(dot(lightDir, normal), 0.0);
    vec3 diffuse = diff * lightColor;
    // specular
    vec3 viewDir = normalize(viewPos - fs_in.FragPos);
    float spec = 0.0;
    vec3 halfwayDir = normalize(lightDir + viewDir);  
    spec = pow(max(dot(normal, halfwayDir), 0.0), 64.0);
    vec3 specular = spec * lightColor;    
    // calculate shadow
    float shadow = ShadowCalculation(fs_in.FragPosLightSpace);       
    vec3 lighting = (ambient + (1.0 - shadow) * (diffuse + specular)) * color;    
    
    FragColor = vec4(lighting, 1.0);
}*/

#version 330 core

void main()
{             
    // gl_FragDepth = gl_FragCoord.z;
}