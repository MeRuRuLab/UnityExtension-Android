using System.Runtime.InteropServices;

public class IPhoneCall
{

    [DllImport("__Internal")]
    public static extern void UnityDidStart();
    [DllImport("__Internal")]
    public static extern void CloseUnity();
}
