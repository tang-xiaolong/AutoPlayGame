using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定义感知器结构
[System.Serializable]
public class MyORPerceptron
{
    public double[] input;
    public double output;
}

public class ORPerceptron : MonoBehaviour
{
    public MyORPerceptron[] trainingSet;
    public int trainEpoch = 10;
    double[] weights = { 0, 0 };
    double bias = 0;
    double totalError = 0;

    private void Start()
    {
        Train(trainEpoch);
        Test();
    }

    void Test()
    {
        Debug.Log("0 0 res = " + CalcOutput(0,0));
        Debug.Log("0 1 res = " + CalcOutput(0,1));
        Debug.Log("1 0 res = " + CalcOutput(1,0));
        Debug.Log("1 1 res = " + CalcOutput(1,1));
    }
    int CalcOutput(double i1,double i2)
    {
        double[] tempArray = new double[] { i1, i2 };
        double dp = DotProductBias(weights, tempArray);
        return dp > 0 ? 1 : 0;
    }
    void InitWeight()
    {
        for(int i = 0;i < weights.Length;++i)
        {
            weights[i] = Random.Range(-1.0f, 1.0f);
        }
        bias = Random.Range(-1.0f, 1.0f);
    }
    void Train(int epoch)
    {
        InitWeight();
        int count = trainingSet.Length;
        for(int i = 0;i < epoch;++i)
        {
            //每次训练前将错误清零
            totalError = 0;
            for(int j = 0;j < count;++j)//更新权值
            {
                UpdateWeights(j);
                Debug.Log("w1 = " + weights[0] + " w2 = " + weights[1] + " bias = " + bias);
            }
            Debug.Log("total error = " + totalError);
        }
    }
    void UpdateWeights(int j)//针对每行数据，计算结果和真正输出
    {
        double error = trainingSet[j].output - CalOutOut(j);//得到错误大小
        totalError += Mathf.Abs((float)error);
        for(int i = 0;i < weights.Length;++i)
        {
            weights[i] = weights[i] + error * trainingSet[j].input[i];//更新权值
        }
        bias += error;
    }

    double CalOutOut(int j)
    {
        double dp = DotProductBias(weights, trainingSet[j].input);//计算点积
        return dp > 0 ? 1 : 0;
    }
    double DotProductBias(double[] v1,double[] v2)
    {
        if (v1 == null || v2 == null)
            return -1;
        if (v1.Length != v2.Length)
            return -1;
        double d = 0;
        for(int i = 0;i < v1.Length;++i)
        {
            d += v1[i] * v2[i];
        }
        d += bias;
        return d;
    }
}
