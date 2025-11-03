using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MQTTPublisher : MonoBehaviour
{
    public string Topic = "";

    public void Publish(string value)
    {
        MQTTHandler.Instance.Publish(Topic, value);
    }
}
