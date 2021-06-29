using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedFloor : MonoBehaviour
{

    public float inkTimeLimit;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        StartCoroutine(DestroyInk());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DestroyInk()
    {
        yield return new WaitForSeconds(inkTimeLimit);
        Destroy(gameObject);

    }
}
