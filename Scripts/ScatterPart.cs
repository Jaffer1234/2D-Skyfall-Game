using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPart : MonoBehaviour
{
    void Start()
    {
        this.gameObject.AddComponent<Rigidbody2D>();
        Scatter();
    }

    public void Scatter()
    {
        //Vector2 temp = new Vector2(Random.Range(-5, 5), Random.Range(-2, 2));
        Vector2 temp = new Vector2(Random.Range(-2, 2), Random.Range(-1, 1));
        this.GetComponent<Rigidbody2D>().AddForce(temp, ForceMode2D.Impulse);
    }
}
