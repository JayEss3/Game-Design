using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FollowPath : NetworkBehaviour
{
    [SerializeField] private List<Transform> path = new List<Transform>();
    [SerializeField] private FollowPathStates state = FollowPathStates.TraseBackLoop;
    [SerializeField] private float Speed = 5f;
    private int curPath = 0;
    private void Awake()
    {
        var temp = Instantiate(new GameObject(), transform.position, transform.rotation);
        path.Insert(0, temp.transform);
    }

    private void FixedUpdate()
    {
        if (!isServer) return;
        switch (state)
        {
            case FollowPathStates.DoNotLoop:
                if (curPath < path.Count)
                    Move();
                break;
            case FollowPathStates.Loop:
                if (curPath < path.Count)
                    Move();
                else
                {
                    curPath = 0;
                    transform.position = path[0].position;
                }
                break;
            case FollowPathStates.TraseBackLoop:
                if (curPath < path.Count)
                    Move();
                else
                {
                    path.Reverse();
                    curPath = 0;
                }
                break;
        }
    }
    [Server]
    private void Move()
    {
        if (Vector3.Distance(transform.position, path[curPath].position) > 0.01f)
            transform.position = Vector3.MoveTowards(transform.position, path[curPath].position, Time.deltaTime * Speed);
        else
            curPath++;
    }
}
