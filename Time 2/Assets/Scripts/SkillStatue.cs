using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillStatue : MonoBehaviour
{
    // skill que o item vai dar para o player
    public PlayerSkill skill;
    [HideInInspector]public Text helpDescription;
    // Start is called before the first frame update
    void Start()
    {
        helpDescription = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
