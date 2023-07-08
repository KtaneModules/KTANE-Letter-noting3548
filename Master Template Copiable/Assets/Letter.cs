using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class Letter : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;


    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    public KMSelectable button;

    public TextScript fontLetter;


    void Awake() {
        ModuleId = ModuleIdCounter++;
        GetComponent<KMBombModule>().OnActivate += Activate;


        button.OnInteract += delegate () { ButtonPress(); return false; };

    }
    const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string ChangingAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
     string LetterList = "";

    internal void ChangeLetterInternal(int i) {


        LetterList = LetterList + ChangingAlphabet[i % (ChangingAlphabet.Length)];
        ChangingAlphabet = ChangingAlphabet.Remove(i % (ChangingAlphabet.Length), 1);
        Debug.Log("LetterList is now " + LetterList);
        Debug.Log("ChangingAlphabet is now " + ChangingAlphabet);

    }
    private int Checker;

    private bool IsBePressed;
    private bool x = false;
    private bool ButtonPressed = false;
    private bool WaitedSeconds;
    private bool Waiting;
    IEnumerator SubmittionPhase() {
        x = false;


        for (int p = 1; p < 26; p++)
        {

            Checker = p;
            x = false;
            Debug.Log("New Stage");
            int RandomTrueOrFalse = Rnd.Range(0, 3);
            if (RandomTrueOrFalse == 1)
            {

                fontLetter.ChangeLetter(LetterList[p]);
                IsBePressed = true;
                Debug.Log("The button should be pressed");


            } else
            {
                IsBePressed = false;
               
                while (x == false)

                {
                    int RandomLetterNumber = Rnd.Range(0, 26);
                    if (RandomLetterNumber != p) { x = true;
                        fontLetter.ChangeLetter(LetterList[RandomLetterNumber]);
                    }
                }
                Debug.Log("The button should not  be pressed");
            }

            WaitedSeconds = false;
            Waiting =true ;
            Debug.Log("Starting the wait Coroutine");
            StartCoroutine(WaitSomeSeconds(p));
           
            yield return new WaitUntil(() => (ButtonPressed ==true)||(WaitedSeconds==true));
            
            if ((IsBePressed != ButtonPressed)&&(Striked!=true)) {
                Debug.LogFormat("[A Letter #{0}] Incorrect on letter {1}", ModuleId, LetterList[p]);
                Debug.Log("INCORRECT");
                Debug.Log("Incorrect on stage" +p);
                Debug.Log("Incorrect on letter"+ LetterList[p]);
                Debug.Log("Should it be pressed: "+IsBePressed);
                Debug.Log("Was it pressed: "+ButtonPressed);
                Debug.Log("INCORRECT");
                Strike();
                Striked = true;
     
                break;
                
            }
           


            Waiting = false;
            ButtonPressed = false;
            WaitedSeconds = false;
            Debug.Log("CORRECT");
        
        }

        if (Striked == false) {
            Solve();
            ModuleSolved = true;
            fontLetter.ChangeLetter(Convert.ToChar("W"));
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            Debug.Log("Module Solved");

        } else { yield return new WaitForSeconds(4); ModuleReset(); };

    }


    IEnumerator WaitSomeSeconds(int nl) {

        Debug.Log("Starting the wait");
        yield return new WaitForSeconds(1.5f);

        Debug.Log("Have waited");
        
        if (nl == Checker)
        {
                WaitedSeconds = true;
                GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        }


    }


    private bool Started = false;
    private bool Striked = false;
    public void ButtonPress()
    {

        button.AddInteractionPunch(1);
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        Debug.Log("Pressed the button");

        if (Started == false)
        {
            ButtonPressed = false;
            WaitedSeconds = false;
            if (Striked != true)
            {
                Debug.Log("Starting Submittion Phase");
                Started = true;
                StartCoroutine("SubmittionPhase");
            }

        }
        else
        {
            if (Striked != true)
            {
                Debug.Log("Making ButtonPressed true");
                ButtonPressed = true;
            }


        }

    }
   void OnDestroy () {
      
   }
   private void ModuleReset()
    {
        Started = false;
        WaitedSeconds = false;
        Striked = false;
        int p = 1;
        Debug.Log("Module Activated");
        Debug.Log(Bomb.GetSerialNumber());
        Debug.Log(Bomb.GetSerialNumberNumbers().First());
        Debug.Log(Bomb.GetSerialNumberLetters().First());
        LetterList = "";
        ChangingAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";


        int FirstLetterNumber = Rnd.Range(1, 27);
        Debug.Log("FirstLetterNumber is now " + FirstLetterNumber);
        int SecondLetterNumber = Alphabet.IndexOf(Bomb.GetSerialNumberLetters().First());
        Debug.Log("SecondLetterNumber is now " + Convert.ToString(SecondLetterNumber + 1));
        int ThirdLetterNumber = Bomb.GetSerialNumberNumbers().First();
        Debug.Log("ThirdLetterNumber is now " + Convert.ToString(ThirdLetterNumber));
        if (ThirdLetterNumber == 0)
        {
            ThirdLetterNumber = 1; Debug.Log("Since First Serial number number was a 0, the third number got changed to 1");
            Debug.Log("ThirdLetterNumber is now " + Convert.ToString(ThirdLetterNumber));
        }






        fontLetter.ChangeLetter(Alphabet[FirstLetterNumber - 1]);
        ChangeLetterInternal(FirstLetterNumber - 1);
        ChangeLetterInternal(SecondLetterNumber);
        ChangeLetterInternal(ThirdLetterNumber - 1);

        for (int h = 0; h < 23; h++)
        {
            Debug.Log("LetterList is now this long: " + Convert.ToString(LetterList.Length));
            Debug.Log("Reference letter is now " + LetterList[LetterList.Length - 3]);
            Debug.Log("Refernece number is now " + Convert.ToString(1 + Alphabet.IndexOf(LetterList[LetterList.Length - 3])));



            ChangeLetterInternal(Alphabet.IndexOf(LetterList[LetterList.Length - 3]));



        }

        Debug.Log("Submit sequence is now " + LetterList);
        Debug.LogFormat("[A Letter #{0}] Submit sequence is now {1}", ModuleId,LetterList);
        Started = false;

    }

    void Activate () {

        

    }














  
    void Start () {
        ModuleReset();
        Started = false;




    }



   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }





}
