// dito movement + hp ng player

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string _horizontalAxis = "Horizontal", _verticalAxis = "Vertical";

    [SerializeField]
    private Rigidbody2D _rb2d;

    private Vector2 _input;

    [SerializeField]
    private float _speed = 3f;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private void Start()
    {
        if (GameData.hasSavedPosition)
        {
            transform.position = GameData.playerPosition;
        }

        // depende sa laban hp
        if (PlayerData.maxHealth > 0)
        {
            maxHealth = PlayerData.maxHealth;
            currentHealth = PlayerData.currentHealth; // eto
        }
        else
        {
            currentHealth = maxHealth;
        }

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    private void FixedUpdate()
    {
        _rb2d.linearVelocity = _input * _speed;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw(_horizontalAxis);
        float verticalInput = Input.GetAxisRaw(_verticalAxis);

        _input = new Vector2(horizontalInput, verticalInput);
        _input.Normalize();

        /* hp testing lng to
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
        */
    }

    // none
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.SetHealth(currentHealth);

        PlayerData.currentHealth = currentHealth;
    }
}