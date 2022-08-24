using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{ 
    [SerializeField] private Collector Collector;
    [SerializeField] private Hook Hook;

    [SerializeField] private int LenthPrice = 10;
    [SerializeField] private Text LenthPriceText;

    [SerializeField] private int FuelPrice = 10;
    [SerializeField] private Text FuelPriceText;

    [SerializeField] private int SizePrice = 10;
    [SerializeField] private Text SizePriceText;

    [SerializeField] private int CapacityPrice = 10;
    [SerializeField] private Text CapacityPriceText;


    [SerializeField] private int AssemblyLenthPrice = 10;
    [SerializeField] private Text AssemblyLenthPriceText;
    [SerializeField] private Transform AssemblyLine;
    //[SerializeField] private GameObject MaxSizePanel;
    private void Awake()
    {
        LenthPrice = PlayerPrefs.GetInt("LenthPrice", LenthPrice); 
        FuelPrice = PlayerPrefs.GetInt("FuelPrice", FuelPrice); 
        SizePrice = PlayerPrefs.GetInt("SizePrice", SizePrice);
        CapacityPrice = PlayerPrefs.GetInt("CapacityPrice", CapacityPrice);
        Hook.shotTime = PlayerPrefs.GetFloat("shotTime", Hook.shotTime);
        Hook.maxFuel = PlayerPrefs.GetInt("maxFuel", Hook.maxFuel);
        float size = PlayerPrefs.GetFloat("hookBodylocalScale", Hook.HookBody.localScale.x);
        Hook.HookBody.localScale = new Vector3(size, size, size);
        Hook.radius = PlayerPrefs.GetFloat("radius", Hook.radius);
        Hook.maxCapacity = PlayerPrefs.GetInt("maxCapacity", Hook.maxCapacity);

        AssemblyLenthPrice=PlayerPrefs.GetInt("AssemblyLenthPrice", AssemblyLenthPrice);
        AssemblyLine.localScale=new Vector3( PlayerPrefs.GetFloat("AssemblyLinelocalScale", AssemblyLine.localScale.x),1f,1f);

        UpdateText(LenthPriceText, LenthPrice);
        UpdateText(FuelPriceText, FuelPrice);
        UpdateText(SizePriceText, SizePrice);
        UpdateText(CapacityPriceText, CapacityPrice);

        UpdateText(AssemblyLenthPriceText, AssemblyLenthPrice);

    }
    public void AddLenth()
    {
        if(Collector.money>= LenthPrice)
        {
            Collector.Buy(LenthPrice);
            Hook.shotTime += 0.01f;
            LenthPrice += RaiseThePrice(LenthPrice);
            UpdateText(LenthPriceText, LenthPrice);
            PlayerPrefs.SetInt("LenthPrice", LenthPrice);
            PlayerPrefs.SetFloat("shotTime", Hook.shotTime);
        }
    }
    public void AddFuel()
    {
        if (Collector.money >= FuelPrice)
        {
            Collector.Buy(LenthPrice);
            Hook.FuelUpgrade(1);
            FuelPrice += RaiseThePrice(FuelPrice);
            UpdateText(FuelPriceText, FuelPrice);
            PlayerPrefs.SetInt("FuelPrice", FuelPrice);
            PlayerPrefs.SetInt("maxFuel", Hook.maxFuel);
        }
    }
    public void AddSize()
    {
        if (Collector.money >= SizePrice)
        {
            Collector.Buy(LenthPrice);
            Hook.HookBody.localScale += Vector3.one * 0.1f;
            Hook.radius = Hook.HookBody.localScale.x * 0.3f;
            SizePrice += RaiseThePrice(SizePrice);
            UpdateText(SizePriceText, SizePrice);
            PlayerPrefs.SetInt("SizePrice", SizePrice);
            PlayerPrefs.SetFloat("radius", Hook.radius);
            PlayerPrefs.SetFloat("hookBodylocalScale", Hook.HookBody.localScale.x);
        }
    }
    
    public void AddCapacity()
    {
        if (Collector.money >= CapacityPrice)
        {
            Collector.Buy(CapacityPrice);
            CapacityPrice += RaiseThePrice(SizePrice);
            Hook.maxCapacity += 2;
            UpdateText(CapacityPriceText, CapacityPrice);
            PlayerPrefs.SetInt("CapacityPrice", CapacityPrice);
            PlayerPrefs.SetInt("maxCapacity", Hook.maxCapacity);
        }
    }

    public void AddAssemblyLineLenth()
    {
        if (Collector.money >= AssemblyLenthPrice)
        {
            Collector.Buy(AssemblyLenthPrice);
            AssemblyLine.localScale += new Vector3(0.2f,0f,0f);
            AssemblyLenthPrice += RaiseThePrice(AssemblyLenthPrice);
            UpdateText(AssemblyLenthPriceText, AssemblyLenthPrice);
            PlayerPrefs.SetInt("AssemblyLenthPrice", AssemblyLenthPrice);
            PlayerPrefs.SetFloat("AssemblyLinelocalScale", AssemblyLine.localScale.x);
        }
    }
    private int RaiseThePrice(int price)
    {
        float temp = (float)price * 0.28f;
        //float temp = (float)price * 0.1f;
        return (int)temp;
        
    }
    private void UpdateText(Text text, int price)
    {
        text.text = price.ToString()+"$";
    }
    /*
    private void CheckIsMax()
    {
        if (Hook.HookBody.localScale.x > 4)
        {

        }
    }
    */
    public void ResetProgress()
    {
        LenthPrice = 10;
        FuelPrice = 10;
        SizePrice = 10;
        CapacityPrice = 10;
        AssemblyLenthPrice = 10;
        Hook.shotTime = 0.2f;
        Hook.maxFuel = 4;
        Hook.maxCapacity = 8;
        Hook.FuelUpgrade(1);
        Hook.HookBody.localScale = Vector3.one;
        Hook.radius = Hook.HookBody.localScale.x * 0.3f;

        AssemblyLine.localScale = Vector3.one;

        Collector.Buy(Collector.money);
        UpdateText(LenthPriceText, LenthPrice);
        UpdateText(FuelPriceText, FuelPrice);
        UpdateText(SizePriceText, SizePrice);
        UpdateText(CapacityPriceText, CapacityPrice);

        UpdateText(AssemblyLenthPriceText, AssemblyLenthPrice);

        PlayerPrefs.SetFloat("radius", Hook.radius);
        PlayerPrefs.SetInt("LenthPrice", LenthPrice);
        PlayerPrefs.SetInt("FuelPrice", FuelPrice);
        PlayerPrefs.SetInt("SizePrice", SizePrice);
        PlayerPrefs.SetInt("maxCapacity",Hook.maxCapacity);
        PlayerPrefs.SetFloat("shotTime", Hook.shotTime);
        PlayerPrefs.SetInt("maxFuel", Hook.maxFuel);
        PlayerPrefs.SetFloat("hookBodylocalScale", Hook.HookBody.localScale.x);
        PlayerPrefs.SetFloat("radius", 0.3f);
        PlayerPrefs.SetInt("CapacityPrice", CapacityPrice);

        PlayerPrefs.SetInt("AssemblyLenthPrice", AssemblyLenthPrice);
        PlayerPrefs.SetFloat("AssemblyLinelocalScale", AssemblyLine.localScale.x);
    }
}
