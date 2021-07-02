using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeStatue : MonoBehaviour
{
    private PlayerStatus _playerStatus;
    // Start is called before the first frame update
    void Start()
    {
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
    }

    public void FillLife()
    {
        PlayerHealth life = _playerStatus.playerDamage.life;
        life.life = life.totalLife;
    }
    
    public void AddNewHeart(int statueId)
    {
        for (int i = 0; i < _playerStatus.NewHeartsId.Length; i++)
        {
            if (statueId == _playerStatus.NewHeartsId[i])
            {
                return;
            }
        }
        
        _playerStatus.totalLife += 1;
        _playerStatus.playerDamage.life.totalLife += 1;
        List<int> heartsIds = new List<int>(_playerStatus.NewHeartsId);
        heartsIds.Add(statueId);
        _playerStatus.NewHeartsId = heartsIds.ToArray();
        SaveSystem.SavePlayer(_playerStatus);
        FillLife();
    }
}
