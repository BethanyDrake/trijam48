using UnityEngine;
using System.Collections;


public class PlaceCoins : MonoBehaviour {

    public float spacing;
    public GameObject coinPrefab;
    public int numCoins = 5;
    GameObject coinLayer;
    public float height;

    //base numcoins on the total distance and spacing

    //public GameObject test;
    // Use this for initialization
    void Start () {
	    coinLayer = new GameObject();
        coinLayer.name = "CoinLayer";
        center = Vector3.zero;
        
        //test = Instantiate(coinPrefab);
        //test.transform.position = Vector3.zero;
        /*
        points[0] = new Vector3(-3, 0, 4);
        points[1] = new Vector3(4, 0, 5);
        points[2] = new Vector3(1, 0, -4);
        DefineCircle();
        */
    }
	
	// Update is called once per frame
	void Update () {
        //test.transform.RotateAround(center, Vector3.up, Time.deltaTime*20);
        if (Input.GetMouseButtonDown(0) && numpoints >= 3) Reset();
        if (Input.GetMouseButtonDown(0)&& numpoints <3) AddPoint();
        if (Input.GetKeyDown(KeyCode.Space)&& numpoints==3)
        {
            DefineCircle();
            AddCoins_Circle();
        }
        if (Input.GetKeyDown(KeyCode.Space) && numpoints == 2)
        {
            DefineLine();
            AddCoins_Line();
        }



    }

    public Vector3 lineStart;
    public Vector3 lineDir;
    public float lineLength;

    void DefineLine()
    {
        lineStart = points[0];
        lineDir = (points[1] - points[0]).normalized;
        lineLength = (points[1] - points[0]).magnitude;

    }

    void AddCoins_Line()
    {
        GameObject coins = new GameObject();
        coins.transform.position = center;
        coins.transform.parent = coinLayer.transform;
        coins.name = "Coins";


        

        float dist = 0;
        for (int i = 0; i < numCoins; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.transform.parent = coins.transform;
            
            
            coin.transform.position = lineStart + dist*lineDir;
            dist += spacing;
            if (dist >= lineLength) break;
            
            //print("i=" + i + " dist=" + dist + "angle=" + angle);
        }
        Reset();
    }

    void AddCoins_Circle()
    {
        GameObject coins = new GameObject();
        coins.transform.position = center;
        coins.transform.parent = coinLayer.transform;
        coins.name = "Coins";

        float maxAngle = Vector3.Angle(points[0]-center, points[2]-center)/2;
        
        float dist = 0;
        for (int i = 0; i <numCoins; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.transform.parent = coins.transform;
            coin.transform.position = start;
            dist += spacing*i * Mathf.Pow(-1, i + 1);
            float angle = 360 * dist / circumference;
            
            /*coin.transform.position = center - start;
            coin.transform.Rotate(Vector3.up, angle, Space.World);
            coin.transform.position += start;
            */
            coin.transform.RotateAround(center, Vector3.up, angle);
            if (Mathf.Abs(angle) >= maxAngle) break;
            //print("i=" + i + " dist=" + dist + "angle=" + angle);
        }

        Reset();
    }

    
    void Reset()
    {
        center = Vector3.zero;
        radius = 0;
        circumference = 0;
         
        lineStart = Vector3.zero;
        lineDir = Vector3.zero;
        lineLength = 0;

        points = new Vector3[3];
        numpoints = 0;

        

    }

    public Vector3 center;
    public Vector3 start;
    public float radius;
    public float circumference;

    void DefineCircle()
    {
        //print("defining circle");
        float x1 = ((Vector3)points[0]).x;
        float x2 = ((Vector3)points[1]).x;
        float x3 = ((Vector3)points[2]).x;

        float y1 = ((Vector3)points[0]).z;
        float y2 = ((Vector3)points[1]).z;
        float y3 = ((Vector3)points[2]).z;

        float A = x1 * (y2 - y3) - y1 * (x2 - x3) + x2 * y3 - x3 * y2;
        float B = (x1 * x1 + y1 * y1) * (y3 - y2) + (x2 * x2 + y2 * y2) * (y1 - y3) + (x3 * x3 + y3 * y3) * (y2 - y1);
        float C = (x1 * x1 + y1 * y1) * (x2-x3) + (x2 * x2 + y2 * y2) * (x3-x1) + (x3 * x3 + y3 * y3) * (x1-x2);
        float D = (x1 * x1 + y1 * y1) * (x3*y2 - x2*y3) + (x2 * x2 + y2 * y2) * (x1*y3 - x3*y1) + (x3 * x3 + y3 * y3) * (x2*y1-x1*y2);

        //print("A=" + A + " B=" + B + " C=" + C + " D=" + D);

        center = new Vector3(-B / (2 * A), 0, -C / (2 * A));
        start = (Vector3)points[1];
        radius = Mathf.Sqrt((B * B + C * C - 4 * A * D) / (4 * A * A));
        circumference = 2 * Mathf.PI * radius;



    }


    public Vector3[] points = new Vector3[3];
    public int numpoints = 0;

    void AddPoint()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit info;
        Physics.Raycast(r, out info);
        Vector3 p = info.point;
        points[numpoints] = p+Vector3.up*height;
        numpoints++;
        

    }
}
