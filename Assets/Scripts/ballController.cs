using System;
using UnityEngine;
using System.Collections;

public class ballController : MonoBehaviour {


   //public GameObject boxAreaL= ; // = GameObject.FindGameObjectWithTag("DestroyAreaL");
   //public GameObject boxAreaR; //= GameObject.FindGameObjectWithTag("DestroyAreaR");

    public GameObject gameman;
    [HideInInspector]
    public GameManager GameManagerScript;
    public Transform BallTransform;
    private scoreScript scr;



    private void OnCollisionEnter(Collision other)
    {
		
        if (other.gameObject.tag == "DestroyAreaL")
        {
            Debug.Log("left");
            // Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            GameManagerScript.goalTouched(1);

           //scoreScript scr = (scoreScript) gameman.GetComponent(typeof(scoreScript));

        }
        else if (other.gameObject.tag == "DestroyAreaR")
        {
            Debug.Log("right");
            // Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            GameManagerScript.goalTouched(2);
        }
        else {
            GetComponent<AudioSource>().Play();
        }

    }
	// Use this for initialization
	void Start () {
         
         bool isRandomInit = gameman.GetComponent<GameManager>().m_ballRandomInit;
         GameManagerScript = gameman.GetComponent<GameManager>();

	}

	
	
	// Update is called once per frame
	void Update () {
        
        
	
	}
}
