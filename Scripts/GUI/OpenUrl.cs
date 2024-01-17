using UnityEngine;
using System.Collections;

public class OpenUrl : MonoBehaviour
{

    public string URL;
    public void OpenURL()
    {
        Application.OpenURL(URL);
    }
    public void OpenURL(string _url)
    {
        Application.OpenURL(_url);
    }
}
