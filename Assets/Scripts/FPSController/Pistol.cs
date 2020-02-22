using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Pistol : MonoBehaviour
{
    public float pistolDamage;
    public float pistolRange;
    public AudioClip shotSound;
    public AudioClip reloadSound;
    public AudioClip emptyGunSound;
    public AudioClip collectSound;
    public GameObject bloodSplat;    
    public Text ammoText;

    public int ammoAmount;
    public int ammoClipSize;

    public GameObject bulletHole;

    GameObject explosionInstantiated;       

    public int ammoLeft;
    int ammoClipLeft;

    bool isShot;
    bool isReloading;

    private AudioSource source;
    

    void Awake()
    {
        source = GetComponent<AudioSource>();
        ammoLeft = ammoAmount;
        ammoClipLeft = ammoClipSize;        
    }

    void Update()
    {        
        ammoText.text = ammoClipLeft + " / " + ammoLeft;

        if (Input.GetButtonDown("Fire1") && isReloading == false)
            isShot = true;
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            Reload();
        }
    }

    void FixedUpdate()
    {
        // Obliczenie losowego przesunięcia w obrębie okręgu
        // Promień okręgu zależny jest od aktualnej wartości zmiennej 'spread'
        Vector2 bulletOffset = Random.insideUnitCircle * DynamicCrosshair.spread;
        // Tworzymy promień, który wychodzi od naszej kamery do środka ekranu wraz z przesunięciem
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/ 2 + bulletOffset.x, Screen.height / 2 + bulletOffset.y, 0));
        RaycastHit hit;
        if (isShot == true && ammoClipLeft > 0 && isReloading == false)
        {
            isShot = false;
            ammoClipLeft--;            
             source.PlayOneShot(shotSound);
            
            //Jesli po wcisnieciu przycisku 'Fire1' promien wszedl w kolizje z jakims obiektem
            //Wykonuje ponizsze instrukcje
            if (Physics.Raycast(ray, out hit, pistolRange))
            {
                Debug.Log("Wszedlem w kolizje z " + hit.collider.gameObject.name);
                if (hit.transform.CompareTag("Enemy"))
                {
                    // Wyslanie informacji do trafionego obiektu, ze go trafilismy
                    // Trafiony obiekt powinien u siebie odpalic funkcje pistolHit z parametrem pistolDamage
                    GameObject bs = Instantiate(bloodSplat, hit.point, Quaternion.identity);
                    hit.collider.gameObject.SendMessage("PistolHit", pistolDamage, SendMessageOptions.DontRequireReceiver);                
                    if (hit.collider.gameObject.GetComponent<EnemyStates>().currentState == hit.collider.gameObject.GetComponent<EnemyStates>().patrolState){
                        hit.collider.gameObject.SendMessage("HiddenShot", transform.position, SendMessageOptions.DontRequireReceiver);    
                    }  
                    Destroy(bs, 5);
                } else {
                //Tworzymy obiekt dziury po kuli na obiekcie
                    Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)).transform.parent = hit.collider.gameObject.transform;
                }
            }
        }
        else if (isShot == true && ammoClipLeft <= 0 && isReloading == false)
        {
            //Gdy strzelimy, lecz nie mamy już amunicji, przeładowujemy broń
            isShot = false;
            Reload();
        }
    }

    // Funkcja odpowiedzialna za przeładowywanie broni
    void Reload()
    {
        //Obliczanie ile pocisków powinniśmy przeładować
        int bulletsToReload = ammoClipSize - ammoClipLeft;
        if (ammoLeft >= bulletsToReload)
        {
            StartCoroutine("ReloadWeapon");
            ammoLeft -= bulletsToReload;
            ammoClipLeft = ammoClipSize;
        }
        else if (ammoLeft < bulletsToReload && ammoLeft > 0)
        {
            StartCoroutine("ReloadWeapon");
            ammoClipLeft += ammoLeft;
            ammoLeft = 0;
        }
        else if (ammoLeft <= 0)
        {
            source.PlayOneShot(emptyGunSound);
        }
    }

    // Funkcja odtwarza dzwiek przeladowywania
    IEnumerator ReloadWeapon()
    {
        isReloading = true;
        source.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(2);
        isReloading = false;
    }
        
}
