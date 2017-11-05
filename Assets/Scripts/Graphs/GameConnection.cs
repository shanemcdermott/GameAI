using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConnection : BaseConnection<GameObject>
{
    public GameConnection(GameObject from, GameObject to) : base(from, to)
    {
    }
}
