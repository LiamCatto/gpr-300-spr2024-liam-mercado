#version 450

out vec4 FragColor; //The color of this fragment

in Surface{
	vec3 WorldPos; //Vertex position in world space
	vec3 WorldNormal; //Vertex normal in world space
	vec2 TexCoord;
    vec4 FragPosLightSpace; // Added by Liam
}fs_in;

uniform sampler2D _MainTex; 
uniform vec3 _EyePos;
uniform vec3 _LightDirection = vec3(0.0,-1.0,0.0);
uniform vec3 _LightColor = vec3(1.0);
uniform vec3 _AmbientColor = vec3(0.3,0.4,0.46);

struct Material{
	float Ka; //Ambient coefficient (0-1)
	float Kd; //Diffuse coefficient (0-1)
	float Ks; //Specular coefficient (0-1)
	float Shininess; //Affects size of specular highlight
};

uniform Material _Material;

uniform sampler2D shadowMap; // Added by Liam

float ShadowCalculation(vec4 lightSpacePos)	// Added by Liam
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

void main(){
	//Make sure fragment normal is still length 1 after interpolation.
	vec3 normal = normalize(fs_in.WorldNormal);

	//Light pointing straight down
	vec3 toLight = -_LightDirection;
	float diffuseFactor = max(dot(normal,toLight),0.0);

	//Calculate specularly reflected light
	vec3 toEye = normalize(_EyePos - fs_in.WorldPos);

	//Blinn-phong uses half angle
	vec3 h = normalize(toLight + toEye);
	float specularFactor = pow(max(dot(normal,h),0.0),_Material.Shininess);

	//Combination of specular and diffuse reflection
	
    float shadow = ShadowCalculation(fs_in.FragPosLightSpace);  // Added by Liam
	vec3 lightColor = (_Material.Kd * diffuseFactor + _Material.Ks * specularFactor * (1.0 - shadow)) * _LightColor;	// " * (1.0 - shadow)" added by Liam
	lightColor+=_AmbientColor * _Material.Ka;
	vec3 objectColor = texture(_MainTex,fs_in.TexCoord).rgb;
	FragColor = vec4(objectColor * lightColor,1.0);
}
