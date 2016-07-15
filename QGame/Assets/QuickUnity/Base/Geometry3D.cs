using UnityEngine;
using System.Collections;
using System;

namespace QuickUnity
{
    public class Geometry3D
    {
        ////////////////////////////////////////
        // Base relative, vector and position //
        ////////////////////////////////////////

        
        ///////////////////////
        // Graphic algorithm //
        ///////////////////////

        // Like a ray, shoot a [distance] length ray from [start] point along [direct] vector, return the destination point 
        public static Vector3 DefiniteProportion(Vector3 start, Vector3 direct, float distance)
        {
            return start + direct.normalized * distance;
        }


        public static bool CompareVectors(Vector3 a, Vector3 b, float angleError)
        {
            bool arg_62_0;
            if (!Mathf.Approximately(a.magnitude, b.magnitude))
            {
                arg_62_0 = false;
            }
            else
            {
                float num = Mathf.Cos(angleError * 0.0174532924f);
                float num2 = Vector3.Dot(a.normalized, b.normalized);
                arg_62_0 = (num2 >= num);
            }
            return arg_62_0;
        }
    }


    public class G3D : Geometry3D { }
}


