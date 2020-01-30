using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipTravel : MonoBehaviour
{
    private KeyBindings keyBindings;
    [Header("User Interface")]
    [SerializeField] private string prebuttonText;
    [SerializeField] private string postbuttonText;
    [SerializeField] private GameObject TravelCamera;
    [Header("Path Control")]
    [SerializeField] private List<Transform> path = new List<Transform>();
    [SerializeField] private FollowPathStates state = FollowPathStates.Stopped;
    [SerializeField] private float Speed = 5f;
    private int curPath = 0;
    private GameObject _player;
    private void OnValidate()
    {
        if (!keyBindings)
            keyBindings = Resources.Load<KeyBindings>("Objects/KeyBindings");
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            case FollowPathStates.DoNotLoop:
                if (curPath < path.Count)
                    Move();
                else
                {
                    _player.SetActive(true);
                    TravelCamera.SetActive(false);
                    _player.transform.parent = null;
                    state = FollowPathStates.Stopped;
                }
                break;
        }
    }
    private void Move()
    {
        if (Vector3.Distance(transform.position, path[curPath].position) > 0.05f)
            transform.position = Vector3.MoveTowards(transform.position, path[curPath].position, Time.deltaTime * Speed);
        else
            curPath++;
    }
    public void Interact(GameObject player)
    {
        _player = player;
        player.transform.parent = transform;
        player.SetActive(false);
        TravelCamera.SetActive(true);
        state = FollowPathStates.DoNotLoop;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.transform.Find("Player_UserInterface").Find("UserInteraction").GetComponent<TextMeshProUGUI>().text = $"{prebuttonText} {keyBindings.Interact} {postbuttonText}";
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.transform.Find("Player_UserInterface").Find("UserInteraction").GetComponent<TextMeshProUGUI>().text = "";
    }
}
