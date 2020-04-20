using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//定义感知器结构
[System.Serializable]
public class MyAvoidPerceptron
{
    public double[] input;
    public double output;
}

public class AvoidBallPerceptron : MonoBehaviour
{
    private List<MyAvoidPerceptron> trainingSet = new List<MyAvoidPerceptron>();
    double[] weights = { 0, 0 };
    double bias = 0;
    double totalError = 0;

    public GameObject npc;
    public Animator npcAnimator;
    public Rigidbody npcRigidbody;  


    public void SendInput(double i1,double i2,double o)
    {
        double result = CalcOutput(i1, i2);
        Debug.Log(result);
        if(result == 0)
        {
            npcAnimator.SetTrigger("Crouch");
            npcRigidbody.isKinematic = false;
        }
        else
            npcRigidbody.isKinematic = true;

        MyAvoidPerceptron myAvoid = new MyAvoidPerceptron();
        myAvoid.input = new double[2] { i1, i2 };
        myAvoid.output = 0;
        trainingSet.Add(myAvoid);
        Train();

    }

    private void Start()
    {
        if(npc != null)
        {
            npcRigidbody = npc.GetComponent<Rigidbody>();
            npcAnimator = npc.GetComponent<Animator>();
        }
        InitWeight();
    }

    void Test()
    {
        Debug.Log("0 0 res = " + CalcOutput(0, 0));
        Debug.Log("0 1 res = " + CalcOutput(0, 1));
        Debug.Log("1 0 res = " + CalcOutput(1, 0));
        Debug.Log("1 1 res = " + CalcOutput(1, 1));
    }
    double CalcOutput(double i1, double i2)
    {
        double[] tempArray = new double[] { i1, i2 };
        return ActivationFunction(DotProductBias(weights, tempArray));
    }
    void InitWeight()
    {
        for (int i = 0; i < weights.Length; ++i)
        {
            weights[i] = Random.Range(-1.0f, 1.0f);
        }
        bias = Random.Range(-1.0f, 1.0f);
    }
    void Train()
    {
        int count = trainingSet.Count;
        //每次训练前将错误清零
        totalError = 0;
        for (int j = 0; j < count; ++j)//更新权值
        {
            UpdateWeights(j);
        }
    }
    void UpdateWeights(int j)//针对每行数据，计算结果和真正输出
    {
        double error = trainingSet[j].output - CalOutOut(j);//得到错误大小
        totalError += Mathf.Abs((float)error);
        for (int i = 0; i < weights.Length; ++i)
        {
            weights[i] = weights[i] + error * trainingSet[j].input[i];//更新权值
        }
        bias += error;
    }

    double CalOutOut(int j)
    {
        return ActivationFunction(DotProductBias(weights, trainingSet[j].input));//计算点积
    }

    double ActivationFunction(double dp)
    {
        return dp > 0 ? 1 : 0;
    }
    double DotProductBias(double[] v1, double[] v2)
    {
        if (v1 == null || v2 == null)
            return -1;
        if (v1.Length != v2.Length)
            return -1;
        double d = 0;
        for (int i = 0; i < v1.Length; ++i)
        {
            d += v1[i] * v2[i];
        }
        d += bias;
        return d;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitWeight();
            trainingSet.Clear();
        }
        else if (Input.GetKeyDown(KeyCode.S))
            SaveWeights();
        else if (Input.GetKeyDown(KeyCode.L))
            LoadWeights();   
    }

    void LoadWeights()
    {
        string path = Application.dataPath + "/weights.txt";
        if(File.Exists(path))
        {
            var sr = File.OpenText(path);
            string line = sr.ReadLine();
            string[] w = line.Split(',');
            weights[0] = System.Convert.ToDouble(w[0]);
            weights[1] = System.Convert.ToDouble(w[1]);
            bias = System.Convert.ToDouble(w[2]);
        }
    }

    void SaveWeights()
    {
        string path = Application.dataPath + "/weights.txt";
        var sr = File.CreateText(path);
        sr.WriteLine(weights[0] + "," + weights[1] + "," + bias);
        sr.Close();
    }
}
