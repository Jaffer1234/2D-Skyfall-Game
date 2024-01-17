using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool unscaledTime;
    public Vector3 rotateAlong;
    // Update is called once per frame
    void Update()
    {
        if (unscaledTime)
            transform.Rotate(rotateAlong * Time.unscaledDeltaTime);
        else
            transform.Rotate(rotateAlong * Time.deltaTime);
    }
}
