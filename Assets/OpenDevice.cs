using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
enum ConnectState
{
    Disconnected = 0,
    Paired = 1
}
public class OpenDevice : MonoBehaviour
{
    public Text log;

    AndroidJavaObject bleManage;

    private byte[] rawdata;
    ConnectState bleState = ConnectState.Disconnected;

    BLEDveice ble;

    void Start()
    {
        ble = new BLEDveice();
   
        // CallAndroidInit();

    }

    void Update()
    {
   

    }
    public void BleSendMessage(string data)
    {
        rawdata= bleManage.Get<byte[]>("RawData");
        string p="";
        for (int i = 0; i < rawdata.Length; i++)
        {
            p += rawdata[i] + "--";
        }
        log.text = p;
        //  Debug.Log(data);
        //int f = 0;
        //rawdata = data;
        //log.text = data.Split('&')[1];


        //byte[] b=new byte[log.text.Length/2];
        //for (int i = 0; i < log.text.Length; i++)
        //{

        //    if (i%2==0)
        //    {
        //        string t = log.text[i] + "" + log.text[i + 1];
        //        int result = int.Parse(t, System.Globalization.NumberStyles.AllowHexSpecifier);
        //        b[f] =(byte)result;
        //        //Debug.Log(result);
        //        f++;
        //    }


        //}
   

    }
    public void StateChanged(string state)
    {
        bleState = (ConnectState)Enum.Parse(typeof(ConnectState), state);
        switch (bleState)
        {
            default:
                break;
        }
        AndroidCall.AndroidToast("蓝牙的状态改变了,当前状态-"+state);
        log.text = bleState.ToString();
    }

    private   void CallAndroidInit()
    {
        bleManage = new AndroidJavaObject("com.lanyouwei.blemanager.BleManager");
        AndroidCall.unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            bleManage.Call("init", AndroidCall.unityActivity);
            bleManage.Call("addUUid","CAD,00001816-0000-1000-8000-00805f9b34fb,00002a5b-0000-1000-8000-00805f9b34fb,3");
            bleManage.Call("ScanDevice","CAD");
        }));
  
        AndroidCall.AndroidToast("调用Init初始化成功");
    }

    public string GetRawData()
    {
        return bleManage.Get<string>("RawData");
    }
}
