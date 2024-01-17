using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paratrooper : MonoBehaviour
{
    public int parachuteNumber;
    public GameObject[] parachutes;
    public CapsuleCollider2D capsuleCollider;

    [HideInInspector]
    public bool hasParachute = true;

    private GameObject myPara;
    private Rotate rotateScript;


    private void Start()
    {
        rotateScript = GetComponentInChildren<Rotate>();
        rotateScript.enabled = false;
        parachuteNumber = 2;
        parachuteNumber = GameManager.Instance.GetLoadoutInfo(parachuteNumber);
        //parachuteNumber = Random.Range(0, parachutes.Length);
        myPara = parachutes[parachuteNumber];
        myPara.SetActive(true);

        // Play PARA Animation
        hasParachute = true;
    }

    public void CollidedWithPlayer()
    {
        if (hasParachute == true)
        {
            hasParachute = false;

            rotateScript.enabled = true;

            myPara.SetActive(false);

            PlayerStaticInstance.Instance.gameObject.GetComponent<PlayerController>().EquipParachute(parachuteNumber);
        }
    }
}