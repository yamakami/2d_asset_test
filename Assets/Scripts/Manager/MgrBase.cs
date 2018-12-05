using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgrBase : MonoBehaviour
{
    protected MgrGame mgrGame;

    // Use this for initialization
    protected virtual void Start ()
    {
        mgrGame = transform.root.GetComponentInParent<MgrGame>();
    }

    protected virtual void StartInputBlock()
    {
        MgrGame.inputBlock = true;
    }

    protected virtual void StopInputBlock()
    {
        MgrGame.inputBlock = false;
    }
}
