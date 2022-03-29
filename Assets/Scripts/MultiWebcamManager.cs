using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class MultiWebcamManager : MonoBehaviour
{
	public GameObject webcamTexturePrefab;
	public GameObject BotonFoto;
	public GameObject BotonEnviar;
	public GameObject ResetAll;
	public RawImage foto;
	public GameObject foto2;

	public Text errorMessages;
	public Text errorMessages2;
	public Text errorMessages3;
	public Text errorMessages4;

	public Text textForm,textForm2,textForm3,textForm4;
	public InputField  textForm5,textForm6,textForm7,textForm8;

	private string [] nameOfCams;
	private List<WebCamTexture> webCamTextures = new List<WebCamTexture>();
	private Texture2D texture;
	private	byte[] bytes;
	public string sFileName = "";
	private int numOfCams = 1;

	public string m_LocalFileName = "C:/Users/Usuario/Documents/Unity/Multiple Webcams/Assets/demo.txt";
    private string m_URL = "http://altouch.pe/apps/bullet/uploadfile.php";

	//Charizard = GameObject.FindWithTag ("Charizard");

    // Start is called before the first frame update
    void Start()
    {
		nameOfCams = new string [numOfCams];

		for (int i = 0; i< numOfCams; i++)
		{
			nameOfCams[i] = WebCamTexture.devices[i].name;

			//GameObject go = Instantiate(webcamTexturePrefab, new Vector3(-11.38f,2f,0), Quaternion.Euler(0, 0, -180)) as GameObject;
			GameObject go = Instantiate(webcamTexturePrefab, new Vector3(-5,1,0), Quaternion.identity) as GameObject;

			go.transform.parent = gameObject.transform;

			WebCamTexture webcamTexture = new WebCamTexture ();
			webcamTexture.deviceName = nameOfCams[i];

			webCamTextures.Add(webcamTexture);

			go.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = webCamTextures[i];
			webCamTextures[i].Play();
		}

		BotonEnviar.SetActive(false);
    }

	void Update(){
        if(textForm.text!=""&&textForm2.text!=""&&textForm3.text!=""&&textForm4.text!=""&&BotonFoto.activeInHierarchy == false)showEnviar();
	}

     public void SaveImage()
     {
		if(textForm.text!=""&&textForm2.text!=""&&textForm3.text!=""&&textForm4.text!="")
		{
			foto2.SetActive(true);
			BotonFoto.SetActive(false);
			
		sFileName =  System.DateTime.Now.ToString("MM-dd-yy_hh-mm-ss");


          //Create a Texture2D with the size of the rendered image on the screen.
         texture = new Texture2D(webCamTextures[0].width, webCamTextures[0].height, TextureFormat.RGB24, false);

          //Save the image to the Texture2D
         texture.SetPixels(webCamTextures[0].GetPixels());	
         texture.Apply();
 
          //Encode it as a PNG.
         bytes = texture.EncodeToPNG();
         
         //Save it in a file.
			File.WriteAllBytes(Application.persistentDataPath +sFileName+".jpg", bytes);

			StartCoroutine(captureScreenshot(Application.persistentDataPath +sFileName+"-X.jpg"));
			//ScreenCapture.CaptureScreenshot();
			foto.texture = texture;
		}
	
     }

     public void showEnviar()
     {
		BotonEnviar.SetActive(true);
     }

    IEnumerator captureScreenshot(string imagePath)
    {
        yield return new WaitForEndOfFrame();
        //about to save an image capture
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);


        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        Debug.Log(" screenImage.width" + screenImage.width + " texelSize" + screenImage.texelSize);
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        Debug.Log("imagesBytes=" + imageBytes.Length);

        //Save image to file
        System.IO.File.WriteAllBytes(imagePath, imageBytes);
    }

     IEnumerator UploadFileCo(string localFileName, string uploadURL)
     {
         WWW localFile = new WWW("file:///" + localFileName);
         yield return localFile;
         if (localFile.error == null) errorMessages.text = "Loaded file successfully";
         else
         {
            errorMessages2.text = "Open file error: "+localFile.error;
             yield break; // stop the coroutine here
         }
         WWWForm postForm = new WWWForm();
         postForm.AddBinaryData("theFile",localFile.bytes,localFileName,"image/jpeg");
         WWW upload = new WWW(uploadURL,postForm);        
         yield return upload;
         if (upload.error == null)
             errorMessages3.text ="upload done :" + upload.text;
         else
             errorMessages4.text ="Error during upload: " + upload.error;
     }
     void UploadFile(string localFileName, string uploadURL)
     {
         StartCoroutine(UploadFileCo(localFileName, uploadURL));
     }


     public void SendServer()
     {
		m_LocalFileName = Application.persistentDataPath +sFileName+"-X.jpg";
		UploadFile(m_LocalFileName,m_URL);
		Reset();
     }


     private void Reset()
     {
		BotonFoto.SetActive(true);
		BotonEnviar.SetActive(false);
		foto2.SetActive(false);
		textForm5.text ="";
		textForm6.text ="";
		textForm7.text ="";
		textForm8.text ="";
     }

}
