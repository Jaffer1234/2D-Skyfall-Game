using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class BodyPartsReferences : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioPlayer;

    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject leftLeg;
    public GameObject rightLeg;
    public GameObject leftArmBlood;
    public GameObject rightArmBlood;
    public GameObject leftLegBlood;
    public GameObject rightLegBlood;
    public GameObject body;
    public GameObject bodyBlood;

    private bool isParatrooper = false;

    [Header ("SOUNDS")]
    public AudioClip hurtSound;
    public AudioClip deathSound;

    void Awake()
    {
        if (!audioPlayer)
        {
            if (GetComponent<AudioSource>())
            {
                audioPlayer = GetComponent<AudioSource>();
            }
        }
    }

    void Start()
    {
        if (GetComponent<Paratrooper>() || this.gameObject.tag == "Parachute")
        {
            isParatrooper = true;
        }
        RestoreAll();
        body.SetActive(true);
        bodyBlood.SetActive(false);
    }
    public void CutBodyPartOff(int health)
    {
        PlayHurtSound();
         
        if (health == 3)
        {
            AttachScatterParts(leftArm);
            leftArm.SetActive(false);
            leftArmBlood.SetActive(true);
        }
        else if (health == 2)
        {
            AttachScatterParts(rightLeg);
            leftArm.SetActive(false);
            leftArmBlood.SetActive(true);
            rightArm.SetActive(false);
            rightArmBlood.SetActive(true);
        }
        else if (health == 1)
        {
            AttachScatterParts(leftLeg);
            leftArm.SetActive(false);
            leftArmBlood.SetActive(true);
            rightArm.SetActive(false);
            rightArmBlood.SetActive(true);
            leftLeg.SetActive(false);
            leftLegBlood.SetActive(true);
        }
        else if (health == 0)
        {
            AttachScatterParts(rightArm);
            leftArm.SetActive(false);
            leftArmBlood.SetActive(true);
            rightArm.SetActive(false);
            rightArmBlood.SetActive(true);
            leftLeg.SetActive(false);
            leftLegBlood.SetActive(true);
            rightLeg.SetActive(false);
            rightLegBlood.SetActive(true);
        }
    }
    public void RestoreAll()
    {
        leftArm.SetActive(true);
        leftArmBlood.SetActive(false);
        rightArm.SetActive(true);
        rightArmBlood.SetActive(false);
        leftLeg.SetActive(true);
        leftLegBlood.SetActive(false);
        rightLeg.SetActive(true);
        rightLegBlood.SetActive(false);
    }
    public void AttachBody()
    {
        body.SetActive(true);
        bodyBlood.SetActive(false);
    }
    public void DisattachBody()
    {
        AttachScatterParts(body);
        body.SetActive(false);
        bodyBlood.SetActive(true);
    }
    void AttachScatterParts(GameObject part)
    {
        if (!isParatrooper)
        {
            GameObject obj = (GameObject)Instantiate(part, part.transform.position, part.transform.rotation);
            obj.transform.parent = part.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.parent = null;

            GameObject bloodObj = (GameObject)Instantiate(bodyBlood, obj.transform.position, obj.transform.rotation);
            bloodObj.SetActive(true);
            bloodObj.transform.parent = obj.transform;
            bloodObj.transform.localScale = Vector3.one;

            obj.AddComponent<ScatterPart>();
            if (obj.GetComponent<SpriteSkin>())
            {
                Destroy(obj.GetComponent<SpriteSkin>());
            }
            Destroy(obj, 3);
        }
    }

    public void PlayHurtSound()
    {
        if (audioPlayer)
        {
            if (hurtSound)
            {
                audioPlayer.PlayOneShot(hurtSound, GameManager.Instance.soundValue);
            }
            else
                Utility.ErrorLog("Sound of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }

    public void PlayDeathSound()
    {
        if (audioPlayer)
        {
            if (hurtSound)
            {
                audioPlayer.PlayOneShot(deathSound, GameManager.Instance.soundValue);
            }
            else
                Utility.ErrorLog("Sound of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }
}
