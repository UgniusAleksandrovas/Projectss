using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {

    public enum WeaponType { SemiAuto, Shotgun };
    public WeaponType shootType = WeaponType.SemiAuto;

    public int Ammo;
    public int MagAmmo;
    public int MagSize;
    public float accuracy;
    public float fireRate;
    public float burstAmount;
    public float bulletDamage;
    public float critDamage;
    public float shootRange;
    public float falloffRange;
    public AnimationCurve damageFallOff = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
    public LayerMask collisionLayers;
    public float reloadTime;
    public bool singleReload;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleSmoke;
    public ParticleSystem bulletTrail;
    public ParticleSystem shellParticle;

    public bool Shooting;
    [HideInInspector] public bool canShoot;
    private AudioSource m_AudioSource;
    private PlayerHealth healthScript;
    private float WaitTilNextFire;
    private Vector3 Direction;

    private int MagAmmoLost;
    [HideInInspector] public int MagAmmoMax;
    [HideInInspector] public int ammoMax;
    private bool doReload;
    private bool startReloadTimer;
    private float reloadTimeO;
    private SoldierAI theSoldier;

    // Start is called before the first frame update
    void Start() {
        theSoldier = transform.root.GetComponent<SoldierAI>();
        m_AudioSource = GetComponent<AudioSource>();
        MagAmmoMax = MagSize;
        ammoMax = Ammo;
        reloadTimeO = reloadTime;
    }
    void OnEnable() {
        startReloadTimer = false;
        doReload = false;
        canShoot = true;
    }

    // Update is called once per frame
    void Update() {
        CalculateAmmo();
        WaitTilNextFire = Mathf.Clamp(WaitTilNextFire, -1.0f, 1.0f);
        WaitTilNextFire -= fireRate * Time.deltaTime;
    }

    public void Shoot(Vector3 dir) {
        if (startReloadTimer == false) {
            Vector3 Origin = muzzleFlash.transform.position;
            Direction = GetPointOnUnitSphereCap(Quaternion.LookRotation(dir, transform.up), accuracy);
            RaycastHit hit;
            if (WaitTilNextFire <= 0 && canShoot == true) {
                Shooting = true;
                if (Physics.Raycast(Origin, Direction, out hit, falloffRange, collisionLayers) == true) {
                    if (hit.collider.gameObject.tag == "CritSpot") {
                        hit.transform.root.GetComponent<FPSController>().blood.transform.position = hit.point;
                        hit.transform.root.GetComponent<FPSController>().blood.Emit(3);
                        healthScript = hit.transform.root.GetComponent<PlayerHealth>();
                        float dmg = Mathf.Round(critDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                        healthScript.health -= Mathf.RoundToInt(dmg);
                        healthScript.UpdateHealth();
                        healthScript.regenDelay = healthScript.initialRegenDelay;
                    }
                    else {
                        if (hit.collider.GetComponent<PlayerHealth>()) {
                            hit.collider.GetComponent<FPSController>().blood.transform.position = hit.point;
                            hit.collider.GetComponent<FPSController>().blood.Emit(3);
                            healthScript = hit.collider.GetComponent<PlayerHealth>();
                            float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                            healthScript.health -= Mathf.RoundToInt(dmg);
                            healthScript.UpdateHealth();
                            healthScript.regenDelay = healthScript.initialRegenDelay;
                        }
                    }
                }
                bulletTrail.transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
                bulletTrail.Emit(1);
                muzzleFlash.Emit(1);
                muzzleSmoke.Emit(3);
                shellParticle.Emit(1);
                MagAmmo -= 1;
                PlayShootSound();
                WaitTilNextFire = 1;
            }
        }
    }

    public void Shotgun(Vector3 dir) {
        Vector3 Origin = muzzleFlash.transform.position;
        if (WaitTilNextFire <= 0 && canShoot == true) {
            Shooting = true;
            for (int fragments = 0; fragments < burstAmount; fragments++) {
                Direction = GetPointOnUnitSphereCap(Quaternion.LookRotation(dir, transform.up), accuracy);
                RaycastHit[] hits;
                hits = Physics.SphereCastAll(Origin, 0.01f, Direction, falloffRange, collisionLayers);
                System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.collider.gameObject.tag == "CritSpot") {
                        hit.transform.root.GetComponent<FPSController>().blood.transform.position = hit.point;
                        hit.transform.root.GetComponent<FPSController>().blood.Emit(3);
                        healthScript = hit.transform.root.GetComponent<PlayerHealth>();
                        float dmg = Mathf.Round(critDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                        healthScript.health -= Mathf.RoundToInt(dmg);
                        healthScript.UpdateHealth();
                        healthScript.regenDelay = healthScript.initialRegenDelay;
                    }
                    else {
                        if (hit.collider.GetComponent<PlayerHealth>()) {
                            hit.collider.GetComponent<FPSController>().blood.transform.position = hit.point;
                            hit.collider.GetComponent<FPSController>().blood.Emit(3);
                            healthScript = hit.collider.GetComponent<PlayerHealth>();
                            float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                            healthScript.health -= Mathf.RoundToInt(dmg);
                            healthScript.UpdateHealth();
                            healthScript.regenDelay = healthScript.initialRegenDelay;
                        }
                    }
                }
                WaitTilNextFire = 1;
                bulletTrail.transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
                bulletTrail.Emit(1);
            }
            muzzleFlash.Emit(1);
            muzzleSmoke.Emit(3);
            StartCoroutine(ShotgunShell(1.3f));
            MagAmmo -= 1;
            PlayShootSound();
        }
    }

    public IEnumerator ShotgunShell(float wait) {
        yield return new WaitForSeconds(wait);
        shellParticle.Emit(1);
    }

    public static Vector3 GetPointOnUnitSphereCap(Quaternion targetDirection, float angle) {
        var angleInRad = Random.Range(0.0f, angle) * Mathf.Deg2Rad;
        var PointOnCircle = (Random.insideUnitCircle.normalized) * Mathf.Sin(angleInRad);
        var V = new Vector3(PointOnCircle.x, PointOnCircle.y, Mathf.Cos(angleInRad));
        return targetDirection * V;
    }

    void PlayShootSound() {
        m_AudioSource.PlayOneShot(shootSound);
        theSoldier.MakeNoise(shootRange);
        
    }

    void PlayReloadSound() {
        m_AudioSource.PlayOneShot(reloadSound);
        theSoldier.MakeNoise(2f);
    }

    void CalculateAmmo() {
        if (Ammo > 0 && MagAmmo == 0) {
            if (doReload != true) {
                if (singleReload == true) {
                    StartCoroutine(enumIndividualReload());
                }
                else {
                    StartCoroutine(enumReload());
                }
            }
        }
    }

    private IEnumerator enumReload() {
        Shooting = false;
        doReload = true;
        PlayReloadSound();
        startReloadTimer = true;
        yield return new WaitForSeconds(reloadTimeO);
        MagAmmoLost = (MagAmmoMax - MagAmmo) < Ammo ? (MagAmmoMax - MagAmmo) : Ammo;
        MagAmmo += MagAmmoLost;
        Ammo -= MagAmmoLost;
        startReloadTimer = false;
        doReload = false;
    }

    public IEnumerator enumIndividualReload() {
        doReload = true;
        while (MagAmmo < MagAmmoMax && Ammo > 0) {
            PlayReloadSound();
            Shooting = false;
            doReload = true;
            startReloadTimer = true;
            yield return new WaitForSeconds(reloadTimeO);
            MagAmmo += 1;
            Ammo -= 1;
        }
        startReloadTimer = false;
        doReload = false;
    }
}
