using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class MyTestScript : MonoBehaviour
{
    private Texture2D resultantImage;
    public Camera camOV;
    public RenderTexture currentRT;

    [DllImport("__Internal")]
    private static extern void logErrors(string str);

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MyTestScript: Start()");

        camOV = this.GetComponent<Camera>();
        camOV.targetTexture = currentRT;

        //Debug.Log("currentRT width: " + currentRT.width);
        //Debug.Log("currentRT height: " + currentRT.height);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("MyTestScript: Update()");
        test();

    }

 
    private void test()
    {
        int textureWidth = camOV.activeTexture.width;
        int textureHeight = camOV.activeTexture.height;

        // Create a RenderTexture with the specified width and height
        RenderTexture rt = new RenderTexture(textureWidth, textureHeight, 24);

        // Set the target texture of the main camera to the RenderTexture
        Camera.main.targetTexture = rt;

        // Render the camera to the RenderTexture
        Camera.main.Render();

        // Read pixels from the RenderTexture
        Texture2D tex = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
        tex.Apply();

        // Reset the camera's target texture to null
        Camera.main.targetTexture = null;
        RenderTexture.active = null;

        resultantImage = tex;
        ReadPixelsOut();
    }

    public void TakeScreenShot()
    {
        RenderTexture.active = camOV.targetTexture;
        camOV.Render();

        resultantImage = new Texture2D(camOV.targetTexture.width, camOV.targetTexture.height, TextureFormat.RGB24, false);
        resultantImage.ReadPixels(new Rect(0, 0, camOV.targetTexture.width, camOV.targetTexture.height), 0, 0);
        resultantImage.Apply();
    }

    private void ReadPixelsOut()
    {
        if (resultantImage != null)
        {
            resultantImage.GetPixels();
            RenderTexture.active = currentRT;

            byte[] bytes = resultantImage.EncodeToPNG();
            // save on disk
            var path = PathToWrite();
            File.WriteAllBytes(path, bytes);

            // Destroy(resultantImage);
        }
    }

    private string PathToWrite()
    {
        string path = "C:\\Users\\Smith\\Downloads\\WEbgl Images";
        string name = string.Format("{0}/{1:D04}shot.png", Application.persistentDataPath, Time.frameCount);
        //Debug.Log("MyTestScript: PathToWrite() -> " + path);
        logErrors("MyTestScript: PathToWrite() -> " + path);
        return path;
    }
}
