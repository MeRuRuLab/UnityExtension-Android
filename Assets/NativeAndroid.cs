using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class NativeAndroid : MonoBehaviour {

    [SerializeField]
    Image head;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SetHead(string path)
    {
            FileStream file = new FileStream(path,FileMode.Open,FileAccess.Read);
            file.Seek(0, SeekOrigin.Begin);  
            byte[] binary = new byte[file.Length]; //创建文件长度的buffer   
            file.Read(binary, 0, (int)file.Length);  
            file.Close();  
            file.Dispose();
            file = null;
        Texture2D tex = new Texture2D(400, 400, TextureFormat.ARGB32, false);
        var f= tex.LoadImage(binary);
        if (tex != null)
        {
            Debug.Log("加载数据");
            Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            head.sprite = s;
        }
    }
}
