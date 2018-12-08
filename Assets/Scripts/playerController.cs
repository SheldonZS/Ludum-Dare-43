using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour {

    public float walkSpeed = 5f;

    [HideInInspector]
    public bool busy = false;

    private DataBucket db;
    private InventoryManager im;
    private SkeletonGraphic spine;
    private textBox textBox;
    private RectTransform rt;
    private roomController roomController;
    private AudioSource BGM;
    private AudioSource SFX;
    private AudioSource[] chord;
    private altarController altarWheel;

    private GameObject clickable;
    private Transform objectsLayer;

    private string currentAnimation = "idle";
    private string passcode = "";

    private int candlesLit;
    private int chordNotes;
    private int dietiesPleased;
    private bool docentUnlocked = false;

	// Use this for initialization
	void Start () {
        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        im = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        rt = GetComponent<RectTransform>();
        textBox = GameObject.Find("TextBox").GetComponent<textBox>();
        spine = GetComponent<SkeletonGraphic>();
        roomController = GetComponentInParent<roomController>();
        BGM = GameObject.Find("BGM 1").GetComponent<AudioSource>();
        SFX = GameObject.Find("SFX").GetComponent<AudioSource>();
        altarWheel = GameObject.Find("Wheel").GetComponent<altarController>();

        chord = new AudioSource[6];
        chord[0] = GameObject.Find("Chord 1").GetComponent<AudioSource>();
        chord[1] = GameObject.Find("Chord 2").GetComponent<AudioSource>();
        chord[2] = GameObject.Find("Chord 3").GetComponent<AudioSource>();
        chord[3] = GameObject.Find("Chord 4").GetComponent<AudioSource>();
        chord[4] = GameObject.Find("Chord 5").GetComponent<AudioSource>();
        chord[5] = GameObject.Find("Chord 6").GetComponent<AudioSource>();

        clickable = Resources.Load("Clickable") as GameObject;
        objectsLayer = GameObject.Find("Objects").transform;

        int[] temp = db.getPlayer();
        Debug.Log(temp);

        candlesLit = temp[0];
        chordNotes = temp[1];
        dietiesPleased = temp[2];
        docentUnlocked = (temp[3] == 1);
    }
	
    public void clickedObject(clickableController target)
    {
        if(!busy)
            StartCoroutine(playerAction(target, im.SelectedItem()));
    }

    private IEnumerator playerAction(clickableController targetController, item item)
    {
        busy = true;

        float frameWalk = walkSpeed * Time.deltaTime;

        clickable target = targetController.clickable;

        if (spine.enabled == true)
        {

            Vector3 pos = rt.localPosition;

            //move to target
            Vector3 moveDirection = (target.moveTo - pos).normalized;

            currentAnimation = "idle";

            while (Vector3.Distance(target.moveTo, pos) > frameWalk)
            {
                if (currentAnimation == "idle")
                {
                    spine.AnimationState.SetAnimation(0, "walk", true);
                    currentAnimation = "walk";
                    spine.Skeleton.FlipX = moveDirection.x > 0;
                }

                pos += moveDirection * frameWalk;
                rt.localPosition = pos;
                yield return null;
            }

            rt.localPosition = target.moveTo;
            spine.Skeleton.FlipX = target.facingRight;

        }
        characterAnimation animation;
        clickableAction[] actions;
        string text;

        if (item.name == "Hand")
        {
            animation = target.handAnimation;
            actions = target.handActions;
            text = target.handText;
        }
        else if (item == target.correctItem)
        {
            if (target.consumeItem)
                im.removeItem(item);
            animation = target.correctAnimation;
            actions = target.correctActions;
            text = target.correctText;
        }
        else
        {
            animation = target.wrongAnimation;
            actions = target.wrongActions;
            if (target.overrideWrongText)
                text = target.wrongOverrideText;
            else text = item.useWrongText[Random.Range(0, item.useWrongText.Length)];
        }

        if (spine.enabled == true)
        {
            string nextAnimation;

            switch (animation)
            {
                case characterAnimation.reachHigh: nextAnimation = "interacthighTWEEN"; break;
                case characterAnimation.reachMid: nextAnimation = "interactmidTWEEN"; break;
                case characterAnimation.reachLow: nextAnimation = "interactlowTWEEN"; break;
                default: nextAnimation = "idle"; break;
            }

            if (nextAnimation != "idle")
            {
                spine.AnimationState.SetAnimation(0, nextAnimation, false);
                spine.AnimationState.AddAnimation(0, "idle", true, .5f);
                yield return new WaitForSeconds(.5f);
            }
            else if (currentAnimation != "idle")
                spine.AnimationState.SetAnimation(0, "idle", true);


            currentAnimation = "idle";
        }

        busy = false;

        if (text != "")
            textBox.displayText(text);

        foreach (clickableAction a in actions)
        {
            switch(a.action)
            {
                case action.removeThis: roomController.removeFromRoom(targetController.gameObject); break;
                case action.addClickable:
                    roomController.addToRoom(a.clickable);
                    break;
                case action.addItemToInventory:
                    im.addItem(a.item);
                    if (item.name == "Sandal")
                    {
                        SFX.PlayOneShot((AudioClip)Resources.Load("SFX/flying"));
                        spine.AnimationState.SetAnimation(0, "fly", true);

                        float startTime = Time.time;

                        Vector3 pos = rt.localPosition;

                        float startY = pos.y;
                        float targetY = 250;

                        if (target.name == "Item Anansi Piece")
                            targetY = 300;
                        if (target.name == "Item Mirror")
                            targetY = 250;

                        while (Time.time - startTime < 1f)
                        {
                            pos.y = startY + (Time.time - startTime) * (targetY - startY);
                            rt.localPosition = pos;
                            yield return null;
                        }

                        spine.AnimationState.SetAnimation(0, "interacthighTWEEN", false);
                        spine.AnimationState.AddAnimation(0, "fly", true, 0.5f);
                        yield return new WaitForSeconds(0.5f);

                        targetController.GetComponent<Image>().enabled = false;
                        SFX.PlayOneShot((AudioClip)Resources.Load("SFX/pickuptune"));

                        startTime = Time.time;
                        while (Time.time - startTime < 1f)
                        {
                            pos.y = targetY - (Time.time - startTime) * (targetY - startY);
                            rt.localPosition = pos;
                            yield return null;
                        }

                        spine.AnimationState.AddAnimation(0, "idle", true, 0.05f);

                    }
                    else SFX.PlayOneShot((AudioClip)Resources.Load("SFX/pickuptune"));
                    break;
                case action.codeAction:
                    SpecialAction(target, item);
                    break;
                default: break;
            }
        }
    }

    private void SpecialAction(clickable target, item item)
    {
        if (target.name == "Door Desk")
            roomController.moveToRoom("Room Desk Closeup");

        if (target.name == "Door Altar")
            roomController.moveToRoom("Room Altar Closeup");

        if (target.name == "Door Back Arrow")
            roomController.moveToRoom("Room Main Temple");

        if (target.name == "Door Docent")
        {
            if (docentUnlocked)
            {
                SFX.PlayOneShot((AudioClip)Resources.Load("SFX/open_withcreak"));
                if (im.haveDimmedOrb())
                    roomController.moveToRoom("Room Docent Room");
                else
                {
                    StartCoroutine(roomController.darkDocentRoom(item.name == "Orb"));
                }
            }
            else textBox.displayText("The door is locked.");
        }

        if (target.name == "Door Keypad")
        {
            if (docentUnlocked)
                textBox.displayText("The door is already unlocked.");
            else roomController.moveToRoom("Room Keypad Closeup");

        }

        if (target.name == "Door Main Temple")
        {
            roomController.moveToRoom("Room Main Temple");
            SFX.PlayOneShot((AudioClip) Resources.Load("SFX/open_withcreak"));
        }

        if (target.name == "Door Baldr Ending")
        {
            BGM.volume = 1;
            ending(Diety.Baldr);
        }

            //keypad code
        if (target.name == "1" || target.name == "2" || target.name == "3" || target.name == "4" || target.name == "5" 
            || target.name == "6" || target.name == "7" || target.name == "8" || target.name == "9" || target.name == "0")
        {
            SFX.PlayOneShot((AudioClip)Resources.Load("SFX/beep"));
            passcode += target.name;

            Debug.Log(passcode);
        }

        if (target.name == "clear")
        {
            SFX.PlayOneShot((AudioClip)Resources.Load("SFX/beep"));
            passcode = "";
        }

        if (target.name == "enter")
        {
            if (passcode == "6024")
            {
                SFX.PlayOneShot((AudioClip)Resources.Load("SFX/unlock"));
                docentUnlocked = true;
                roomController.moveToRoom("Room Main Temple");
            }
            else
            {
                SFX.PlayOneShot((AudioClip)Resources.Load("SFX/errorbeep"));
                passcode = "";
            }
        }

        //altar
        if ((target.name == "Candle_Empty 1" || target.name == "Candle_Empty 2" || target.name == "Candle_Empty 3")
            && item.name == "Candle")
        {
            chordNotes++;
            playNote(chordNotes);
        }

        if ((target.name == "Candle_Unlit 1" || target.name == "Candle_Unlit 2" || target.name == "Candle_Unlit 3") 
            && item.name == "Matches")
        {
            candlesLit++;
            chordNotes++;
            Debug.Log(candlesLit + " candles lit");
            playNote(chordNotes);
        }

        //wheel
        if (target.name == "Wheel")
        {
            if (item.name.Contains("Piece") == true)
            {
                altarWheel.addDiety(item);
                im.removeItem(item);
                SFX.PlayOneShot((AudioClip)Resources.Load("SFX/stoneclick"));
            }
            else
            {
                altarWheel.rotateWheel();
                SFX.PlayOneShot((AudioClip)Resources.Load("SFX/GrindingStone"));
            }
        }

        //make sacrifices
        {
            if (target.name == "Altar Top")
            {
                if (candlesLit == 0)
                {
                    textBox.displayText("I should light some candles.");
                }
                else if (candlesLit < 3)
                {
                    textBox.displayText("I need more candles.");
                }
                else
                {
                    Diety diety = altarWheel.currentSelection();
                    if (diety == Diety.None)
                    {
                        textBox.displayText("I need to choose to whom I will sacrifice on the wheel.");
                    }
                    else
                    {
                        bool sacrifice = false;
                        if (diety == Diety.Cuchulain && item.name == "Fast Food")
                        {
                            sacrifice = true;
                            roomController.addToRoom(altarWheel.belt);
                        }
                        if (diety == Diety.Anansi && item.name == "Pot of Wisdom")
                        {
                            sacrifice = true;
                            roomController.addToRoom(altarWheel.scroll);
                        }
                        if (diety == Diety.Nephthys && item.name == "Cheetos")
                        {
                            sacrifice = true;
                            roomController.addToRoom(altarWheel.lion);
                        }
                        if (diety == Diety.Amaterasu && item.name == "Mirror")
                        {
                            sacrifice = true;
                            roomController.addToRoom(altarWheel.orb);
                        }
                        if (diety == Diety.Mercury && item.name == "Lockpicks")
                        {
                            sacrifice = true;
                            roomController.addToRoom(altarWheel.sandal);
                        }
                        if (diety == Diety.Sekhmet && item.name == "Wine")
                        {
                            sacrifice = true;
                            roomController.addToRoom(altarWheel.knife);
                        }
                        if (diety == Diety.MonkeyKing && item.name == "Staff")
                        {
                            sacrifice = true;
                            roomController.addToRoom(altarWheel.hair);
                        }
                        if (diety == Diety.Cthulhu && item.name == "Bloody Knife")
                        {
                            db.saveGame();
                            altarWheel.pleaseDiety();
                            db.CthulhuEnding = true;
                            GameObject flame = (GameObject)Instantiate(Resources.Load("Flame"), roomController.gameObject.transform);
                            flame.GetComponent<RectTransform>().localPosition = new Vector3(620, 80);
                            BGM.clip = Resources.Load<AudioClip>("BGM/CthuDidItRight");
                            BGM.loop = false;
                            BGM.Play();
                            im.removeItem(item);
                            StartCoroutine(CthulhuEnding());
                            return;
                        }
                        if (diety == Diety.GummyBear && item.name == "Candy")
                        {
                            db.saveGame();
                            altarWheel.pleaseDiety();
                            db.GummyBearEnding = true;
                            GameObject flame = (GameObject)Instantiate(Resources.Load("Flame"), roomController.gameObject.transform);
                            flame.GetComponent<RectTransform>().localPosition = new Vector3(620, 80);
                            BGM.clip = Resources.Load<AudioClip>("BGM/GumDidItRight");
                            BGM.loop = false;
                            BGM.Play();
                            im.removeItem(item);
                            StartCoroutine(GummyEnding());
                            return;
                        }
                        if (diety == Diety.Baldr && (item.name != "Hand" && item.name != "Bloody Knife"))
                        {
                            db.saveGame();
                            altarWheel.pleaseDiety();
                            db.BaldrEnding = true;
                            GameObject flame = (GameObject)Instantiate(Resources.Load("Flame"), roomController.gameObject.transform);
                            flame.GetComponent<RectTransform>().localPosition = new Vector3(620, 80);
                            BGM.clip = Resources.Load<AudioClip>("BGM/BaldrDidItRight");
                            BGM.volume = .5f;
                            BGM.loop = false;
                            BGM.Play();
                            im.removeItem(item);
                            StartCoroutine(BaldrEnding());
                            return;
                        }

                        if (sacrifice)
                        {
                            im.removeItem(item);
                            GameObject flame = (GameObject) Instantiate(Resources.Load("Flame"), roomController.gameObject.transform);
                            flame.GetComponent<RectTransform>().localPosition = new Vector3(620, 80);
                            altarWheel.pleaseDiety();

                            dietiesPleased++;

                            if (dietiesPleased <= 5)
                            {
                                AudioClip nextVersion = (AudioClip)Resources.Load("BGM/Wandering-" + dietiesPleased);

                                float time = BGM.time;
                                BGM.clip = nextVersion;
                                BGM.time = time;
                                BGM.loop = true;
                                BGM.Play();
                            }
                        }
                        else textBox.displayText("My sacrifice was not accepted.");

                    }
                }
            }
        }
    }

    private IEnumerator CthulhuEnding()
    {
        busy = true;
        yield return new WaitForSeconds(2f);
        RectTransform rt = GameObject.Find("Background").GetComponent<RectTransform>();
        float scale = GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
        Image fader = GameObject.Find("Fader").GetComponent<Image>();
        Color fadeColor = Color.black;
        fadeColor.a = 0;
        fader.enabled = true;

        float startTime = Time.time;
        float time = 0;
        SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/sfx_templecollapse"));
        while (time < 5f)
        {
            if((time >= 1f && time - Time.deltaTime < 1f) ||
                (time >= 2f && time - Time.deltaTime < 2f) ||
                (time >= 3f && time - Time.deltaTime < 3f) ||
                (time >= 4f && time - Time.deltaTime < 4f))
                SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/sfx_templecollapse"));
            if (time > 3f)
            {
                fadeColor.a = Mathf.Clamp(time - 3f, 0f, 1f);
                fader.color = fadeColor;
            }
            rt.localScale = new Vector3 (1 + time / 100, 1 + time / 100);
            rt.position = new Vector3(Random.Range(-1280 * time / 100,0f), 128 + Random.Range(-672 * time / 100,0f)) * scale;
            yield return null;
            time = Time.time - startTime;
        }
        db.ending = Diety.Cthulhu;
        SceneManager.LoadScene("ending");
        yield return 0;
    }

    private IEnumerator GummyEnding()
    {
        busy = true;
        yield return new WaitForSeconds(2f);

        Image[] gummy = new Image[10];
        for(int x = 0; x < 10; x++)
        gummy[x] = GameObject.Find("Slot " + x).GetComponent<Image>();

        float startTime = Time.time;
        float time = 0;
        SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/gummy"));
        while (time < 2f)
        {
            gummy[0].color = new Color(1, 1 - time / 4, 1 - time / 4);
            yield return null;
            time = Time.time - startTime;
        }
        yield return new WaitForSeconds(.5f);

        startTime = Time.time;
        time = 0;
        gummy[0] = GameObject.Find("Outlines").GetComponent<Image>();
        SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/gummy"));
        while (time < 2f)
        {
            for(int x = 0;x < 10; x++)
                gummy[x].color = new Color(1, 1 - time / 4, 1 - time / 4);
            yield return null;
            time = Time.time - startTime;
        }
        yield return new WaitForSeconds(.5f);

        startTime = Time.time;
        time = 0;
        Image bg = GameObject.Find("Background").GetComponent<Image>();
        SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/gummy"));
        while (time < 2f)
        {
            bg.color = new Color(1, 1 - time / 4, 1 - time / 4);
            yield return null;
            time = Time.time - startTime;
        }
        yield return new WaitForSeconds(.5f);

        Image fader = GameObject.Find("Fader").GetComponent<Image>();
        fader.enabled = true;
        Color fadeColor = new Color(1, .5f, .5f);
        startTime = Time.time;
        time = 0;
        SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/gummy"));
        while (time < 2f)
        {
            fadeColor.a = time / 2;
            fader.color = fadeColor;
            yield return null;
            time = Time.time - startTime;
        }
        yield return new WaitForSeconds(.5f);

        db.ending = Diety.GummyBear;
        SceneManager.LoadScene("ending");

        yield return 0;
    }

    private IEnumerator BaldrEnding()
    {
        busy = true;
        yield return new WaitForSeconds(2f);
        SFX.PlayOneShot(Resources.Load<AudioClip>("SFX/big door"));
        yield return new WaitForSeconds(2f);
        roomController.moveToRoom("Room Baldr Ending");
        busy = false;
        yield return 0;
    }

    private void ending(Diety diety)
    {

        db.ending = diety;
        //fade out
        roomController.moveToRoom("Ending");
    }

    private void playNote(int notes)
    {
        for (int x = 0; x < notes; x++)
        {
            chord[x].Stop();
            chord[x].time = 0;
            chord[x].Play();
        }
    }

    public void save()
    {
        db.savePlayer(candlesLit, chordNotes, dietiesPleased, docentUnlocked);
    }
}
