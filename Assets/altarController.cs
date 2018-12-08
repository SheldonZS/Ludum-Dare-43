using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class altarController : MonoBehaviour {

    public Sprite CuchulainPiece;
    public Sprite AnansiPiece;
    public Sprite AmaterasuPiece;
    public Sprite NephthysPiece;
    public Sprite SekhmetPiece;
    public Sprite MercuryPiece;
    public Sprite MonkeyKingPiece;
    public Sprite CthulhuPiece;
    public Sprite GummyBearPiece;
    public Sprite BaldrPiece;
    public Sprite CuchulainPleased;
    public Sprite AnansiPleased;
    public Sprite AmaterasuPleased;
    public Sprite NephthysPleased;
    public Sprite SekhmetPleased;
    public Sprite MercuryPleased;
    public Sprite MonkeyKingPleased;
    public Sprite CthulhuPleased;
    public Sprite GummyBearPleased;
    public Sprite BaldrPleased;

    private wheelSlot[] wheel;
    private Image[] slotImages;
    private Image wheelOutline;
    private DataBucket db;

    [Header("Rewards")]
    public clickable belt;
    public clickable scroll;
    public clickable lion;
    public clickable orb;
    public clickable sandal;
    public clickable knife;
    public clickable hair;

    private int selectedPiece = 0;
    // Update is called once per frame

    private void Start()
    {
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();

        wheel = new wheelSlot[10];
        slotImages = new Image[10];

        for (int x = 0; x < 10; x++)
        {
            slotImages[x] = GameObject.Find("Slot " + x).GetComponent<Image>();
        }
        wheelOutline = GameObject.Find("Outlines").GetComponent<Image>();

        wheel = db.getWheel();
    }

    public Diety currentSelection()
    {
        return wheel[selectedPiece].diety;
    }

    public void rotateWheel()
    {
        selectedPiece = (selectedPiece+1)%10;
        displayWheel();
    }

    public bool isPleased()
    {
        return wheel[selectedPiece].pleased;
    }

    public void pleaseDiety()
    {
        wheel[selectedPiece].pleased = true;
        displayWheel();

        switch(wheel[selectedPiece].diety)
        {
            case Diety.Cuchulain: db.CuchulainOffering = true; break;
            case Diety.Anansi: db.AnansiOffering = true; break;
            case Diety.Nephthys: db.NephthysOffering = true; break;
            case Diety.Amaterasu: db.AmaterasuOffering = true; break;
            case Diety.Mercury: db.MercuryOffering = true; break;
            case Diety.Sekhmet: db.SekhmetOffering = true; break;
            case Diety.MonkeyKing: db.MonkeyKingOffering = true; break;
            case Diety.Baldr: db.BaldrEnding = true; break;
            case Diety.Cthulhu: db.CthulhuEnding = true; break;
            case Diety.GummyBear: db.GummyBearEnding = true; break;

        }
    }

    public void addDiety(item item)
    {
        int target = selectedPiece;

        while(wheel[target].diety != Diety.None)
        {
            target = (target + 1) % 10;
            if (target == selectedPiece)
            {
                Debug.Log("Error: tried to add piece while wheel is full");
                return;
            }
        }

        if (item.name == "Cuchulain Piece")
            wheel[target].diety = Diety.Cuchulain;
        if (item.name == "Anansi Piece")
            wheel[target].diety = Diety.Anansi;
        if (item.name == "Amaterasu Piece")
            wheel[target].diety = Diety.Amaterasu;
        if (item.name == "Nephthys Piece")
            wheel[target].diety = Diety.Nephthys;
        if (item.name == "Sekhmet Piece")
            wheel[target].diety = Diety.Sekhmet;
        if (item.name == "Mercury Piece")
            wheel[target].diety = Diety.Mercury;
        if (item.name == "MonkeyKing Piece")
            wheel[target].diety = Diety.MonkeyKing;
        if (item.name == "Gummy Piece")
            wheel[target].diety = Diety.GummyBear;
        if (item.name == "Baldr Piece")
            wheel[target].diety = Diety.Baldr;
        if (item.name == "Cthulhu Piece")
            wheel[target].diety = Diety.Cthulhu;

        wheel[target].pleased = false;
        displayWheel();
    }

    public void displayWheel()
    {
        wheelOutline.enabled = true;

        for(int x = 0; x < 10; x++)
        {
            int target = (x + selectedPiece) % 10;

            if(wheel[target].diety == Diety.None)
            {
                slotImages[x].enabled = false;
            }
            else
            {
                slotImages[x].enabled = true;
                switch(wheel[target].diety)
                {
                    case Diety.Cuchulain:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = CuchulainPleased;
                        else slotImages[x].overrideSprite = CuchulainPiece;
                        break;
                    case Diety.Anansi:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = AnansiPleased;
                        else slotImages[x].overrideSprite = AnansiPiece;
                        break;
                    case Diety.Nephthys:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = NephthysPleased;
                        else slotImages[x].overrideSprite = NephthysPiece;
                        break;
                    case Diety.Amaterasu:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = AmaterasuPleased;
                        else slotImages[x].overrideSprite = AmaterasuPiece;
                        break;
                    case Diety.Mercury:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = MercuryPleased;
                        else slotImages[x].overrideSprite = MercuryPiece;
                        break;
                    case Diety.Sekhmet:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = SekhmetPleased;
                        else slotImages[x].overrideSprite = SekhmetPiece;
                        break;
                    case Diety.MonkeyKing:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = MonkeyKingPleased;
                        else slotImages[x].overrideSprite = MonkeyKingPiece;
                        break;
                    case Diety.Baldr:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = BaldrPleased;
                        else slotImages[x].overrideSprite = BaldrPiece;
                        break;
                    case Diety.Cthulhu:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = CthulhuPleased;
                        else slotImages[x].overrideSprite = CthulhuPiece;
                        break;
                    case Diety.GummyBear:
                        if (wheel[target].pleased)
                            slotImages[x].overrideSprite = GummyBearPleased;
                        else slotImages[x].overrideSprite = GummyBearPiece;
                        break;
                }
            }
        }
    }

    public void hideWheel()
    {
        wheelOutline.enabled = false;
        foreach (Image i in slotImages)
            i.enabled = false;
    }

    public void save()
    {
        db.saveWheel(wheel);
    }

}

[System.Serializable]
public class wheelSlot
{
    public Diety diety = Diety.None;
    public bool pleased = false;

    public wheelSlot()
    {
        diety = Diety.None;
        pleased = false;
    }

    public wheelSlot(Diety d)
    {
        diety = d;
        pleased = false;
    }

    public wheelSlot(wheelSlot input)
    {
        diety = input.diety;
        pleased = input.pleased;
    }
}

public enum Diety { None, Cuchulain, Anansi, Amaterasu, Nephthys, Sekhmet, Mercury, MonkeyKing, Cthulhu, GummyBear, Baldr};