using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public static event Action onHit;
    public float speed = 2f;
    public float health = 10f;
    public float damage = 50f;

    private Animator animator;
    AudioSource deathSound;
    bool isDead = false;
    public ScoreScript scoreScript;
    public ParticleSystem blood;

    // For player damage
    public playerMove player;

    // For movement
    private NavMeshAgent agent = null;
    [SerializeField] public Transform target;
    public Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        deathSound = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        spawner = FindObjectOfType<Spawner>();

        GetReferences();

        // Score stuff
        if (scoreScript == null)
        {
            scoreScript = FindObjectOfType<ScoreScript>();
        }
        // Health stuff
        if (player == null)
        {
            player = FindObjectOfType<playerMove>();
        }
    }

    void Update()
    {
        if (!isDead)
        {
            MoveToTarget();
        }
        else
        {
            Destroy(gameObject);
            // Spawner Shizz
            if (spawner != null)
            {
                spawner.enemiesKilled++;
                Debug.Log($"Enemies killed: {spawner.enemiesKilled}");
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        Debug.Log($"Enemy took {amount} damage. Current health: {health}");
        health -= amount;
        scoreScript.AddScore(10);

        if (health <= 0f)
        {
            Debug.Log("Enemy is dying...");
            isDead = true;
            blood.Play();

            SoundManager.PlaySound(SoundType.ZOMBDEATH);

            scoreScript.AddScore(100);

            animator.SetBool("Isdead", true);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            onHit?.Invoke();
            player.TakeDamage(damage);
            SoundManager.PlaySound(SoundType.HIT);
        }
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.position);
    }

    private void GetReferences()
    {

    }
}

