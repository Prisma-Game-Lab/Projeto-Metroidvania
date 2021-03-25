using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerSkill
{
    boatMode
}

public class PlayerStatus : MonoBehaviour
{

    private bool _boatSkill = false;
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Water" && !_boatSkill)
        {
            Debug.Log(_boatSkill);
            gameObject.GetComponent<PlayerDamage>().KillPlayer();
        }

    }

    public void SetPlayerSkill(PlayerSkill skill)
    {
        if (skill == PlayerSkill.boatMode)
            _boatSkill = true;
    }

}
