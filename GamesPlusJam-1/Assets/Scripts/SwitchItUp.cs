using UnityEngine;
using TMPro;
using System.Collections;

public enum SwitchUp { Normal, UpsideDownCam, InvertControls, PlayerModelInvisible, MinusObjectScore, SmallCameraFOV, PlatformDeath, PlayerJumpHeight, PlayerMovementSpeed, BackgroundChange, RogueObject, HulkSmash, RemoveAllObjects };

public class SwitchItUp : MonoBehaviour
{
    public static SwitchItUp instance;

    [SerializeField] private TMP_Text switchUpText;

    [Header("Current Switch Up")]
    [SerializeField] private SwitchUp currentSwitchUp = SwitchUp.Normal;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    private Quaternion currentRot;
    private Quaternion rotatedRot;
    [SerializeField] private float camRotateSpeed;
    [SerializeField] private bool isCameraRotating = false;
    [SerializeField] private bool isCameraAtDesiredRot = false;
    [SerializeField] private bool isCameraRotated = false;

    [Header("Inverted Control")]
    [SerializeField] private GameObject player;
    private PlayerController controller;
    [SerializeField] private bool isInvertControls = false;

    [Header("Player Model Invisible")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private bool isPlayerModelInvisible = false;

    [Header("Minus Points")]
    [SerializeField] private bool takePoints = false;

    [Header("Camera FOV")]
    [SerializeField] private float currentFOV;
    [SerializeField] private float smallFOV;
    [SerializeField] private float normalFOV;
    [SerializeField] private float FOVSpeed;
    [SerializeField] private bool isFOVChanged = false;
    private CameraFollow camFollow;

    [Header("Platform Death")]
    [SerializeField] private GameObject[] grounds;
    [SerializeField] private GameObject selectedGround;
    [SerializeField] private Material[] materials;
    [SerializeField] private Renderer rend;
    [SerializeField] private bool hasPlatform = false;

    [Header("Player Jump")]
    [SerializeField] private float currentJumpForce;
    [SerializeField] private float jupiterJumpForce;
    [SerializeField] private float normalJumpForce;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private bool isJupiterJumpForce = false;

    [Header("Player Movement")]
    [SerializeField] private float currentMovementSpeed;
    [SerializeField] private float fastMovementSpeed;
    [SerializeField] private float normalMovementSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isMoveFast = false;

    [Header("Background Colour")]
    [SerializeField] private Color currentBGColour;
    [SerializeField] private Color changeBGTo;
    [SerializeField] private float backgroundSpeed;
    [SerializeField] private bool isBackgroundChanged = false;

    [Header("Rogue Object")]
    [SerializeField] private bool isObjectSpawned = false;
    [SerializeField] private bool hasRogueObjectSpawned = false;
    private SpawnObjects spawn;

    [Header("Hulk Smash")]
    [SerializeField] private float currentThrowingForce;
    [SerializeField] private float hulkThrowingForce;
    [SerializeField] private float normalThrowingForce;
    [SerializeField] private float throwSpeed;
    [SerializeField] private bool isThrowingChanged = false;
    private PickUpObject pickup;

    [Header("Remove All Objects")]
    [SerializeField] private bool hasAllObjectsBeenRemoved = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;

        player = GameObject.FindGameObjectWithTag("Player");

        controller = player.GetComponent<PlayerController>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        camFollow = cam.GetComponent<CameraFollow>();

        pickup = player.GetComponent<PickUpObject>();

        normalJumpForce = controller.jumpForce;
        normalMovementSpeed = controller.movementSpeed;
        normalFOV = cam.orthographicSize;
        normalThrowingForce = pickup.throwForce;

        currentBGColour = cam.backgroundColor;

        GameObject objectSpawner = GameObject.FindGameObjectWithTag("ObjectSpawner");
        spawn = objectSpawner.GetComponent<SpawnObjects>();
    }

    public void PickRandomSwitchUp()
    {
        int currentlySelectedEnum = ((int)currentSwitchUp);
        int randomValue = Random.Range(0, System.Enum.GetValues(typeof(SwitchUp)).Length);

        if (currentlySelectedEnum != randomValue)
        {
            currentSwitchUp = (SwitchUp)randomValue;

            return;
        }

        PickRandomSwitchUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PickRandomSwitchUp();
        }

        switch (currentSwitchUp)
        {
            case SwitchUp.Normal:

                switchUpText.text = "Phew.. play as normal";
                currentSwitchUp = SwitchUp.Normal;

                ResetOtherEnums();
                break;

            case SwitchUp.UpsideDownCam:

                switchUpText.text = "Camera is upside down. But you knew that.";
                UpsideDownCam();

                ResetOtherEnums();
                break;

            case SwitchUp.InvertControls:

                switchUpText.text = "Controls are inverted. Is it 'a' or 'e'? I'm confused";
                currentSwitchUp = SwitchUp.InvertControls;

                if (!isInvertControls)
                isInvertControls = InvertPlayerControls();

                ResetOtherEnums();

                break;

            case SwitchUp.PlayerModelInvisible:

                switchUpText.text = "Poof!! You're invisible. Spoooooky eyes!";
                MakePlayerModelInvisible();

                ResetOtherEnums();
                break;

            case SwitchUp.MinusObjectScore:

                switchUpText.text = "Points are now minus, or are points worth double? I can't remember";
                MinusPoints();

                ResetOtherEnums();

                break;

            case SwitchUp.SmallCameraFOV:

                switchUpText.text = "Do you need glasses??";
                LimitCameraFOV();

                ResetOtherEnums();
                break;

            case SwitchUp.PlatformDeath:

                switchUpText.text = "Wait! Don't touch the red platform";
                ChangePlatform();

                ResetOtherEnums();

                break;

            case SwitchUp.PlayerJumpHeight:

                switchUpText.text = "Do you like that Jupiter gravity?";
                ApplyJupiterJumpForce();
                ResetOtherEnums();

                break;

            case SwitchUp.PlayerMovementSpeed:

                switchUpText.text = "ZOOOOOOOOOOOOOOOM..";
                ChangeMovementSpeed();
                ResetOtherEnums();

                break;

            case SwitchUp.BackgroundChange:

                switchUpText.text = "Did you pack your sunglasses?";
                ChangeBackgroundColour();
                ResetOtherEnums();

                break;

            case SwitchUp.RogueObject:

                switchUpText.text = "Hmm.. Seems like a rogue object has spawned. I'm guessing you'd better watch out for that one";
                SpawnRogueObject();
                ResetOtherEnums();
                break;

            case SwitchUp.HulkSmash:

                switchUpText.text = "HULK SMASH!! Becareful on where you throw the objects";
                ChangeThrowForce();
                ResetOtherEnums();
                break;

            case SwitchUp.RemoveAllObjects:

                switchUpText.text = "Erm, where did the objects go?";
                NoMoreObjects();
                ResetOtherEnums();
                break;
        }

        if(!isCameraAtDesiredRot && isCameraRotating)
        {
            StartCoroutine(FlipCamera(rotatedRot.eulerAngles));
        }
    }

    private void ResetOtherEnums()
    {
        if (isCameraRotated && currentSwitchUp != SwitchUp.UpsideDownCam)
            ResetUpsideDownCam();

        if (isInvertControls && currentSwitchUp != SwitchUp.InvertControls)
            ResetInvertControls();

        if (isPlayerModelInvisible && currentSwitchUp != SwitchUp.PlayerModelInvisible)
            ResetPlayerModelVisibility();

        if (takePoints && currentSwitchUp != SwitchUp.MinusObjectScore)
            ResetMinusPoints();

        if (isFOVChanged && currentSwitchUp != SwitchUp.SmallCameraFOV)
            ResetCameraFOV();

        if (hasPlatform && currentSwitchUp != SwitchUp.PlatformDeath)
            ResetPlatform();

        if (isJupiterJumpForce && currentSwitchUp != SwitchUp.PlayerJumpHeight)
            ResetJumpHeight();

        if (isMoveFast && currentSwitchUp != SwitchUp.PlayerMovementSpeed)
            ResetMovementSpeed();

        if (isBackgroundChanged && currentSwitchUp != SwitchUp.BackgroundChange)
            ResetBackgroundColour();

        if (isThrowingChanged && currentSwitchUp != SwitchUp.HulkSmash)
            ResetThrowingForce();

        if (hasRogueObjectSpawned && currentSwitchUp != SwitchUp.RogueObject)
            ResetRogueObjectSpawn();

        if (hasAllObjectsBeenRemoved && currentSwitchUp != SwitchUp.RemoveAllObjects)
            ResetRemovedObjects();
    }

    private void ResetInvertControls()
    {
        isInvertControls = InvertPlayerControls();
    }

    private void ResetUpsideDownCam()
    {
        isCameraAtDesiredRot = false;

        StartCoroutine(FlipCamera(new Vector3(0, 0, 0)));
    }

    private void ResetPlayerModelVisibility()
    {
        isPlayerModelInvisible = false;

        playerSprite.enabled = true;
    }

    private void ResetMinusPoints()
    {
        takePoints = Collecting.instance.TogglePoints();
    }

    private void ResetCameraFOV()
    {
        StartCoroutine(SetCameraFOV(normalFOV, false));
    }

    private void ResetPlatform()
    {
        rend = selectedGround.GetComponent<Renderer>();
        rend.sharedMaterial = materials[0];

        selectedGround = null;
        hasPlatform = false;
    }

    private void ResetJumpHeight()
    {
        StartCoroutine(FlipGravity(normalJumpForce));
    }

    private void ResetMovementSpeed()
    {
        StartCoroutine(FlipMovementSpeed(normalMovementSpeed));
    }

    private void ResetBackgroundColour()
    {
        cam.clearFlags = CameraClearFlags.SolidColor;

        cam.backgroundColor = Color.Lerp(changeBGTo, currentBGColour, backgroundSpeed);

        isBackgroundChanged = false;
    }

    private void ResetThrowingForce()
    {
        StartCoroutine(ChangeThrowingForce(normalThrowingForce));
    }

    private void ResetRogueObjectSpawn()
    {
        hasRogueObjectSpawned = false;
    }

    private void ResetRemovedObjects()
    {
        hasAllObjectsBeenRemoved = false;
    }

    private void UpsideDownCam()
    {
        currentSwitchUp = SwitchUp.UpsideDownCam;

        isCameraAtDesiredRot = false;
        isCameraRotated = true;

        StartCoroutine(FlipCamera(new Vector3(0,0,180)));
    }

    private bool InvertPlayerControls()
    {
        return controller.InvertControls();
    }

    private void MakePlayerModelInvisible()
    {
        currentSwitchUp = SwitchUp.PlayerModelInvisible;

        isPlayerModelInvisible = true;

        playerSprite.enabled = false;
    }

    private void MinusPoints()
    {
        currentSwitchUp = SwitchUp.MinusObjectScore;

        if (!takePoints)
            takePoints = Collecting.instance.TogglePoints();
    }

    private void LimitCameraFOV()
    {
        currentSwitchUp = SwitchUp.SmallCameraFOV;

        isFOVChanged = true;

        StartCoroutine(SetCameraFOV(smallFOV, true));
    }

    private void PickRandomPlatform()
    {
        int platform = Random.Range(0, grounds.Length);

        selectedGround = grounds[platform];

        rend = selectedGround.GetComponent<Renderer>();
        rend.sharedMaterial = materials[1];

        hasPlatform = true;
    }

    private void ChangePlatform()
    {
        currentSwitchUp = SwitchUp.PlatformDeath;

        if (!hasPlatform)
            PickRandomPlatform();
    }

    private void ApplyJupiterJumpForce()
    {
        currentSwitchUp = SwitchUp.PlayerJumpHeight;

        isJupiterJumpForce = true;

        StartCoroutine(FlipGravity(jupiterJumpForce));
    }

    private void ChangeMovementSpeed()
    {
        currentSwitchUp = SwitchUp.PlayerMovementSpeed;

        isMoveFast = true;

        StartCoroutine(FlipMovementSpeed(fastMovementSpeed));
    }

    private void ChangeBackgroundColour()
    {
        currentSwitchUp = SwitchUp.BackgroundChange;

        cam.clearFlags = CameraClearFlags.SolidColor;

        cam.backgroundColor = Color.Lerp(currentBGColour, changeBGTo, backgroundSpeed);

        isBackgroundChanged = true;
    }

    private void ChangeThrowForce()
    {
        currentSwitchUp = SwitchUp.HulkSmash;

        isThrowingChanged = true;

        StartCoroutine(ChangeThrowingForce(hulkThrowingForce));
    }

    private void SpawnRogueObject()
    {
        currentSwitchUp = SwitchUp.RogueObject;

        if (!hasRogueObjectSpawned)
        {
            spawn.SpawnRogueObject();
            hasRogueObjectSpawned = true;
        }
    }

    private void NoMoreObjects()
    {
        currentSwitchUp = SwitchUp.RemoveAllObjects;

        if (!hasAllObjectsBeenRemoved)
        {
            spawn.RemoveAllObjects();
            hasAllObjectsBeenRemoved = true;
        }
    }

    IEnumerator FlipCamera(Vector3 desiredRot)
    {
        currentRot = cam.transform.rotation;
        rotatedRot = Quaternion.Euler(desiredRot);

        Quaternion rotateCam = Quaternion.RotateTowards(currentRot, rotatedRot, camRotateSpeed * Time.deltaTime);
        cam.transform.rotation = rotateCam;

        isCameraRotating = true;

        yield return null;

        if (cam.transform.rotation == Quaternion.Euler(desiredRot))
        {
            isCameraAtDesiredRot = true;
            isCameraRotating = false;
        }
    }

    IEnumerator SetCameraFOV(float size, bool disableClamp)
    {
        camFollow.disableClamp = disableClamp;

        currentFOV = cam.orthographicSize;

        float cameraFOV = Mathf.MoveTowards(currentFOV, size, FOVSpeed * Time.deltaTime);
        cam.orthographicSize = cameraFOV;

        yield return null;

        if(cam.orthographicSize == size && !disableClamp)
        {
            isFOVChanged = false;
        }
    }

    IEnumerator FlipGravity(float amount)
    {
        currentJumpForce = controller.jumpForce;

        float jumpForce = Mathf.MoveTowards(currentJumpForce, amount, jumpSpeed * Time.deltaTime);

        controller.jumpForce = jumpForce;

        yield return null;

        if (currentJumpForce == normalJumpForce)
            isJupiterJumpForce = false;
    }

    IEnumerator FlipMovementSpeed(float amount)
    {
        currentMovementSpeed = controller.movementSpeed;

        float movementSpeed = Mathf.MoveTowards(currentMovementSpeed, amount, moveSpeed * Time.deltaTime);

        controller.movementSpeed = movementSpeed;

        yield return null;

        if (currentMovementSpeed == normalMovementSpeed)
            isMoveFast = false;
    }

    IEnumerator ChangeThrowingForce(float amount)
    {
        currentThrowingForce = pickup.throwForce;

        float throwForce = Mathf.MoveTowards(currentThrowingForce, amount, throwSpeed * Time.deltaTime);

        pickup.throwForce = throwForce;

        yield return null;

        if (currentThrowingForce == normalThrowingForce)
            isThrowingChanged = false;
    }
}