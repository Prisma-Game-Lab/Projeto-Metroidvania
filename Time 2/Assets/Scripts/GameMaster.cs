using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

    public void OnReset(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
            SceneManager.LoadScene("TestScene2");
    }

}
