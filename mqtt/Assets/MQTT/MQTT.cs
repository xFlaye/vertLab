

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using System.Net.Security;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Security.Cryptography.X509Certificates;
using System;

public class MQTT : MonoBehaviour
{
    //http://tdoc.info/blog/2014/11/10/mqtt_csharp.html
    private MqttClient client;
    // The connection information
    public string brokerHostname = "ec2-11-111-11.us-west-2.compute.amazonaws.com";
    public int brokerPort = 8883;
    public string userName = "test";
    public string password = "test";
    public TextAsset certificate;
    // listen on all the Topic
    static string subTopic = "#";

	//! data out
	public float fData;

    void Start()
	{
		if (brokerHostname != null && userName != null && password != null)
		{
			Debug.Log("connecting to " + brokerHostname + ":" + brokerPort);
			Connect();
			client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
			byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
			client.Subscribe(new string[] { subTopic }, qosLevels);
		}
	}

    private void Connect()
	{
		Debug.Log("about to connect on '" + brokerHostname + "'");
		// Forming a certificate based on a TextAsset
		X509Certificate cert = new X509Certificate();
		//cert.Import(certificate.bytes);
		Debug.Log("Using the certificate '" + cert + "'");
		client = new MqttClient(brokerHostname, brokerPort, true, null/*cert*/, null, MqttSslProtocols.TLSv1_0, MyRemoteCertificateValidationCallback);
		string clientId = Guid.NewGuid().ToString();
		Debug.Log("About to connect using '" + userName + "' / '" + password + "'");
		try
		{
			client.Connect(clientId, userName, password);
		}
		catch (Exception e)
		{
			Debug.LogError("Connection error: " + e);
		}
	}

    public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		return true;
	}

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
	{
		string msg = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log ("Received message from " + e.Topic + " : " + msg);

		//! convert msg string to float
		fData = float.Parse(msg);
		

		Debug.Log("float value: "+ fData);
		
		
	}



    private void Publish(string _topic, string msg)
	{
		client.Publish(
			_topic, Encoding.UTF8.GetBytes(msg),
			MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
	}

    void OnDestroy() {
        client.Disconnect();
    }
}

