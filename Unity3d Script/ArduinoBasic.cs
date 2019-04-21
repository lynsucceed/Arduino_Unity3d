using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ArduinoBasic : MonoBehaviour {
    private SerialPort arduinoStream;
    public string port;
    private Thread readThread; // 宣告執行緒
    public string readMessage;
    bool isNewMessage;
    Animator anim;
    public float maxSpeed = 2f;
    public float leftLimit = -7.5f;
    public float rightLimit = 7.5f;


    void Start () {
        if (port != "") {
            arduinoStream = new SerialPort (port, 9600); //指定連接埠、鮑率並實例化SerialPort
            arduinoStream.ReadTimeout = 10;
            try {
                arduinoStream.Open (); //開啟SerialPort連線
                readThread = new Thread (new ThreadStart (ArduinoRead)); //實例化執行緒與指派呼叫函式
                readThread.Start (); //開啟執行緒
                Debug.Log ("SerialPort Succeed");
            } catch {
                Debug.Log ("SerialPort Fail");
            }
        }
        anim = GetComponent<Animator>();
        anim.SetBool("left", false);
        anim.SetBool("right",false);
    }
    void Update () {
        if (isNewMessage) {
            Debug.Log (readMessage);
            Vector3 player = gameObject.transform.position;
            if (readMessage == "stop")
            {
                anim.SetBool("left", false);
                anim.SetBool("right", false);

            }
            else if (readMessage == "left")
            {
                anim.SetBool("left", true);
                anim.SetBool("right", false);
                player.x -= maxSpeed * Time.deltaTime;
                gameObject.transform.position = player;
            }
            else if (readMessage == "right")
                {
                    anim.SetBool("right", true);
                    anim.SetBool("left", false);
                    player.x += maxSpeed * Time.deltaTime;
                    gameObject.transform.position = player;
                }
        }
        isNewMessage = false;
    }
private void ArduinoRead () {
    while (arduinoStream.IsOpen) {
        try {
            readMessage = arduinoStream.ReadLine (); // 讀取SerialPort資料並裝入readMessage
            isNewMessage = true;
        } catch (System.Exception e) {
            Debug.LogWarning (e.Message);
        }
    }
}
public void ArduinoWrite (string message) {
    Debug.Log (message);
    try {
        arduinoStream.Write (message);
    } catch (System.Exception e) {
        Debug.LogWarning (e.Message);
    }
}
void OnApplicationQuit () {
    if (arduinoStream != null) {
        if (arduinoStream.IsOpen) {
            arduinoStream.Close ();
        }
    }
}

}