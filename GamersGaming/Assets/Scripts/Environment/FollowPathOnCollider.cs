using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathOnCollider : MonoBehaviour
{
    [SerializeField] private List<Transform> path = new List<Transform>();
    [SerializeField] private FollowPathStates state = FollowPathStates.TraseBackLoop;
    [SerializeField] private float Speed = 5f;
    private int curPath = 0;
    private bool attached = false;
    private void Awake()
    {
        var temp = Instantiate(new GameObject(), transform.position, transform.rotation);
        path.Insert(0, temp.transform);
    }

    private void FixedUpdate()
    {
        if (attached)
        {
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
    }
    private void Move()
    {
        if (Vector3.Distance(transform.position, path[curPath].position) > 0.01f)
            transform.position = Vector3.MoveTowards(transform.position, path[curPath].position, Time.deltaTime * Speed);
        else
            curPath++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            attached = true;
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            attached = false;
            other.transform.parent = null;
        }
    }
}
