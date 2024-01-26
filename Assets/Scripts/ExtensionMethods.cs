using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 WithX(this Vector3 vec, float x)
    {
        return new Vector3(x, vec.y, vec.z);
    }
    
    public static Vector3 WithY(this Vector3 vec, float y)
    {
        return new Vector3(vec.x, y, vec.z);
    }
    
    public static Vector3 WithZ(this Vector3 vec, float z)
    {
        return new Vector3(vec.x, vec.y, z);
    }
}