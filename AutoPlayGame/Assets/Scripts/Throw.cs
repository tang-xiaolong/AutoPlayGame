using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject spherePrefab;
    public GameObject cubePrefab;
    public Material green;
    public Material red;

    AvoidBallPerceptron avoidBallPerceptron;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        avoidBallPerceptron = GetComponent<AvoidBallPerceptron>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            GameObject obj = Instantiate(spherePrefab, mainCamera.transform.position, mainCamera.transform.rotation);
            obj.GetComponent<Renderer>().material = red;
            obj.GetComponent<Rigidbody>().AddForce(0, 0, 1000);
            avoidBallPerceptron.SendInput(0, 0, 0);
        }
        else if (Input.GetKeyDown("2"))
        {
            GameObject obj = Instantiate(spherePrefab, mainCamera.transform.position, mainCamera.transform.rotation);
            obj.GetComponent<Renderer>().material = green;
            obj.GetComponent<Rigidbody>().AddForce(0, 0, 1000);
            avoidBallPerceptron.SendInput(0, 1, 1);
        }
        else if (Input.GetKeyDown("3"))
        {
            GameObject obj = Instantiate(cubePrefab, mainCamera.transform.position, mainCamera.transform.rotation);
            obj.GetComponent<Renderer>().material = red;
            obj.GetComponent<Rigidbody>().AddForce(0, 0, 1000);
            avoidBallPerceptron.SendInput(1, 0, 1);
        }
        else if (Input.GetKeyDown("4"))
        {
            GameObject obj = Instantiate(cubePrefab, mainCamera.transform.position, mainCamera.transform.rotation);
            obj.GetComponent<Renderer>().material = green;
            obj.GetComponent<Rigidbody>().AddForce(0, 0, 1000);
            avoidBallPerceptron.SendInput(1, 1, 1);
        }

    }
}
