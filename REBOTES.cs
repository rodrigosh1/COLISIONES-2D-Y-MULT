using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class REBOTES : MonoBehaviour
{
    public GameObject Esf1;
    public GameObject Esf2;
    public GameObject Esf3;

    //VARIABLES INICIALES
    float t = 0;
    float K1, K2, K3, K4, L1, L2, L3, L4;
    float h = 0.001f;
    float vx1 = -50f;
    float vx2 = 1f;
    float vy1 = 0f;
    float vy2 = 0f;
    bool coli, coli2;
    bool toque;

    Vector3 v1;
    Vector3 v2;
    Vector3 v3;

    float xdis, ydis;
    float d;
    bool condM;

    float teta;
    float m1, m2, m3;
    float e;
    float vp, vn;
    float vp2, vn2;
    float v1prim, vx1prim, vy1prim;
    float v2prim, vx2prim, vy2prim;

    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;

    public Text vel1;
    public Text vel1_txt;

    public Text vel2;
    public Text vel2_txt;

    public Text mas1;
    public Text mas1_txt;

    public Text mas2;
    public Text mas2_txt;

    public Text mas3;
    public Text mas3_txt;

    // Start is called before the first frame update
    void Start()
    {
        pos1 = new Vector3(-5.37f, -0.03f, 0);
        pos2 = new Vector3(0f, 0.95f, 0); 
        pos3 = new Vector3(2.16f, 1.35f, 0);

        Esf1.GetComponent<Transform>().position = new Vector3(pos1.x, pos1.y, 0);
        Esf2.GetComponent<Transform>().position = new Vector3(pos2.x, pos2.y, 0);
        Esf3.GetComponent<Transform>().position = new Vector3(pos3.x, pos3.y, 0);

        coli = false;
        toque = true;
        vp = vn = vn2 = vp2 = 0;
        v1prim = vy1prim = vx1prim = 0;
        
        v1 = new Vector3(70f, 0, 0);
        v2 = new Vector3(5f, 0f, 0);
        v3 = new Vector3(0, -2f, 0);

        m1 = 1000f;
        m2 = 100f;
        m3 = 100f;
        e = 0.95f;

        condM = false;


        //----
        vel1.text = "0";
        vel2.text = "0";
        //----
        mas1.text = "0";
        mas2.text = "0";
        mas3.text = "0";
    }

    void enviardatos()
    {
        v1.x = float.Parse(vel1.text);
        vel1_txt.text = "VELOCIDAD ACT = " + vel1.text;


        v2.x = float.Parse(vel2.text);
        vel2_txt.text = "VELOCIDAD ACT = " + vel2.text;

        m1 = float.Parse(mas1.text);
        mas1_txt.text = "MASA 1 ACT = " + mas1.text;

        m2 = float.Parse(mas2.text);
        mas2_txt.text = "MASA 2 ACT = " + mas2.text;

        m3 = float.Parse(mas3.text);
        mas3_txt.text = "MASA 3 ACT = " + mas3.text;
    }


    // Update is called once per frame
    void Update()
    {
        if (condM)
        {
            mov();
        }

        if (Input.GetKeyDown("p"))
        {
            condM = true;
        }
        if (Input.GetKeyDown("e"))
        {
            enviardatos();

        }
        if (Input.GetKeyDown("r"))
        {
            Start();
            vel1_txt.text = "VELOCIDAD ACT = 70";
            vel2_txt.text = "VELOCIDAD ACT = 5";
            mas1_txt.text = "MASA ACT = 1000";
            mas2_txt.text = "MASA ACT = 100 ";
            mas3_txt.text = "MASA ACT = 100";

            condM = false;
        }
    }

    void mov()
    {
        pos1 += new Vector3(runge_pos(pos1.x, v1.x, 1), runge_pos(pos1.y, v1.y, 1), 0);
        pos2 += new Vector3(runge_pos(pos2.x, v2.x, 2), runge_pos(pos2.y,v2.y,2), 0);
        pos3 += new Vector3(runge_pos(pos3.x, v3.x, 3), runge_pos(pos3.y, v3.y, 3), 0);

        v1 = colisiones(pos1, pos2, Esf1.GetComponent<Transform>().localScale.x / 2, Esf2.GetComponent<Transform>().localScale.x / 2, m1, m2, 1, v1, v2);
        v2 = colisiones(pos1, pos2, Esf1.GetComponent<Transform>().localScale.x / 2, Esf2.GetComponent<Transform>().localScale.x / 2, m1, m2, 2, v1, v2);
   
        v2 = colisiones(pos2, pos3, Esf2.GetComponent<Transform>().localScale.x / 2, Esf3.GetComponent<Transform>().localScale.x / 2, m2, m3, 1, v2, v3);
        v3 = colisiones(pos2, pos3, Esf2.GetComponent<Transform>().localScale.x / 2, Esf3.GetComponent<Transform>().localScale.x / 2, m2, m3, 2, v2, v3);

        v1 = colisiones(pos1, pos3, Esf1.GetComponent<Transform>().localScale.x / 2, Esf3.GetComponent<Transform>().localScale.x / 2, m1, m3, 1, v1, v3);
        v3 = colisiones(pos1, pos3, Esf1.GetComponent<Transform>().localScale.x / 2, Esf3.GetComponent<Transform>().localScale.x / 2, m1, m3, 2, v1, v3);
        
        Esf1.GetComponent<Transform>().position = new Vector3(pos1.x, pos1.y, 0);
        Esf2.GetComponent<Transform>().position = new Vector3(pos2.x, pos2.y, 0);
        Esf3.GetComponent<Transform>().position = new Vector3(pos3.x, pos3.y, 0);

    }

    Vector3 colisiones(Vector3 pose1, Vector3 pose2, float r1, float r2,float m1,float m2,float condi, Vector3 v1, Vector3 v2)
    {
        float de;
        de = Mathf.Sqrt(Mathf.Pow(pose2.x - pose1.x, 2) + Mathf.Pow(pose2.y - pose1.y, 2));

        float radio = (r1 + r2);
        xdis = pose2.x - pose1.x;
        ydis = pose2.y - pose1.y;

        if(de < radio)
        {
            if (ydis < 0) {
                if (xdis >= 0)
                {
                    teta = -Mathf.Acos(xdis / de);
                }
                else
                {
                    xdis = pose1.x - pose2.x;
                    teta = Mathf.Acos(xdis / de);
                }
            } else
            {
                if (xdis >= 0)
                {
                    teta = Mathf.Acos(xdis / de);
                }
                else
                {
                    //xdis = pose1.x - pose2.x;
                    //Debug.Log("Grados antes = " + Mathf.Acos(xdis / de));
                    //Debug.Log("PI = " + Mathf.PI);
                    teta = -Mathf.PI + Mathf.Acos(xdis / de);
                  
                }
            }
            

            vp = v1.x * Mathf.Cos(teta) + v1.y * Mathf.Sin(teta);
            vn = -v1.x * Mathf.Sin(teta) + v1.y * Mathf.Cos(teta);
            //------------------------
            vp2 = v2.x * Mathf.Cos(teta) + v2.y * Mathf.Sin(teta);
            vn2 = -v2.x * Mathf.Sin(teta) + v2.y * Mathf.Cos(teta);

            v1prim = ((m1 - (e * m2)) / (m1 + m2)) * v1.x + (((1 + e) * m2) / (m1 + m2)) * v2.x;
            vx1prim = v1prim * Mathf.Cos(teta) - vn * Mathf.Sin(teta);
            vy1prim = v1prim * Mathf.Sin(teta) + vn * Mathf.Cos(teta);
            //--------------------------
            v2prim = ((m2 - (e * m1)) / (m1 + m2)) * v2.x + (((1 + e) * m1) / (m1 + m2)) * v1.x;
            vx2prim = v2prim * Mathf.Cos(teta) - vn * Mathf.Sin(teta);
            vy2prim = v2prim * Mathf.Sin(teta) + vn * Mathf.Cos(teta);

            if(condi <= 1) { return new Vector3(vx1prim,vy1prim,0); } else
            {
                toque = false;
                return new Vector3(vx2prim, vy2prim, 0);
            }
        }
        else
        {
            if (condi <= 1) { return v1; }
            else
            {
                return v2;
            }
        }
      
    }

    float runge_pos(float pos, float v, int cnd)
    {//EL METODO DE RUNGE HACE LAS RESPECTIVAS OPERACIONES DEL METODO MATEMATICO y RETORNA EL VALOR DE LA POSICION

        K1 = h * v;
        L1 = h * func(t, pos, v);
        K2 = h * (v + (L1 / 2));
        L2 = h * func(t + (h / 2), pos + (K1 / 2), v + (L1 / 2));
        K3 = h * (v + (L2 / 2));
        L3 = h * func(t + (h / 2), pos + (K2 / 2), v + (L2 / 2));
        K4 = h * (v + L3);
        L4 = h * func(t + h, pos + K3, v + L3);

        t = t + h;

        //DEVULVE LA POSICION CALCULADA POR EL METODO
        pos = ((K1 + 2 * K2 + 2 * K3 + K4) / 6);

        //CALCULA LA POSICION QUE DIO EL METODO
        if (cnd == 1) { v1.x = v1.x + ((L1 + 2 * L2 + 2 * L3 + L4) / 6);
            v1.y = v1.y + ((L1 + 2 * L2 + 2 * L3 + L4) / 6);
        }
        if (cnd == 2) { v2.x = v2.x + ((L1 + 2 * L2 + 2 * L3 + L4) / 6);
            v2.y = v2.y + ((L1 + 2 * L2 + 2 * L3 + L4) / 6);
        }
        if (cnd == 3) { v3.x = v3.x + ((L1 + 2 * L2 + 2 * L3 + L4) / 6);
            v3.y = v3.y + ((L1 + 2 * L2 + 2 * L3 + L4) / 6);
        }

        //RETORNA LA POSICION
        return pos;
    }

    //FUNCION QUE DETERMINA EL MOVIMIENTO SEGUN LA ECUACION DIFERENCIAL
    //EN ESTE CASO NO ESTAMOS TOMANDO UNA ACELERACION, POR LO CUAL ES CONTANTE(0)
    float func(float t, float x1, float vx1)
    {
        float res = 0;
        return res;
    }
}
