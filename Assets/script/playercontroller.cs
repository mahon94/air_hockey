using UnityEngine;
using System.Collections;

public class playercontroller : MonoBehaviour
{
    //private Transform camera;
    Vector3 jahat;
    // Use this for initialization
    //public float speed;
    private bool isLeftTouched,isRightTouched;
    //public Hashtable touches = new Hashtable();
    public int finId;
    public bool istouched;
    void Start()
    {
        istouched = false;
        //player2 = this.gameObject.transform.Find("MainCamera");
        //Debug.Log(player2.position);
    }

    // Update is called once per frame
    void Update()
    {

       /* 
        isLeftTouched=isRightTouched=false;
        Touch[] myTouches = Input.touches;
        
        
            for (int i = 0; i < Input.touchCount; i++)
            {
                Vector3 touchPos = myTouches[i].position;
                Vector3 touchView = Camera.main.ScreenToViewportPoint(touchPos);
                if (touchView.x >= 0.55)
                {
                    isRightTouched=true;
                    touchPos.z = 18.7f;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);
                    Vector3 jahat = worldPos - this.transform.position;
                    this.rigidbody.velocity = jahat * Time.deltaTime * speed;
                }
                else if (touchView.x <= 0.45)
                {
                    isLeftTouched = true;
                    touchPos.z = 18.7f;


                    Vector3 worldPos    = Camera.main.ScreenToWorldPoint(touchPos);
                    Vector3 jahat = worldPos - player2.position;
                    player2.rigidbody.velocity = jahat * Time.deltaTime * speed;
                }

            }
        
        if (!isRightTouched)
        {
            this.rigidbody.velocity = new Vector3(0, 0, 0);
            //this.gameObject.transform.Find("");    
        }
        if (!isLeftTouched)
        {
            player2.rigidbody.velocity = new Vector3(0, 0, 0);
        }

        //if (Input.GetMouseButton(0))
        //{
        //    Vector3 mousePos = Input.mousePosition;
        //    Debug.Log(mousePos);
        //    mousePos.z = 18.7f;
        //    Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        //    Debug.Log(objectPos);
        //        Vector3 jahat = objectPos - this.transform.position;
        //        this.rigidbody.velocity=jahat *Time.deltaTime * speed;

        //}
        */

    }
}
