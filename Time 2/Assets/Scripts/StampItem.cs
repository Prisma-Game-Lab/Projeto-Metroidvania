using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampItem : MonoBehaviour
{
    public Stamp stamp;
    private PlayerStatus _playerStatus;
    
    // Start is called before the first frame update

    void Start()
    {
        if (stamp.obtained)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStatus>().SetStampStatus(stamp.mailBoxToGo);
            collision.GetComponent<PlayerStatus>().UpdateStampStatus(stamp);
            Destroy(gameObject);
        }

    }
}
