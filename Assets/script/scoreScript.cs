using UnityEngine;
using System.Collections;

public class scoreScript : MonoBehaviour {

    private int scorenum1 = 0, scorenum2=0;
    public TextMesh score1,score2;

    //add score for this obj
    public void addScore(int comnd)
    {
        if (comnd == 1) {
            scorenum1++;
            score1.text = scorenum1.ToString();
        }

        else if (comnd == 2)
        {
            scorenum2++;
            score2.text = scorenum2.ToString();
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
