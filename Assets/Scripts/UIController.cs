using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject backGround;
    public Slider sM;
    public Slider sN;
    public Slider sC;
    public Slider sA;
    public Text tM;
    public Text tN;
    public Text tC;
    public Text tA;
    public Gen gen;
    
    private void Awake() {
        tM = sM.transform.GetComponentInChildren<Text>();
        tN = sN.transform.GetComponentInChildren<Text>();
        tC = sC.transform.GetComponentInChildren<Text>();
        tA = sA.transform.GetComponentInChildren<Text>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        sM.onValueChanged.AddListener((v) => {
            gen.m = (int)sM.value;
            string tempString = "M = " + sM.value.ToString();
            tM.text = tempString;
        });

        sN.onValueChanged.AddListener((v) => {
            gen.n = (int)sN.value;
            string tempString = "N = " + sN.value.ToString();
            tN.text = tempString;
        });

        sC.onValueChanged.AddListener((v) => {
            gen.c = sC.value;
            string tempString = "C = " + sC.value.ToString();
            tC.text = tempString;
        });

        sA.onValueChanged.AddListener((v) => {
            gen.amplitude = sA.value;
            string tempString = "A = " + sA.value.ToString();
            tA.text = tempString;
        });

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
