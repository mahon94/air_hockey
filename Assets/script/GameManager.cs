using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //Hashtable touches = new Hashtable();
    private playercontroller p1scr, p2scr;
    public GameObject GOP1,GOP2;
    public float speed;

    private Vector3 tpos, tview, wview;
    void analyseTouch(Touch T, out Vector3 touchPos, out Vector3 touchView, out Vector3 worldPos)
    {
        touchPos = T.position;
        touchView = Camera.main.ScreenToViewportPoint(touchPos);
        touchPos.z = 18.7f;
        worldPos = Camera.main.ScreenToWorldPoint(touchPos);
    }

    void Start()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;

        p1scr = (playercontroller)GOP1.GetComponent(typeof(playercontroller));
        p2scr = (playercontroller)GOP2.GetComponent(typeof(playercontroller));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            analyseTouch(Input.GetTouch(i),out tpos,out tview,out wview);
//touch began
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Debug.Log(Input.GetTouch(i) + "began");
                if(tview.x<0.48 && !p1scr.istouched){
                    p1scr.finId=Input.GetTouch(i).fingerId;
                    p1scr.istouched=true;

                    GOP1.transform.position = new Vector3(GOP1.transform.position.x, GOP1.transform.position.y - 1, GOP1.transform.position.z);

                    Vector3 jahat = wview - GOP1.transform.position;
                    GOP1.GetComponent<Rigidbody>().velocity= jahat * Time.deltaTime * speed;
                }
                else if(tview.x>0.52 && !p2scr.istouched){
                    p2scr.finId=Input.GetTouch(i).fingerId;
                    p2scr.istouched=true;

                    GOP2.transform.position = new Vector3(GOP2.transform.position.x, GOP2.transform.position.y - 1, GOP2.transform.position.z);

                    Vector3 jahat = wview- GOP2.transform.position;
                    GOP2.GetComponent<Rigidbody>().velocity = jahat * Time.deltaTime * speed;
                }
            }

//touch moved
            else if (Input.GetTouch(i).phase == TouchPhase.Moved){
                Debug.Log(Input.GetTouch(i) + "moved");

                if(p1scr.istouched && p1scr.finId == Input.GetTouch(i).fingerId){
                    if (.05<tview.x && tview.x < 0.5 && tview.y>.05 && tview.y<0.95)
                    {
                        Vector3 jahat = wview - GOP1.transform.position;
                        GOP1.GetComponent<Rigidbody>().velocity = jahat * Time.deltaTime * speed;
                    }
                    else {
                        GOP1.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); 
                    }

                }

                else if(p2scr.istouched && p2scr.finId == Input.GetTouch(i).fingerId){
                    if (tview.x >= 0.5)
                    {
                        Vector3 jahat = wview - GOP2.transform.position;
                        GOP2.GetComponent<Rigidbody>().velocity = jahat * Time.deltaTime * speed;
                    }
                    else {
                        GOP2.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    }
                }
            }

//touch isn't moved
            else if(Input.GetTouch(i).phase == TouchPhase.Stationary){
                Debug.Log(Input.GetTouch(i) + "not moved");
                if(p1scr.istouched && p1scr.finId == Input.GetTouch(i).fingerId){
                    GOP1.GetComponent<Rigidbody>().velocity= new Vector3(0,0,0);
                }
                else if(p2scr.istouched && p2scr.finId == Input.GetTouch(i).fingerId){
                    GOP2.GetComponent<Rigidbody>().velocity= new Vector3(0,0,0);
                }
            }
// touch is ended
            else if ( (Input.GetTouch(i).phase == TouchPhase.Ended)||
                 (Input.GetTouch(i).phase == TouchPhase.Canceled) )
            {
                Debug.Log(Input.GetTouch(i)+"ended");
                if(p1scr.istouched && p1scr.finId == Input.GetTouch(i).fingerId){
                    p1scr.istouched=false; //change finid for debug

                    GOP1.transform.position = new Vector3(GOP1.transform.position.x, GOP1.transform.position.y + 1, GOP1.transform.position.z);

                    GOP1.GetComponent<Rigidbody>().velocity= new Vector3(0,0,0);
                }
                else if(p2scr.istouched && p2scr.finId == Input.GetTouch(i).fingerId){
                    p2scr.istouched=false; //change finid for debug

                    GOP2.transform.position = new Vector3(GOP2.transform.position.x, GOP2.transform.position.y + 1, GOP2.transform.position.z);

                    GOP2.GetComponent<Rigidbody>().velocity= new Vector3(0,0,0);
                }
          
            }
        }

        if (!p1scr.istouched)
        {
            GOP1.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        if (!p2scr.istouched)
        {
            GOP2.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

    }
}
