using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    GameObject SeciliObje;
    GameObject SeciliStand;
    Cember _Cember;
    public bool HareketVar;    
    public int HedefStandSayisi;
    int TamamlananStandSayisi;

    public AudioSource[] Sesler;
    public TextMeshProUGUI LevelAd;
    public GameObject BolumPaneli;

    private void Start()
    {
        LevelAd.text = "LEVEL : " + SceneManager.GetActiveScene().buildIndex;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                if (hit.collider != null && hit.collider.CompareTag("Stand"))
                {
                    // Tıklanan standı alıyoruz
                    Stand _Stand = hit.collider.GetComponent<Stand>();

                    // 1. Durum: Elimizde seçili bir halka VARSA (Yerleştirme işlemi)
                    if (SeciliObje != null && SeciliStand != hit.collider.gameObject)
                    {
                        // HATA BURADAYDI: "4" yerine "_Stand.StandKapasitesi" yazdık.
                        // Ayrıca listenin boş olup olmadığını kontrol ediyoruz.
                        if (_Stand._Cemberler.Count < _Stand.StandKapasitesi && _Stand._Cemberler.Count != 0)
                        {
                            // Renkler aynı mı?
                            if (_Cember.Renk == _Stand._Cemberler[^1].GetComponent<Cember>().Renk)
                            {
                                SeciliStand.GetComponent<Stand>().SoketDegistirmeIslemleri(SeciliObje);
                                _Cember.HareketEt("PozisyonDegistir", hit.collider.gameObject, _Stand.MusaitSoketiVer(), _Stand.HareketPozisyonu);
                                _Stand.BosOlanSoket++;
                                _Stand._Cemberler.Add(SeciliObje);
                                _Stand.CemberleriKontrolEt();

                                // Seçimi sıfırla
                                SeciliObje = null;
                                SeciliStand = null;
                            }
                            else // Renkler farklıysa geri git
                            {
                                _Cember.HareketEt("SoketeGeriGit");
                                SeciliObje = null;
                                SeciliStand = null;
                            }
                        }
                        // Stand tamamen BOŞSA (Direkt koy)
                        else if (_Stand._Cemberler.Count == 0)
                        {
                            SeciliStand.GetComponent<Stand>().SoketDegistirmeIslemleri(SeciliObje);
                            _Cember.HareketEt("PozisyonDegistir", hit.collider.gameObject, _Stand.MusaitSoketiVer(), _Stand.HareketPozisyonu);
                            _Stand.BosOlanSoket++;
                            _Stand._Cemberler.Add(SeciliObje);
                            _Stand.CemberleriKontrolEt();

                            SeciliObje = null;
                            SeciliStand = null;
                        }
                        else // Stand DOLUYSA (Geri git)
                        {
                            _Cember.HareketEt("SoketeGeriGit");
                            SeciliObje = null;
                            SeciliStand = null;
                        }
                    }
                    // Aynı standa tıkladıysak seçimi iptal et
                    else if (SeciliStand == hit.collider.gameObject)
                    {
                        _Cember.HareketEt("SoketeGeriGit");
                        SeciliObje = null;
                        SeciliStand = null;
                    }
                    // 2. Durum: Elimizde seçili halka YOKSA (Seçme işlemi)
                    else
                    {
                        // Standın en üstündeki objeyi almaya çalışıyoruz
                        GameObject usttekiObje = _Stand.EnUsttekiCemberiVer();

                        // EKSTRA GÜVENLİK: Eğer stand boşsa 'null' döner, kontrol etmezsek oyun çöker.
                        if (usttekiObje != null)
                        {
                            SeciliObje = usttekiObje;
                            _Cember = SeciliObje.GetComponent<Cember>();
                            HareketVar = true;

                            if (_Cember.HareketEdebilirmi)
                            {
                                _Cember.HareketEt("Secim", null, null, _Cember._AitOlduguStand.GetComponent<Stand>().HareketPozisyonu);
                                SeciliStand = _Cember._AitOlduguStand;
                            }
                        }
                    }
                }
            }
        }
    }
    public void StandTamamlandi()
    {
        TamamlananStandSayisi++;
        if (TamamlananStandSayisi == HedefStandSayisi)
            Kazandin(); 
    }
    public void SesOynat(int Index)
    {
        Sesler[Index].Play();
    }

    void Kazandin()
    {
        BolumPaneli.SetActive(true);
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        Time.timeScale = 0;       
    }

    public void Butonlarinislemleri(string Deger)
    {
        switch (Deger)
        {            
            case "Tekrar":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                break;
            case "SonrakiLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Time.timeScale = 1;
                break;
            case "Ayarlar":
                // ayarlar paneli yapılabilir. sana bırakıyorum
                break;

            case "cikis":
                Application.Quit(); // emin msin paneli ypaıalbilir. Run controlde yaptık.
                break;
        }
    }
}
