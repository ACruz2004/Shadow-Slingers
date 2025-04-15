using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class Buyables : MonoBehaviour
{
    private bool canPurchase = false;

    [SerializeField]
    public string buyableName;
    public int buyableCost;
    public TextMeshProUGUI prompt;

    private ScoreScript scoreManager;
    private playerMove player;
    AudioSource buySound;
    Gun hasGun;

    [SerializeField] private Transform doorTransform; // Assign this in the Inspector
    [SerializeField] private float doorMoveSpeed = 2f; // Adjust speed as needed
    [SerializeField] private float doorMoveDistance = 3f; // Distance the door moves down

    private bool isDoorMoving = false;

    void Start()
    {
        buySound = GetComponent<AudioSource>();
        scoreManager = FindObjectOfType<ScoreScript>();
        player = FindObjectOfType<playerMove>();

    }

    void OnTriggerEnter(Collider Player)
    {
        if (Player.tag == "Player")
        {
            // Check if player already has the perk
            if (PlayerHasPerk())
            {
                prompt.enabled = false;
                canPurchase = false;
            }
            else
            {
                Debug.Log("Near " + buyableName);
                canPurchase = true;
                prompt.text = "Press [E] to buy " + buyableName + " [Cost: " + buyableCost + "]";
                prompt.enabled = true;
            }
        }
    }

    void Update()
    {
        if (canPurchase && Input.GetKeyDown(KeyCode.E))
        {
            if (scoreManager != null && scoreManager.GetCurrentScore() >= buyableCost)
            {
                PurchasePerk();
                SoundManager.PlaySound(SoundType.BUY);
            }
            else
            {
                Debug.Log($"Not enough points! Current: {scoreManager.GetCurrentScore()}, Required: {buyableCost}");
            }
        }
    }

    void OnTriggerExit(Collider Player)
    {
        if (Player.tag == "Player")
        {
            Debug.Log("Bye bye " + buyableName);
            canPurchase = false;
            prompt.enabled = false;
        }
    }

    // Cheking if the player hasd the perk
    private bool PlayerHasPerk()
    {
        switch (buyableName)
        {
            case "Double Dealer":
                return player.hasDoubleDealer;
            case "Get Jacked":
                return player.hasIronJack;
            case "Ace Revive":
                return player.hasAceRevive;
            case "Quick Draw":
                return player.hasQuickDraw;
            case "Risky Runs":
                return player.hasRiskRunner;
            default:
                return false;
        }
    }

    // Handle the actual purchase logic
    private void PurchasePerk()
    {
        switch (buyableName)
        {
            case "Double Dealer":
                player.hasDoubleDealer = true;
                break;
            case "Get Jacked":
                player.hasIronJack = true;
                break;
            case "Ace Revive":
                player.hasAceRevive = true;
                break;
            case "Quick Draw":
                player.hasQuickDraw = true;
                break;
            case "Risky Runs":
                player.hasRiskRunner = true;
                break;
            case "Door":
                if (!isDoorMoving)
                {
                    StartCoroutine(MoveDoorDown());
                }
                break;
            case "Shotgun":
                hasGun.hasShotgun = true;

                break;
        }

        // Deduct points and disable purchase prompt
        scoreManager.AddScore(-buyableCost);
        canPurchase = false;
        prompt.enabled = false;
        Debug.Log($"{buyableName} purchased");

    }
    private IEnumerator MoveDoorDown()
    {
        isDoorMoving = true;
        Vector3 startPos = doorTransform.position;
        Vector3 targetPos = startPos - Vector3.up * doorMoveDistance;

        while (Vector3.Distance(doorTransform.position, targetPos) > 0.01f)
        {
            doorTransform.position = Vector3.MoveTowards(doorTransform.position, targetPos, doorMoveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        doorTransform.position = targetPos; // Ensure the final position is exact
        isDoorMoving = false;
        Destroy(gameObject);
    }
}