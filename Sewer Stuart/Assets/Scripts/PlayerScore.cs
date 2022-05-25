using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    int score;
    float lifetime;
    [HideInInspector] public bool alive;
    [SerializeField] Inventory inventory;

    [Header("GUI")]
    [SerializeField] Text scoreDisplay;
    [SerializeField] Text timeDisplay;
    [SerializeField] GameObject clockHand;
    [SerializeField] GameObject cheeseUISpawnPosition;
    [SerializeField] GameObject cheeseUIPrefab;
    //[SerializeField] GameObject cheeseImage;
    [SerializeField] GrowShrink cheeseImage;

    [Header("Pause Screen")]
    [SerializeField] Text pauseScoreDisplay;
    [SerializeField] Text pauseTimeDisplay;

    [Header("Game Over")]
    [SerializeField] Text gameOverScoreDisplay;
    [SerializeField] Text gameOverTimeDisplay;

    private void Start()
    {
        alive = true;
        UpdateUI();
    }

    private void Update()
    {
        if (alive)
        {
            lifetime += Time.deltaTime;
            UpdateUI();
        }
    }

    public void SaveScore()
    {
        inventory.AddCheese(score);
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateUI();
    }

    public void AddScore(int points)
    {
        score += points;
        if (score < 0)
        {
            score = 0;
        }
        GameObject cheesePoint = Instantiate(cheeseUIPrefab, cheeseUISpawnPosition.transform);
        cheesePoint.GetComponent<CheesePointUI>().UpdateText(points);
        cheeseImage.Burst(points);
        UpdateUI();
    }

    public void UpdateUI()
    {
        //GUI
        scoreDisplay.text = score.ToString();
        timeDisplay.text = lifetime.ToString("f1") + "s";
        clockHand.transform.localEulerAngles = new Vector3(0, 0, -lifetime * 30f);
        //cheeseImage.transform.localScale = new Vector3(Mathf.Log10(score + 2), Mathf.Log10(score + 2), 1);

        //Pause Screen
        pauseScoreDisplay.text = score.ToString();
        pauseTimeDisplay.text = lifetime.ToString("f2") + "s";

        //Game Over
        gameOverScoreDisplay.text = score.ToString();
        gameOverTimeDisplay.text = lifetime.ToString("f2") + "s";
    }
}
