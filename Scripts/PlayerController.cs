using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 controllerPositionTemp;
    private Vector2 controllerPositionNext;
    private Camera mainCamera;

    public float upLimitOffset;
    public float downLimitOffset;
    public float leftLimitOffset;
    public float rightLimitOffset;
    public float titleAngle;
    public float titleSpeed;

    public float speed = 1f;


    [HideInInspector]
    Vector3 paraPos;

    [HideInInspector]
    public bool isFreeFalling = false;

    public static int speedMultiplier;

    public static bool isInputing;

    public static bool isRestricting = false;

    private float moveSpeed;
    private Vector3 movementVector;

    public Vector3 MovementVector { get => movementVector;}

    private BodyPartsReferences bodyScript;

    private int maxHealth = 4;
    private int currentHealth = 5;
    public int CurrentHealth { get => currentHealth; }

    private Animator myAnimator;
    private PlayerRotate rotateScript;

    void OnEnable()
    {
        SensitivityValue.changeSensitivity += ChangeSensitivity;
    }
    void OnDisable()
    {
        SensitivityValue.changeSensitivity -= ChangeSensitivity;
    }
    void ChangeSensitivity(float value)
    {
        moveSpeed = value;
    }
    // Use this for initialization
    void Start()
    {
        moveSpeed = EncryptedPlayerPrefs.GetFloat("SensitivityValue");
        mainCamera = Camera.main;
        if (GetComponent<Animator>())
        {
            myAnimator = GetComponent<Animator>();
            //myAnimator.enabled = false;
        }
        if (GetComponentInChildren<PlayerRotate>())
        {
            rotateScript = GetComponentInChildren<PlayerRotate>();
            rotateScript.StopRotation();
        }
        mainCamera.gameObject.transform.position = new Vector3(0, 0, -10);
        mainCamera.gameObject.transform.rotation = Quaternion.identity;
        mainCamera.gameObject.transform.localScale = Vector3.one;

        IngameUI.Instance.UpdateHealthImage(maxHealth, currentHealth);

        MusicManager.Instance.AdjustVolumeInStart();

        bodyScript = GetComponent<BodyPartsReferences>();
        bodyScript.RestoreAll();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            speed = speed / 10;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.Instance.gameOver == true)
        {
            return;
        }
        Vector3 movement = Vector3.zero;
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            float deltaX = Input.GetAxis("Horizontal") * speed * moveSpeed;
            float deltaY = Input.GetAxis("Vertical") * speed * moveSpeed;

            movement = new Vector3(deltaX, deltaY, 0);

            //clamp magnitude for diagonal movement
            movement = Vector3.ClampMagnitude(movement, speed * moveSpeed);
            movement *= 5;

            if (movement != Vector3.zero)
            {
                isInputing = true;
            }
            else
            {
                isInputing = false;
            }
        }
        else
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Vector2 dir = TouchDirection();
            movement = new Vector3(dir.x, -dir.y, 0) * speed * moveSpeed;
        }

        // movement code Frame rate independent
        movement *= Time.deltaTime;

        //convert local to global coordinates
        movement = transform.TransformDirection(movement);

        Vector3 tempMovement = transform.position;

        transform.position += movement;

        ApplyScreenBounds(tempMovement);
        movementVector = movement;

        if (!isFreeFalling)
        {
            if (tempMovement.x < transform.position.x)
            {
                Quaternion target = Quaternion.Euler(0, 0, titleAngle * (-1));
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * titleSpeed);
            }
            else if (tempMovement.x > transform.position.x)
            {
                Quaternion target = Quaternion.Euler(0, 0, titleAngle);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * titleSpeed);
            }
            else if (tempMovement.x == transform.position.x)
            {
                Quaternion target = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * titleSpeed);
            }
        }
        else
        {
            if (transform.eulerAngles != Vector3.zero)
            {
                transform.eulerAngles = Vector3.zero;
            }
        }
    }
    private void ApplyScreenBounds(Vector3 tempMovement)
    {
        bool restricting = false;
        float halfCamHeight = mainCamera.orthographicSize;
        float halfCamWidth = halfCamHeight * mainCamera.aspect;

        if ((transform.position.x > (0 + halfCamWidth - rightLimitOffset)) || (transform.position.x < (0 - halfCamWidth + leftLimitOffset)))
        {
            restricting = true;
            transform.position = new Vector3(tempMovement.x, transform.position.y, transform.position.z);
        }
        if ((transform.position.y > (0 + halfCamHeight - upLimitOffset)) || (transform.position.y < (0 - halfCamHeight + downLimitOffset)))
        {
            restricting = true;
            transform.position = new Vector3(transform.position.x, tempMovement.y, transform.position.z);
        }
        isRestricting = restricting;
    }
    public Vector2 TouchDirection()
    {
        Vector2 direction = Vector2.zero;

        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                for (var i = 0; i < Input.touchCount; ++i)
                {
                    Vector2 touchpos = Input.GetTouch(i).position;
                    if (Input.GetTouch(i).phase != TouchPhase.Ended)
                    {
                        if (Input.GetTouch(i).phase == TouchPhase.Began)
                        {
                            controllerPositionNext = new Vector2(Input.GetTouch(i).position.x, Screen.height - Input.GetTouch(i).position.y);
                            controllerPositionTemp = controllerPositionNext;
                        }
                        else
                        {
                            controllerPositionNext = new Vector2(Input.GetTouch(i).position.x, Screen.height - Input.GetTouch(i).position.y);
                            Vector2 deltagrag = (controllerPositionNext - controllerPositionTemp);
                            direction.x = deltagrag.x;
                            direction.y = deltagrag.y;

                            controllerPositionTemp = Vector2.Lerp(controllerPositionTemp, controllerPositionNext, 0.5f);
                        }
                    }
                }
            }
            isInputing = true;
        }
        else
        {
            isInputing = false;
        }
        return direction;
    }
    public void ParentHasBeenTriggered(Collider2D collision)
    {
        if (currentHealth < 0)
            return;

        if (collision.gameObject.tag == "Enemy")
        {
            if (myAnimator)
            {
                myAnimator.SetBool("falling", true);
                //myAnimator.enabled = true;
            }
            if (rotateScript)
            {
                rotateScript.StartRotation();
            }

            if (!isFreeFalling)
            {
                MusicManager.Instance.LerpSoundVolume(true);
                GetComponentInChildren<ParachuteSound>().PlayParaBrakeSound();
            }
            isFreeFalling = true;
            speedMultiplier = 2;


            if (collision.gameObject.GetComponent<MoveTowards>())
            {
                collision.gameObject.GetComponent<MoveTowards>().CollisionWithPlayerEffect();
            }
            if (MoveTowards.movingInstances != null)
            {
                if (MoveTowards.movingInstances.Count > 0)
                {
                    foreach (var item in MoveTowards.movingInstances)
                    {
                        item.DecideForParachute();
                    }
                }
            }
            DecreaseHealth();

            if (GetComponent<PlayerProps>().playerParachute)
            {
                //Parachute Dispatch Animation
                //paraPos = new Vector3(0, GetComponent<PlayerProps>().playerParachute.GetComponentInChildren<ParachuteSound>().gameObject.transform.position.y, 0);

                //GetComponent<PlayerProps>().playerParachute.GetComponentInChildren<Animator>().SetTrigger("Break");
                GetComponent<PlayerProps>().playerParachute.GetComponentInChildren<ParachuteSound>().PlayParaBrakeSound();
                StartCoroutine(DestroyParachute());
            }
        }

        if (collision.gameObject.tag == "Parachute")
        {
            PlayerReset(false, collision.gameObject);
        }
        if (collision.gameObject.tag == "Prize")
        {
            if (collision.gameObject.GetComponent<MoveTowards>())
            {
                collision.gameObject.GetComponent<MoveTowards>().CollisionWithPlayerEffect();
            }
            PrizeManager.Instance.PrizeCollected();
        }

        if (collision.gameObject.tag == "Score")
        {
            IngameUI.Instance.AddScore(30);
            if (collision.gameObject.GetComponent<MoveTowards>())
            {
                collision.gameObject.GetComponent<MoveTowards>().CollisionWithPlayerEffect();
            }
        }
    }

    public IEnumerator DestroyParachute()
    {
        GetComponent<PlayerProps>().playerParachute.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        //GetComponent<PlayerProps>().playerParachute.SetActive(false);
        //GetComponent<PlayerProps>().playerParachute.GetComponentInChildren<ParachuteSound>().gameObject.transform.position = paraPos;

        //GetComponent<PlayerProps>().playerParachute.transform.position = paraPos;
    }

    void ResetHealth()
    {
        currentHealth = 5;
        IngameUI.Instance.UpdateHealthImage(maxHealth, currentHealth);
        bodyScript.RestoreAll();
    }
    void DecreaseHealth()
    {
        if (currentHealth < 0)
            return;

        currentHealth--;

        IngameUI.Instance.UpdateHealthImage(maxHealth, currentHealth);
        bodyScript.CutBodyPartOff(currentHealth);

        if (currentHealth < 0)
        {
            PlayerDead();
        }
    }
    void PlayerDead()
    {
        GetComponent<BodyPartsReferences>().DisattachBody();
        this.GetComponent<BodyPartsReferences>().PlayDeathSound();
        GameManager.Instance.GameLost();
    }
    public void EquipParachute(int parachuteNumber)
    {
        //this.GetComponent<PlayerProps>().OpenNewParachute(parachuteNumber);
        this.GetComponent<PlayerProps>().ChangeParachute();
    }
    public void PlayerReset(bool isRevival, GameObject collisionObject = null)
    {
        GetComponent<PlayerProps>().ChangeParachute();
        GetComponent<PlayerProps>().playerParachute.GetComponentInChildren<ParachuteSound>().PlayParaOpenSound();

        if (isFreeFalling)
        {
            if (myAnimator)
            {
                myAnimator.SetBool("falling", false);
                //myAnimator.enabled = false;
            }
            if (rotateScript)
            {
                rotateScript.StopRotation();
            }

            if (isFreeFalling)
            {
                MusicManager.Instance.LerpSoundVolume(false);
                if(GetComponentInChildren<ParachuteSound>())
                    GetComponentInChildren<ParachuteSound>().PlayParaIdleSound();
            }


            isFreeFalling = false;
            speedMultiplier = 1;

            if (collisionObject)
            {
                if (collisionObject.gameObject.GetComponent<MoveTowards>())
                {
                    collisionObject.gameObject.GetComponent<MoveTowards>().CollisionWithPlayerEffect();
                }
            }
        }
        if (MoveTowards.movingInstances != null)
        {
            if (MoveTowards.movingInstances.Count > 0)
            {
                foreach (var item in MoveTowards.movingInstances)
                {
                    item.DecideForFreeFall();
                }
            }
        }
        if(collisionObject)
        {
            collisionObject.gameObject.GetComponent<BodyPartsReferences>().CutBodyPartOff(currentHealth);
        }

        ResetHealth();
    }
}