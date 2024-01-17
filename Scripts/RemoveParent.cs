
using UnityEditor.XR;
using UnityEngine;

public class RemoveParent : MonoBehaviour
{
    public bool resetTransform = false;
    void Awake()
    {
        transform.parent = null;
        if (resetTransform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}
