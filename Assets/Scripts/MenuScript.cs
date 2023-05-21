using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Slider sliderMouse;
    public Slider sliderVolume;
    public Slider sliderVolumeHit;
    public AudioSource sliderVolumeHitAS;
    public Slider sliderShakeMult;
    public Text mouseValor;
    public Text volumeValor;
    public Text volumeHitValor;
    public Text shakeMultValor;

    public Text textBuild;
    public Text textCarregando;
    public bool escSair;

    [Header("Preview de fases")]
    public Text previewTxt;
    public Image previewObj;
    public Sprite[] previewSprites;

    void Start()
    {
        if (sliderMouse != null && StaticClass.sensibilidadeMouse != 0f)
        {
            sliderMouse.value = StaticClass.sensibilidadeMouse;
        }

        if (sliderVolume != null)
        {
            sliderVolume.value = StaticClass.volumeGlobal;
        }

        if (sliderVolumeHit != null)
        {
            sliderVolumeHit.value = StaticClass.volumeHitSound;
        }

        if (sliderShakeMult != null)
        {
            sliderShakeMult.value = StaticClass.shakeMult;
        }

        if (textBuild != null)
        {
            if (StaticClass.debug)
            {
                textBuild.text = "Build " + Application.version.ToString();
            }
            else
            {
                textBuild.text = "Versão " + Application.version.ToString();
            }
        }

        if (textCarregando != null)
        {
            textCarregando.enabled = false;
        }

        if (previewTxt != null)
        {
            previewTxt.text = "";
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;

        StaticClass.totalDeInimigos = 0;
        StaticClass.clicouEmBotao = false;
        StaticClass.segundosVivo = 0;

        StaticClass.estadoDeJogo = 0;
        StaticClass.ondasPassadas = 0;
        StaticClass.inimigosMortos = 0;
        StaticClass.inimigosVivos = 0;

        // Carregar jogo salvo.
        StaticClass.Carregar();

        // A fase 1 precisa estar disponível.
        if (StaticClass.faseDesbloqueada < 1)
        {
            StaticClass.faseDesbloqueada = 1;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Se o esc faz sair do jogo ou apenas voltar para o menu de título.
            if (escSair)
            {
                SairDoJogo();
            }
            else
            {
                IrParaTitulo();
            }
        }

        // Texto que representa os valores das opções.
        if (mouseValor != null)
        {
            mouseValor.text = "(" + Mathf.RoundToInt(StaticClass.sensibilidadeMouse).ToString() + ")";
        }
        if (volumeValor != null)
        {
            volumeValor.text = "(" + Mathf.RoundToInt(StaticClass.volumeGlobal * 100f).ToString() + "%)";
        }
        if (volumeHitValor != null)
        {
            volumeHitValor.text = "(" + Mathf.RoundToInt(StaticClass.volumeHitSound * 100f).ToString() + "%)";
        }
        if (shakeMultValor != null)
        {
            shakeMultValor.text = "(" + Mathf.RoundToInt(StaticClass.shakeMult).ToString() + "x)";
        }

        // Trapaças de teste
        if (Input.GetKeyDown(KeyCode.Comma) && StaticClass.debug)
        {
            StaticClass.faseDesbloqueada--;
        }
        if (Input.GetKeyDown(KeyCode.Period) && StaticClass.debug)
        {
            StaticClass.faseDesbloqueada++;
        }
        if(Input.GetKeyDown(KeyCode.Delete) && StaticClass.debug)
        {
            // Apagar dados salvos
            SaveSystem.DeleteSave();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            StaticClass.faseDesbloqueada = 1;
            StaticClass.inimigosMortosRecorde = 0;
        }
    }

    public void IrParaJogo()
    {
        StartCoroutine(IrPara("Gameplay"));
    }

    public void IrParaFase(int fase)
    {
        StaticClass.faseAtual = fase;
        StartCoroutine(IrPara("Gameplay"));
    }

    public void IrParaSelecionarFase()
    {
        StartCoroutine(IrPara("SelecionarFases"));
    }

    public void IrParaTitulo()
    {
        StaticClass.Salvar();
        StartCoroutine(IrPara("Titulo"));
    }

    public void IrParaOpcoes()
    {
        StaticClass.Salvar();
        StartCoroutine(IrPara("Opcoes"));
    }

    public void IrParaControles()
    {
        StartCoroutine(IrPara("Controles"));
    }

    public void IrParaCreditos()
    {
        StartCoroutine(IrPara("Creditos"));
    }

    public void MudarPreview(int n)
    {
        if(previewObj != null && previewTxt != null && StaticClass.faseDesbloqueada > n)
        {
            previewObj.sprite = previewSprites[n];

            switch (n + 1)
            {
                case 1:
                    previewTxt.text = "Fase 1: Derrote todos os Cavaleiros para vencer.";
                    break;
                case 2:
                    previewTxt.text = "Fase 2: Derrote todos os Cavaleiros e Gladiadores para vencer.";
                    break;
                case 3:
                    previewTxt.text = "Fase 3: Derrote todos os Cavaleiros e Samurais para vencer.";
                    break;
                case 4:
                    previewTxt.text = "Fase 4: Derrote todos os Cavaleiros e Ninjas para vencer.";
                    break;
                case 5:
                    previewTxt.text = "Fase 5: Todos os inimigos que você já encontrou estão aqui, junto com variações mais fortes deles e Soldados Romanos.";
                    break;
                case 6:
                    previewTxt.text = "Fase Infinita: Uma fase que só termina quando você é derrotado pelas ondas de inimigos que ficam cada vez mais fortes. Seu recorde de inimigos mortos é: " + StaticClass.inimigosMortosRecorde.ToString() + ".";
                    break;
                default:
                    break;
            }
        }
        else
        {
            previewObj.sprite = previewSprites[0];
            previewTxt.text = "";
        }
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    IEnumerator IrPara(string cena)
    {
        // Se tem o texto de carregamento no layout, mostra ele.
        if (textCarregando != null)
        {
            textCarregando.enabled = true;
        }

        // Se não houver delay, o som dos botões não toca.
        if (StaticClass.clicouEmBotao == false)
        {
            StaticClass.clicouEmBotao = true;
            yield return new WaitForSeconds(0.2f);
            SceneManager.LoadScene(cena);
        }
    }

    // Opções
    public void MudarSensitividadeDoMouse()
    {
        StaticClass.sensibilidadeMouse = sliderMouse.value;
    }

    public void MudarVolumeGlobal()
    {
        StaticClass.volumeGlobal = sliderVolume.value;
        AudioListener.volume = StaticClass.volumeGlobal;
    }

    public void MudarVolumeHit()
    {
        StaticClass.volumeHitSound = sliderVolumeHit.value;
        sliderVolumeHitAS.volume = StaticClass.volumeHitSound;
    }

    public void MudarShakeMult()
    {
        StaticClass.shakeMult = sliderShakeMult.value;
    }
}
