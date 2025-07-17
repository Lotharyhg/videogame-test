using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    public GameObject player;
    public Animator zombieAnimator;
    // Start is called before the first frame update
    public float damage = 20f;
    public float health = 100f;

    public GameManager gameManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        gameManager = FindObjectOfType<GameManager>();
        //Funciona con el primer hijo que contenga el componente que se busca, no es seguro.
        // zombieAnimator = GetComponentInChildren<Animator>(); 

        /* Forma larga de acceder al compente de un hijo, pero funciona para accder a componentes hijo de otros Gameobjects
         * rootZombie(Variable GameObject) = this.transform.GetChild(1).gameObject;
         * zombieAnimator = rootZombie.GetComponent<Animator>(); */

        // Forma directa al estar ubicados en el objeto padre.
        zombieAnimator = this.transform.GetChild(1).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = player.transform.position;

        if(GetComponent<NavMeshAgent>().velocity.magnitude > 0)
        {
            zombieAnimator.SetBool("isRuning", true);
        }
        else
        {
            zombieAnimator.SetBool("isRuning", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Te estoy atacando");
            player.GetComponent<PlayerManager>().Hit(damage);
        }
    }

    public void Hit (float damage)
    {
        health -= damage;

        if (health < 0)
        {
            // Eliminar al Zombie
            Destroy(this.gameObject);
            gameManager.enemiesAlive--;
        }
    }

}
