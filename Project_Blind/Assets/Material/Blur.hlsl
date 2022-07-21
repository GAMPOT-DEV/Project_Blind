#ifndef __BLUR_HLSL__
#define __BLUR_HLSL__

#define GAUSSIAN_BLUR_UNROLL 1
#include "Assets/Material/Shaders/GaussianBlur.hlsl"

void Blur_float(UnityTexture2D tex,float offset,float2 uv,out float res)
{
    res = 0;
    float filter[25] = {
        1,4,7,4,1,
        4,16,26,16,4,
        7,26,41,26,7,
        4,16,26,16,4,
        1,4,7,4,1,
    };
    for(float x = 0;x<5;x++)
    {
        for(float y = 0;y<5;y++)
        {
            res += tex2D(tex,uv + float2(x - 1, y - 1)*(offset)).x * filter[x+y*5];
        }
    }
    res /=273;
}

void test_float(UnityTexture2D tex,float2 uv,out float res)
{
    res = tex2D(tex,uv).x;
}


#endif

