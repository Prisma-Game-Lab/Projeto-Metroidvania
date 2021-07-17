using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelLogic : MonoBehaviour
{
    public GameObject Boss;

    public GameObject respawnPosition;
    private bool isActive = false;

    private void Start()
    {
        AudioManager.instance.Play("Lobby");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerStatus>().BossSafePosition = respawnPosition;
                StartCoroutine(BossBattleStart());
                isActive = true;
            }
        }

    }

    private IEnumerator BossBattleStart()
    {
        AudioManager.instance.Stop("Lobby");
        AudioManager.instance.Play("Boss");
        yield return new WaitForSeconds(1);
        Boss.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1.2f);
        Boss.GetComponent<BossLogic>().StartLogic();
    }
    
}
