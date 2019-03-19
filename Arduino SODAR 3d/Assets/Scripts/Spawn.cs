using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Spawn : MonoBehaviour {
    public Transform point;

    private List<Position> scan;
    private double rX, rY, rDis, rAng;
    private static int LIMITE = 80;

    SerialPort sp = new SerialPort("\\\\.\\COM3", 9600);

	void Start () {
        sp.Open();
        sp.ReadTimeout = 1000;

        scan = new List<Position>();

        rX = -1;
        rY = -1;
        rDis = -1;
        rAng = -1;
	}
	

	void Update () {
		if(sp.IsOpen){
            try
            {
                string str = sp.ReadLine();
                
                string[] str_split = str.Split(',');

                SplitAndConvert(str_split);

                if (rDis < LIMITE && rDis > -1)
                {
                    Position p = new Position(rX, rY, rDis, rAng);
                    scan.Add(p);
                }

                if(rDis > LIMITE && scan.Count > 13)
                //if ((rDis > LIMITE && scan.Count > 13) || scan.Count > 13)
                {
                    int pos = scan.Count / 2;
                    scan[pos].calculaPosicao();

                    float x = (float) System.Math.Round(scan[pos].getX(), 2);
                    float z = (float) System.Math.Round(scan[pos].getY(), 2);

                    x = x / 10f;
                    z = z / 10f;
                    
                    Debug.Log("x = " + x);
                    Debug.Log("y = " + z);

                    Vector3 instantiate_position = new Vector3(x, 0.15f, z);
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

public class Position
{
            private double posX;
        private double posY;
        private double distance;
        private double angle;

        private double x;
        private double y;

        public Position()
        {

        }

        public Position(Position p)
        {
            posX = p.getPosX();
            posY = p.getPosY();
            distance = p.getDistance();
            angle = p.getAngle();
        }

        public Position(double posX_, double posY_, double dis, double ang)
        {
            posX = posX_;
            posY = posY_;
            distance = dis;
            angle = ang;
        }

        public void setPosX(double posX_)
        {
            posX = posX_;
        }

        public void setPosY(double posY_)
        {
            posY_ = posY;
        }

        public void setDistance(double dis)
        {
            distance = dis;
        }

        public void setAngle(double ang)
        {
            angle = ang;
        }

        public double getPosX()
        {
            return posX;
        }

        public double getPosY()
        {
            return posY;
        }

        public double getDistance()
        {
            return distance;
        }

        public double getAngle()
        {
            return angle;
        }

        public double getX()
        {
            return x;
        }

        public double getY()
        {
            return y;
        }

        public void calculaPosicao()
        {
            double catetoOposto;
            double catetoAdjacente;

            double cosInRadian = System.Math.Cos(angle * (System.Math.PI / 180.0));
            double sinInRadian = System.Math.Sin(angle * (System.Math.PI / 180.0));

            catetoOposto = sinInRadian * distance;
            catetoAdjacente = cosInRadian * distance;

            x = catetoAdjacente + posX;
            y = catetoOposto + posY;
        }
    }