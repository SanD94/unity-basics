#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	StructuredBuffer<float4x4> _Matrices;
#endif

float _Step;

void ConfigureProcedural () {
	#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
		unity_ObjectToWorld = _Matrices[unity_InstanceID];
	#endif
}


void ShaderGraphFunction_float (float3 IN, out float3 OUT) {
    OUT = IN;
}

void ShaderGraphFunction_half (half3 IN, out half3 OUT) {
    OUT = IN;
}