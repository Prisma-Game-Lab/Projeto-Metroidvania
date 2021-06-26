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
    [HideInInspector]public GameObject inkTileWall;
    private void Start()
    {
        
    }

    public void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        Vector3 hitPosition = other.transform.position;
        Vector3 bulletPosition = gameObject.transform.position;
        if (other.gameObject.CompareTag("Floor"))
        {
            Vector3 position = gameObject.GetComponent<Transform>().position;
            Vector3Int cell = floor.WorldToCell(position);
            position = floor.CellToWorld(cell);
            Vector3 positionCorrected = new Vector3(position.x + 0.5f, position.y - 0.5f, position.z);
            GameObject ink = Instantiate(inkTileFloor, positionCorrected, Quaternion.identity);
                   
            Destroy(gameObject);
            // REALIZAR ANIMACAO DE TINTA ESPARRAMANDO NA PAREDE e depois destruir o objeto
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Vector3 position = gameObject.GetComponent<Transform>().position;
            Vector3Int cell = floor.WorldToCell(position);
            position = floor.CellToWorld(cell);
            Vector3 positionCorrected = new Vector3(position.x + 0.5f, position.y - 0.5f, position.z);
            GameObject ink = Instantiate(inkTileWall, positionCorrected, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.GetMask("Water"))
        {
            Destroy(gameObject);
        }


    }
    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(bulletTimeLimit);
        Destroy(gameObject);

    }
}
