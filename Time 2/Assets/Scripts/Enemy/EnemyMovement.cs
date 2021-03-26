using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float speed;

    public Transform pointStart;

    public Transform pointEnd;

    public SimpleEnemyMovements enemyMovement;

    // movementControl
    private bool _back = false;
    // Update is called once per frame
    void Update()
    {
        if(enemyMovement == SimpleEnemyMovements.Horizontal)
        {
            HorizontalMovement();
        }
    }


    // Realização de movimento horizontalk simples
    private void HorizontalMovement()
    {
        Vector3 pointToStart = new Vector3(pointStart.position.x, transform.position.y, transform.position.z);
        Vector3 pointToEnd = new Vector3(pointEnd.position.x, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(pointToStart, pointToEnd, Mathf.PingPong(Time.time * speed, 1.0f));
    }

}

public enum SimpleEnemyMovements
{
    Horizontal,
    Vertical
}
