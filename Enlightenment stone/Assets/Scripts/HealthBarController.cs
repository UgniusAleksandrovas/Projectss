using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBarController : MonoBehaviour, IDamageable
{
    
     [SerializeField] Image healthBar;
     [SerializeField] float health;
     [SerializeField] float MaxHealth;
     [SerializeField] GameObject DeathScreen;

    Animator _animator;

     private Scene scene;
     bool isDead;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        UpdateBarHealthUI();
        scene = SceneManager.GetActiveScene();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, MaxHealth);
        if (health <= 0)
        {
            if (!isDead)
            {
                StartCoroutine(StartDeath());
            }
        }
        UpdateBarHealthUI();
    }

    IEnumerator StartDeath()
    {
        isDead = true;
        GameObject.Find("Player");
        FindObjectOfType<audioManager>().Play("PlayerDeath");
        gameObject.GetComponent<PlayerScript>().enabled = false;
        gameObject.GetComponent<Attack>().enabled = false;
        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(2.5f);
        DeathScreen.SetActive(true);
        Destroy(gameObject);
    }

    void UpdateBarHealthUI()
    {
        healthBar.fillAmount = health / MaxHealth;
    }
}
