using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Animations;

public class Rain : MonoBehaviour
{
    public List<ParticleSystem> rainParticles;
    public List<Tilemap> inkTiles;
    public List<Vector3> enemiesPositions;
    public Tilemap Floor;
    //public Tilemap waterTiles;
    public ParticleSystem spitParticles;
    [Header("Tempo de duração da chuva")]
    public float rainTime;
    [Header("Tempo entre o boss jogar a tinta e a tinta cair")]
    public float spitTime;
    [Header("Tempo que o chão permanece com tinta, após a chuva")]
    public float inkFloorTime;
    public float ySuperiorLimit;
    public float yInferiorLimit;
    public float yAboveFloor;
    public GameObject BasicAggroEnemyPrefab;
    public GameObject EnemyTurretPrefab;
    public GameObject EnemyMagentaPrefab;
    public GameObject EnemyCyanPrefab;
    public GameObject EnemiesParent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRainActive()
    {
        foreach (ParticleSystem rain in rainParticles)
        {
            int sortOption = Random.Range(1, 100);
            if (sortOption % 2 == 0)
                rain.gameObject.SetActive(true);
        }
    }
    public void SetRainTilesActive()
    {
        foreach(ParticleSystem rain in rainParticles)
        {
            int sortOption = Random.Range(1, 100);
            if (rain.gameObject.activeSelf && (sortOption % 4 == 0 || sortOption % 4 == 2 || sortOption % 4 == 1))// 3/4 de chance de ser so tinta
            {
                inkTiles[rainParticles.IndexOf(rain)].gameObject.SetActive(true);
            }
            else if(rain.gameObject.activeSelf && sortOption % 4 == 3)// 1/4 de chance de ter inimigo
            {
                enemiesPositions.Add(FindEnemyPosition(rain.gameObject.transform.position.x));
            }       
        }
    }

    public void SetRainTilesUnactive()
    {
        foreach (Tilemap inkTile in inkTiles)
        {
            if (inkTile.gameObject.activeSelf)
                inkTile.gameObject.SetActive(false);
        }
    }

    public void SetRainTilesTriggerOn()
    {
        foreach (Tilemap inkTile in inkTiles)
        {
            if (inkTile.gameObject.activeSelf)
                inkTile.gameObject.GetComponent<Animator>().SetTrigger("Rain");
        }
    }

    private IEnumerator WaitRain()
    {
        spitParticles.gameObject.SetActive(true);
        yield return new WaitForSeconds(spitTime);
        SetRainActive();
        yield return new WaitForSeconds(rainTime);
        SetRainTilesActive();
        SetEnemyPosition();
        //waterTiles.gameObject.SetActive(true);
        yield return new WaitForSeconds(inkFloorTime);
        spitParticles.gameObject.SetActive(false);
        enemiesPositions.Clear();
        SetRainTilesTriggerOn();
        //waterTiles.gameObject.GetComponent<Animator>().SetTrigger("FillWater");
        yield return new WaitForSeconds(1f);
        SetRainTilesUnactive();
        //waterTiles.gameObject.SetActive(false);
        foreach (ParticleSystem rain in rainParticles)
            rain.gameObject.SetActive(false);
    }

    public void StartRain()
    {
        StartCoroutine(WaitRain());
    }

    public Vector3 FindEnemyPosition(float positionX)//coloca inimigos ´somente onde tem tile de chao
    {
        int i = Mathf.FloorToInt(yInferiorLimit);
        Vector3Int cell;
        Vector3Int cell2;
        Vector3 worldPosition = new Vector3(0, 0, 0);
        for(; i < Mathf.FloorToInt(ySuperiorLimit); i++)
        {
            cell = Floor.WorldToCell(new Vector3(positionX, i, 0));
            cell2 = Floor.WorldToCell(new Vector3(positionX, i + 1, 0));
            if (Floor.HasTile(cell) && !Floor.HasTile(cell2))
                worldPosition = new Vector3(positionX, i + 1f, 0);
        }
        return worldPosition;
    }
    public void SetEnemyPosition()
    {
        int i = 0;
        bool avaiablePosition = true;
        foreach(Vector3 enemyPosition in enemiesPositions)
        {
            while (i < EnemiesParent.transform.childCount)//verifica se ja tem inimigo naquele tile e impede que apareca outro
            {
                if (Floor.WorldToCell(EnemiesParent.transform.GetChild(i).position) == Floor.WorldToCell(enemyPosition))
                {
                    avaiablePosition = false;
                    break;
                }  
                i++;
            }

            if (enemyPosition.y < yAboveFloor && avaiablePosition)
            {
                GameObject enemy = Instantiate(SortFloorEnemy(), enemyPosition, Quaternion.identity);
                enemy.transform.SetParent(EnemiesParent.transform);
            }
            else if(enemyPosition.y >= yAboveFloor && avaiablePosition)
            {
                GameObject enemy = Instantiate(EnemyTurretPrefab, enemyPosition, Quaternion.identity);
                enemy.transform.SetParent(EnemiesParent.transform);
            }
        }
    }

    public GameObject SortFloorEnemy()//funcao para sortear qual inimigo vai surgir na posicao
    {
        int sortOption = Random.Range(1, 1000) % 4;

        switch (sortOption)
        {
            case 0:
                return EnemyMagentaPrefab;
            case 1:
                return EnemyCyanPrefab;
            case 2:
                return EnemyTurretPrefab;
            default:
                return BasicAggroEnemyPrefab;
        }  
    }
 
}
