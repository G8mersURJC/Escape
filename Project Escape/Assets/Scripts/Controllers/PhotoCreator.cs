using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCreator : MonoBehaviour
{ 
    private bool bTakingPhoto = false;
    public int iWidth = 1920;
    public int iHeight = 1080;

    public static string photoName(int width, int height)
    {
        return string.Format("{0}/Screenshots/photo_{1}x{2}_{3}.png",
                Application.dataPath,
                width, 
                height,
                System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    } 

    public void takePhoto() //Para permitir realizar capturas desde fuera.
    {
        bTakingPhoto = true;
    }

    private void Start()
    {

    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (bTakingPhoto || Input.GetKeyDown("f"))
        {
            RenderTexture rt = new RenderTexture(iWidth, iHeight, 24); //Creamos el lugar donde almacenar la información de la cámara.
            GetComponent<Camera>().targetTexture = rt;                                                  //Asignamos la textura a la cámara.
            Texture2D t2dPhoto = new Texture2D(iWidth, iHeight, TextureFormat.RGB24, false); //Creamos el buffer donde guardar los pixeles.
            GetComponent<Camera>().Render();                                                            //Activamos una renderización de la cámara.
            RenderTexture.active = rt;                                                       //Activamos la textura de render.
            t2dPhoto.ReadPixels(new Rect(0, 0, iWidth, iHeight), 0, 0);          //Guardamos en el buffer los pixeles recogidos del render.
            GetComponent<Camera>().targetTexture = null;                                   //Desactivamos el lugar de renderizado.
            RenderTexture.active = null;                                        //Desactivamos el render activo.
            Destroy(rt);
            //Convertimos la imagen de píxeles a formato PNG.
            byte[] bytes = t2dPhoto.EncodeToPNG();
            string sFileName = photoName(iWidth, iHeight);
            //Creamos la imagen.
            System.IO.File.WriteAllBytes(sFileName, bytes);
            bTakingPhoto = false;
        }
    }
}
