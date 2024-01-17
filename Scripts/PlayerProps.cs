using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProps : MonoBehaviour
{
    public GameObject playerParachute;
    public GameObject[] parachutes;
    //private GameObject parachute;

    private void Start()
    {
        ChangeParachute();
    }

    public void ChangeParachute()
    {
        //if (parachute != null)
        //{
        //    Destroy(parachute);
        //}

        int parachuteNumber = 2;

        parachuteNumber = GameManager.Instance.GetLoadoutInfo(parachuteNumber);

        foreach (var item in parachutes)
        {
            item.SetActive(false);
        }

        //if (parachutes[parachuteNumber] != null)
        {
            //parachute = (GameObject)Instantiate(parachutes[parachuteNumber], parachutePosition.transform.position, Quaternion.identity);

            playerParachute = parachutes[parachuteNumber];
            playerParachute.SetActive(true);

            playerParachute.transform.parent = this.transform;
            //playerParachute = parachute;
        }
    }

    public void OpenNewParachute(int index)
    {
        //parachute = parachutes[index];

        //parachute.SetActive(true);
        //playerParachute = parachute;
        //Para Animation
    }
}