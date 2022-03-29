using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGChanger : MonoBehaviour
{
	public GameObject [] fondos;
    // Start is called before the first frame update
    void Start()
    {
		//fondos = new GameObject [6];
		fondos[Random.Range(0, 6)].SetActive(true);
        //BotonCrear.SetActive(true);
    }

    // Update is called once per frame
    public void reset()
    {
 		for (int i = 0; i< 6; i++)
		{
			fondos[i].SetActive(false);
		}    
		
		fondos[Random.Range(0, 6)].SetActive(true);
    }

	public void stopIt()
	{
		System.Threading.Thread.Sleep(1000);
	}
}
