using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    enable,
    disable
}

public class PlayerController : MonoBehaviour
{
    //variables
    [SerializeField] PlayerState state;
    public event System.Action<int> onChangeCoin;
    public event System.Action<SOitem> onGetItem;
    public event System.Action onLoseItem;

    public static event System.Action onPressStart;

    [Header("general")]
    [SerializeField] Animator animator;
    [SerializeField] string playerName;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Collider2D playerColl;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] bool canMove = true;
    [SerializeField] GameObject directionObj;
    [SerializeField] SpriteRenderer outline;
    Color colorPlayer;

    [Header("Rotation")]
    Vector3 velocity;
    [SerializeField] float rotationSpeed;

    [Header("acceleracion")]
    [SerializeField] int maxSpeed = 30;
    [SerializeField] float currentSpeed = 0;
    [SerializeField] float aditionalsForces;
    [SerializeField] bool acelerated;
    [SerializeField] bool acelerating;//saber si el jugador esta presionando el boton de acelerar, incluso cuando no pueda
    [SerializeField] float acelerationSpeed = 2;
    [SerializeField] float decelerationSpeed = 3;

    [Header("BurstAceleration")]
    [SerializeField] int aditionalAceleration = 3;
    [SerializeField] bool isBursting;

    [Header("mud State")]
    bool inMud;
    int mudMaxSpeed;//velocidad cuando este en el barro

    [Header("Shield")]
    [SerializeField] GameObject shieldObj;
    [SerializeField] bool shieldActivated;

    [Header("braking")]
    [SerializeField] float brakingForce = 8f;
    [SerializeField] bool braking;

    [Header("Racing")]
    //representa los puntos de la pista a los que a llegado el jugador
    [SerializeField] int currentControlPoints;
    //la vuelta en la que va el jugador
    [SerializeField] int currentLap;
    //indica si el jugador termino la carrera
    [SerializeField] bool win;
    //indica el puntaje del jugador
    [SerializeField] int score;

    [Header("characterSelector")]
    public SlotCharacter slot;//slot donde se le cambiara la skin al personaje
    [SerializeField] SpriteRenderer spriteRenderer;
    bool isReady;

    [Header("Items")]
    [SerializeField] Item currentItem;
    [SerializeField] Transform frontSpawn;
    [SerializeField] Transform BackSpawn;
    [SerializeField] int attackItem;

    [Header("rotateCoroutine")]
    [SerializeField] float angularVelocity = 360f;
    [SerializeField] bool rotating;

    [Header("Recoil")]
    [SerializeField] float recoilForce = 300;
    [SerializeField] float timeToRecoil = 1.5f;
    [SerializeField] bool inRecoil;

    [Header("Health")]
    [SerializeField] int maxHealth = 100;
    float currentHealth;

    [Header("Damage")]
    [SerializeField] bool canGetDamage = true;
    [SerializeField] int wallCollisionDamage = 20;

    [Header("Death")]
    [SerializeField] string deathAnimation;
    [SerializeField] float timeToRespawn = 1f;

    [Header("Coins")]
    [SerializeField] int currentCoins;

    //properties
    public PlayerInput PlayerInput { get => playerInput; }
    public SpriteRenderer Sp { get => spriteRenderer; }
    public int CurrentControlPoints {  get => currentControlPoints; set => currentControlPoints = value; }
    public int CurrentLap {  get => currentLap; set => currentLap = value; }
    public float CurrentSpeed { get => currentSpeed;}
    public int MaxSpeed { get => maxSpeed; set => maxSpeed = value;}
    public float AcelerationSpeed { get => acelerationSpeed; set  => acelerationSpeed = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public bool Win { get => win; set => win = value; }
    public int Score { get => score; set => score = value; }
    public int MaxHealth {get => maxHealth;}
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            if(currentHealth <= 0)
            {
                Die();
            }
        }
    }
    public int CurrentCoins { get => currentCoins;}
    public bool CanGetDamage { get => canGetDamage; set => canGetDamage = value; }
    public bool IsReady { get => isReady; }
    public Color PlayerColor { get => colorPlayer;}

    //methods
    #region unity methods
    private void Start()
    {
        PlayersManager.instance.AddPlayer(this);
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        playerInput.onActionTriggered += HandleInput;
        GameManager.OnStateChange += RestoreReadyValue;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= HandleInput;
        GameManager.OnStateChange -= RestoreReadyValue;
    }

    private void Update()
    {
        //trabajar animaciones
        SetAnimations();
        if (GameManager.CurrentState == GameState.Gameplay && state == PlayerState.enable)
        {
            directionObj.gameObject.SetActive(true);
            RotatePlayer(velocity.x);
            Aceelerate();
            brake();
            if (inMud) { currentSpeed = Mathf.Clamp(currentSpeed, 0, mudMaxSpeed); } else { currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed); }
        }
        else
        {
            directionObj.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.CurrentState == GameState.Gameplay && state == PlayerState.enable)
        {
            float totalForces = currentSpeed + aditionalsForces;
            if(totalForces < 0) { totalForces = 0; }
            if (!inRecoil)
            {
                rb.linearVelocity = transform.up * totalForces;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (inRecoil == false)
            {
                if (canGetDamage)
                {
                    GetDamage(wallCollisionDamage * (currentSpeed / maxSpeed));
                }
                StartCoroutine(Recoil(collision.gameObject));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Interactable interactable))
        {
            interactable.Interact(this.gameObject);
        }
    }
    #endregion

    #region inputs

    void HandleInput(InputAction.CallbackContext context)
    {
        MoveInput(context);
        AcelerateInput(context);
        BrakeInput(context);
        if (slot != null) { ChangeCharacterInput(context); }
        AttackInput(context);
        PressStart(context);
    }

    void MoveInput(InputAction.CallbackContext context)
    {
        if(context.action.name != "Move" || canMove == false) { return; }

        Vector2 move = context.ReadValue<Vector2>();
        velocity = move;
    }

    void AcelerateInput(InputAction.CallbackContext context)
    {
        if(context.action.name != "Acelerate") return;

        if (context.started && canMove == true)
        {
            acelerated = true;
        }
        if (context.performed)
        {
            acelerating = true;
        }

        if(context.canceled)
        {
            acelerated = false;
            acelerating = false;
        }
    }

    void AttackInput(InputAction.CallbackContext context)
    {
        if (context.action.name != "Attack") return;

        if (context.started)
        {
            //usar objeto
            UseItem();
        }
    }

    void BrakeInput(InputAction.CallbackContext context)
    {
        if(context.action.name != "Brake") return;

        if (context.started)
        {
            braking = true;
        }
        if (context.canceled)
        {
            braking = false;
        }
    }

    void ChangeCharacterInput(InputAction.CallbackContext context)
    {
        if (context.action.name != "CharacterSelector") { return; }

        slot.TryChangeCharacter(context);
    }

    void PressStart(InputAction.CallbackContext context)
    {
        if (context.action.name != "Start") { return; }

        if (context.started)
        {
            if(LevelsManager.instance.Pressed == false)
            {
                isReady = !isReady;
                if (slot != null) { slot.SetReady(isReady); }
            }
            onPressStart?.Invoke();
        }
    }
    #endregion

    #region mecanics
    void RotatePlayer(float moveX)
    {
        float rotationAmount = -moveX * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationAmount, Space.Self);
    }
    void Aceelerate()
    {
        if(acelerated)
        {
            if(!canMove) { return; }
            //aumentar gradualmente currentSpeed hasta llegar a MaxSpeed
            if(currentSpeed < maxSpeed)
            {
                currentSpeed += acelerationSpeed * Time.deltaTime;
            }
            else
            {
                currentSpeed = maxSpeed;
            }
        }
        else
        {
            //disminuir gradualmente currentSpeed hasta llegar a 0
            if (currentSpeed > 0)
            {
                currentSpeed -= decelerationSpeed * Time.deltaTime;
            }
            else
            {
                currentSpeed = 0;
            }
        }
    }

    void brake()
    {
        if (braking)
        {
            currentSpeed -= brakingForce * Time.deltaTime;
        }
    }

    void UseItem()
    {
        if(currentItem == null) {  return; }
        GameObject item = currentItem.gameObject;
        GameObject obj = null;
        if(currentItem.type == ItemType.spawneable)
        {
            if (currentItem.spawnPos == SpawnPos.Front)
            {
                obj = Instantiate(item, frontSpawn.position, Quaternion.identity);
            }
            else if (currentItem.spawnPos == SpawnPos.Back)
            {
                obj = Instantiate(item, BackSpawn.position, Quaternion.identity);
            }
            obj.GetComponent<Item>().SetPlayerUser(this);
        }
        else if(currentItem.type == ItemType.utility)
        {
            currentItem.SetPlayerUser(this);
            currentItem.Use();
        }
        currentItem = null;
        onLoseItem?.Invoke();
    }

    public void GetItem(SOitem _item)
    {
        currentItem = _item.Prefab;
        onGetItem?.Invoke(_item);
    }

    public void ActivateShield(bool _activate)
    {
        if(_activate == true)
        {
            //activar escudo
            shieldActivated = true;
            shieldObj.SetActive(true);
        }
        else
        {
            //desactivar escudo
            shieldActivated = false;
            shieldObj.SetActive(false);
        }
    }

    public void BurstAcelerate(float burstTime)
    {
        StartCoroutine(TimeToBurst(burstTime));
    }

    IEnumerator TimeToBurst(float burstTime)
    {
        aditionalsForces += aditionalAceleration;
        isBursting = true;

        //espera el tiempo que dura la acceleracion
        yield return new WaitForSeconds(burstTime);

        if (isBursting)
        {
            aditionalsForces -= aditionalAceleration;
            isBursting = false;
        }
    }

    #endregion
    public void RestoreValues()
    {
        currentSpeed = 0;
        currentControlPoints = 0;
        currentLap = 0;
        velocity = Vector3.zero;
        currentItem = null;
        CurrentHealth = maxHealth;
        rb.linearVelocity = Vector3.zero;
        acelerated = false;
        win = false;
        //transform.rotation = Quaternion.identity;
        spriteRenderer.transform.localRotation = Quaternion.identity;
    }

    public void ChangeCar(SOcars car)
    {
        spriteRenderer.sprite = car.icon;
        animator.runtimeAnimatorController = car.animator;

        maxSpeed = car.defaultSpeed;
        acelerationSpeed = car.aceleration;
        rotationSpeed = car.rotationSpeed;
        maxHealth = car.defaultHealth;
        attackItem = car.attack;
    }

    public void ChangeCoins(int value)
    {
        currentCoins += value;
        if(currentCoins < 0) {currentCoins = 0; }
        onChangeCoin?.Invoke(value);
    }

    public void MudState(bool _state)
    {
        inMud = _state;
        mudMaxSpeed = (int)maxSpeed/3;
    }

    public void SwitchState(PlayerState _state)
    {
        state = _state;

        if(state == PlayerState.disable)
        {
            spriteRenderer.gameObject.SetActive(false);
            playerColl.enabled = false;
        }
        else if(state == PlayerState.enable)
        {
            spriteRenderer.gameObject.SetActive(true);
            playerColl.enabled = true;
        }
    }

    void SetAnimations()
    {
        spriteRenderer.gameObject.transform.rotation = Quaternion.identity;

        Vector2 dir = transform.up;

        animator.SetFloat("InputX", dir.x);
        animator.SetFloat("InputY", dir.y);
        if(GameManager.CurrentState == GameState.Gameplay)
        {
            if (acelerated)
            {
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Running", false);
            }
        }
        else
        {
            animator.SetBool("Running", false);
        }

        OutLineSprite();
    }

    void OutLineSprite()
    {
        outline.sprite = spriteRenderer.sprite;
        outline.color = colorPlayer;
        outline.sortingOrder = spriteRenderer.sortingOrder -1;
    }

    public void setColor(Color _color)
    {
        colorPlayer = _color;
    }

    void RestoreReadyValue(GameState state)
    {
        isReady = false;
    }

    #region Damage
    IEnumerator Recoil(GameObject other)
    {
        canMove = false;
        inRecoil = true;
        float speedValue = (currentSpeed / maxSpeed);
        if(isBursting) { aditionalsForces = 0; isBursting = false;}
        currentSpeed = 0;
        rb.linearVelocity = Vector3.zero;
        velocity = Vector3.zero;
        Vector2 direction = -transform.up;
        rb.AddForce(direction * recoilForce * speedValue, ForceMode2D.Impulse);
        yield return new WaitForSeconds(timeToRecoil);
        canMove = true;
        inRecoil = false;
    }

    public void LoseControl()
    {
        if(rotating == false)
        {
            StartCoroutine(RotateLoseControl());
        }
    }

    public IEnumerator RotateLoseControl()
    {
        rotating = true;
        canMove = false;
        acelerated = false;

        float totalGrades = 0f;
        float CountGrades = 2 * 360f;

        while (totalGrades < CountGrades)
        {
            float increment = angularVelocity * Time.deltaTime;

            totalGrades += increment;
            spriteRenderer.transform.Rotate(0, 0, increment);

            yield return null;
        }

        canMove = true;
        acelerated = acelerating;
        rotating = false;
        spriteRenderer.transform.localRotation = Quaternion.identity;
    }

    public void GetDamage(float _damage)
    {
        if (canGetDamage)
        {
            if (shieldActivated)
            {
                ActivateShield(false);
                Debug.Log("bloqueado");
            }
            else
            {
                Debug.Log("me he hecho daño");
                CurrentHealth -= _damage;
                //feedback de recibir daño
            }
        }
    }

    void Die()
    {
        animator.Play("PlayerDeath");
        canGetDamage = false;
        playerColl.enabled = false;
        canMove = false;
        velocity = Vector3.zero;
        currentSpeed = 0f;
        rb.linearVelocity = Vector3.zero;
    }

    //funcion llamada de la animacion playerDeath
    public void Respawn()
    {
        StartCoroutine(WaitForRespawn());
    }

    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(timeToRespawn);

        animator.Play("PlayerRespawn");
        playerColl.enabled = true;
        canMove = true;
        //duracion de la animacion de respawn
        yield return new WaitForSeconds(.40f);
        canGetDamage = true;
        currentHealth = maxHealth;
    }
    #endregion
}
