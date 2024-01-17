//using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class GameInitializer : MonoBehaviour
{
    [Header("Players Prefabs")]
    public GameObject[] playersPrefabs;
    public Transform PlayerSpawnPosition;

    [Header("Spawn Probability")]
    public float probabilityOfRaven;
    public float probabilityOfEagle;
    public float probabilityOfBalloon;
    public float probabilityOfParatrooper;
    public float probabilityOfJet;
    public float probabilityOfHelicopter;
    public float probabilityOfScore;

    [Header("Spawn Delay")]
    public float timeAfterWhichEagleCanBeSpawned;
    public float timeAfterWhichBalloonCanBeSpawned;
    public float timeAfterWhichParatrooperCanBeSpawned;
    public float timeAfterWhichJetCanBeSpawned;
    public float timeAfterWhichHelicopterCanBeSpawned;
    public float timeAfterWhichScoreCanBeSpawned;

    [Header("Spawn Prefabs")]
    public GameObject[] ravenPrefabs;
    public GameObject[] eaglePrefabs;
    public GameObject[] balloonPrefabs;
    public GameObject[] paratrooperPrefabs;
    public GameObject[] jetPrefabs;
    public GameObject[] helicopterPrefabs;
    public GameObject[] scorePrefabs;
    public GameObject[] prizePrefabs;

    private PlayerController playerController;


    public static GameInitializer Instance;


    private Transform[] leftPoints;
    private Transform[] rightPoints;
    private Transform[] upPoints;
    private Transform[] downPoints;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        PlayerController.isInputing = false;
        PlayerController.isRestricting = false;


        //SpawnSelectedPlayer();
    }
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.totalTimePassed = 1;
        TimeManager.totalDistanceCovered = 0;
        PlayerController.speedMultiplier = 1;

        MoveTowards.movingInstances = new List<MoveTowards>();

        leftPoints = LevelInfo.Instance.leftSpawners.GetComponentsInChildren<Transform>();
        rightPoints = LevelInfo.Instance.rightSpawners.GetComponentsInChildren<Transform>();
        upPoints = LevelInfo.Instance.upSpawners.GetComponentsInChildren<Transform>();
        downPoints = LevelInfo.Instance.downSpawners.GetComponentsInChildren<Transform>();
        
        //SpawnObject();
    }

    public void SpawnSelectedPlayer()
    {
        if (PlayerStaticInstance.Instance != null)
        {
            Destroy(PlayerStaticInstance.Instance.gameObject);
        }
        int playerLoadoutNumber = 3;
        int playerNumber = GameManager.Instance.GetLoadoutInfo(playerLoadoutNumber);
        GameObject player = Instantiate(playersPrefabs[playerNumber], PlayerSpawnPosition.transform.position, Quaternion.identity);
        playerController = player.GetComponent<PlayerController>();
        SpawnObject();
    }

    public int SelectRandomPoint(int ptsLength)
    {
        int randomPoint = Random.Range(1, ptsLength);
        return randomPoint;
    }
    
    public void SpawnObject()
    {
        StartCoroutine(SpawnObjectAfterTime());
        float selectedProbablity = Random.Range(0f, 1f);
        if (selectedProbablity <= probabilityOfRaven)
        {
            SpawnRaven();
            if (TimeManager.totalTimePassed > 15)
            {
                SpawnRaven();
            }
        }
        else if (selectedProbablity <= (probabilityOfRaven + probabilityOfEagle) && selectedProbablity > probabilityOfRaven && TimeManager.totalTimePassed > timeAfterWhichEagleCanBeSpawned)
        {
            SpawnEagle();
        }
        else if (selectedProbablity <= (probabilityOfRaven + probabilityOfEagle + probabilityOfBalloon) && selectedProbablity > (probabilityOfRaven + probabilityOfEagle) && TimeManager.totalTimePassed > timeAfterWhichBalloonCanBeSpawned)
        {
            SpawnBalloon();
        }
        else if (selectedProbablity <= (probabilityOfRaven + probabilityOfEagle + probabilityOfBalloon + probabilityOfParatrooper) && selectedProbablity > (probabilityOfRaven + probabilityOfEagle + probabilityOfBalloon) && TimeManager.totalTimePassed > timeAfterWhichParatrooperCanBeSpawned && playerController.isFreeFalling && playerController.CurrentHealth < 3)
        {
            SpawnParatrooper();
        }
        else if (selectedProbablity <= (probabilityOfRaven + probabilityOfEagle + probabilityOfBalloon + probabilityOfParatrooper + probabilityOfJet) && selectedProbablity > (probabilityOfRaven + probabilityOfEagle + probabilityOfBalloon + probabilityOfParatrooper) && TimeManager.totalTimePassed > timeAfterWhichJetCanBeSpawned)
        {
            SpawnJet();
        }
        else if (selectedProbablity <= (probabilityOfRaven + probabilityOfEagle + probabilityOfBalloon + probabilityOfParatrooper + probabilityOfJet + probabilityOfHelicopter) && selectedProbablity > (probabilityOfRaven + probabilityOfEagle + probabilityOfBalloon + probabilityOfParatrooper + probabilityOfJet) && TimeManager.totalTimePassed > timeAfterWhichHelicopterCanBeSpawned)
        {
            SpawnHelicopter();
        }
        else if (selectedProbablity <= (probabilityOfRaven + probabilityOfEagle + probabilityOfBalloon + probabilityOfParatrooper + probabilityOfJet + probabilityOfHelicopter + probabilityOfScore) && selectedProbablity > (probabilityOfRaven + probabilityOfEagle + probabilityOfBalloon + probabilityOfParatrooper + probabilityOfJet + probabilityOfHelicopter) && TimeManager.totalTimePassed > timeAfterWhichScoreCanBeSpawned)
        {
            SpawnScore();
        }
        else
        {
            SpawnRaven();
        }
    }
    int GetClosestSpawner()
    {
        int spawnerNumber = 0;
        float distanceTemp = 555555f;
        float distanceGot = Utility.Distance(PlayerStaticInstance.Instance.transform.position, LevelInfo.Instance.downSpawners.transform.position);
        if (distanceGot < distanceTemp)
        {
            spawnerNumber = 0;
        }
        distanceGot = Utility.Distance(PlayerStaticInstance.Instance.transform.position, LevelInfo.Instance.upSpawners.transform.position);
        if (distanceGot < distanceTemp)
        {
            spawnerNumber = 1;
        }
        distanceGot = Utility.Distance(PlayerStaticInstance.Instance.transform.position, LevelInfo.Instance.leftSpawners.transform.position);
        if (distanceGot < distanceTemp)
        {
            spawnerNumber = 2;
        }
        distanceGot = Utility.Distance(PlayerStaticInstance.Instance.transform.position, LevelInfo.Instance.rightSpawners.transform.position);
        if (distanceGot < distanceTemp)
        {
            spawnerNumber = 3;
        }
        return spawnerNumber;
    }
    Transform GetClosestPlayer(Transform[] points)
    {
        Transform point = null;
        float distanceTemp = 555555f;
        for (int i = 0; i < points.Length; i++)
        {
            float distanceGot = Utility.Distance(PlayerStaticInstance.Instance.transform.position, points[i].position);
            if (distanceGot < distanceTemp)
            {
                distanceTemp = distanceGot;
                point = points[i];
            }
        }
        return point;
    }
    void SpawnRaven()
    {
        int randomSource = Random.Range(0, 4);
        //int randomDestination = Random.Range(0, 4);
        int randomDestination = GetClosestSpawner();

        //while (randomDestination == randomSource)
        //{
        //    randomDestination = Random.Range(0, 4);
        //}
        if (randomDestination == randomSource)
        {
            randomDestination += 1;
            if (randomDestination == 4)
                randomDestination = 0;
        }

        Transform source = null;
        Transform destinationPoint = null;

        if (randomDestination == 0)
        {
            //destinationPoint = downPoints[Random.Range(downPoints.Length / 4, ((downPoints.Length * 3) / 4))];
            destinationPoint = GetClosestPlayer(downPoints);
        }
        else if (randomDestination == 1)
        {
            //destinationPoint = upPoints[Random.Range(upPoints.Length / 4, ((upPoints.Length * 3) / 4))];
            destinationPoint = GetClosestPlayer(upPoints);
        }
        else if (randomDestination == 2)
        {
            //destinationPoint = leftPoints[Random.Range(leftPoints.Length / 4, ((leftPoints.Length * 3) / 4))];
            destinationPoint = GetClosestPlayer(leftPoints);
        }
        else if (randomDestination == 3)
        {
            //destinationPoint = rightPoints[Random.Range(rightPoints.Length / 4, ((rightPoints.Length * 3) / 4))];
            destinationPoint = GetClosestPlayer(rightPoints);
        }

        if (randomSource == 0)
        {
            source = downPoints[Random.Range(downPoints.Length / 4, ((downPoints.Length * 3) / 4))];
        }
        else if (randomSource == 1)
        {
            source = upPoints[Random.Range(upPoints.Length / 4, ((upPoints.Length * 3) / 4))];
        }
        else if (randomSource == 2)
        {
            source = leftPoints[Random.Range(leftPoints.Length / 4, ((leftPoints.Length * 3) / 4))];
        }
        else if (randomSource == 3)
        {
            source = rightPoints[Random.Range(rightPoints.Length / 4, ((rightPoints.Length * 3) / 4))];
        }

        if (ravenPrefabs.Length > 0)
        {
            GameObject raven = (GameObject)Instantiate(ravenPrefabs[Random.Range(0, ravenPrefabs.Length)], source.position, Quaternion.identity);
            raven.GetComponent<MoveTowards>().points.Add(destinationPoint);
        }
    }
    void SpawnEagle()
    {
        int randomSource = Random.Range(0, 4);
        int randomDestination = Random.Range(0, 4);
        while (randomDestination == randomSource)
        {
            randomDestination = Random.Range(0, 4);
        }
        Transform source = GetPoint(randomSource);
        Transform destinationPoint = GetPoint(randomDestination);

        if (eaglePrefabs.Length > 0)
        {
            GameObject eagle = (GameObject)Instantiate(eaglePrefabs[Random.Range(0, eaglePrefabs.Length)], source.position, Quaternion.identity);
            eagle.GetComponent<MoveTowards>().points.Add(PlayerStaticInstance.Instance.gameObject.transform);
            eagle.GetComponent<MoveTowards>().points.Add(destinationPoint);
        }
    }
    void SpawnBalloon()
    {
        int randomPoint = Random.Range(0, downPoints.Length);

        Transform source = downPoints[randomPoint];
        Transform destinationPoint = upPoints[randomPoint];

        if (balloonPrefabs.Length > 0)
        {
            GameObject balloon = (GameObject)Instantiate(balloonPrefabs[Random.Range(0, balloonPrefabs.Length)], source.position, Quaternion.identity);
            balloon.GetComponent<MoveTowards>().points.Add(destinationPoint);
        }
    }
    void SpawnParatrooper()
    {
        int randomPoint = Random.Range(0, downPoints.Length);

        Transform source = downPoints[randomPoint];
        Transform destinationPoint = upPoints[randomPoint];

        if (paratrooperPrefabs.Length > 0)
        {
            GameObject paratrooper = (GameObject)Instantiate(paratrooperPrefabs[Random.Range(0, paratrooperPrefabs.Length)], source.position, Quaternion.identity);
            paratrooper.GetComponent<MoveTowards>().points.Add(source);
            paratrooper.GetComponent<MoveTowards>().points.Add(destinationPoint);
        }
    }
    void SpawnJet()
    {
        int randomSource = Random.Range(0, 4);
        while (randomSource == 1) { randomSource = Random.Range(0, 4); }

        int randomDestination = 0;

        Transform source = null;
        Transform destinationPoint = null;

        if (randomSource == 0)
        {
            randomDestination = Random.Range(2, 4);

            if (randomDestination == 2)
            {
                source = downPoints[Random.Range(5, downPoints.Length - 5)];
                destinationPoint = leftPoints[Random.Range(0, (3 * leftPoints.Length) / 4)];
            }
            else if (randomDestination == 3)
            {
                source = downPoints[Random.Range(5, downPoints.Length - 5)];
                destinationPoint = rightPoints[Random.Range(0, (3 * rightPoints.Length) / 4)];
            }

        }
        else if (randomSource == 2)
        {
            source = leftPoints[Random.Range(leftPoints.Length / 4, leftPoints.Length)];
            destinationPoint = rightPoints[Random.Range(0, rightPoints.Length / 4)];
        }
        else if (randomSource == 3)
        {
            source = rightPoints[Random.Range(rightPoints.Length / 4, rightPoints.Length)];
            destinationPoint = leftPoints[Random.Range(0, leftPoints.Length / 4)];
        }

        if (jetPrefabs.Length > 0)
        {
            GameObject jet = (GameObject)Instantiate(jetPrefabs[Random.Range(0, jetPrefabs.Length)], source.position, Quaternion.identity);
            jet.GetComponent<MoveTowards>().points.Add(destinationPoint);
        }
    }
    void SpawnHelicopter()
    {
        int randomPoint = Random.Range(0, downPoints.Length);

        Transform source = downPoints[randomPoint];
        Transform destinationPoint = upPoints[randomPoint];

        if (helicopterPrefabs.Length > 0)
        {
            GameObject helicopter = (GameObject)Instantiate(helicopterPrefabs[Random.Range(0, helicopterPrefabs.Length)], source.position, Quaternion.identity);
            helicopter.GetComponent<MoveTowards>().points.Add(destinationPoint);
        }
    }
    void SpawnScore()
    {
        int randomPoint = Random.Range(0, downPoints.Length);

        Transform source = downPoints[randomPoint];
        Transform destinationPoint = upPoints[randomPoint];

        if (scorePrefabs.Length > 0)
        {
            GameObject score = (GameObject)Instantiate(scorePrefabs[Random.Range(0, scorePrefabs.Length)], source.position, Quaternion.identity);
            score.GetComponent<MoveTowards>().points.Add(destinationPoint);
        }
    }
    public void ChangeLevel()
    {
        int totalUnlockedLevels = EncryptedPlayerPrefs.GetInt("LevelsUnocked");
        totalUnlockedLevels++;
        EncryptedPlayerPrefs.SetInt("LevelsUnocked", totalUnlockedLevels);
    }
    public void SpawnPrize()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }
        int randomPoint = Random.Range(0, downPoints.Length);

        Transform source = downPoints[randomPoint];
        Transform destinationPoint = upPoints[randomPoint];

        if (prizePrefabs.Length > 0)
        {
            GameObject prize = (GameObject)Instantiate(prizePrefabs[Random.Range(0, prizePrefabs.Length)], source.position, Quaternion.identity);
            prize.GetComponent<MoveTowards>().points.Add(destinationPoint);
        }
    }
    private Transform GetPoint(int number)
    {
        switch (number)
        {
            case 0: //DownSpawner
                return downPoints[Random.Range(0, downPoints.Length)];
            case 1: //UpSpawner
                return upPoints[Random.Range(0, upPoints.Length)];
            case 2: //LeftSpawner
                return leftPoints[Random.Range(0, leftPoints.Length)];
            case 3: //RightSpawner
                return rightPoints[Random.Range(0, rightPoints.Length)];
        }
        return null;
    }
    IEnumerator SpawnObjectAfterTime()
    {
        float waitTime = 3 - (Mathf.Log10(TimeManager.totalTimePassed));

        if (waitTime <= 0.5f)
        {
            waitTime = 0.5f;
        }

        WaitForSeconds ws = new WaitForSeconds(waitTime);
        yield return ws;


        StopCoroutine(SpawnObjectAfterTime());

        if (GameManager.Instance.gameOver == false)
        {
            SpawnObject();
        }
    }
}