#ifndef TOO_YOUNG_NOISE_INCLUDED
#define TOO_YOUNG_NOISE_INCLUDED

/// \brief https://thebookofshaders.com/10/
/// \param st 
/// \return 
float Float2ToRandomFloat(float2 st)
{
    return frac(sin(dot(st * 103U, float2(12.9898,78.233))) * 43758.5453123);
}

// From quality hashes collection by nimitz: https://www.shadertoy.com/view/Xt3cDn
uint BaseHash(uint2 p) 
{
    p = 1103515245U * ((p >> 1U) ^ (p.yx));
    uint h32 = 1103515245U * ((p.x) ^ (p.y >> 3U));
    return h32 ^ (h32 >> 16);
}

#endif