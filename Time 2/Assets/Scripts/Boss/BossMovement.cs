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
    [HideInInspector] public bool isFlipped = false;

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
        Flip();
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

    public void Flip()
    {
        if (!gameObject.GetComponent<BossLogic>().tongueAttacking)
        {
            if (!isFlipped && !PlayerInRange())
            {
                Vector3 newLocalScale = transform.localScale;
                newLocalScale.x *= -1;
                transform.localScale = newLocalScale;
                isFlipped = true;
            }

            // movendo para a direta flipado. Vai flipar 
            if (isFlipped && PlayerInRange())
            {
                Vector3 newLocalScale = transform.localScale;
                newLocalScale.x *= -1;
                transform.localScale = newLocalScale;
                isFlipped = false;
            }
        }
    }

    private bool PlayerInRange()
    {
        LayerMask layer = LayerMask.GetMask("Player");
        Collider2D[] hitWall = Physics2D.OverlapCircleAll(transform.position, 300, layer);

        // Verificar se o player esta no range 
        if (hitWall.Length > 0)
        {
            Vector3 playerPosition = hitWall[0].transform.position;
            if (transform.position.x > playerPosition.x)
            {
                return true;
            }
            else
                return false;


        }

        return false;

    }
}
