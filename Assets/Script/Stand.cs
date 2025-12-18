using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    public GameObject HareketPozisyonu;
    public GameObject[] Soketler;
    public int BosOlanSoket;
    public List<GameObject> _Cemberler = new();
    [SerializeField] private GameManager _GameManager;

    int CemberTamamlanmaSayisi;
    public int StandKapasitesi = 4;
    public GameObject EnUsttekiCemberiVer()
    {
        // Eğer listede en az 1 tane çember varsa sonuncuyu ver
        if (_Cemberler.Count > 0)
        {
            return _Cemberler[^1];
        }
        // Eğer liste boşsa 'null' (boş) döndür
        else
        {
            return null;
        }
    }
    public GameObject MusaitSoketiVer()
    {
        return Soketler[BosOlanSoket];
    }
    public void SoketDegistirmeIslemleri(GameObject SilinecekObje)
    {
        _Cemberler.Remove(SilinecekObje);
        
        if (_Cemberler.Count!=0)
        {
            BosOlanSoket--;
            _Cemberler[^1].GetComponent<Cember>().HareketEdebilirmi = true;
        }
        else
        {
            BosOlanSoket = 0;
        }
    }
    public void CemberleriKontrolEt()
    {
        // 1. Kapasite kontrolü (Yaptığın gibi doğru)
        if (_Cemberler.Count == StandKapasitesi)
        {
            // 2. ÖNEMLİ DEĞİŞİKLİK: 
            // Sayacı burada, yerel olarak tanımlıyoruz. 
            // Böylece her kontrolde otomatik olarak 0'dan başlar.
            int AyniRenkSayisi = 0;

            string Renk = _Cemberler[0].GetComponent<Cember>().Renk;

            foreach (var item in _Cemberler)
            {
                if (Renk == item.GetComponent<Cember>().Renk)
                    AyniRenkSayisi++;
            }

            // 3. Kontrol
            if (AyniRenkSayisi == StandKapasitesi)
            {
                if (_GameManager != null)
                    _GameManager.StandTamamlandi();

                TamamlanmisStandIslemleri();
            }
            // BURAYA DİKKAT: "else { sayac = 0 }" yazmana gerek kalmadı.
            // Çünkü "int AyniRenkSayisi" sadece bu parantezler içinde yaşar 
            // ve fonksiyon bitince silinir.
        }
    }
    void TamamlanmisStandIslemleri()
    {
        foreach (var item in _Cemberler)
        {
            item.GetComponent<Cember>().HareketEdebilirmi = false;
            Color32 color = item.GetComponent<MeshRenderer>().material.GetColor("_Color");
            color.a = 150;
            item.GetComponent<MeshRenderer>().material.SetColor("_Color",color);
            gameObject.tag = "TamamlanmisStand";
        }
    }

}
