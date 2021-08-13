using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class BoundsControllerSystem : IEcsRunSystem
{
    private EcsFilter<TransformComponent, BoundsControllerComponent> _filter = null; 

    public void Run() 
    {
        Vector2 cameraMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 cameraMax = Camera.main.ViewportToWorldPoint(Vector2.one);

        foreach (int i in _filter) 
        {
            ref TransformComponent trans = ref _filter.Get1(i);
            ref BoundsControllerComponent controller = ref _filter.Get2(i);

            if (controller.action == BoundsControllerComponent.ActionOnExit.Teleport)
            {
                Vector2 pos = trans.transform.position;
                if (pos.x < cameraMin.x) pos.x = cameraMax.x;
                if (pos.x > cameraMax.x) pos.x = cameraMin.x;
                if (pos.y < cameraMin.y) pos.y = cameraMax.y;
                if (pos.y > cameraMax.y) pos.y = cameraMin.y;
                trans.transform.position = pos;
            }
            else 
            {
                Vector2 pos = trans.transform.position;
                bool outOfX = pos.x < cameraMin.x - 2f || pos.x > cameraMax.x + 2f;
                bool outOfY = pos.y < cameraMin.y - 2f || pos.y > cameraMax.y + 2f;
                if (outOfX || outOfY) 
                {
                    GameObject.Destroy(trans.transform.gameObject);
                    _filter.GetEntity(i).Destroy();
                }
            }
        }
    }
}
