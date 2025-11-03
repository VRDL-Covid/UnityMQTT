using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MQTTSubscriber : MonoBehaviour
{
    public string RootTopic = string.Empty;

    public string Topic = "";

    public string Value = "";

    public delegate void MQTTValueChangedHandler(string value);
    public event MQTTValueChangedHandler ValueUpdated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MQTTHandler.Instance.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async System.Threading.Tasks.Task UpdateValue(string value)
    {
        Value = value;
        await System.Threading.Tasks.Task.Run(() => ValueUpdated.Invoke(value));
    }
}
