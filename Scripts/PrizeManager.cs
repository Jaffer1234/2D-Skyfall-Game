using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeManager : MonoBehaviour
{
    public int timeAfterWhichAPrizeCome;
    public int minimumPrizesForSpawn;
    public int minimumPrizes;
    public static PrizeManager Instance;

    private int timeAfterWhichAPrizeComeTemp;
    private int totalTokenCollected;
    private float timeToSpawnAt;
    private bool coolDown;
    private bool canSpawnGift;
    private bool stopSpawn;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        coolDown = false;
        canSpawnGift = false;
        stopSpawn = false;
        timeToSpawnAt = 0f;
        if (timeAfterWhichAPrizeCome == 0)
            timeAfterWhichAPrizeCome = 1;

        timeAfterWhichAPrizeComeTemp = timeAfterWhichAPrizeCome;
        totalTokenCollected = EncryptedPlayerPrefs.GetInt("TokenCollected");
        CheckTokensCollected();
    }

    // Update is called once per frame
    void Update()
    {
        if (IngameUI._isPlaying && stopSpawn == false)
        {
            if (((int)TimeManager.totalTimePassed) % timeAfterWhichAPrizeCome == 0)
            {
                if (coolDown == false)
                {
                    coolDown = true;
                    StartCoroutine(BackUp());
                    CheckTokensCollected();
                    DecideGiftTime();
                    GameInitializer.Instance.ChangeLevel();
                }
            }
            if (canSpawnGift)
            {
                if (TimeManager.totalTimePassed % timeAfterWhichAPrizeCome < 1f)
                {
                    GameInitializer.Instance.SpawnPrize();
                    canSpawnGift = false;
                    timeToSpawnAt = 0f;
                }
            }
        }
    }
    IEnumerator BackUp()
    {
        yield return new WaitForSeconds(1.5f);
        coolDown = false;
    }
    void DecideGiftTime()
    {
        timeToSpawnAt = Random.Range(TimeManager.totalTimePassed, TimeManager.totalTimePassed + (float)timeAfterWhichAPrizeCome);
        canSpawnGift = true;
    }
    public void PrizeCollected()
    {
        int tokenCollected = EncryptedPlayerPrefs.GetInt("TokenCollected");
        tokenCollected++;
        EncryptedPlayerPrefs.SetInt("TokenCollected", tokenCollected);
        totalTokenCollected++;
        CheckTokensCollected();
    }
    public void CheckTokensCollected()
    {
        if (totalTokenCollected > minimumPrizesForSpawn && totalTokenCollected < minimumPrizes)
        {
            timeAfterWhichAPrizeCome = timeAfterWhichAPrizeComeTemp * 20;
        }
        else if (totalTokenCollected > minimumPrizes)
        {
            stopSpawn = true;
        }
    }
}
