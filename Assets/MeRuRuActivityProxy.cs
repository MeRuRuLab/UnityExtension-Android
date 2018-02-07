using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MeRuRuActivity : AndroidJavaProxy
{

    public delegate void ActivityResultDelegate(int requestCode, int resultCode, AndroidJavaObject Intentdata);
    public delegate void RequestPermissionsResultDelegate(int requestCode, string[] permissions, int[] grantResults);

    public ActivityResultDelegate onActivityResultDelegate;
    public RequestPermissionsResultDelegate onRequestPermissionsResultDelegate;

    public MeRuRuActivity() : base("com.meruru.unityextension.IMeRuRuActivity")
    {

    }

    /// <summary>
    /// Ons the activity result.
    /// Called When Activity Result CallBack
    /// </summary>
    /// <param name="requestCode">Request code.</param>
    /// <param name="resultCode">ResultCode.</param>
    /// <param name="Intentdata">Intent data.</param>
    public void onActivityResult(int requestCode, int resultCode, AndroidJavaObject Intentdata)
    {
        Debug.Log("调用onActivityResult");
        if (onActivityResultDelegate != null)
        {
            onActivityResultDelegate(requestCode, resultCode, Intentdata);
        }
    }

    /// <summary>
    /// Ons the request permissions result.
    /// Called When Permission Result 
    /// </summary>
    /// <param name="requestCode">Request code.</param>
    /// <param name="permissions">Permissions.</param>
    /// <param name="grantResults">Grant results.</param>
    public void onRequestPermissionsResult(int requestCode, string permissions, string grantResults)
    {
        if (onRequestPermissionsResultDelegate != null)
        {
            //string splitPermissions = permissions.toCString ();
            string[] permissioinList = permissions.Split(';');

            //string splitgrantResults = permissions.toCString ();
            string[] grantResultsList = grantResults.Split(';');
            int[] intGrantResults = new int[grantResultsList.Length];
            for (int i = 0; i < grantResultsList.Length; i++)
            {
                intGrantResults[i] = int.Parse(grantResultsList[i]);
            }
            onRequestPermissionsResultDelegate(requestCode, permissioinList, intGrantResults);
        }
    }

}
/*--------------------------------------------------------------------------------------------------------------------------------------------*/


public class DialogOnClickListener : AndroidJavaProxy
{

    public delegate void OnClickDelegate(AndroidJavaObject dialog, int which);


    public OnClickDelegate onClickDelegate;


    public DialogOnClickListener() : base("android.content.DialogInterface$OnClickListener")
    {

    }

    public void onClick(AndroidJavaObject dialog, int which)
    {
        //
        if (onClickDelegate != null)
        {
            onClickDelegate(dialog, which);
        }
    }


}
