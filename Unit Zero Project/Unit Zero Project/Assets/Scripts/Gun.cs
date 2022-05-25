using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gun : MonoBehaviour {

    public enum WeaponType { SemiAuto, FullyAuto, Burst, Shotgun, FullAutoShotgun, Minigun, Flamethrower };
    public WeaponType shootType = WeaponType.SemiAuto;

    public float weight;
    public int Ammo;
    public int MagAmmo;
    public int MagSize;
    public float FireSpeed;
    public float recoilVer;
    public float gunRecoilVer;
    public float recoilHor;
    public float gunRecoilHor;
    public float recoilKick;
    public float accuracy;
    public float defaultAccuracy;
    public float adsAccuracy;
    [Range(0.0f, 1.0f)] public float crouchAccuracyMultiplier;
    [Range(1.0f, 5.0f)] public float walkAccuracyMultiplier;
    [Range(1.0f, 5.0f)] public float runAccuracyMultiplier;
    public float bloom;
    public float maxBloom;
    public float bloomRate;
    public float bloomRefreshRate;
    public bool adsCrosshair;
    public float aimSpeed;
    public float zoomAim;
    public int burstAmount;
    public float bulletDamage;
    public float critDamage;
    public float falloffRange;
    public AnimationCurve damageFallOff = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
    public float penetration;
    public float bulletForce;
    public float reloadTime;
    public bool singleReload;
    public bool autoReload;

    public LayerMask collisionLayers;
    public Text AmmoCount;
    public Text MagCount;
    public AudioClip shootSound;
    public AudioClip blankShotSound;
    public AudioClip reloadSound;
    private float WaitTilNextFire;
    public bool Shooting;
    public bool Aiming;
    public GameObject model;
    public GameObject ADSUI;
    public GameObject barrels;
    public GameObject hitUI;
    public GameObject critUI;
    public ParticleSystem fire;
    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleSmoke;
    public ParticleSystem bulletTrail;
    public ParticleSystem shellParticle;
    //public GameObject reloadingText;
    public GameObject crosshair;
    public Vector3 originalPos;
    public Vector3 aimedPos;
    
    [HideInInspector] public int inventorySlot;
    public GameObject inventoryItem;
    public GameObject pickupItem;

    private float fireRate;
    private bool doReload;
    private bool startReloadTimer;
    [HideInInspector] public bool canShoot;
    private Health healthScript;
    private AudioSource m_AudioSource;
    private int MagAmmoLost;
    [HideInInspector] public int MagAmmoMax;
    [HideInInspector] public int ammoMax;
    private Quaternion recoilRot1;
    private Quaternion recoilRot2;
    private Quaternion recoilCamRot1;
    private Quaternion recoilCamRot2;
    private Vector3 recoilPos1;
    private Vector3 recoilPos2;
    private Vector3 Direction;
    private float reloadTimeO;
    private float speed;
    private FPSController thePlayer;
    //private Inventory inventoryScript;
    //private LineRenderer line;
    
    // Use this for initialization
    void Start() {
        transform.parent = thePlayer.hands.transform;
        transform.localPosition = originalPos;
        //AmmoMax = Ammo;
        MagAmmoMax = MagSize;
        ammoMax = Ammo;
        AmmunitionCount();
        MagazineCount();
        fireRate = FireSpeed;
        m_AudioSource = GetComponent<AudioSource>();
        reloadTimeO = reloadTime;
        if (shootType == WeaponType.Minigun) {
            FireSpeed = 0;
        }
        crosshair = GameObject.Find("Canvas").transform.Find("Player").transform.Find("Crosshair").gameObject;
        hitUI = GameObject.Find("Canvas").transform.Find("Player").transform.Find("HitUI").gameObject;
        critUI = GameObject.Find("Canvas").transform.Find("Player").transform.Find("CritUI").gameObject;
        ADSUI = GameObject.Find("Canvas").transform.Find("SniperUI").gameObject;
        Inventory thePlayerInv = thePlayer.GetComponent<Inventory>();
        thePlayerInv.weaponItems[inventorySlot] = pickupItem;
        GameObject weaponInv = Instantiate(inventoryItem);
        weaponInv.transform.parent = thePlayerInv.slots[inventorySlot].transform;
        weaponInv.transform.localPosition = Vector3.zero;
        weaponInv.transform.localEulerAngles = Vector3.zero;
        //reloadingText = GameObject.Find("Canvas").transform.Find("Reloading...").gameObject;
        //reloadingText.GetComponent<Text>().enabled = false;
    }

    private IEnumerator WaitTillReady() {
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    void OnEnable() {
        StartCoroutine(WaitTillReady());
        AmmoCount = GameObject.Find("Canvas").transform.Find("WeaponInfo").transform.Find("AmmoCount").GetComponent<Text>();
        MagCount = GameObject.Find("Canvas").transform.Find("WeaponInfo").transform.Find("MagCount").GetComponent<Text>();
        GameObject.Find("Canvas").transform.Find("WeaponInfo").transform.Find("Name").GetComponent<Text>().text = gameObject.name.Replace("(Clone)", "");
        thePlayer = FindObjectOfType<FPSController>();
        thePlayer.ChangeMovementSpeed(weight);
        thePlayer.zoom = zoomAim;
        thePlayer.zoomSpeed = aimSpeed / 2;
        startReloadTimer = false;
        doReload = false;
        transform.localRotation = Quaternion.Euler(45, 0, 0);
    }

    // Update is called once per frame
    void Update() {
        if (startReloadTimer == false) {
            if (Input.GetKeyDown(KeyCode.Mouse0) && MagAmmo <= 0) PlayBlankShotSound();
            if (shootType == WeaponType.SemiAuto) {
                if (Input.GetKeyDown(KeyCode.Mouse0) && MagAmmo > 0) {
                    Shoot();
                }
                else {
                    Shooting = false;
                }
            }
            if (shootType == WeaponType.FullyAuto) {
                if (Input.GetKey(KeyCode.Mouse0) && MagAmmo > 0) {
                    Shoot();
                }
                else {
                    Shooting = false;
                }
            }
            if (shootType == WeaponType.Burst) {
                if (Input.GetKey(KeyCode.Mouse0) && MagAmmo > 0) {
                    StartCoroutine("ShootBurst");
                }
            }
            if (shootType == WeaponType.Minigun) {
                if (Input.GetKey(KeyCode.Mouse0) && MagAmmo > 0) {
                    Minigun();
                }
                else {
                    Shooting = false;
                }
            }
            if (shootType == WeaponType.Flamethrower) {
                if (Input.GetKey(KeyCode.Mouse0) && MagAmmo > 0) {
                    Flamethrower();
                }
                else {
                    Shooting = false;
                }
            }
            if (tag != "Sniper") {
                ADSUI.GetComponent<Image>().enabled = false;
            }
            if (Input.GetKey(KeyCode.Mouse1)) {
                Aiming = true;
                if (adsCrosshair == false) {
                    crosshair.SetActive(false);
                }
                thePlayer.canAim = true;
            }
            else {
                Aiming = false;
                crosshair.SetActive(true);
                thePlayer.canAim = false;
            }
        }
        if (shootType == WeaponType.Shotgun) {
            if (Input.GetKeyDown(KeyCode.Mouse0) && MagAmmo > 0) {
                doReload = false;
                startReloadTimer = false;
                Shotgun();
                StopCoroutine("enumIndividualReload");
                StopCoroutine("enumReload");
            }
            else {
                Shooting = false;
            }
        }
        if (shootType == WeaponType.FullAutoShotgun) {
            if (Input.GetKey(KeyCode.Mouse0) && MagAmmo > 0) {
                doReload = false;
                startReloadTimer = false;
                Shotgun();
                StopCoroutine("enumIndividualReload");
                StopCoroutine("enumReload");
            }
            else {
                Shooting = false;
            }
        }
        float acc = Aiming ? adsAccuracy : defaultAccuracy;
        float crouch = thePlayer.isCrouching ? crouchAccuracyMultiplier : 1;
        float moving = thePlayer.isMoving ? (thePlayer.isWalking ? walkAccuracyMultiplier : runAccuracyMultiplier) : 1;
        accuracy = Mathf.Lerp(accuracy, acc * crouch * moving * bloom, aimSpeed * Time.deltaTime);
        float distance = Mathf.Sin(2 * accuracy * (Mathf.PI / 180)) * falloffRange * 20;
        crosshair.transform.Find("Top").GetComponent<RectTransform>().localPosition = new Vector3(0, distance, 0);
        crosshair.transform.Find("Bottom").GetComponent<RectTransform>().localPosition = new Vector3(0, -distance, 0);
        crosshair.transform.Find("Left").GetComponent<RectTransform>().localPosition = new Vector3(-distance, 0, 0);
        crosshair.transform.Find("Right").GetComponent<RectTransform>().localPosition = new Vector3(distance, 0, 0);

        bloom = Mathf.Lerp(bloom, 1, bloomRefreshRate * Time.deltaTime);
        WaitTilNextFire = Mathf.Clamp(WaitTilNextFire, -1.0f, 1.0f);
        WaitTilNextFire -= FireSpeed * Time.deltaTime;
        Recoil();
        AimDownSight();
        CalculateAmmo();
        AmmunitionCount();
        MagazineCount();
    }

    void Shoot() {
        Vector3 Origin = muzzleFlash.transform.position;
        Direction = GetPointOnUnitSphereCap(Camera.main.transform.rotation, accuracy);
        RaycastHit hit;
        if (WaitTilNextFire <= 0 && canShoot == true) {
            Shooting = true;
            if (Physics.Raycast(Origin, Direction, out hit, falloffRange, collisionLayers) == true) {
                if (hit.collider.gameObject.tag == "CritSpot") {
                    hit.transform.root.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                    hit.transform.root.GetComponent<SoldierAI>().blood.Emit(3);
                    hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                    healthScript = hit.transform.root.GetComponent<Health>();
                    float dmg = Mathf.Round(critDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                    healthScript.health -= Mathf.RoundToInt(dmg);
                    healthScript.UpdateHealth();
                    healthScript.regenDelay = healthScript.initialRegenDelay;
                    critUI.SetActive(true);
                }
                else if (hit.collider.GetComponent<Health>()) {
                    healthScript = hit.collider.GetComponent<Health>();
                    float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                    healthScript.health -= Mathf.RoundToInt(dmg);
                    healthScript.UpdateHealth();
                    healthScript.regenDelay = healthScript.initialRegenDelay;
                    hitUI.SetActive(true);
                }
                else if (hit.collider.GetComponent<SoldierAI>()) {
                    hit.collider.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                    hit.collider.GetComponent<SoldierAI>().blood.Emit(3);
                    hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                }
                else if (hit.collider.CompareTag("Ragdoll")) {
                    hit.collider.transform.root.GetComponent<Ragdoll>().blood.transform.position = hit.point;
                    hit.collider.transform.root.GetComponent<Ragdoll>().blood.Emit(3);
                }
                if (hit.collider.GetComponent<Rigidbody>()) {
                    hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(Direction * bulletForce, hit.point);
                }
            }
            bulletTrail.transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
            bulletTrail.Emit(1);
            muzzleFlash.Emit(1);
            muzzleSmoke.Emit(3);
            shellParticle.Emit(1);
            MagAmmo -= 1;
            PlayShootSound();
            bloom = Mathf.Lerp(bloom, maxBloom, bloomRate * Time.deltaTime);
            WaitTilNextFire = 1;
        }
    }

    IEnumerator ShootBurst() {
        if (WaitTilNextFire <= 0 && canShoot == true) {
            int burstNum = 1;
            while (burstNum <= burstAmount && MagAmmo > 0) {
                Shooting = true;
                Vector3 Origin = muzzleFlash.transform.position;
                Direction = GetPointOnUnitSphereCap(Camera.main.transform.rotation, accuracy);
                RaycastHit hit;
                if (Physics.Raycast(Origin, Direction, out hit, falloffRange, collisionLayers) == true) {
                    if (hit.collider.gameObject.tag == "CritSpot") {
                        hit.transform.root.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                        hit.transform.root.GetComponent<SoldierAI>().blood.Emit(3);
                        hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                        healthScript = hit.transform.root.GetComponent<Health>();
                        float dmg = Mathf.Round(critDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                        healthScript.health -= Mathf.RoundToInt(dmg);
                        healthScript.UpdateHealth();
                        healthScript.regenDelay = healthScript.initialRegenDelay;
                        critUI.SetActive(true);
                    }
                    else if (hit.collider.GetComponent<Health>()) {
                        healthScript = hit.collider.GetComponent<Health>();
                        float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                        healthScript.health -= Mathf.RoundToInt(dmg);
                        healthScript.UpdateHealth();
                        healthScript.regenDelay = healthScript.initialRegenDelay;
                        hitUI.SetActive(true);
                    }
                    else if (hit.collider.GetComponent<SoldierAI>()) {
                        hit.collider.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                        hit.collider.GetComponent<SoldierAI>().blood.Emit(3);
                        hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                        healthScript = hit.collider.GetComponent<Health>();
                        float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                        healthScript.health -= Mathf.RoundToInt(dmg);
                        healthScript.UpdateHealth();
                        healthScript.regenDelay = healthScript.initialRegenDelay;
                        hitUI.SetActive(true);
                    }
                    else if (hit.collider.CompareTag("Ragdoll")) {
                        hit.collider.transform.root.GetComponent<Ragdoll>().blood.transform.position = hit.point;
                        hit.collider.transform.root.GetComponent<Ragdoll>().blood.Emit(3);
                    }
                    if (hit.collider.GetComponent<Rigidbody>()) {
                        hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(Direction * bulletForce, hit.point);
                    }
                }
                bulletTrail.transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
                bulletTrail.Emit(1);
                muzzleFlash.Emit(1);
                muzzleSmoke.Emit(3);
                shellParticle.Emit(1);
                MagAmmo -= 1;
                PlayShootSound();
                bloom = Mathf.Lerp(bloom, maxBloom, bloomRate * Time.deltaTime);
                WaitTilNextFire = 1;
                burstNum += 1;
                yield return new WaitForSeconds(0.1f);
                Shooting = false;
            }
        }
        Shooting = false;
    }

    void Shotgun() {
        Vector3 Origin = muzzleFlash.transform.position;
        if (WaitTilNextFire <= 0 && canShoot == true) {
            Shooting = true;
            for (int fragments = 0; fragments < burstAmount; fragments++) {
                Direction = GetPointOnUnitSphereCap(Camera.main.transform.rotation, accuracy);
                RaycastHit[] hits;
                hits = Physics.SphereCastAll(Origin, 0.01f, Direction, falloffRange, collisionLayers);
                System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.collider.CompareTag("Shield")) {
                        break;
                    }
                    if (hit.collider.gameObject.tag == "CritSpot") {
                        hit.transform.root.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                        hit.transform.root.GetComponent<SoldierAI>().blood.Emit(3);
                        hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                        healthScript = hit.transform.root.GetComponent<Health>();
                        float dmg = Mathf.Round(critDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                        dmg *= Mathf.Round(Mathf.Pow(penetration, i));
                        healthScript.health -= Mathf.RoundToInt(dmg);
                        healthScript.UpdateHealth();
                        healthScript.regenDelay = healthScript.initialRegenDelay;
                        critUI.SetActive(true);
                    }
                    else if (hit.collider.GetComponent<Health>()) {
                        healthScript = hit.collider.GetComponent<Health>();
                        float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                        healthScript.health -= Mathf.RoundToInt(dmg);
                        healthScript.UpdateHealth();
                        healthScript.regenDelay = healthScript.initialRegenDelay;
                        hitUI.SetActive(true);
                    }
                    else if (hit.collider.GetComponent<SoldierAI>()) {
                        hit.collider.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                        hit.collider.GetComponent<SoldierAI>().blood.Emit(3);
                        hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                        healthScript = hit.collider.GetComponent<Health>();
                        float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                        dmg *= Mathf.Round(Mathf.Pow(penetration, i));
                        healthScript.health -= Mathf.RoundToInt(dmg);
                        healthScript.UpdateHealth();
                        healthScript.regenDelay = healthScript.initialRegenDelay;
                        hitUI.SetActive(true);
                    }
                    else if (hit.collider.CompareTag("Ragdoll")) {
                        hit.collider.transform.root.GetComponent<Ragdoll>().blood.transform.position = hit.point;
                        hit.collider.transform.root.GetComponent<Ragdoll>().blood.Emit(3);
                    }
                    if (hit.collider.GetComponent<Rigidbody>()) {
                        hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(Direction * bulletForce, hit.point);
                    }
                }
                WaitTilNextFire = 1;
                bulletTrail.transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
                bulletTrail.Emit(1);
            }
            muzzleFlash.Emit(1);
            muzzleSmoke.Emit(3);
            StartCoroutine(ShotgunShell(0.7f));
            bloom = Mathf.Lerp(bloom, maxBloom, bloomRate * Time.deltaTime);
            MagAmmo -= 1;
            PlayShootSound();
        }
    }

    public IEnumerator ShotgunShell(float wait) {
        yield return new WaitForSeconds(wait);
        shellParticle.Emit(1);
    }

    void Minigun() {
        Vector3 Origin = muzzleFlash.transform.position;
        Direction = GetPointOnUnitSphereCap(Camera.main.transform.rotation, accuracy);
        RaycastHit hit;
        if (WaitTilNextFire <= 0 && canShoot == true) {
            Shooting = true;
            if (Physics.Raycast(Origin, Direction, out hit, falloffRange, collisionLayers) == true) {
                if (hit.collider.gameObject.tag == "CritSpot") {
                    hit.transform.root.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                    hit.transform.root.GetComponent<SoldierAI>().blood.Emit(3);
                    hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                    healthScript = hit.transform.root.GetComponent<Health>();
                    float dmg = Mathf.Round(critDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                    healthScript.health -= Mathf.RoundToInt(dmg);
                    healthScript.UpdateHealth();
                    healthScript.regenDelay = healthScript.initialRegenDelay;
                    critUI.SetActive(true);
                }
                else if (hit.collider.GetComponent<Health>()) {
                    healthScript = hit.collider.GetComponent<Health>();
                    float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                    healthScript.health -= Mathf.RoundToInt(dmg);
                    healthScript.UpdateHealth();
                    healthScript.regenDelay = healthScript.initialRegenDelay;
                    hitUI.SetActive(true);
                }
                else if (hit.collider.GetComponent<SoldierAI>()) {
                    hit.collider.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                    hit.collider.GetComponent<SoldierAI>().blood.Emit(3);
                    hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                    healthScript = hit.collider.GetComponent<Health>();
                    float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                    healthScript.health -= Mathf.RoundToInt(dmg);
                    healthScript.UpdateHealth();
                    healthScript.regenDelay = healthScript.initialRegenDelay;
                    hitUI.SetActive(true);
                }
                else if (hit.collider.CompareTag("Ragdoll")) {
                    hit.collider.transform.root.GetComponent<Ragdoll>().blood.transform.position = hit.point;
                    hit.collider.transform.root.GetComponent<Ragdoll>().blood.Emit(3);
                }
                if (hit.collider.GetComponent<Rigidbody>()) {
                    hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(Direction * bulletForce, hit.point);
                }
            }
            bulletTrail.transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
            bulletTrail.Emit(1);
            muzzleFlash.Emit(1);
            muzzleSmoke.Emit(3);
            shellParticle.Emit(1);
            MagAmmo -= 1;
            PlayShootSound();
            bloom = Mathf.Lerp(bloom, maxBloom, bloomRate * Time.deltaTime);
            WaitTilNextFire = 1;
        }
    }
    
    void Flamethrower() {
        Vector3 Origin = Camera.main.transform.position;
        Direction = GetPointOnUnitSphereCap(Camera.main.transform.rotation, accuracy);
        if (WaitTilNextFire <= 0 && canShoot == true) {
            Shooting = true;
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(Origin, 0.2f, Direction, falloffRange, collisionLayers);
            for (int i = 0; i < hits.Length; i++) {
                RaycastHit hit = hits[i];
                if (hit.collider.gameObject.tag == "CritSpot") {
                    hit.transform.root.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                    hit.transform.root.GetComponent<SoldierAI>().blood.Emit(3);
                    hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                    healthScript = hit.transform.root.GetComponent<EnemyHealth>();
                    float dmg = Mathf.Round(critDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                    healthScript.health -= Mathf.RoundToInt(dmg);
                    healthScript.UpdateHealth();
                    healthScript.regenDelay = healthScript.initialRegenDelay;
                    healthScript.StartCoroutine(healthScript.DamageOverTime(50, 5, 0.5f));
                    //healthScript.StartCoroutine(healthScript.Burn(5f));
                    critUI.SetActive(true);
                }
                else {
                    if (hit.collider.GetComponent<SoldierAI>()) {
                        hit.collider.GetComponent<SoldierAI>().blood.transform.position = hit.point;
                        hit.collider.GetComponent<SoldierAI>().blood.Emit(3);
                        hit.collider.GetComponent<SoldierAI>().combatTime = hit.collider.GetComponent<SoldierAI>().searchTime;
                        healthScript = hit.collider.GetComponent<EnemyHealth>();
                        float dmg = Mathf.Round(bulletDamage * damageFallOff.Evaluate(Vector3.Distance(hit.point, Origin) / falloffRange));
                        healthScript.health -= Mathf.RoundToInt(dmg);
                        healthScript.UpdateHealth();
                        healthScript.regenDelay = healthScript.initialRegenDelay;
                        healthScript.StartCoroutine(healthScript.DamageOverTime(50, 5, 0.5f));
                        //healthScript.StartCoroutine(healthScript.Burn(5f));
                        hitUI.SetActive(true);
                    }
                }
            }
            fire.Emit(burstAmount);
            MagAmmo -= 1;
            PlayShootSound();
            bloom = Mathf.Lerp(bloom, maxBloom, bloomRate * Time.deltaTime);
            WaitTilNextFire = 1;
        }
    }

    public static Vector3 GetPointOnUnitSphereCap(Quaternion targetDirection, float angle) {
        var angleInRad = Random.Range(0.0f, angle) * Mathf.Deg2Rad;
        var PointOnCircle = (Random.insideUnitCircle.normalized) * Mathf.Sin(angleInRad);
        var V = new Vector3(PointOnCircle.x, PointOnCircle.y, Mathf.Cos(angleInRad));
        return targetDirection * V;
    }

    int Choose(float[] probs) {
        float total = 0;
        foreach (float elem in probs) {
            total += elem;
        }
        float randomPoint = Random.value * total;
        for (int i = 0; i < probs.Length; i++) {
            if (randomPoint < probs[i]) {
                return i;
            }
            else {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

    private IEnumerator enumReload() {
        Aiming = false;
        thePlayer.canAim = false;
        Shooting = false;
        doReload = true;
        PlayReloadSound();
        startReloadTimer = true;
        //reloadingText.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(reloadTimeO);
        MagAmmoLost = (MagAmmoMax - MagAmmo) < Ammo ? (MagAmmoMax - MagAmmo) : Ammo;
        MagAmmo += MagAmmoLost;
        Ammo -= MagAmmoLost;
        startReloadTimer = false;
        doReload = false;
        //reloadingText.GetComponent<Text>().enabled = false;
    }

    void Reload() {
        startReloadTimer = true;
        if (reloadTime <= 0) {
            if (Ammo < MagAmmoLost) {
                MagAmmoLost = Ammo;
            }
            MagAmmo += MagAmmoLost;
            Ammo -= MagAmmoLost;
            reloadTime = reloadTimeO;
            startReloadTimer = false;
            doReload = false;
        }
    }

    public IEnumerator enumIndividualReload() {
        doReload = true;
        while (MagAmmo < MagAmmoMax && Ammo > 0) {
            Aiming = false;
            PlayReloadSound();
            thePlayer.canAim = false;
            Shooting = false;
            doReload = true;
            startReloadTimer = true;
            //reloadingText.GetComponent<Text>().enabled = true;
            yield return new WaitForSeconds(reloadTimeO);
            MagAmmo += 1;
            Ammo -= 1;
        }
        startReloadTimer = false;
        doReload = false;
        //reloadingText.GetComponent<Text>().enabled = false;
    }

    void IndividualReload() {
        if (MagAmmo < MagAmmoMax && Ammo > MagAmmoLost) {
            startReloadTimer = true;
            if (reloadTime <= 0) {
                MagAmmo += 1;
                Ammo -= 1;
                reloadTime = reloadTimeO;
                startReloadTimer = false;
            }
        }
        else {
            doReload = false;
        }
    }

    void CalculateAmmo() {
        if (Input.GetKeyDown(KeyCode.R) && Ammo > 0) {
            if (MagAmmo < MagAmmoMax) {
                //doReload = true;
                if (singleReload == true) {
                    StartCoroutine(enumIndividualReload());
                }
                else {
                    StartCoroutine(enumReload());
                }
            }
        }
        if (autoReload == true && Ammo > 0 && MagAmmo == 0) {
            //doReload = true;
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

    public void MaxAmmo() {
        MagAmmo = MagAmmoMax;
        Ammo = ammoMax;
    }

    void AmmunitionCount() {
        AmmoCount.text = Ammo.ToString();
    }
    void MagazineCount() {
        MagCount.text = MagAmmo.ToString();// + " / " + MagAmmoMax.ToString();
    }
    void PlayShootSound() {
        m_AudioSource.PlayOneShot(shootSound);
        thePlayer.MakeNoise(falloffRange);
    }

    void PlayBlankShotSound() {
        m_AudioSource.PlayOneShot(blankShotSound);
        thePlayer.MakeNoise(2f);
    }

    void PlayReloadSound() {
        m_AudioSource.PlayOneShot(reloadSound);
        thePlayer.MakeNoise(2f);
    }

    void Recoil() {
        recoilRot1 = transform.localRotation;
        recoilCamRot1 = Camera.main.transform.localRotation;
        if (Shooting == true) {
            recoilCamRot2 = Quaternion.Euler(-recoilVer, Random.Range(-recoilHor, recoilHor), Random.Range(-recoilHor, recoilHor));
            recoilRot2 = Quaternion.Euler(-gunRecoilVer, Random.Range(-gunRecoilHor, gunRecoilHor), Random.Range(-gunRecoilHor, gunRecoilHor));
            if (shootType == WeaponType.SemiAuto || shootType == WeaponType.Shotgun || shootType == WeaponType.FullAutoShotgun || shootType == WeaponType.Burst) {
                transform.localRotation = Quaternion.Slerp(recoilRot1, recoilRot2, Time.deltaTime * (FireSpeed * 20));
                Camera.main.transform.localRotation = Quaternion.Slerp(recoilCamRot1, recoilCamRot2, Time.deltaTime * (FireSpeed * 20));
            }
            else {
                transform.localRotation = Quaternion.Slerp(recoilRot1, recoilRot2, Time.deltaTime * (FireSpeed / 3));
                Camera.main.transform.localRotation = Quaternion.Slerp(recoilCamRot1, recoilCamRot2, Time.deltaTime * (FireSpeed / 3));
            }
        }
        else {
            if (startReloadTimer == true) {
                recoilRot2 = Quaternion.Euler(-50, 0, -30);
                recoilCamRot2 = Quaternion.Euler(Vector3.zero);
            }
            else {
                recoilRot2 = Quaternion.Euler(Vector3.zero);
                recoilCamRot2 = Quaternion.Euler(Vector3.zero);
            }
            transform.localRotation = Quaternion.Slerp(recoilRot1, recoilRot2, Time.deltaTime * 5);
            Camera.main.transform.localRotation = Quaternion.Slerp(recoilCamRot1, recoilCamRot2, Time.deltaTime * 5);
        }
    }

    IEnumerator SniperADS() {
        yield return new WaitForSeconds(2f / aimSpeed);
        if (Aiming == true) {
            Direction = GetPointOnUnitSphereCap(Camera.main.transform.rotation, adsAccuracy);
            model.SetActive(false);
            //ADSUI.SetActive (true);
            ADSUI.GetComponent<Image>().enabled = true;
        }
    }

    void AimDownSight() {
        recoilPos1 = transform.localPosition;
        float kick = Mathf.PingPong(Time.time * fireRate * 20, 0.1f * recoilKick) * -0.1f;

        if (Aiming == true) {
            if (doReload == true) {
                recoilPos2 = originalPos;
                speed = 5;
                if (tag == "Sniper") {
                    model.SetActive(true);
                    //ADSUI.SetActive (false);
                    ADSUI.GetComponent<Image>().enabled = false;
                }
            }
            else {
                speed = aimSpeed;
                if (Shooting == true) {
                    recoilPos2 = new Vector3(aimedPos.x, aimedPos.y, aimedPos.z + kick);
                }
                else {
                    recoilPos2 = aimedPos;
                }
                if (tag == "Sniper") {
                    StartCoroutine("SniperADS");
                }
                if (shootType == WeaponType.Minigun) {
                    if (FireSpeed < fireRate) {
                        FireSpeed += 1;
                    }
                }
            }
        }
        else {
            if (tag == "Sniper") {
                model.SetActive(true);
                //ADSUI.SetActive (false);
                ADSUI.GetComponent<Image>().enabled = false;
            }
            speed = aimSpeed;
            if (Shooting == true) {
                recoilPos2 = new Vector3(originalPos.x, originalPos.y, originalPos.z + kick);
                if (shootType == WeaponType.Minigun) {
                    if (FireSpeed < fireRate) {
                        FireSpeed += 1;
                    }
                }
            }
            else {
                recoilPos2 = originalPos;
                if (shootType == WeaponType.Minigun) {
                    if (FireSpeed >= 1) {
                        FireSpeed -= 1;
                    }
                }
            }
        }
        if (shootType == WeaponType.Minigun) {
            barrels.transform.Rotate(0, 0, 15 * Time.deltaTime * FireSpeed);
        }
        transform.localPosition = Vector3.Lerp(recoilPos1, recoilPos2, Time.deltaTime * speed);
    }
}
