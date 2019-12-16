using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdDNA
{
    List<float> genes = new List<float>();
    int dnaLength = 0;
    float maxValue = 0;
    public BirdDNA(int length,float maxValue)
    {
        dnaLength = length;
        this.maxValue = maxValue;
        SetRandomValue();
    }
    private void SetRandomValue()
    {
        genes.Clear();
        for (int i = 0; i < dnaLength; i++)
        {
            genes.Add(Random.Range(-maxValue, maxValue));
        }
    }

    private void SetValue(int index,float value)
    {
        genes[index] = value;
    }
    //繁殖后代 重组基因
    public void Combine(BirdDNA birdDNA1,BirdDNA birdDNA2)
    {
        for (int i = 0; i < dnaLength; i++)
        {
            genes[i] = Random.Range(0, 10) < 5 ? birdDNA1.genes[i] : birdDNA2.genes[i];
        }
    }

    public void Mutate()
    {
        genes[Random.Range(0, dnaLength)] = Random.Range(-maxValue, maxValue);
    }
    public float GetGene(int index)
    {
        return genes[index];
    }
}
