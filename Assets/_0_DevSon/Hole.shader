Shader "Custom/HoleShader"
{
//    SubShader
//    {
//        Tags { "Queue" = "Geometry+1" }
//        Lighting Off
//        Pass
//        {
//            ZWrite On
//            //ZTest Always
//            ZTest Less
//            ColorMask 0
//        }
//        
//    }
    
    SubShader
    {
        Tags { "Queue" = "Geometry+1" }
        Pass
        {
            ZWrite On
            ZTest Less
            ColorMask 0
        }
    }
}
