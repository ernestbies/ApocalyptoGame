using UnityEngine;
using UnityEngine.UI;

public class Messages : MonoBehaviour
{
    public Text messageText;
    public GameObject controlText;
    string[] messages;
    int messageLife;
    float timer;
    int numberMessage;

    bool isVisible;

    void Start()
    {
        controlText.SetActive(true);
        messages = new string[10];
        numberMessage = 0;
        messageLife = 10;
        isVisible = true;
        messages[0] = "Podpowiedzi: Pokonuj przeciwników, zbieraj ogniwa.\nZbierz wszystkie ogniwa celem wysadzenia statku nieprzyjaciela.\nNa planszy rozmieszczone są pojemniki z amunicją oraz apteczki.\nPierwsze z nich znajdziesz w pobliżu pierwszego przeciwnika.";
        messages[1] = "Znalazłeś ogniwo!\nPodążaj główną drogą, następnie skręć w prawo.\nPo prawej stronie przy kaplicy znajdziesz kolejne ogniwo!";
        messages[2] = "Znalazłeś ogniwo!\nTeraz już nie będzie tak łatwo!\nNa cmentarzu jeden z obcych ma przy sobie ogniwo.\nZdobądź je!";
        messages[3] = "Gratulacje! Zostało ostatnie ogniwo!\nPo wyjściu z cmentarza skręć w prawo.\nPokonaj wszystkich przeciwników.\nOstatnie ogniwo posiada Boss przy Ratuszu!";
        messages[4] = "Boss pokonany! Wszystkie ogniwa odnalezione!\nOdnieś ogniwa do generatora celem\nwysadzenia statku nieprzyjaciela!\nGenerator znajduje się przy Ratuszu, nad którym jest statek kosmiczny.";
        messages[9] = "Brakuje Ci ogniw!\nZnajdź je wszystkie!\nPodpowiedzi: przycisk M";
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isVisible){
            messageText.text = messages[numberMessage];
            timer += Time.deltaTime;
            if (timer > messageLife){
                isVisible = false;
                controlText.SetActive(false);
                messageText.text = "";
                timer = 0;
            }
        }
        
        if(Input.GetKeyDown(KeyCode.M))
        {
            isVisible = true;
            controlText.SetActive(true);
            Debug.Log("Podpowiedź w grze");            
        } 
    }

    public void nextNumber(int number){
        numberMessage = number;
        isVisible = true;
    }
}
