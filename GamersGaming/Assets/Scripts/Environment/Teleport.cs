using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private KeyBindings keyBindings;
    [SerializeField] private string prebuttonText;
    [SerializeField] private string postbuttonText;
    [SerializeField] private Transform location;
    private void OnValidate()
    {
        if (!keyBindings)
            keyBindings = Resources.Load<KeyBindings>("Objects/KeyBindings");
    }
    public void Interact(GameObject player)
    {
        player.transform.position = location.position;
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
