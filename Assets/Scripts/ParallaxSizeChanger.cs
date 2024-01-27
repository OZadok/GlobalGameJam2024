using System;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxSizeChanger : MonoBehaviour
{
#if UNITY_EDITOR
    private void Update()
    {
        if (Application.isPlaying)
        {
            return;
        }

        SetSize();
    }

    private void SetSize()
    {
        var camZ = Camera.main.transform.position.z;
        var z = transform.position.z;
        var ratio = (z - camZ)/-camZ;
        transform.localScale = Vector3.one * ratio;
    }
#endif
}
