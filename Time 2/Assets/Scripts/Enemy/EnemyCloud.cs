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

    [Header("Tempo de duracao da gota(Se estiver zerado, usa o tempo padrao de 2 segundos)")]
    public float bulletTimeLimit;

    private Transform _transform;
    void Start()
    {
        _transform = transform;
        InvokeRepeating("Rain", 2.0f, rainWaitTime);
    }

    private void Rain()
    {
        Vector3 pos = new Vector3(_transform.position.x, _transform.position.y - 0.5f, _transform.position.z);
        GameObject bullet = Instantiate(enemyBullet, pos, _transform.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.down*bulletSpeed, ForceMode2D.Impulse);
        bullet.GetComponent<EnemyBullet>().isRain = true;
        bullet.transform.up = Vector3.down;
        if (bulletTimeLimit == 0f)
            bulletTimeLimit = 2f;
        bullet.GetComponent<EnemyBullet>().bulletTimeLimit = bulletTimeLimit;
    }

}
