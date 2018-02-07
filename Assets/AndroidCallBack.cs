using UnityEngine;
using System;

public enum  MediaStoreType{Images=0,Audio=1,Video=2}
public static class AndroidCall
{
    public static AndroidJavaObject unityActivity;

    public static AndroidJavaObject alertDialog;
    static AndroidCall()
    {
        try
        {
            if (unityActivity == null)
            {
                unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }

    }

    /// <summary>
    /// Androids the toast.
    /// </summary>
    /// <param name="text">Text.</param>
    public static void AndroidToast(string text)
    {

        AndroidJavaClass toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject context = unityActivity.Call<AndroidJavaObject>("getApplicationContext");
        unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", text);
            toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, toast.GetStatic<int>("LENGTH_SHORT")).Call("show");
        }));
    }
    /// <summary>
    /// Installs the apk.
    /// </summary>
    /// <param name="path">Path.</param>
    public static void InstallApk(string path)
    {
#if UNITY_ANDROID
        AndroidJavaClass Intent = new AndroidJavaClass("android.content.Intent");
        AndroidJavaClass Uri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", Intent.GetStatic<AndroidJavaObject>("ACTION_VIEW"));
        intent.Call<AndroidJavaObject>("setDataAndType", Uri.CallStatic<AndroidJavaObject>("fromFile", new AndroidJavaObject("java.io.File", new AndroidJavaObject("java.lang.String", path))), new AndroidJavaObject("java.lang.String", "application/vnd.android.package-archive"));
        unityActivity.Call("startActivity", intent);
#endif
    }
    /// <summary>
    /// Launchs the application.
    /// </summary>
    /// <param name="packageName">Package name.</param>
    public static void LaunchApplication(string packageName)
    {
#if UNITY_ANDROID
        AndroidJavaObject intent = unityActivity.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
        unityActivity.Call("startActivity", intent);
#endif
    }

#if UNITY_ANDROID
    /// <summary>
    /// Ises the installed application.
    /// </summary>
    /// <param name="packageName">Package name.</param>
    public static bool IsInstalledApplication(string packageName)
    {

        AndroidJavaObject packageManager = unityActivity.Call<AndroidJavaObject>("getPackageManager");
        var packageinfos = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);
        for (int i = 0; i < packageinfos.Get<int>("size"); i++)
        {
            string name = packageinfos.Call<AndroidJavaObject>("get", i).Get<string>("packageName");
            if (packageName.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
        }
        return false;

    }
#endif
#if UNITY_ANDROID
    /// <summary>
    /// Shows the alert dialog.
    /// </summary>
    /// <param name="title">Title.</param>
    /// <param name="Message">Message.</param>
    /// <param name="positiveButtonText">Positive button text.</param>
    /// <param name="negativeButtonText">Negative button text.</param>
    /// <param name="PositiveEvent">Positive event.</param>
    /// <param name="NegativeEvent">Negative event.</param>
    public static void ShowAlertDialog(string title = "提示!", string Message = "是否退出?", string positiveButtonText = "是", string negativeButtonText = "否", Action PositiveEvent = null, Action NegativeEvent = null)
    {


        alertDialog = new AndroidJavaObject("android.app.AlertDialog$Builder", unityActivity);
        var PositiveClick = new DialogOnClickListener();
        PositiveClick.onClickDelegate = (d, w) =>
        {
            if (PositiveEvent != null)
            {
                PositiveEvent();
            }
        };
        var NegativeClick = new DialogOnClickListener();
        NegativeClick.onClickDelegate = (d, w) =>
        {
            if (NegativeEvent != null)
            {
                NegativeEvent();
            }
        };
        alertDialog = alertDialog.Call<AndroidJavaObject>("setTitle", title);
        alertDialog = alertDialog.Call<AndroidJavaObject>("setMessage", Message);
        alertDialog = alertDialog.Call<AndroidJavaObject>("setPositiveButton", positiveButtonText, PositiveClick);
        alertDialog = alertDialog.Call<AndroidJavaObject>("setNegativeButton", negativeButtonText, NegativeClick);
        alertDialog = alertDialog.Call<AndroidJavaObject>("create");

        alertDialog.Call("show");

    }
#endif

#if UNITY_ANDROID
    public static void GetImage(string objName,string funName,MediaStoreType type=MediaStoreType.Images)
    {
        byte[] image=null;
        AndroidJavaClass Intent = new AndroidJavaClass("android.content.Intent");
        AndroidJavaClass Uri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaClass media = new AndroidJavaClass(string.Format("android.provider.MediaStore${0}$Media",type.ToString()));//    android.provider.MediaStore.Images.Media
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", Intent.GetStatic<string>("ACTION_PICK"),media.GetStatic<AndroidJavaObject>("EXTERNAL_CONTENT_URI"));
        AndroidCall.unityActivity.Call("startActivityForResult", intent, 1);

        MeRuRuActivity listener = new MeRuRuActivity();
        listener.onActivityResultDelegate = (requestCode,resultCode,Intentdata)=>
        {
            if (requestCode == 1 && resultCode == new AndroidJavaClass("android.app.Activity").GetStatic<int>("RESULT_OK") && Intentdata != null)
            {
                Debug.Log("选了图片开始返回路劲");
                var ImageUrl = Intentdata.Call<AndroidJavaObject>("getData");
                if (media == null)
                {
                    Debug.Log("媒体对象没有定义");
                }
                AndroidJavaObject c = unityActivity.Call<AndroidJavaObject>("getContentResolver").Call<AndroidJavaObject>("query", ImageUrl, null, null, null, null);
                Debug.Log("返回光标");
                var isMove = c.Call<bool>("moveToFirst");
                var columnIndex = c.Call<int>("getColumnIndex", "_data");
                var imagePath = c.Call<string>("getString", columnIndex);
          //      image = imagePath.Call<byte[]>("getBytes");
                new AndroidJavaClass("com.unity3d.player.UnityPlayer").CallStatic("UnitySendMessage",objName,funName,imagePath);
            }
  
        };
        AndroidCall.unityActivity.Call("AddUnityCallBack", listener);
    }

    /*
         //加载图片
    private void showImage(string imaePath){
        var bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory");
       var bitmap = bitmapFactory.CallStatic<AndroidJavaObject>("decodeFile", imaePath);
        ((ImageView)findViewById(R.id.image)).setImageBitmap(bm);
    }
 */
#endif
  
}
