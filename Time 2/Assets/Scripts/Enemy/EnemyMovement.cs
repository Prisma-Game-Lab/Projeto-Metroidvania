using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float magnetude;

    public SimpleEnemyMovements enemyMovement;

    // Update is called once per frame
    void Update()
    {
        if(enemyMovement == SimpleEnemyMovements.Horizontal)
        {
            HorizontalMovement();
        }
        else if(enemyMovement == SimpleEnemyMovements.Vertical)
        {
            VerticalMovement();
        }
    }

    // Realização de movimento vertical simples
    private void VerticalMovement()
    {
        transform.position = transform.position + transform.up * Mathf.Sin(Time.time) * magnetude;
    }

    // Realização de movimento horizontalk simples
    private void HorizontalMovement()
    {
        transform.position = transform.position + transform.right * Mathf.Sin(Time.time) * magnetude/100f;
    }

}

public enum SimpleEnemyMovements
{
    Horizontal,
    Vertical
}
