using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*   
 * @ for lawlietz 2018
 *真机测试 需要在 AndroidManifest.xml 文件下添加如下权限
  <uses-permission android:name="android.permission.BLUETOOTH" />
  <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />
 * 
 */

public class BLEDveice 
{

    AndroidJavaObject bluetoothAdapter;
    AndroidJavaObject buetoothGatt;
    AndroidJavaObject gattService;
    AndroidJavaObject gattCharacteristic;

    readonly string serviceID = "00001816-0000-1000-8000-00805f9b34fb";
    readonly string characteristicID = "00002a5b-0000-1000-8000-00805f9b34fb";
    readonly  string descriptorID = "00002902-0000-1000-8000-00805f9b34fb";
    public   BLEDveice()
    {

            var bluetooth = new AndroidJavaClass("android.bluetooth.BluetoothAdapter");
            bluetoothAdapter = bluetooth.CallStatic<AndroidJavaObject>("getDefaultAdapter");

            if (bluetoothAdapter == null)
            {
                return;
            }
           var isOpne= OpenDevice();
            if (isOpne)
            {
            ScanDveice();
            }

    }

   /// <summary>
   /// Opens the device.
   /// </summary>
   /// <returns><c>true</c>, if device was opened, <c>false</c> otherwise.</returns>
    public   bool OpenDevice()
    {
        if (!bluetoothAdapter.Call<bool>("isEnabled"))
        {
            var isOpen = bluetoothAdapter.Call<bool>("enable");  //打开蓝牙，需要BLUETOOTH_ADMIN权限  
            if (!isOpen)
            {
                OpenDevice();
            }
           
        }
        return bluetoothAdapter.Call<bool>("isEnabled");
    }

    /// <summary>
    /// Scans the dveice.
    /// </summary>
    void ScanDveice()
    {
        var mBluetoothLeScanner = bluetoothAdapter.Call<AndroidJavaObject>("getBluetoothLeScanner");
        ScanCallback scanCallback = new ScanCallback();
        AndroidJavaObject wfzScanCallback = new AndroidJavaObject("com.wfz.bletounity.WFZScanCallback");
        scanCallback.onScanResultDelegate = (t, r) =>
        {
            if (r == null)
            {
                return;
            }
            var bluetoothDevice = r.Call<AndroidJavaObject>("getDevice");
            var deviceName = bluetoothDevice.Call<string>("getName");
            var deviceAddress = bluetoothDevice.Call<string>("getAddress");
            Debug.Log("扫描到设备:" + deviceName + "地址:" + deviceAddress);
         
                if (deviceName =="CAD")
                {
                    if (buetoothGatt == null)
                    {
                        ConnectGatt(bluetoothDevice);
                        mBluetoothLeScanner.Call("stopScan", wfzScanCallback);
                    }
                }
           
        };

        wfzScanCallback.Call("AddUnityCallback", scanCallback);
        mBluetoothLeScanner.Call("startScan", wfzScanCallback);
    }

    /// <summary>
    /// Connects the gatt.
    /// </summary>
    /// <param name="device">Device.</param>
    void ConnectGatt(AndroidJavaObject device)
    {
        AndroidJavaObject wfzBluetoothGattCallback = new AndroidJavaObject("com.wfz.bletounity.WFZBluetoothGattCallback");
        BluetoothGattCallback bluetoothGattCallback = new BluetoothGattCallback();
        bluetoothGattCallback.onCharacteristicChangedDelegate = (g, c) =>
        {
            var data = c.Call<byte[]>("getValue");
            string d = "";
            for (int i = 0; i < data.Length; i++)
            {
                d += data[i];
            }
            Debug.Log(d);
        };


        bluetoothGattCallback.onConnectionStateChangeDelegate = (g, s, n) =>
        {
            switch (n)
            {
                case 0:
                    Debug.Log("断开连接");
                    break;
                case 1:
                    Debug.Log("连接中");
                    break;
                case 2:
                    Debug.Log("连接成功");

                    Debug.Log("获取到gatt对象"+g.Call<AndroidJavaObject>("getDevice").Call<string>("getName"));
                   var isFind= g.Call<bool>("discoverServices");
                    break;
                case 3:
                    Debug.Log("断开中");
                    break;
                default:
                    break;
            }
        };
        bluetoothGattCallback.onServicesDiscoveredDelegate = (g, c) =>
        {
          var  gattServices = g.Call<AndroidJavaObject>("getServices");
            Debug.Log("发现新服务..." + gattServices.Get<int>("size"));

            if (gattCharacteristic==null)
            {
                gattCharacteristic = GetCharacteristic(GetService(g, serviceID), characteristicID);
                NotificationData(g, gattCharacteristic, descriptorID);
            }
        };
        wfzBluetoothGattCallback.Call("AddUnityCallback", bluetoothGattCallback);
        buetoothGatt = device.Call<AndroidJavaObject>("connectGatt", AndroidCall.unityActivity, true, wfzBluetoothGattCallback);
    }

    /// <summary>
    /// Gets the service.
    /// </summary>
    /// <returns>The service.</returns>
    /// <param name="gatt">Gatt.</param>
    /// <param name="uuid">UUID.</param>
    public AndroidJavaObject GetService(AndroidJavaObject gatt, string uuid)
    {
        return gatt.Call<AndroidJavaObject>("getService", ToAndroidUUID(uuid));
    }

    /// <summary>
    /// Gets the characteristic.
    /// </summary>
    /// <returns>The characteristic.</returns>
    /// <param name="Service">Service.</param>
    /// <param name="uuid">UUID.</param>
    public  AndroidJavaObject GetCharacteristic(AndroidJavaObject Service,string uuid)
    {
        return Service.Call<AndroidJavaObject>("getCharacteristic", ToAndroidUUID(uuid));
    }

    /// <summary>
    /// Notifications the data.
    /// </summary>
    /// <param name="gatt">Gatt.</param>
    /// <param name="characteristic">Characteristic.</param>
    /// <param name="uuid">UUID.</param>
    public void NotificationData(AndroidJavaObject gatt,AndroidJavaObject characteristic,string uuid)
    {
        var b = gatt.Call<bool>("setCharacteristicNotification", characteristic, true);
        if (b)
        {
            var bluetoothGattDescriptor = new AndroidJavaClass("android.bluetooth.BluetoothGattDescriptor");
            var descriptor= characteristic.Call<AndroidJavaObject>("getDescriptor",ToAndroidUUID(uuid));

            var  isSet=  descriptor.Call<bool>("setValue", bluetoothGattDescriptor.GetStatic<byte[]>("ENABLE_NOTIFICATION_VALUE"));
            if (isSet)
            {
                var iSwrite=  gatt.Call<bool>("writeDescriptor", descriptor);
                if (iSwrite)
                {
                    Debug.Log("打开特性通知");
                }
            }
        }
    }

    /// <summary>
    /// Tos the android UUID.
    /// </summary>
    /// <returns>The android UUID.</returns>
    /// <param name="uuid">UUID.</param>
    public AndroidJavaObject ToAndroidUUID(string uuid)
    {
        var UUID = new AndroidJavaClass("java.util.UUID");
        return  UUID.CallStatic<AndroidJavaObject>("fromString", uuid);
    }
}
