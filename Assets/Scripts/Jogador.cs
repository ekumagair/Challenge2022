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
    public GameObject fumacaAtaqueGiratorio;
    public static int armaEquipada;
    public static float armaDelay = 0.0f;
    public static int inimigosMortosHabilidade = 0;
    public static int inimigosMortosHabilidadeObjetivo = 15;
    public bool[] armasDisponiveis;
    public int[] armasQuantidade;
    public GameObject[] armasModelos;
    public ParticleSystem[] armasTrail;

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

    // Lista de itens:
    // 0 = Espada. Equipada por padrão.
    // 1 = Machado.
    // 2 = Poção.

    // Áudio
    public GameObject audioSource2D;
    public AudioClip clipUsarPocao;
    public AudioClip[] clipMochila;
    public AudioClip[] clipSwoosh;

    // Instruções
    public GameObject instrucao;

    // Texto Intro
    public Text textIntro;

    Invector.vMelee.vMeleeManager arma;
    Invector.vHealthController vida;
    Invector.vCharacterController.vThirdPersonMotor motor;
    Animator animator;
    CameraShake cameraShaker;
    Personagem personagemScript;
    public Invector.vCamera.vThirdPersonCamera cameraThirdPerson;

    private void Start()
    {
        arma = GetComponent<Invector.vMelee.vMeleeManager>();
        vida = GetComponent<Invector.vHealthController>();
        motor = GetComponent<Invector.vCharacterController.vThirdPersonMotor>();
        animator = GetComponent<Animator>();
        personagemScript = GetComponent<Personagem>();
        cameraShaker = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();
        armaDelay = 0.0f;
        inimigosMortosHabilidade = 0;
        inimigosMortosHabilidadeObjetivo = 15;
        vida.isImmortal = false;
        girando = false;
        StaticClass.clicouEmBotao = false;

        //StartCoroutine(CriarInstrucao("Teste!", 3f, 0f));

        AudioListener.volume = StaticClass.volumeGlobal;

        //Debug.Log("Sensibilidade " + cameraThirdPerson.currentState.xMouseSensitivity);

        // Instruções
        if(StaticClass.faseAtual == 1)
        {
            StartCoroutine(CriarInstrucao("Use [W], [A], [S], [D] para se mover.", 6f, 0f));
            StartCoroutine(CriarInstrucao("Ataque com o [botão esquerdo do mouse]. Defenda segurando o [botão direito do mouse].", 6f, 6f));
            StartCoroutine(CriarInstrucao("Segure o [Shift] para correr.", 6f, 24f));
            StartCoroutine(CriarInstrucao("Você precisa de energia (barra verde) para realizar essas ações.", 6f, 30f));
            StartCoroutine(CriarInstrucao("Você pode apertar a tecla [Tab] para travar/destravar a mira em um inimigo específico.", 6f, 36f));
        }
        else if (StaticClass.faseAtual == 2)
        {
            StartCoroutine(CriarInstrucao("Use [Q] para rolar.", 6f, 0f));
            StartCoroutine(CriarInstrucao("Aperte [E] para realizar um ataque mais forte.", 6f, 6f));
            StartCoroutine(CriarInstrucao("Troque de arma/item com as teclas [1], [2] e [3].", 6f, 24f));
            StartCoroutine(CriarInstrucao("O machado causa mais dano, mas consome mais energia quando você ataca.", 6f, 30f));
            StartCoroutine(CriarInstrucao("As poções largadas por inimigos te curam quando usadas.", 6f, 36f));
        }
        else if (StaticClass.faseAtual == 3)
        {
            StartCoroutine(CriarInstrucao("Use a [Barra De Espaço] para pular.", 6f, 0f));
            StartCoroutine(CriarInstrucao("Alguns inimigos atiram projéteis. Você pode pular para desviar deles.", 6f, 6f));
            StartCoroutine(CriarInstrucao("Evite ficar parado por muito tempo enquanto inimigos que atacam com projéteis estão presentes.", 6f, 12f));
        }
        else if (StaticClass.faseAtual == 6)
        {
            StartCoroutine(CriarInstrucao("Por quanto tempo você consegue sobreviver?", 6f, 0f));
        }
        else if (StaticClass.faseAtual == 7)
        {
            StartCoroutine(CriarInstrucao("Fase de teste.", 6f, 0f));
        }
        else if (StaticClass.faseAtual > 7)
        {
            StartCoroutine(CriarInstrucao("Fase inválida.", 6f, 0f));
        }

        // Texto de Introdução
        if (StaticClass.faseAtual != 6)
        {
            textIntro.text = "Fase " + StaticClass.faseAtual.ToString();
        }
        else
        {
            textIntro.text = "Fase Infinita";
        }
    }

    private void Update()
    {
        // Mudar sensibilidade do mouse de acordo com o valor da opção do menu.
        cameraThirdPerson.currentState.xMouseSensitivity = StaticClass.sensibilidadeMouse;
        cameraThirdPerson.currentState.yMouseSensitivity = StaticClass.sensibilidadeMouse;

        if (StaticClass.estadoDeJogo == 0)
        {
            if (armaDelay == 0 && girando == false)
            {
                // Escolher a espada
                if (Input.GetKeyDown(KeyCode.Alpha1) && armasDisponiveis[0] == true && armaEquipada != 0)
                {
                    armaEquipada = 0;
                    Jogador.armaDelay = 0.4f;
                    animator.Play("Longs_Equip", 3);
                    CriarObjetoDeSom(audioSource2D, clipMochila[Random.Range(0, clipMochila.Length)]);
                }
                // Escolher o machado
                if (Input.GetKeyDown(KeyCode.Alpha2) && armasDisponiveis[1] == true && armaEquipada != 1)
                {
                    armaEquipada = 1;
                    Jogador.armaDelay = 0.8f;
                    animator.Play("WeaponUnsheath", 3);
                    CriarObjetoDeSom(audioSource2D, clipMochila[Random.Range(0, clipMochila.Length)]);
                }
                // Escolher a poção
                if (Input.GetKeyDown(KeyCode.Alpha3) && armasDisponiveis[2] == true && armaEquipada != 2)
                {
                    armaEquipada = 2;
                    Jogador.armaDelay = 0.2f;
                    animator.Play("WeaponSheath", 3);
                    CriarObjetoDeSom(audioSource2D, clipMochila[Random.Range(0, clipMochila.Length)]);
                }

                // Ataque especial (giratório)
                if(Input.GetKeyDown(KeyCode.F) && inimigosMortosHabilidade >= inimigosMortosHabilidadeObjetivo && motor.currentStamina >= 100 && (armaEquipada == 0 || armaEquipada == 1) && armaDelay < 0.1f)
                {
                    StartCoroutine(AtaqueGiratorio());
                }

                // Trapaça de teste
                if(Input.GetKeyDown(KeyCode.P) && StaticClass.debug)
                {
                    inimigosMortosHabilidade += 10;
                }

                // O custo de energia/stamina é controlado aqui. O dano da arma é controlado nas animações.
                if (armaEquipada == 0)
                {
                    //arma.defaultDamage.damageValue = 20; Aparentemente, mudar o dano da arma deste modo não funciona. O dano precisa ser controlado por um multiplicador nos parâmetros da animação de ataque.
                    arma.defaultStaminaCost = 15;
                }
                else if (armaEquipada == 1)
                {
                    //arma.defaultDamage.damageValue = 45;
                    arma.defaultStaminaCost = 45;
                }
                else if (armaEquipada == 2)
                {
                    //arma.defaultDamage.damageValue = 0;
                    arma.defaultStaminaCost = 0;

                    if (Input.GetMouseButtonDown(0))
                    {
                        ItemDeCura(2, 25);
                    }
                }
            }

            // Não deixar que a quantidade de itens seja negativa.
            if (armasQuantidade[armaEquipada] < 0)
            {
                armasQuantidade[armaEquipada] = 0;
            }
            // Mudar para a espada se outro item acabar.
            if (armasQuantidade[armaEquipada] <= 0)
            {
                armaEquipada = 0;
            }

            // Mudar modelo 3d do item no jogo de acordo com o item equipado.
            for (int i = 0; i < armasModelos.Length; i++)
            {
                if (i != armaEquipada)
                {
                    if (armasModelos[i] != null)
                    {
                        armasModelos[i].SetActive(false);
                    }
                    //armasEspacos[i].color = new Color32(150, 150, 150, 200); Sprite antigo
                }
                else
                {
                    if (armasModelos[i] != null)
                    {
                        armasModelos[i].SetActive(true);
                    }
                    //armasEspacos[i].color = new Color32(255, 255, 0, 200); Sprite antigo
                }

                // Mostrar quantidade de poções na HUD.
                if (armasQuantidadeTexto != null && i == 2)
                {
                    armasQuantidadeTexto[i].text = armasQuantidade[i].ToString();
                }

                // Mostrar ícones dos itens no inventário se eles estão disponíveis.
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

            // Mudar sprite do inventário de acordo com o item equipado.
            inventarioUnicoFundo.sprite = inventarioUnicoSprites[armaEquipada];

            // Mudar parâmetro do animator de acordo com item equipado.
            animator.SetInteger("ArmaAtual", armaEquipada);

            // Mudar cor do ícone da habilidade especial (giratória).
            // Quando nenhum inimigo foi morto, o ícone fica totalmente escuro. Ele fica mais claro à medida que inimigos são derrotados.
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

            // Mudar sprite do inventário quando o jogador usa uma poção de cura.
            if (armaEquipada == 2 && Input.GetMouseButton(0))
            {
                inventarioUnicoFundo.sprite = inventarioUnicoSprites[3];
            }

            // Subtrair o valor da demora da arma. Se esse valor for > 0, o jogador não pode trocar de armas ou usá-las. Ele só pode fazer isso se o valor for = 0.
            // Esse delay é contado em segundos.
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

    // Função executada quando um item é usado.
    public void InputAtaque()
    {
        // Efeito visual do rastro da arma.
        if (armasTrail[armaEquipada] != null)
        {
            armasTrail[armaEquipada].Play();
        }
    }

    // Criar objeto que causa dano em área, de acordo com a arma equipada.
    public void CriarSplash()
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

    // Chacoalhar a câmera quando atingir ou for atingido.
    public void HitShake()
    {
        cameraShaker.ShakeCamera(0.3f, 0.045f);
    }

    public void RecoilHitShake()
    {
        cameraShaker.ShakeCamera(0.5f, 0.05f);
    }

    // Cria partículas de fumaça nos pés do protagonista.
    public void CriarFumaca()
    {
        Instantiate(fumacaAtaqueGiratorio, personagemScript.localPes.transform.position + (transform.up / 4), Quaternion.Euler(Vector3.up));
    }

    // Quando um item de cura é usado.
    // Essa função facilita a criação de vários itens de cura diferentes, se necessário.
    // slot = Qual o valor de identificação do item no inventário. No caso da poção, é 2.
    // cura = Quanto de vida é recuperado.

    public void ItemDeCura(int slot, int cura)
    {
        if (vida.currentHealth < vida.maxHealth)
        {
            if (StaticClass.debug)
            {
                Debug.Log("Curar");
            }

            InputAtaque();
            vida.AddHealth(cura);
            armasQuantidade[slot]--;

            if(clipUsarPocao != null)
            {
                CriarObjetoDeSom(audioSource2D, clipUsarPocao);
            }
        }
        else
        {
            if (StaticClass.debug)
            {
                Debug.Log("Jogador já tem vida máxima");
            }
        }
    }

    public void DestruirCamera()
    {
        // Se refere ao script da câmera. Quando isto é executado, a câmera não pode ser movida até a cena mudar ou ser reiniciada.
        Destroy(cameraThirdPerson);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Quando colide em um item coletável.
        if(other.gameObject.tag == "Coletar" && other.gameObject.GetComponent<ItemColetavel>() != null)
        {
            ItemColetavel itemColetavel = other.gameObject.GetComponent<ItemColetavel>();
            int da = itemColetavel.desbloquearArma;
            int rv = itemColetavel.recuperarVida;

            // da = Qual é este item.
            // rv = Recuperar vida instantaneamente ao tocar. Não é mais usado desde que a poção passou a ser parte do inventário.

            if (da >= 0)
            {
                armasQuantidade[da]++;
            }
            if(rv > 0)
            {
                vida.AddHealth(rv);
            }

            CriarObjetoDeSom(itemColetavel.criarAoSerDestruido, itemColetavel.clipAoSerDestruido);

            // Apaga o item do mundo.
            Destroy(other.transform.parent.gameObject);
            Destroy(other.gameObject);
        }
    }

    void CriarObjetoDeSom(GameObject audioSource, AudioClip ac)
    {
        // Cria um objeto e imediatamente ativa o componente de AudioSource dele.
        var go = Instantiate(audioSource, transform.position, transform.rotation);

        if (go.GetComponent<AudioSource>() != null)
        {
            go.GetComponent<AudioSource>().clip = ac;
            go.GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator AtaqueGiratorio()
    {
        // Jogador fica imortal e não pode usar outros ataques enquanto usa o ataque giratório.
        girando = true;
        vida.isImmortal = true;
        Jogador.armaDelay = 3f;

        // Animações da HUD e do modelo 3d do jogador. A animação do modelo está em "Base Layer > Locomotion > MoveAttack1"
        hudAtaqueEspecial.GetComponent<Animator>().Play("AtaqueEspecialHUDApertar");
        animator.Play("MoveAttack1");

        yield return new WaitForSeconds(0.25f);

        // Chacoalhar a câmera durante o ataque.
        cameraShaker.ShakeCamera(2f, 0.04f);

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.25f);

            // O dano do ataque é afetado pela arma usada.
            if (armaEquipada == 0)
            {
                Instantiate(splashGiratorio, transform.position, transform.rotation);
            }
            else if(armaEquipada == 1)
            {
                Instantiate(splashGiratorio2, transform.position, transform.rotation);
            }

            // Reduz energia do jogador, cria partículas de rastro, faz som, cria partículas de poeira.
            motor.currentStamina *= 0.4f;
            InputAtaque();
            CriarObjetoDeSom(audioSource2D, clipSwoosh[Random.Range(0, clipSwoosh.Length)]);
            CriarFumaca();
        }

        Jogador.inimigosMortosHabilidade -= inimigosMortosHabilidadeObjetivo;
        vida.isImmortal = false;
        girando = false;
    }

    // Criar um texto de instrução que aparece na parte de baixo da tela depois de "delayInicial" segundos e fica à mostra por "esconder" segundos.
    public IEnumerator CriarInstrucao(string texto, float esconder, float delayInicial)
    {
        yield return new WaitForSeconds(delayInicial);

        if (StaticClass.estadoDeJogo == 0)
        {
            var inst = Instantiate(instrucao, GameObject.FindGameObjectWithTag("HUD").transform);
            inst.GetComponent<Text>().text = texto;

            yield return new WaitForSeconds(esconder);

            inst.GetComponent<Animator>().Play("InstruçãoEsconder");

            yield return new WaitForSeconds(1.5f);

            Destroy(inst);
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            