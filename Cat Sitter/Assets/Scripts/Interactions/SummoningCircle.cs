using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummoningCircle : Interactable
{


    [SerializeField] GameObject summoningCircle;
    [SerializeField] Image summoningProgressFX;
    [SerializeField] GameObject demon;
    [SerializeField] List<GameObject> fireFX = new();
    [SerializeField] ParticleSystem dustCloud;
    [SerializeField] Collider c;
    float currentTimer;
    float cdTimer;

    public override void CatActivateInteractable()
    {
        LevelManager.Instance.AudioManager.PlayAudio("summon");
        summoningCircle.SetActive(true);
        state = InteractionState.Active;
        currentTimer = timeToCatastrophe;
    }

    public override void StartFixActive()
    {
        dustCloud.Play();
    }
    public override void CancelFixActive()
    {
        dustCloud.Stop();
    }

    public override void FinishFixActive()
    {
        dustCloud.Stop();
        state = InteractionState.Cooldown;
        cdTimer = timeToCooldown;
        summoningProgressFX.fillAmount = 0;
    }
    public override void StartFixCatastrophe()
    {
        return;
    }
    public override void CancelFixCatastrophe()
    {
        return;
    }
    public override void FinishFixCatastrophe()
    {
        LevelManager.Instance.AudioManager.PlayAudio("summon");
        Debug.Log("Summoning circle fixed");
        LeanTween.scale(demon, new(0.01f, 0.01f, 0.01f), .25f).setOnComplete(() =>
        {
            c.enabled = false;
            demon.transform.localScale = Vector3.one;
            demon.SetActive(false);
        });
        foreach (var fx in fireFX)
        {
            fx.SetActive(false);
        }
        state = InteractionState.Cooldown;
        cdTimer = timeToCooldown;

    }

    // Start is called before the first frame update
    void Start()
    {
        summoningCircle.SetActive(false);
        summoningProgressFX.fillAmount = 0;
        c.enabled = false;
        demon.SetActive(false);
        foreach (var fx in fireFX)
        {
            fx.SetActive(false);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        switch (state)
        {
            case InteractionState.Active:
                if (currentTimer > 0)
                {
                    currentTimer -= Time.deltaTime;
                    summoningProgressFX.fillAmount = (timeToCatastrophe - currentTimer) / timeToCatastrophe;
                }
                else
                {
                    LevelManager.Instance.AudioManager.PlayAudio("summon2");
                    state = InteractionState.Catastrophe;
                    c.enabled = true;
                    demon.SetActive(true);
                    foreach (var fx in fireFX)
                    {
                        fx.SetActive(true);
                    }
                }
                if (playerInteracting)
                {
                    if (Time.time - lastClickTime > timeToFixActive)
                    {
                        FinishFixActive();
                    }
                }
                break;
            case InteractionState.Catastrophe:
                demon.transform.eulerAngles = new Vector3(-90, demon.transform.eulerAngles.y + Time.deltaTime * 100, 0);
                if (playerInteracting)
                {
                    if (Time.time - lastClickTime > timeToFixCatastrophe)
                    {
                        FinishFixCatastrophe();
                    }
                }
                break;
            case InteractionState.Cooldown:
                if (cdTimer > 0)
                {
                    cdTimer -= Time.deltaTime;
                }
                else
                {
                    state = InteractionState.Idle;
                    cdTimer = 0;
                }
                break;
            case InteractionState.Idle:
                break;
        }
    }
}
