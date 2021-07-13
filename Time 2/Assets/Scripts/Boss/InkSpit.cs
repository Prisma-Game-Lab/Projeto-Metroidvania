using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InkSpit : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public float bulletTimeLimit = 0f;
    [Header("Tilemap do chao:")]
    [HideInInspector]public Tilemap floor;
    [Header("Tilemap que vai receber as camadas de tinta do cuspe de tinta do Boss:")]
    [HideInInspector]public GameObject inkTileFloor;
    [HideInInspector] public GameObject inkTileWall;
    
    private void Start()
    {
        
    }

    public void Update()
    {
        

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
   
        if (other.gameObject.CompareTag("Floor"))
        {
            Vector3 position = gameObject.GetComponent<Transform>().position;
            Vector3Int cell = floor.WorldToCell(position);
            position = floor.CellToWorld(cell);
            int direction = DetectColisionDirection(cell);
            Vector3 positionCorrected = new Vector3(0f, 0f, 0f);
            GameObject ink;
            switch (direction)
            {
                case 1:
                    position = floor.CellToWorld(cell + new Vector3Int(0, -1, 0));
                    positionCorrected = new Vector3(position.x + 0.5f, position.y + 0.5f, position.z);
                    ink = Instantiate(inkTileFloor, positionCorrected, Quaternion.identity);
                    Debug.Log("Chao\n");
                    break;
                case 2:
                    position = floor.CellToWorld(cell + new Vector3Int(1, 0, 0));
                    positionCorrected = new Vector3(position.x + 0.5f, position.y + 0.5f, position.z);
                    ink = Instantiate(inkTileWall, positionCorrected, Quaternion.identity);
                    Debug.Log("direita\n");
                    break;
                case 3:
                    position = floor.CellToWorld(cell + new Vector3Int(-1, 0, 0));
                    positionCorrected = new Vector3(position.x + 0.5f, position.y + 0.5f, position.z);
                    ink = Instantiate(inkTileWall, positionCorrected, Quaternion.identity);
                    Vector3 newLocalScale = ink.transform.localScale;
                    newLocalScale.x *= -1;
                    ink.transform.localScale = newLocalScale;
                    Debug.Log("esquerda\n");
                    break;
            }
            Destroy(gameObject);

        }
        else if (other.gameObject.layer == LayerMask.GetMask("Water"))
        {
            Destroy(gameObject);
        }
        else if(other.gameObject.layer != LayerMask.GetMask("Player"))
        {
            StartCoroutine(DestroyInk());
            
        }

    }

    public int DetectColisionDirection(Vector3Int cell)
    {
        if (floor.HasTile(cell + new Vector3Int(0, -1, 0)))
            return 1; //tem tile embaixo
        else if (floor.HasTile(cell + new Vector3Int(1, 0, 0)))
            return 2;//tem tile a direita
        else if (floor.HasTile(cell + new Vector3Int(-1, 0, 0)))
            return 3;//tem tile a direita
        else
            return 0;//tem tile a esquerda
    }

    private IEnumerator DestroyInk()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    

}
