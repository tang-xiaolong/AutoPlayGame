using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BirdPopulationManager : MonoBehaviour
{
    public GameObject bot;
    public Transform startPos;
    public int populationSize = 50;
    private List<GameObject> populations = new List<GameObject>();
    public static float elapsed = 0;
    public float trailTime = 5;
    public float timeScale = 1;
    int generation = 1;
    public int maxEpoch = 20;
    private int nowEpoch = 0;

    GUIStyle style = new GUIStyle();
    private void OnGUI()
    {
        style.fontSize = 25;
        style.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "stats", style);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, style);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time:{0:0.0}",elapsed), style);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + populations.Count, style);
        GUI.EndGroup();
    }

    private void Start()
    {
        Time.timeScale = timeScale;//加快速度
        for (int i = 0; i < populationSize; i++)
        {
            GameObject obj = Instantiate(bot, startPos.position, transform.rotation);
            obj.GetComponent<BirdBrain>().Init();
            populations.Add(obj);
        }
    }

    GameObject Breed(GameObject parent1,GameObject parent2)
    {
        GameObject child = Instantiate(bot, startPos.position, transform.rotation);
        BirdBrain brain = child.GetComponent<BirdBrain>();
        brain.Init();
        if (Random.Range(0, 100) < 1)//突变
            brain.dna.Mutate();
        else//否则就结合
            brain.dna.Combine(parent1.GetComponent<BirdBrain>().dna, parent2.GetComponent<BirdBrain>().dna);
        return child;
    }

    void CalculateScole()
    {
        int winnerNum = 0;
        foreach (var item in populations)
        {
            BirdBrain birdBrain = item.GetComponent<BirdBrain>();
            birdBrain.score = Mathf.Pow(birdBrain.distanceTravel + 1, 4) - Mathf.Pow(birdBrain.crashCount, 2) - Mathf.Pow(birdBrain.dieCount, 4) + birdBrain.endLineScore;
            //birdBrain.score = Mathf.Pow(birdBrain.distanceTravel + 1, 3) + birdBrain.aliveTime - Mathf.Pow(birdBrain.crashCount, 2);
            if (birdBrain.endLineScore != 0) ++winnerNum;
        }
        Debug.Log("有"+winnerNum+"只鸟到达了终点！");
    }

    void BreedNewPopulation(bool isTrain = true)
    {
        Time.timeScale = timeScale;//加快速度
        CalculateScole();
        List<GameObject> sortList = populations.OrderBy(o => (o.GetComponent<BirdBrain>().score)).ToList();
        populations.Clear();
        //for (int i = 0; i < 25; i++)
        //{
        //    populations.Add(Breed(sortList[0], sortList[1]));
        //    populations.Add(Breed(sortList[1], sortList[0]));
        //    populations.Add(Breed(sortList[2], sortList[3]));
        //    populations.Add(Breed(sortList[3], sortList[2]));
        //}
        if(isTrain)
        {
            for (int i = (int)(sortList.Count * 9 / 10.0) - 1; i < sortList.Count - 1; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //Debug.Log(sortList[i].GetComponent<BirdBrain>().distanceTravel + "  " + sortList[i].GetComponent<BirdBrain>().crashCount);
                    //Debug.Log(sortList[i + 1].GetComponent<BirdBrain>().distanceTravel + "  " + sortList[i + 1].GetComponent<BirdBrain>().crashCount);
                    populations.Add(Breed(sortList[i], sortList[i + 1]));
                    populations.Add(Breed(sortList[i + 1], sortList[i]));
                }
            }
        }
        else
        {
            for(int i = 0;i < sortList.Count / 2;++i)//需要这么多个
            {
                populations.Add(Breed(sortList[0], sortList[1]));
                populations.Add(Breed(sortList[1], sortList[0]));
            }
        }

        for (int i = 0; i < sortList.Count; i++)
        {
            Destroy(sortList[i]);
        }
        generation++;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= trailTime)
        {
            if (nowEpoch < maxEpoch)
                BreedNewPopulation();
            else
                BreedNewPopulation(false);
            nowEpoch += 1;
            elapsed = 0;
        }
    }
}
