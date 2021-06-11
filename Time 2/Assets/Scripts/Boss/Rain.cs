using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Animations;

public class Rain : MonoBehaviour
{
    public List<ParticleSystem> rainParticles;
    public List<Tilemap> inkTiles;
    public ParticleSystem spitParticles;
    public float rainTime;
    public float spitTime;
    public float inkFloorTime;
    public Tilemap waterTiles;

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
            if (rain.gameObject.activeSelf)
                inkTiles[rainParticles.IndexOf(rain)].gameObject.SetActive(true);
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
        //waterTiles.gameObject.SetActive(true);
        yield return new WaitForSeconds(inkFloorTime);
        spitParticles.gameObject.SetActive(false);
        SetRainTilesTriggerOn();
        //waterTiles.gameObject.GetComponent<Animator>().SetTrigger("FillWater");
        yield return new WaitForSeconds(1f);
        SetRainTilesUnactive();
        //waterTiles.gameObject.SetActive(false);
        foreach (ParticleSystem rain in rainParticles)
        {
            rain.gameObject.SetActive(false);
        }
    }


    public void StartRain()
    {
        StartCoroutine(WaitRain());
    }
}
