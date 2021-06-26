using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Posições onde o Boss fica se movendo")]
    public List<GameObject> BossPositions;

    [Header("Em qual das posições o Boss vai começar a andar (0 ate N)")]
    public int startPosition;

    public float speed;

    private int actualPos;

    [HideInInspector] public bool IsStoped = false;

    // Start is called before the first frame update
    void Start()
    {
        actualPos = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStoped)
        {
            Move();
        }
    }

    private void Move()
    {

        float step = speed * Time.deltaTime;
        Vector3 goPos = new Vector3(BossPositions[actualPos].transform.position.x, BossPositions[actualPos].transform.position.y, transform.position.z );
        transform.position = Vector3.MoveTowards(transform.position, goPos, step);
        if (transform.position == goPos)
        {
            actualPos++;
            if (actualPos >= BossPositions.Count )
            {
                actualPos = startPosition;
                startPosition++;
                if (startPosition >= BossPositions.Count)
                    startPosition = 0;
            }

            IsStoped = true;
        }
    }
}
