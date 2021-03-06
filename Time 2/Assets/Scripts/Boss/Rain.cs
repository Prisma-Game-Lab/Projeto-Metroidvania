using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Animations;

public class Rain : MonoBehaviour
{
    [Header("Tilemap dos tiles de chao")]
    public Tilemap Floor;
    [Header("Tilemap da agua que estara na fase desde o inicio")]
    public Tilemap waterTiles;
    /*[Header("Tilemaps das camadas de agua que vao encher a cada golpe do player na lingua")]
    public List<Tilemap> waterSpecialTiles;*/
    [Header("Lista das particulas de chuva")]
    public List<ParticleSystem> rainParticles;
    [Header("Lista de tilemaps das tintas que mancham o chao quando chove")]
    public List<Tilemap> inkTiles;
    [Header("Lista de possiveis posicoes do inimigo nuvem")]
    public List<GameObject> cloudsPositions;
    [Header("Lista de possiveis posicoes do inimigo torreta")]
    public List<GameObject> turretPositions;
    [Header("Quantidade maxima de inimigos nuvem por chuva")]
    public int cloudsLimit;
    //[Header("Quantidade de camadas de agua")]
    //public int SpecialRainRoundsNumber;
    [Header("Tempo de dura��o da chuva")]
    public float rainTime;
    [Header("Tempo entre o boss jogar a tinta e a tinta cair")]
    public float spitTime;
    [Header("Tempo que o ch�o permanece com tinta, ap�s a chuva")]
    public float inkFloorTime;
    [Header("Altura do limite superior da fase")]
    public float ySuperiorLimit;
    [Header("Altura do limite inferior da fase")]
    public float yInferiorLimit;
    [Header("Altura a partir da qual podem aparecer inimigos nuvem")]
    public float yAboveFloor;
    public ParticleSystem spitParticles;
    [HideInInspector] public List<Vector3> enemiesPositions;
    [HideInInspector] public bool specialRain = false;
    [Header("Minimo de lugares onde deve haver chuva")]
    public int rainsMin;
    [Header("Minimo de inimigos surgindo por chuva")]
    public int enemiesMin;
    [Header("Minimo de manchas de tinta no chao por chuva")]
    public int inkMin;
    /*[Header("Maximo de lugares onde deve haver chuva")]
    public int rainsMax;
    [Header("Maximo de inimigos surgindo por chuva")]
    public int enemiesMax;
    [Header("Maximo de manchas de tinta no chao por chuva")]
    public int inkMax;*/

    //Prefabs dos inimigos
    //public GameObject BasicAggroEnemyPrefab;
    public GameObject EnemyTurretPrefab;
    //public GameObject EnemyMagentaPrefab;
    //public GameObject EnemyCyanPrefab;
    public GameObject EnemyYellowCloudPrefab;
    public GameObject EnemiesParent;

    //private int _actualSpecialRainRound = 0;
    public void SetRainActive()
    {
        int numOfRains = 0;
        /*if (specialRain)
        {
            foreach (ParticleSystem rain in rainParticles)
            {
                    rain.gameObject.SetActive(true);
            }
            return;
        }*/
        while(numOfRains < rainsMin)
        {
            foreach (ParticleSystem rain in rainParticles)
            {
                int sortOption = Random.Range(1, 100);
                if (sortOption % 2 == 0 && !rain.gameObject.activeSelf)
                {
                    rain.gameObject.SetActive(true);
                    numOfRains++;
                }
                    
            }
        }
        
    }
    public void SetRainTilesActive()
    {
        int numOfEnemies = 0;
        int numOfInkTiles = 0;
        /*if (specialRain)
        {
            //encher camada de agua correspondente ao round
            waterSpecialTiles[_actualSpecialRainRound].gameObject.SetActive(true);
            _actualSpecialRainRound++;
            return;
        }*/
        foreach (ParticleSystem rain in rainParticles)
        {
            int sortOption = Random.Range(1, 100);
            if (rain.gameObject.activeSelf && (sortOption % 4 == 0 || sortOption % 4 == 2))// 1/2 de chance de ser so tinta
            {
                if (numOfInkTiles >= inkMin && numOfEnemies < enemiesMin)
                {
                    //Vector3 position = FindEnemyPosition(rain.gameObject.transform.position.x);
                    //if (CheckIfEnemyPositionAvailable(position))
                    //{
                    //enemiesPositions.Add(position);
                    cloudsPositions[rainParticles.IndexOf(rain)].GetComponent<CloudPosition>().haveRained = true;
                    turretPositions[rainParticles.IndexOf(rain)].GetComponent<TurretPosition>().haveRained = true;
                    //}
                    numOfEnemies++;
                }
                else
                {
                    inkTiles[rainParticles.IndexOf(rain)].gameObject.SetActive(true);
                    numOfInkTiles++;
                }

                }
            else if (rain.gameObject.activeSelf && (sortOption % 4 == 1 || sortOption % 4 == 3))// 1/2 de chance de ter inimigo
            {
                    
                if (numOfEnemies >= enemiesMin && numOfInkTiles < inkMin)
                {
                    inkTiles[rainParticles.IndexOf(rain)].gameObject.SetActive(true);
                    numOfInkTiles++;
                }
                else
                {
                    //Vector3 position = FindEnemyPosition(rain.gameObject.transform.position.x);
                    //if (CheckIfEnemyPositionAvailable(position))
                    //{
                    //enemiesPositions.Add(position);
                    cloudsPositions[rainParticles.IndexOf(rain)].GetComponent<CloudPosition>().haveRained = true;
                    turretPositions[rainParticles.IndexOf(rain)].GetComponent<TurretPosition>().haveRained = true;
                    //}
                    numOfEnemies++;
                }

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
        gameObject.GetComponent<Animator>().SetTrigger("Spit");
        yield return new WaitForSeconds(spitTime);
        SetRainActive();
        gameObject.GetComponent<Animator>().SetTrigger("Idle");
        yield return new WaitForSeconds(rainTime);
        SetRainTilesActive();
        SetEnemyPosition();
        yield return new WaitForSeconds(inkFloorTime);
        spitParticles.gameObject.SetActive(false);
        enemiesPositions.Clear();
        SetRainTilesTriggerOn();
        yield return new WaitForSeconds(1f);
        SetRainTilesUnactive();
        foreach (ParticleSystem rain in rainParticles)
            rain.gameObject.SetActive(false);
    }

    private IEnumerator SpecialRain()
    {
        spitParticles.gameObject.SetActive(true);
        yield return new WaitForSeconds(spitTime);
        SetRainActive();
        yield return new WaitForSeconds(rainTime);
        SetRainTilesActive();
        yield return new WaitForSeconds(inkFloorTime);
        spitParticles.gameObject.SetActive(false);
        SetRainTilesTriggerOn();
        yield return new WaitForSeconds(1f);
        foreach (ParticleSystem rain in rainParticles)
            rain.gameObject.SetActive(false);
    }

    public void StartRain()
    {
        StartCoroutine(WaitRain());
    }
    public void StartSpecialRain()
    {
        StartCoroutine(SpecialRain());
    }

    /*public Vector3 FindEnemyPosition(float positionX)//coloca inimigos somente onde tem tile de chao e nao tem agua
    {
        
        Vector3Int cell;
        Vector3Int cell2;
        Vector3 worldPosition = new Vector3(0f, 0f, 0f);
        for(int i = Mathf.FloorToInt(yInferiorLimit); i < Mathf.FloorToInt(ySuperiorLimit) + 1; i++)
        {
            cell = Floor.WorldToCell(new Vector3(positionX, i, 0));
            cell2 = Floor.WorldToCell(new Vector3(positionX, i + 1, 0));
            if (Floor.HasTile(cell) && !(Floor.HasTile(cell2)) && !CheckWater(cell2) && !CheckWater(cell))
            {
                worldPosition = new Vector3(positionX, i + 1.5f, 0);
            }
                
        }
        
        return worldPosition;
    }*/
    public bool CheckIfEnemyPositionAvailable(Vector3 position)//coloca inimigos somente onde tem tile de chao e nao tem agua
    {
        int i = 0;
        while (i < EnemiesParent.transform.childCount)//verifica se ja tem inimigo naquele tile e impede que apareca outro
        {
            //Debug.Log(enemyPosition);
            Vector3 positionRigth = new Vector3(position.x - 1f, position.y, position.z);
            Vector3 positionLeft = new Vector3(position.x + 1f, position.y, position.z);
            float distanceX = Mathf.Abs(EnemiesParent.transform.GetChild(i).position.x - position.x);
            float distanceY = Mathf.Abs(EnemiesParent.transform.GetChild(i).position.y - position.y);
            if (distanceX < 4f && distanceY < 2f)
            {
                return false;
            }
            i++;
        }
        return true;
    }

    public void SetEnemyPosition()
    {
        int i = 0;
        bool availablePosition = true;
        int numberOfClouds = 0;
        int numberOfTurrets = 0;
        /*foreach(Vector3 enemyPosition in enemiesPositions)
        {
            availablePosition = true;
            i = 0;
            /*while (i < EnemiesParent.transform.childCount)//verifica se ja tem inimigo naquele tile e impede que apareca outro
            {
                Debug.Log(enemyPosition);
                if (Floor.WorldToCell(enemyPosition) == Floor.WorldToCell(EnemiesParent.transform.GetChild(i).position))
                {
                    Debug.Log("Ja existe inimigo");
                    availablePosition = false;
                    break;
                }  
                i++;
            }*/

        /* if (enemyPosition.y < yAboveFloor && availablePosition)
         {
             Debug.Log("Inimigo chao");
             GameObject enemy = Instantiate(SortFloorEnemy(), enemyPosition, Quaternion.identity);
             //enemy.transform.SetParent(EnemiesParent.transform);
             //enemy.GetComponent<EnemyMovement>().enemyMovement = SimpleEnemyMovements.None;
         }
         else if(enemyPosition.y >= yAboveFloor && availablePosition)
         {
             Debug.Log("Inimigo plataforma");
             GameObject enemyType = SortPlatformEnemy();
             if(enemyType == EnemyYellowCloudPrefab && numberOfClouds<cloudsLimit)
             {
                 foreach (GameObject cloudPosition in cloudsPositions)
                 {
                     if (cloudPosition.GetComponent<CloudPosition>().haveRained && cloudPosition.GetComponent<CloudPosition>().isCloudPosition)
                     {
                         GameObject enemy = Instantiate(enemyType, cloudPosition.transform.position, Quaternion.identity);
                         enemy.GetComponent<EnemyMovement>().enemyMovement = SimpleEnemyMovements.None;
                         //enemy.transform.SetParent(EnemiesParent.transform);
                         cloudPosition.GetComponent<CloudPosition>().haveRained = false;
                         numberOfClouds++;
                         break;
                     }
                 }

             }
             else
             {
                 GameObject enemy = Instantiate(EnemyTurretPrefab, enemyPosition, Quaternion.identity);
                 //enemy.transform.SetParent(EnemiesParent.transform);
             } 
         }
         else
         {
             Debug.Log("Posicao j� ocupada");
         }
     }*/

        foreach (ParticleSystem rain in rainParticles)
        {
            availablePosition = true;
            i = 0;
            GameObject enemyType = SortPlatformEnemy();
            
            if(enemyType == EnemyYellowCloudPrefab && numberOfClouds<cloudsLimit)
            {
                foreach (GameObject cloudPosition in cloudsPositions)
                {
                    if (cloudPosition.GetComponent<CloudPosition>().haveRained && cloudPosition.GetComponent<CloudPosition>().isCloudPosition && CheckIfEnemyPositionAvailable(cloudPosition.transform.position))
                    {
                        GameObject enemy = Instantiate(EnemyYellowCloudPrefab, cloudPosition.transform.position, Quaternion.identity);
                        enemy.GetComponent<EnemyMovement>().enemyMovement = SimpleEnemyMovements.None;
                        enemy.transform.SetParent(EnemiesParent.transform);
                        cloudPosition.GetComponent<CloudPosition>().haveRained = false;
                        numberOfClouds++;
                        break;
                    }
                }

            }
            else
            {
                foreach (GameObject turretPosition in turretPositions)
                {
                    if (turretPosition.GetComponent<TurretPosition>().haveRained && CheckIfEnemyPositionAvailable(turretPosition.transform.position))
                    {
                        GameObject enemy = Instantiate(EnemyTurretPrefab, turretPosition.transform.position, Quaternion.identity);
                        enemy.transform.SetParent(EnemiesParent.transform);
                        turretPosition.GetComponent<TurretPosition>().haveRained = false;
                        enemy.GetComponent<EnemyMovement>().enemyMovement = SimpleEnemyMovements.None;
                        numberOfTurrets++;
                        break;
                    }
                }
            }
        }

    }



     public GameObject SortFloorEnemy()//funcao para sortear qual inimigo vai surgir na posicao
     {
         return EnemyTurretPrefab;
     }

     public GameObject SortPlatformEnemy()//funcao para sortear qual inimigo vai surgir na posicao
     {
         int sortOption = Random.Range(1, 1000) % 2;

         switch (sortOption)
         {
             case 0:
                 return EnemyTurretPrefab;
             default:
                 return EnemyYellowCloudPrefab;
         }
     }

     public bool CheckWater(Vector3Int cell)
     {
         if (waterTiles.HasTile(cell))
             return true;
         /*foreach(Tilemap water in waterSpecialTiles)
         {
             if (water.gameObject.activeSelf && water.HasTile(cell))
             {
                 return true;
             }
         }*/
            return false;
    }

}
