using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreScript : MonoBehaviour
{
    [SerializeField]
    private int scoreNum = 500;
    public int currentScore;
    private bool isDoublePoints = false;
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;

    // Duration for double points to be active
    private float doublePointsDuration = 25f;

    void Start()
    {
        scoreNum = 500;
        Debug.Log("Starting with " + scoreNum);
        // Subscribe to the DoublePoints event
        DoublePoints.onCollected += ActivateDoublePoints;

        score.text = "" + scoreNum;
    }

    void OnDestroy()
    {
        // Unsubscribe from the DoublePoints event to avoid memory leaks
        DoublePoints.onCollected -= ActivateDoublePoints;
    }

    void Update()
    {

    }

    public void AddScore(int amount)
    {
        if (isDoublePoints && amount > 0)
        {
            scoreNum += amount * 2;
        }
        else
        {
            scoreNum += amount;
        }
        score.text = scoreNum.ToString();
    }

    // Method to activate double points
    private void ActivateDoublePoints()
    {
        isDoublePoints = true;
        // Start a coroutine to disable double points after the duration
        StartCoroutine(DisableDoublePointsAfterTime(doublePointsDuration));
    }

    // Coroutine to disable double points
    private IEnumerator DisableDoublePointsAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDoublePoints = false;
    }

    public int GetCurrentScore()
    {
        currentScore = scoreNum;
        return currentScore;
    }
}