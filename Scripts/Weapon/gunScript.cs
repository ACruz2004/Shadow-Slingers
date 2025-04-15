using UnityEngine;
using System.Collections;
using UnityEngine.Animations;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem flash;
    AudioSource gunSound;
    public AmmoScript ammoScript;
    private int magSize = 8;
    private int sgMagSize = 2;
    private int pistolMax = 8;
    private int sgMax = 2;

    // Pistol Fire Rate
    private float pistolFireRate = 0.1f;
    public bool pistolFiring = false;

    // Shotgun Fire Rate
    private float sgFireRate = 1f;
    public bool sgFiring = false;
    public bool hasShotgun = false;

    // Reloading
    bool reloading = false;
    private float pistolReloadDuration = 2.3f;
    private float sgReloadDuration = 3f;

    // Insta-kill
    bool isInstaKill = false;

    // Duration of instakill
    private float instaDuration = 25f;

    // Animator Reference
    private Animator animator;

    [SerializeField]
    public string gunName;

    void Start()
    {
        gunSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        if (ammoScript == null)
        {
            ammoScript = FindObjectOfType<AmmoScript>();
        }

        // Subscribe to the Instakill event
        Instakill.onCollected += ActivateInstaKill;
    }

    void OnDestroy()
    {
        // Unsubscribe from the Instakill event
        Instakill.onCollected -= ActivateInstaKill;
    }

    void Update()
    {
        HandleWeaponSwitch();

        if (gunName == "Pistol")
        {
            HandlePistol();
        }

        if (gunName == "Shotgun" && hasShotgun)
        {
            HandleShotgun();
        }
    }

    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gunName = "Shotgun";
            Debug.Log("Switched to Shotgun");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gunName = "Pistol";
            Debug.Log("Switched to Pistol");
        }
    }

    private void HandlePistol()
    {
        if (Input.GetButtonDown("Fire1") && ammoScript.CurrentMagAmmo > 0 && !reloading && !pistolFiring)
        {
            StartCoroutine(FireRateControl(pistolFireRate, "Pistol"));
        }

        if (Input.GetButtonDown("Fire1") && ammoScript.CurrentMagAmmo == 0 && !reloading)
        {
            SoundManager.PlaySound(SoundType.NOAMMO);
        }

        if (Input.GetKeyDown(KeyCode.R) && ammoScript.CurrentMagAmmo < pistolMax && ammoScript.TotalAmmo != 0 && !reloading)
        {
            SoundManager.PlaySound(SoundType.PISTOLRELOAD);
            Reload(pistolReloadDuration, pistolMax);
        }
    }

    private void HandleShotgun()
    {
        if (Input.GetButtonDown("Fire1") && ammoScript.CurrentMagAmmo > 0 && !reloading && !sgFiring)
        {
            StartCoroutine(FireRateControl(sgFireRate, "Shotgun"));
        }

        if (Input.GetButtonDown("Fire1") && ammoScript.CurrentMagAmmo == 0 && !reloading)
        {
            SoundManager.PlaySound(SoundType.NOAMMO);
        }

        if (Input.GetKeyDown(KeyCode.R) && ammoScript.CurrentMagAmmo < sgMax && ammoScript.TotalAmmo != 0 && !reloading)
        {
            SoundManager.PlaySound(SoundType.SGRELOAD);
            Reload(sgReloadDuration, sgMax);
        }
    }

    private IEnumerator FireRateControl(float fireRate, string weaponType)
    {
        if (weaponType == "Pistol") pistolFiring = true;
        if (weaponType == "Shotgun") sgFiring = true;

        Shoot(weaponType);
        ammoScript.DecMagAmmo(1);

        yield return new WaitForSeconds(fireRate);

        if (weaponType == "Pistol") pistolFiring = false;
        if (weaponType == "Shotgun") sgFiring = false;
    }

    void Shoot(string weaponType)
    {
        if (animator != null)
        {
            animator.SetTrigger("shooting");
        }

        flash.Play();
        gunSound.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            EnemyScript enemy = hit.transform.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                float damageToDeal = isInstaKill ? 100000 : damage;
                if (weaponType == "Shotgun") damageToDeal *= 2; // Shotgun deals double damage
                enemy.TakeDamage(damageToDeal);
            }
        }
    }

    void Reload(float reloadDuration, int maxAmmo)
    {
        if (animator != null)
        {
            animator.SetTrigger("reloading");
        }

        reloading = true;
        StartCoroutine(DisableReload(reloadDuration, maxAmmo));
    }

    private IEnumerator DisableReload(float duration, int maxAmmo)
    {
        yield return new WaitForSeconds(duration);
        ammoScript.ReloadAmmo(maxAmmo);
        reloading = false;
    }

    private void ActivateInstaKill()
    {
        isInstaKill = true;
        StartCoroutine(DisableInstaKill(instaDuration));
    }

    private IEnumerator DisableInstaKill(float duration)
    {
        yield return new WaitForSeconds(duration);
        isInstaKill = false;
    }
}
