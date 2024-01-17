//using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Experimental;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public enum MoveAction
    {
        once,
        repeat,
        loop
    }

    public enum Type
    {
        raven,
        eagle,
        ballon,
        jet,
        helicopter,
        paratrooper,
        prize,
        score
    }

    public Type type;
    public MoveAction moveType;
    public float speed = 5f;
    public float speedMultiplier = 1f;
    public float waitTimeAtPoints;

    [Header("Destruction Prefabs")]
    public GameObject[] destroyParticles;

    [HideInInspector]
    public List<Transform> points;

    public static List<MoveTowards> movingInstances;

    private Transform target;
    private Vector3 targetTemp;
    private int index = -1;
    private bool move = false;
    private int inverted = 0;
    private bool onceReached = false;

    private AudioSource myAudio;

    private void Awake()
    {
        movingInstances.Add(this);
        targetTemp = Vector3.zero;
        points = new List<Transform>();
        inverted = Random.Range(0, 2);
        if (GetComponent<AudioSource>())
        {
            myAudio = GetComponent<AudioSource>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (points.Count > 0)
        {
            StartCoroutine(WaitFor(true, true));
        }
    }

    IEnumerator WaitFor(bool assign, bool wait)
    {
        if (type == Type.paratrooper || wait == false)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(waitTimeAtPoints);
        }
        if (assign)
        {
            AssignNext();
        }
        else
        {
            move = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            float distance = 0;

            if (target == null)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, targetTemp, Time.deltaTime * speed * speedMultiplier * PlayerController.speedMultiplier * 4);

                distance = Utility.Distance(this.transform.position, targetTemp);
            }
            else
            {
                if (type == Type.eagle)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, target.position, Time.deltaTime * speed * speedMultiplier);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, target.position, Time.deltaTime * speed * speedMultiplier * PlayerController.speedMultiplier);
                }

                distance = Utility.Distance(this.transform.position, target.position);
            }

            if (type == Type.raven || type == Type.eagle)
            {
                if (type == Type.raven)
                {
                    transform.up = 1 * (target.position - transform.position);
                }

                if (type == Type.eagle)
                {
                    if (!target)
                    {
                        if (targetTemp != Vector3.zero)
                        {
                            transform.up = 1 * (targetTemp - transform.position);
                        }
                    }
                    else
                    {
                        transform.up = 1 * (target.position - transform.position);
                    }
                    if (distance < 2f && target == PlayerStaticInstance.Instance.gameObject.transform)
                    {
                        targetTemp = target.position;
                        target = null;


                        move = false;
                        StartCoroutine(WaitFor(false, true));
                        return;
                    }
                }
            }
            else if (/*type == Type.eagle || */type == Type.jet || type == Type.helicopter)
            {
                int invertValue = 0;

                //if (type == Type.eagle)
                //{
                //    if (distance < 2f && target == PlayerStaticInstance.Instance.gameObject.transform)
                //    {
                //        targetTemp = target.position;
                //        target = null;


                //        move = false;
                //        StartCoroutine(WaitFor(false, true));
                //        return;
                //    }
                //}
                //else 
                if (type == Type.jet)
                {
                    invertValue = 180;

                }
                else if (type == Type.helicopter)
                {
                    if (inverted == 1)
                    {
                        invertValue = 180;
                    }
                }

                if (target)
                {
                    if (target.position.x < transform.position.x)
                    {
                        transform.eulerAngles = new Vector3(0, invertValue + 180, 0);
                    }
                    else if (target.position.x >= transform.position.x)
                    {
                        transform.eulerAngles = new Vector3(0, invertValue, 0);
                    }
                }
            }

            if (distance < 0.1f)
            {
                if (targetTemp != Vector3.zero)
                {
                    targetTemp = Vector3.zero;
                }
                if (!onceReached)
                {
                    onceReached = true;
                }
                else
                {
                    if (type == Type.paratrooper)
                    {
                        DestroyMe(false);
                    }    
                }
                move = false;


                if (type == Type.eagle)
                {
                    StartCoroutine(WaitFor(true, false));
                    return;
                }
                StartCoroutine(WaitFor(true, true));
            }
        }
    }

    void AssignNext()
    {
        if (index < points.Count - 1)
        {
            index++;
            if (type == Type.raven || type == Type.eagle)
            {
                if (myAudio)
                {
                    myAudio.PlayDelayed(Random.Range(0.5f, 1.5f));
                }
            }
        }
        else if (index >= (points.Count - 1))
        {
            if (moveType == MoveAction.repeat)
            {
                index = 0;
            }
            else if (moveType == MoveAction.loop)
            {
                index = 0;
                transform.position = points[index].transform.position;
                index++;
                if (index < points.Count)
                {
                    target = points[index].transform;
                    move = true;
                }
                else
                {
                    index--;
                }
                return;
            }
            else if (moveType == MoveAction.once)
            {
                DestroyMe(false);
                return;
            }
        }
        target = points[index].transform;
        move = true;
    }
    
    public void CollisionWithPlayerEffect()
    {
        if (destroyParticles.Length > 0)
        {
            GameObject particle = (GameObject)Instantiate(destroyParticles[Random.Range(0, destroyParticles.Length)], transform.position, Quaternion.identity);
        }
        if (type == Type.score ||type == Type.prize || type == Type.ballon || type == Type.eagle || type == Type.raven)
        {
            DestroyMe(false);
        }
        else if (type == Type.paratrooper)
        {
            GoDown();
            GetComponent<Paratrooper>().CollidedWithPlayer();
        }
    }

    public void DecideForParachute()
    {
        if (type == Type.paratrooper)
        {
            if (GetComponent<Paratrooper>().hasParachute)
            {
                speedMultiplier = 1;
                GoUp();
            }
            else
            {
                speedMultiplier = 0;
            }
        }    
    }

    public void DecideForFreeFall()
    {
        if (type == Type.paratrooper)
        {
            if (GetComponent<Paratrooper>().hasParachute == false)
            {
                speedMultiplier = 1;
                GoDown();
            }
            else
            {
                speedMultiplier = 0;
            }
        }
    }

    public void GoUp()
    {
        if (points.Count > 0)
        {
            index = 1;
            target = points[index];
        }
    }

    public void GoDown()
    {
        if (points.Count > 0)
        {
            index = 0;
            target = points[index];
        }
    }

    void DestroyMe(bool callFromOnDestroy)
    {
        movingInstances.Remove(this);
        if (!callFromOnDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        DestroyMe(true);
    }

    public void CollidedWithThings()
    {
        if (type == Type.eagle || type == Type.raven || type == Type.paratrooper)
        {
            if (destroyParticles.Length > 0)
            {
                GameObject particle = (GameObject)Instantiate(destroyParticles[Random.Range(0, destroyParticles.Length)], transform.position, Quaternion.identity);
            }
            DestroyMe(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            MoveTowards moveScript = collision.gameObject.GetComponent<MoveTowards>();

            if (moveScript.type == Type.helicopter || moveScript.type == Type.jet)
            {
                CollidedWithThings();
            }
        }
    }
}