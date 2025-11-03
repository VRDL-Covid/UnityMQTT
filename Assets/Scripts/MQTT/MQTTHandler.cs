using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using MQTTnet.Client.Extensions;
using NUnit.Framework.Constraints;
using MQTTnet.Client;
using MQTTnet.Client.Extensions;
using MQTTnet;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using MQTTnet.Server;
using System.Linq;

public class MQTTHandler
{

    public List<MQTTSubscriber> subscribers = new List<MQTTSubscriber>();

    public List<string> subscribedTopics = new List<string>();

    private  IMqttClient mqttClient;

    // Singleton Accessors
    private static MQTTHandler instance = null;
    public static MQTTHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MQTTHandler();

                instance.InitializeHandlerAsync();
            }
            return instance;
        }
    }





    public MQTTHandler()
    {
    }


    private async void InitializeHandlerAsync()
    {
        bool connectionSussessfull = false;
        try
        {
            MqttConnectionSettings cs = new MqttConnectionSettings("127.0.0.1")
            {
                TcpPort = 8900,
                UseTls = false
            };

            MqttClientOptions opsBuilder = new MqttClientOptionsBuilder().WithConnectionSettings(cs).Build();

            MqttFactory mqttfactory = new MqttFactory();

            mqttClient = mqttfactory.CreateMqttClient();

            MqttClientConnectResult connAck = await mqttClient.ConnectAsync(opsBuilder);

            Debug.Log($"Client Connected: {mqttClient.IsConnected} with CONNACK: {connAck.ResultCode}");

            mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;

        }
        catch
        {
            connectionSussessfull = true;
        }
    }

    private async Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        foreach (MQTTSubscriber subscriber in subscribers.Where(r => r.Topic == arg.ApplicationMessage.Topic))
        {
            await subscriber.UpdateValue(arg.ApplicationMessage.ConvertPayloadToString());
        }
    }


    public async void Publish(string topic, string value)
    {
        await mqttClient.PublishStringAsync(topic, value);
    }

    public async void Subscribe(MQTTSubscriber mQTTSubscriber)
    {

        if (!string.IsNullOrEmpty(mQTTSubscriber.RootTopic))
        {
            await mqttClient.SubscribeAsync(mQTTSubscriber.RootTopic + @"/#", MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);
        }
        else
        {
            await mqttClient.SubscribeAsync(mQTTSubscriber.Topic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);
        }

        subscribers.Add(mQTTSubscriber);

            return;

        subscribers.Add(mQTTSubscriber);

        if (!string.IsNullOrEmpty(mQTTSubscriber.RootTopic))
        {
            if (!subscribedTopics.Contains(mQTTSubscriber.RootTopic))
            {
                
                await mqttClient.SubscribeAsync(mQTTSubscriber.RootTopic);
            }
        }
        else
        {
            if (!subscribedTopics.Contains(mQTTSubscriber.Topic))
            {

            }
        }

    }



}
