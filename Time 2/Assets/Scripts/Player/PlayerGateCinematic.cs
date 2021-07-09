using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerGateCinematic : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    public float WalkVelocity = 5;
    private PlayerStatus _playerStatus;

    public float timeUntilFade;
    // Start is called before the first frame update
    void Start()
    {
        CinemachineVirtualCamera[] components = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        _cinemachineVirtualCamera = components[0];
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
    }

    public float PerformWarpAnimation(bool isFlipped)
    {
        _playerStatus.playerInput.enabled = false;
        _cinemachineVirtualCamera.Follow = null;
        StartCoroutine(WarpAnimation(isFlipped));
        return timeUntilFade;
    }

    private IEnumerator WarpAnimation(bool isFlipped)
    {
        Vector3 reachPoint = transform.position;
        _playerStatus.playerMovement.enabled = false;
        _playerStatus.playerAnimator.SetBool("Moving", true);
        if (isFlipped)
        {         
            reachPoint.x += 10;
            while (transform.position.x < reachPoint.x )
            {
                Vector3 aux = transform.position;
                aux.x -= WalkVelocity/100f;
                transform.position = aux;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            reachPoint.x -= 10;
            while (transform.position.x > reachPoint.x )
            {
                Vector3 aux = transform.position;
                aux.x += WalkVelocity/100f;
                transform.position = aux;
                yield return new WaitForEndOfFrame();
            }
            
        }
            


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
