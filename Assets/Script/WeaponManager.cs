using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject playerCamera;
    public float range = 100f;
    public float damage = 25f;
    public Animator playerAnimator;
    void Start()
    {

    }

    void Update()
    {
        if (playerAnimator.GetBool("isShooting") == true)
        {
            playerAnimator.SetBool("isShooting", false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        playerAnimator.SetBool("isShooting", true);
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, this.transform.forward, out hit, range))
        {
            Debug.Log("Hemos Herido Algo");
            EnemyAgent enemyAgent = hit.transform.GetComponent<EnemyAgent>();
            if(enemyAgent != null )
            {
                enemyAgent.Hit(damage);
            }
        }
    }
}
