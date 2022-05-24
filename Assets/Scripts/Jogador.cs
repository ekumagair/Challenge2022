using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Jogador : MonoBehaviour
{
    public GameObject splashEspada;
    public GameObject splashMachado;
    public GameObject splashGiratorio;
    public GameObject splashGiratorio2;
    public static int armaEquipada;
    public static float armaDelay = 0.0f;
    public static int inimigosMortosHabilidade = 0;
    public static int inimigosMortosHabilidadeObjetivo = 15;
    public bool[] armasDisponiveis;
    public int[] armasQuantidade;
    public GameObject[] armasModelos;

    // Fazendo ataque especial
    public static bool girando = false;

    // Inventário
    public Text[] armasQuantidadeTexto;
    public Image inventarioUnicoFundo;
    public Sprite[] inventarioUnicoSprites;
    public Image[] inventarioIcones;
    public Image hudAtaqueEspecial;
    Color32 hudAtaqueEspecialCor = new Color(255, 255, 255, 255);
    public Text textAtaqueEspecial;

    // Áudio
    public GameObject audioSource2D;
    public AudioClip clipUsarPocao;
    public AudioClip[] clipMochila;

    Invector.vMelee.vMeleeManager arma;
    Invector.vHealthController vida;
    Invector.vCharacterController.vThirdPersonMotor motor;
    Animator animator;
    CameraShake cameraShaker;
    public Invector.vCamera.vThirdPersonCamera cameraThirdPerson;

    private void Start()
    {
        arma = GetComponent<Invector.vMelee.vMeleeManager>();
        vida = GetComponent<Invector.vHealthController>();
        motor = GetComponent<Invector.vCharacterController.vThirdPersonMotor>();
        animator = GetComponent<Animator>();
        cameraShaker = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();
        armaDelay = 0.0f;
        inimigosMortosHabilidade = 0;
        inimigosMortosHabilidadeObjetivo = 15;
        vida.isImmortal = false;
        girando = false;
        StaticClass.clicouEmBotao = false;

        AudioListener.volume = StaticClass.volumeGlobal;

        //Debug.Log("Sensibilidade " + cameraThirdPerson.currentState.xMouseSensitivity);
    }

    private void Update()
    {
        cameraThirdPerson.currentState.xMouseSensitivity = StaticClass.sensibilidadeMouse;
        cameraThirdPerson.currentState.yMouseSensitivity = StaticClass.sensibilidadeMouse;

        if (StaticClass.estadoDeJogo == 0)
        {
            if (armaDelay == 0 && girando == false)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) && armasDisponiveis[0] == true)
                {
                    armaEquipada = 0;
                    Jogador.armaDelay = 0.4f;
                    CriarObjetoDeSom(audioSource2D, clipMochila[Random.Range(0, clipMochila.Length)]);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) && armasDisponiveis[1] == true)
                {
                    armaEquipada = 1;
                    Jogador.armaDelay = 0.8f;
                    CriarObjetoDeSom(audioSource2D, clipMochila[Random.Range(0, clipMochila.Length)]);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) && armasDisponiveis[2] == true)
                {
                    armaEquipada = 2;
                    Jogador.armaDelay = 0.2f;
                    CriarObjetoDeSom(audioSource2D, clipMochila[Random.Range(0, clipMochila.Length)]);
                }
                if(Input.GetKeyDown(KeyCode.F) && inimigosMortosHabilidade >= inimigosMortosHabilidadeObjetivo && motor.currentStamina >= 100 && (armaEquipada == 0 || armaEquipada == 1))
                {
                    StartCoroutine(AtaqueGiratorio());
                }
                if(Input.GetKeyDown(KeyCode.P) && StaticClass.debug)
                {
                    inimigosMortosHabilidade += 10;
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
                    //armasEspacos[i].color = new Color32(150, 150, 150, 200);
                }
                else
                {
                    if (armasModelos[i] != null)
                    {
                        armasModelos[i].SetActive(true);
                    }
                    //armasEspacos[i].color = new Color32(255, 255, 0, 200);
                }

                if(armasQuantidadeTexto != null && i == 2)
                {
                    armasQuantidadeTexto[i].text = armasQuantidade[i].ToString();
                }

                if(armasQuantidade[i] > 0)
                {
                    armasDisponiveis[i] = true;
                    inventarioIcones[i].enabled = true;
                }
                else
                {
                    armasDisponiveis[i] = false;
                    inventarioIcones[i].enabled = false;
                }
            }

            inventarioUnicoFundo.sprite = inventarioUnicoSprites[armaEquipada];
            animator.SetInteger("ArmaAtual", armaEquipada);

            int corHab;

            if (inimigosMortosHabilidade < inimigosMortosHabilidadeObjetivo)
            {
                corHab = Mathf.RoundToInt((255 / inimigosMortosHabilidadeObjetivo) * inimigosMortosHabilidade);
            }
            else
            {
                corHab = 255;
            }

            hudAtaqueEspecialCor = new Color32((byte) corHab, (byte) corHab, (byte) corHab, 255);
            hudAtaqueEspecial.color = hudAtaqueEspecialCor;

            if (armaEquipada == 2 && Input.GetMouseButton(0))
            {
                inventarioUnicoFundo.sprite = inventarioUnicoSprites[3];
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

        textAtaqueEspecial.text = inimigosMortosHabilidade.ToString() + "/" + inimigosMortosHabilidadeObjetivo.ToString();
        //textAtaqueEspecial.text = ((255 / inimigosMortosHabilidadeObjetivo) * inimigosMortosHabilidade).ToString();
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

    public void HitShake()
    {
        cameraShaker.ShakeCamera(0.3f, 0.04f);
    }

    public void RecoilHitShake()
    {
        cameraShaker.ShakeCamera(0.5f, 0.05f);
    }

    public void ItemDeCura(int slot, int cura)
    {
        if (vida.currentHealth < vida.maxHealth)
        {
            Debug.Log("Curar");
            vida.AddHealth(cura);
            armasQuantidade[slot]--;

            if(clipUsarPocao != null)
            {
                CriarObjetoDeSom(audioSource2D, clipUsarPocao);
            }
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
        if(other.gameObject.tag == "Coletar" && other.gameObject.GetComponent<ItemColetavel>() != null)
        {
            ItemColetavel itemColetavel = other.gameObject.GetComponent<ItemColetavel>();
            int da = itemColetavel.desbloquearArma;
            int rv = itemColetavel.recuperarVida;

            if (da >= 0)
            {
                //armasDisponiveis[da] = true;
                armasQuantidade[da]++;
            }
            if(rv > 0)
            {
                vida.AddHealth(rv);
            }

            CriarObjetoDeSom(itemColetavel.criarAoSerDestruido, itemColetavel.clipAoSerDestruido);

            Destroy(other.transform.parent.gameObject);
            Destroy(other.gameObject);
        }
    }

    void CriarObjetoDeSom(GameObject audioSource, AudioClip ac)
    {
        var go = Instantiate(audioSource, transform.position, transform.rotation);

        if (go.GetComponent<AudioSource>() != null)
        {
            go.GetComponent<AudioSource>().clip = ac;
            go.GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator AtaqueGiratorio()
    {
        girando = true;
        vida.isImmortal = true;
        Jogador.armaDelay = 1.9f;

        hudAtaqueEspecial.GetComponent<Animator>().Play("AtaqueEspecialHUDApertar");
        animator.Play("MoveAttack1");

        yield return new WaitForSeconds(0.25f);

        cameraShaker.ShakeCamera(2f, 0.04f);

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.25f);

            if (armaEquipada == 0)
            {
                Instantiate(splashGiratorio, transform.position, transform.rotation);
            }
            else if(armaEquipada == 1)
            {
                Instantiate(splashGiratorio2, transform.position, transform.rotation);
            }

            motor.currentStamina *= 0.5f;
        }

        Jogador.inimigosMortosHabilidade -= inimigosMortosHabilidadeObjetivo;
        vida.isImmortal = false;
        girando = false;
    }
}
