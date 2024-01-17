using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SetOnCorners : MonoBehaviour
{
    public enum Corner
    {
        up,
        down,
        right,
        left
    }
    public Corner cornerName;
    public float offset;

    private void Start()
    {
        Set();
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            StartCoroutine(UpdateMyPosition());
        }
    }
    IEnumerator UpdateMyPosition()
    {
        while(true)
        {
            yield return null;
            Set();
        }
    }
    private void Set()
    {
        float halfCamHeight = Camera.main.orthographicSize;
        float halfCamWidth = halfCamHeight * Camera.main.aspect;

        if (cornerName == Corner.up)
        {
            transform.position = new Vector3(transform.position.x, halfCamHeight + offset, transform.position.z);
        }
        else if (cornerName == Corner.down)
        {
            transform.position = new Vector3(transform.position.x, -halfCamHeight+ offset, transform.position.z);
        }
        else if(cornerName == Corner.right)
        {
            transform.position = new Vector3(halfCamWidth + offset, transform.position.y, transform.position.z);
        }
        else if (cornerName == Corner.left)
        {
            transform.position = new Vector3(-halfCamWidth + offset, transform.position.y, transform.position.z);
        }
    }
}
