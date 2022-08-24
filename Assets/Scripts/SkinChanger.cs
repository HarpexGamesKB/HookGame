using UnityEngine;
using UnityEngine.UI;

public class SkinChanger : MonoBehaviour
{
    public Skin[] info;
    private bool[] StockCheck;
    public Button buyBttn;
    public Text priceText;
    public Transform player;
    public Collector Collector;
    public int index;

    public bool DeleteAtStart;
    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    private void Awake()
    {
        index = PlayerPrefs.GetInt("chosenSkin");

        StockCheck = new bool[100];
        if (PlayerPrefs.HasKey("StockArray"))
            StockCheck = PlayerPrefsX.GetBoolArray("StockArray");

        else
            StockCheck[0] = true;

        info[index].isChosen = true;

        for (int i = 0; i < info.Length; i++)
        {
            info[i].inStock = StockCheck[i];
            if (i == index)
                player.GetChild(i).gameObject.SetActive(true);
            else if (DeleteAtStart)
            {
                Destroy(player.GetChild(i).gameObject);
            }
            else
            {
                player.GetChild(i).gameObject.SetActive(false);
            }
                
        }
        if(priceText != null) { priceText.text = "Chosen"; }

        if (buyBttn != null) { buyBttn.interactable = false; }
        
    }

    public void Save()
    {
        PlayerPrefsX.SetBoolArray("StockArray", StockCheck);
    }
    public void Back()
    {
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i].isChosen)
            {
                player.GetChild(i).gameObject.SetActive(true);
                index = i;
            }
            else
            {
                player.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }
    public void Scroll()
    {
        if (info[index].inStock && info[index].isChosen)
        {
            priceText.text = "Chosen";
            buyBttn.interactable = false;
        }
        else if (!info[index].inStock)
        {
            priceText.text = info[index].cost.ToString();
            buyBttn.interactable = true;
        }
        else if (info[index].inStock && !info[index].isChosen)
        {
            priceText.text = "Choose";
            buyBttn.interactable = true;
        }

        for (int i = 0; i < player.childCount; i++)
        {
            player.GetChild(i).gameObject.SetActive(false);
        }
            

        player.GetChild(index).gameObject.SetActive(true);
    }
    public void ScrollRight()
    {
        if (index < player.childCount-1)
        {
            index++;
            Scroll();
        }
    }

    public void ScrollLeft()
    {
        if (index>0)
        {
            index--;
            Scroll();
        }
    }

    public void BuyButtonAction()
    {
        if (buyBttn.interactable && !info[index].inStock)
        {
            if (Collector.money >= int.Parse(priceText.text))
            {
                Collector.Buy(int.Parse(priceText.text));
                PlayerPrefs.SetInt("coins", Collector.money);
                StockCheck[index] = true;
                info[index].inStock = true;
                Save();
            }
        }

        if (buyBttn.interactable && !info[index].isChosen && info[index].inStock)
        {
            PlayerPrefs.SetInt("chosenSkin", index);
            buyBttn.interactable = false;
            info[index].isChosen = true;
            for (int i = 0; i < info.Length; i++)
            {
                if (i != index)
                {
                    info[i].isChosen = false;
                }
            }
            priceText.text = "Choosen";
        }
    }
}


[System.Serializable]
public class Skin
{
    public int cost;
    public bool inStock;
    public bool isChosen;
}