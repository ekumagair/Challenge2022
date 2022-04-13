using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Jogador : MonoBehaviour
{
    public GameObject splashEspada;
    public GameObject splashMachado;
    public static int armaEquipada;
    public static float armaDelay = 0.0f;
    public bool[] armasDisponiveis;
    public int[] armasQuantidade;
    public GameObject[] armasModelos;

    public Image[] armasEspacos;
    public Text[] armasQuantidadeTexto;

    Invector.vMelee.vMeleeManager arma;
    Invector.vHealthController vida;
    public Invector.vCamera.vThirdPersonCamera cameraThirdPerson;

    private void Start()
    {
        arma = GetComponent<Invector.vMelee.vMeleeManager>();
        vida = GetComponent<Invector.vHealthController>();
        armaDelay = 0.0f;
    }

    private void Update()
    {
        if (StaticClass.estadoDeJogo == 0)
        {
            if (armaDelay == 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) && armasDisponiveis[0] == true)
                {
                    armaEquipada = 0;
                    Jogador.armaDelay = 0.4f;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) && armasDisponiveis[1] == true)
                {
                    armaEquipada = 1;
                    Jogador.armaDelay = 0.8f;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) && armasDisponiveis[2] == true)
                {
                    armaEquipada = 2;
                    Jogador.armaDelay = 0.2f;
                }

                if (armaEquipada == 0)
                {
                    arma.defaultDamage.damageValue = 20;
                    arma.defaultStaminaCost = 5;
                }
                else if (armaEquipada == 1)
                {
                    arma.defaultDamage.damageValue = 45;
                    arma.defaultStaminaCost = 30;
                }
                else if (armaEquipada == 2)
                {
                    arma.defaultDamage.damageValue = 0;
                    arma.defaultStaminaCost = 0;

                    if (Input.GetMouseButtonDown(0))
                    {
                        ItemDeCura(2, 25);
                    }
                }
            }

            if (armasQuantidade[armaEquipada] < 0)
            {
                armasQuantidade[armaEquipada] = 0;
            }
            if (armasQuantidade[armaEquipada] <= 0)
            {
                armaEquipada = 0;
            }

            for (int i = 0; i < armasModelos.Length; i++)
            {
                if (i != armaEquipada)
                {
                    if (armasModelos[i] != null)
                    {
                        armasModelos[i].SetActive(false);
                    }
                    armasEspacos[i].color = new Color32(150, 150, 150, 200);
                }
                else
                {
                    if (armasModelos[i] != null)
                    {
                        armasModelos[i].SetActive(true);
                    }
                    armasEspacos[i].color = new Color32(255, 255, 0, 200);
                }

                if(armasQuantidadeTexto != null && i == 2)
                {
                    armasQuantidadeTexto[i].text = armasQuantidade[i].ToString();
                }

                if(armasQuantidade[i] > 0)
                {
                    armasDisponiveis[i] = true;
                }
                else
                {
                    armasDisponiveis[i] = false;
                }
            }

            if(armaDelay > 0)
            {
                armaDelay -= Time.deltaTime;
            }
            if (armaDelay < 0)
            {
                armaDelay = 0;
            }
        }
    }

    public void CriarSplashEspada()
    {
        if (armaEquipada == 0)
        {
            Instantiate(splashEspada, transform.position + (transform.forward * 1.5f), transform.rotation);
        }
        else if (armaEquipada == 1)
        {
            Instantiate(splashMachado, transform.position + (transform.forward * 1.75f), transform.rotation);
        }
    }

    public void ItemDeCura(int slot, int cura)
    {
        if (vida.currentHealth < vida.maxHealth)
        {
            Debug.Log("Curar");
            vida.AddHealth(cura);
            armasQuantidade[slot]--;
        }
        else
        {
            Debug.Log("Jogador já tem vida máxima");
        }
    }

    public void DestruirCamera()
    {
        Destroy(cameraThirdPerson);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Coletar")
        {
            int da = other.gameObject.GetComponent<ItemColetavel>().desbloquearArma;
            int rv = other.gameObject.GetComponent<ItemColetavel>().recuperarVida;

            if (da >= 0)
            {
                //armasDisponiveis[da] = true;
                armasQuantidade[da]++;
            }
            if(rv > 0)
            {
                vida.AddHealth(rv);
            }
            
            Destroy(other.transform.parent.gameObject);
            Destroy(other.gameObject);
        }
    }
}
