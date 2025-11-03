using TMPro;
using UnityEngine;

public class ValueUpdater : MonoBehaviour
{
    TextMeshProUGUI tmp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmp = gameObject.GetComponent<TextMeshProUGUI>();

        MQTTSubscriber subsciber = gameObject.GetComponent<MQTTSubscriber>();


        subsciber.ValueUpdated += Subsciber_ValueUpdated;
    }

    private void Subsciber_ValueUpdated(string value)
    {
        tmp.text = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
