using UnityEngine;


public class BluetoothGattCallback : AndroidJavaProxy
{
    public delegate void OnCharacteristicChangedDelegate(AndroidJavaObject gatt, AndroidJavaObject characteristic);
    public OnCharacteristicChangedDelegate onCharacteristicChangedDelegate;


    public delegate void OnCharacteristicReadDelegate(AndroidJavaObject gatt, AndroidJavaObject characteristic, int status);
    public OnCharacteristicReadDelegate onCharacteristicReadDelegate;

    public delegate void OnCharacteristicWriteDelegate(AndroidJavaObject gatt, AndroidJavaObject characteristic, int status);
    public OnCharacteristicWriteDelegate onCharacteristicWriteDelegate;

    public delegate void OnConnectionStateChangeDelegate(AndroidJavaObject gatt, int status, int newState);
    public OnConnectionStateChangeDelegate onConnectionStateChangeDelegate;

    public delegate void OnDescriptorReadDelegate(AndroidJavaObject gatt, AndroidJavaObject descriptor, int status);
    public OnDescriptorReadDelegate onDescriptorReadDelegate;

    public delegate void OnDescriptorWriteDelegate(AndroidJavaObject gatt, AndroidJavaObject descriptor, int status);
    public OnDescriptorWriteDelegate onDescriptorWriteDelegate;

    public delegate void OnReadRemoteRssiDelegate(AndroidJavaObject gatt, int rssi, int status);
    public OnReadRemoteRssiDelegate onReadRemoteRssiDelegate;

    public delegate void OnReliableWriteCompletedDelegate(AndroidJavaObject gatt, int status);
    public OnReliableWriteCompletedDelegate onReliableWriteCompletedDelegate;

    public delegate void OnServicesDiscoveredDelegate(AndroidJavaObject gatt, int status);
    public OnServicesDiscoveredDelegate onServicesDiscoveredDelegate;

    public BluetoothGattCallback() : base("com.meruru.unityextension.IBluetoothGattCallback")
    {

    }

    public void onCharacteristicChanged(AndroidJavaObject gatt, AndroidJavaObject characteristic)
    {
        if (onCharacteristicChangedDelegate != null)
        {
            onCharacteristicChangedDelegate(gatt, characteristic);
        }
    }
    public void onCharacteristicRead(AndroidJavaObject gatt, AndroidJavaObject characteristic, int status)
    {
        if (onCharacteristicReadDelegate != null)
        {
            onCharacteristicReadDelegate(gatt, characteristic, status);
        }
    }
    public void onCharacteristicWrite(AndroidJavaObject gatt, AndroidJavaObject characteristic, int status)
    {
        if (onCharacteristicWriteDelegate != null)
        {
            onCharacteristicWriteDelegate(gatt, characteristic, status);
        }
    }
    public void onConnectionStateChange(AndroidJavaObject gatt, int status, int newState)
    {
        if (onConnectionStateChangeDelegate != null)
        {
            onConnectionStateChangeDelegate(gatt, status, newState);
        }
    }
    public void onDescriptorRead(AndroidJavaObject gatt, AndroidJavaObject descriptor, int status)
    {
        if (onDescriptorReadDelegate != null)
        {
            onDescriptorReadDelegate(gatt, descriptor, status);
        }
    }
    public void onDescriptorWrite(AndroidJavaObject gatt, AndroidJavaObject descriptor, int status)
    {
        if (onDescriptorWriteDelegate != null)
        {
            onDescriptorWriteDelegate(gatt, descriptor, status);
        }
    }
    public void onReadRemoteRssi(AndroidJavaObject gatt, int rssi, int status)
    {
        if (onReadRemoteRssiDelegate != null)
        {
            onReadRemoteRssiDelegate(gatt, rssi, status);
        }
    }
    public void onReliableWriteCompleted(AndroidJavaObject gatt, int status)
    {
        if (onReliableWriteCompletedDelegate != null)
        {
            onReliableWriteCompletedDelegate(gatt, status);
        }
    }
    public void onServicesDiscovered(AndroidJavaObject gatt, int status)
    {
        if (onServicesDiscoveredDelegate != null)
        {
            onServicesDiscoveredDelegate(gatt, status);
        }
    }
}

public class ScanCallback : AndroidJavaProxy
{
    public delegate void OnBatchScanResultsDelegate(AndroidJavaObject results);
    public OnBatchScanResultsDelegate onBatchScanResultsDelegate;

    public delegate void OnScanFailedDelegate(int errorCode);
    public OnScanFailedDelegate onScanFailedDelegate;

    public delegate void OnScanResultDelegate(int callbackType, AndroidJavaObject result);
    public OnScanResultDelegate onScanResultDelegate;

    public ScanCallback() : base("com.wfz.bletounity.IScanCallback")
    {

    }
    public void onBatchScanResults(AndroidJavaObject results)
    {
        if (onBatchScanResultsDelegate != null)
        {
            onBatchScanResultsDelegate(results);
        }
    }
    public void onScanFailed(int errorCode)
    {
        if (onScanFailedDelegate != null)
        {
            onScanFailedDelegate(errorCode);
        }
    }
    public void onScanResult(int callbackType, AndroidJavaObject result)
    {
        if (onScanResultDelegate != null)
        {
            onScanResultDelegate(callbackType, result);
        }
    }
}

