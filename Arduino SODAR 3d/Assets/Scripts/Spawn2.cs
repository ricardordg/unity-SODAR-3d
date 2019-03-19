using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Spawn2 : MonoBehaviour {
    public Transform point;

    private List<Position> scan;
    private double rX, rY, rDis, rAng;
    private static int LIMITE = 80;

    SerialPort sp = new SerialPort("\\\\.\\COM12", 9600);

	void Start () {
        sp.Open();
        sp.ReadTimeout = 1000;

        scan = new List<Position>();

        rX = -1;
        rY = -1;
        rDis = -1;
        rAng = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if(sp.IsOpen){
            try
            {
                string str = sp.ReadLine();

              //  Debug.Log(str);
                
                string[] str_split = str.Split(',');

                SplitAndConvert(str_split);

                if (rDis < LIMITE && rDis > -1)
                {
                    Position p = new Position(rX, rY, rDis, rAng);
                    scan.Add(p);
                }

                if (rDis > LIMITE && scan.Count > 10)
                {
                    int pos = scan.Count / 2;
                    scan[pos].calculaPosicao();

                    float x = (float) System.Math.Round(scan[pos].getX(), 2);
                    float z = (float) System.Math.Round(scan[pos].getY(), 2);

                    x = x / 10f;
                    z = z / 10f;
                    

                    Debug.Log("x = " + x);
                    Debug.Log("y = " + z);
                    //Debug.Log("ang = " + scan[pos].getAngle());

                    Vector3 instantiate_position = new Vector3(z, 0.15f, 20f - x);
                    Instantiate(point, instantiate_position, Quaternion.identity);

                    scan.Clear();
                }
                else if (rDis > LIMITE && scan.Count < 10)
                {
                    scan.Clear();
                }
                
            }
            catch(System.Exception){
                throw;
            }
        }
	}

    private void SplitAndConvert(string[] split)
    {
        try
        {
            rX = System.Convert.ToDouble(split[0]);
            rY = System.Convert.ToDouble(split[1]);
            rDis = System.Convert.ToDouble(split[2]);
            rAng = System.Convert.ToDouble(split[3]);
        }
        catch (System.FormatException)
        {
            rX = -1;
            rY = -1;
            rDis = -1;
            rAng = -1;
        }
        catch (System.IndexOutOfRangeException)
        {
            rX = -1;
            rY = -1;
            rDis = -1;
            rAng = -1;
        }
	}
}



