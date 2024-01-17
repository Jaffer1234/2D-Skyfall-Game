using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class SlideNumberDistance : MonoBehaviour
{
    public Text actuyallDistance;
    public Text halfDistance;
    public Text quaterDistance;
    public float speed = 2f;

    private float desiredNumber = 0;
    private float currentNumber = 0;

    private bool animate = false;

    void Start()
    {
        SetNumber(TimeManager.totalDistanceCovered);
    }

    void Update()
    {
        if (animate)
        {
            if (currentNumber < desiredNumber)
            {
                currentNumber += (speed * Time.deltaTime);

                if (currentNumber > desiredNumber)
                {
                    currentNumber = desiredNumber;
                    animate = false;
                }

                actuyallDistance.text = ((int)currentNumber).ToString() + " m";
                halfDistance.text = ((int)currentNumber / 2).ToString() + " m";
                quaterDistance.text = ((int)currentNumber / 4).ToString() + " m";
            }
        }
        SetNumber(TimeManager.totalDistanceCovered);
    }

    public void SetNumber(float value)
    {
        desiredNumber = value;

        animate = true;
    }
}