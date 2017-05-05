using UnityEngine;
using System.Collections;

public class ballController : MonoBehaviour {


   //public GameObject boxAreaL= ; // = GameObject.FindGameObjectWithTag("DestroyAreaL");
   //public GameObject boxAreaR; //= GameObject.FindGameObjectWithTag("DestroyAreaR");

    public GameObject gameman;
    public Transform ballGenerate;
    private scoreScript scr;

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "DestroyAreaL")
        {
            Debug.Log("left");
            scr.addScore(2);
            Instantiate(ballGenerate, new Vector3(5, 4.970479f, 0), Quaternion.Euler(270, 0, 0));    
            Destroy(this.gameObject);

           //scoreScript scr = (scoreScript) gameman.GetComponent(typeof(scoreScript));

        }
        else if (other.gameObject.tag == "DestroyAreaR")
        {
            Debug.Log("right");
            scr.addScore(1);
            Instantiate(ballGenerate, new Vector3(-5, 4.970479f, 0),  Quaternion.Euler(270,0,0));
            Destroy(this.gameObject);
            
        }

        else {
            GetComponent<AudioSource>().Play();
        }

    }
	// Use this for initialization
	void Start () {
         scr = (scoreScript)gameman.GetComponent(typeof(scoreScript));

	}
	
	// Update is called once per frame
	void Update () {
        
        
	
	}
}
