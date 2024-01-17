using UnityEngine;
public class PlayerRotate : MonoBehaviour
{
    public float rotationSpeed = 20;

    private bool rotate = false;

    private PlayerController playerControllerScript;

    void Update()
    {
        if (rotate)
        {
            this.transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }
        else
        {
            if (transform.localEulerAngles != Vector3.zero)
            {
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, Vector3.zero, (rotationSpeed / 10) * Time.deltaTime);
            }
        }
        playerControllerScript = GetComponentInParent<PlayerController>();
    }
    public void StartRotation()
    {
        rotate = true;
    }
    public void StopRotation()
    {
        rotate = false;
    }
    public void ChangeRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        playerControllerScript.ParentHasBeenTriggered(collision);
    }
}