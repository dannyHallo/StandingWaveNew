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

    private void Awake()
    {
        tM = sM.transform.GetComponentInChildren<Text>();
        tN = sN.transform.GetComponentInChildren<Text>();
        tC = sC.transform.GetComponentInChildren<Text>();
        tA = sA.transform.GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sM.onValueChanged.AddListener((v) =>
        {
            gen.m = (int)sM.value;
        });

        sN.onValueChanged.AddListener((v) =>
        {
            gen.n = (int)sN.value;
        });

        sC.onValueChanged.AddListener((v) =>
        {
            gen.c = sC.value;
        });

        sA.onValueChanged.AddListener((v) =>
        {
            gen.amplitude = sA.value;
        });


    }

    // Update is called once per frame
    void Update()
    {
        tM.text = gen.m.ToString("M = #0");
        tN.text = gen.n.ToString("N = #0");
        tC.text = gen.c.ToString("C = #0");
        tA.text = gen.amplitude.ToString("A = #0.0");
    }


}
