using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCloud : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Projetil da Chuva")]
    public GameObject enemyBullet;
    
    [Header("Tempo para a proxima rajada de chuva")]
    public float rainWaitTime;
    
    [Header("Velocidade da chuva")]
    public float bulletSpeed;

    private Transform _transform;
    void Start()
    {
        _transform = transform;
        InvokeRepeating("Rain", 2.0f, rainWaitTime);
    }

    private void Rain()
    {
        Debug.Log("Choveu");
        Vector3 pos = new Vector3(_transform.position.x, _transform.position.y - 0.5f, _transform.position.z);
        GameObject bullet = Instantiate(enemyBullet, pos, _transform.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.down*bulletSpeed, ForceMode2D.Impulse);
    }
    
}
