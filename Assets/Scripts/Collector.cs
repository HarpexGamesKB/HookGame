using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collector : MonoBehaviour
{
    public GameObject GameWinPanel;
    public GameObject MoneyPrefab;
    public Transform Spawnpoint;
    public List<GameObject> Bricks = new List<GameObject>();
    public int money;
    public int collectedInLevel;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text collectedInLevelWinText;
    [SerializeField] private Text collectedInLevelOverText;

    public AudioSource audioSource;
    public AudioClip audioClip;
    private void Awake()
    {
        Brick[] bricks = FindObjectsOfType<Brick>();
        for (int i = 0; i < bricks.Length; i++)
        {
            Bricks.Add(bricks[i].gameObject);
        }
    }
    private void Start()
    {
        money = PlayerPrefs.GetInt("money", money);
        UpdateText();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Brick brick = collision.gameObject.GetComponent<Brick>();
        if (brick)
        {
            brick.Destroy();
            GameObject money = Instantiate(MoneyPrefab, Spawnpoint.position, Random.rotation);
            Destroy(money,6f);
            audioSource.PlayOneShot(audioClip);
            Bricks.Remove(brick.gameObject);
            AddMoney(1);
            UpdateText();
            CheckIsNull();
        }

    }
    private void AddMoney(int value)
    {
        money += value;
        collectedInLevel += value;
        UpdateText();
    }
    private void CheckIsNull()
    {
        if(Bricks.Count < 1)
        {
            GameWinPanel.SetActive(true);
        }
    }
    public void UpdateText()
    {
        moneyText.text = money.ToString() + "$";
        collectedInLevelWinText.text = collectedInLevel.ToString() + "$"; 
        collectedInLevelOverText.text = collectedInLevel.ToString() + "$"; 
        PlayerPrefs.SetInt("money", money);
    }
    public void Buy(int value)
    {
        money-=value;
        if(money < 0)
        {
            money=0;
        }
        UpdateText();

    }
}
