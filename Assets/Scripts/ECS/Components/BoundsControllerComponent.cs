using UnityEngine;

public struct BoundsControllerComponent
{
    public enum ActionOnExit { Destroy, Teleport}

    public ActionOnExit action;
}
