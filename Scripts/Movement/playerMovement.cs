using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class playerMove : MonoBehaviour
{
    public CharacterController controller;

    //Change depending on Hero Weight hahahahhhaha nvm
    public float speed = 10f;

    //This is for Gravity
    Vector3 velocity;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;
    [SerializeField] public TextMeshProUGUI prompt;

    public bool isAlive = true;

    bool isGrounded;

    float sprintMult = 0.55f;

    // Health shizz
    public float playerHealth = 150f;
    public float maxHealth = 150f;
    bool isRegenerating = false;
    bool isDamaged = false;
    bool tookDamage = false;
    float regenLength = 2f;

    // Perks
    // Quick Revive
    public bool hasAceRevive = false;
    // Juggernog
    public bool hasIronJack = false;
    // Speed Cola
    public bool hasQuickDraw = false;
    // Stamin Up
    public bool hasRiskRunner = false;
    // Double Tap
    public bool hasDoubleDealer = false;

    AudioSource playerSound;

    void Start()
    {
        playerSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //For X
        float x = Input.GetAxis("Horizontal");
        //For Y
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isAlive)
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        //Sprint function!!! 
        //Only for Characters that can sprint, add that functionality later
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!Input.GetKey(KeyCode.S))
            {
                controller.Move(move * speed * sprintMult * Time.deltaTime);
            }
        }
        else
        {
            speed = 12f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Gravity
        //Delta Y = 1/2g * t^2
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        // Health regen
        if (!isRegenerating && playerHealth < maxHealth && isAlive)
        {
            StartCoroutine(RegenerateHealth());
        }

        // Perk System
        // Quick Revive
        if (hasAceRevive)
        {
            regenLength = 1f;
        }
        else if (!hasAceRevive)
        {
            regenLength = 2f;
        }

        // Juggernog
        if (hasIronJack)
        {
            maxHealth = 250f;
        }
        else if (!hasIronJack)
        {
            maxHealth = 150f;
        }

        // Stamin up
        if (hasRiskRunner)
        {
            speed = 13f;
            sprintMult = .55f;
        }
        else if (!hasRiskRunner)
        {
            speed = 10f;
            sprintMult = 0.55f;
        }

        // DoubleTap
        if (hasDoubleDealer)
        {

        }
        else if (!hasDoubleDealer)
        {

        }

        // Speed Cola
        if (hasQuickDraw)
        {

        }
        else if (!hasQuickDraw)
        {

        }

    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        tookDamage = true;
        isDamaged = true;
        tookDamage = false;

        Debug.Log("Actually Taking Damage");

        if (playerHealth <= 0 && !hasAceRevive)
        {
            Debug.Log("Bro is dead lmao");
            prompt.enabled = true;
            isAlive = false;
            StopAllCoroutines();
            StartCoroutine(Death());

        }
        else if (playerHealth <= 0 && hasAceRevive)
        {
            Debug.Log("Bro got that perk");
            SoundManager.PlaySound(SoundType.REVIVE);
            hasAceRevive = false;
            hasDoubleDealer = false;
            hasIronJack = false;
            hasQuickDraw = false;
            hasRiskRunner = false;
            playerHealth = 150;
        }
    }

    private IEnumerator RegenerateHealth()
    {
        isRegenerating = true;
        while (playerHealth < maxHealth && isAlive)
        {
            yield return new WaitForSeconds(regenLength);
            playerHealth += 10f; // Heal by 10 after dos seconds
            playerHealth = Mathf.Min(playerHealth, maxHealth); // Cap health 
            Debug.Log($"Regenerated health. Current health: {playerHealth}");
        }
        isRegenerating = false;
    }

    private IEnumerator Death()
    {
        SoundManager.PlaySound(SoundType.GAMEOVER);
        isAlive = false;
        controller.enabled = false;
        Cursor.lockState = CursorLockMode.None;

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Debug.Log("Returning to main menu");
    }
}