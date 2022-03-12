using UnityEngine;
using TMPro;
using System.Collections;

public enum SwitchUp { Normal, UpsideDownCam, InvertControls, PlayerModelInvisible, MinusObjectScore, SmallCameraFOV, PlatformDeath };

public class SwitchItUp : MonoBehaviour
{
    public static SwitchItUp instance;

    [SerializeField] private TMP_Text switchUpText;
    [SerializeField] private SwitchUp previousSwitchUp;

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
        

        normalFOV = cam.orthographicSize;
    }

    private void PreviousSwitchUp()
    {
        if(previousSwitchUp != currentSwitchUp)
        previousSwitchUp = currentSwitchUp;
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

                switchUpText.text = "Play as normal";
                currentSwitchUp = SwitchUp.Normal;

                ResetAll();
                break;

            case SwitchUp.UpsideDownCam:

                switchUpText.text = "Camera is upside down";
                UpsideDownCam();

                if (isInvertControls)
                    ResetInvertControls();

                if (isPlayerModelInvisible)
                    ResetPlayerModelVisibility();

                if (takePoints)
                    ResetMinusPoints();

                if (isFOVChanged)
                    ResetCameraFOV();

                if (hasPlatform)
                    ResetPlatform();

                break;

            case SwitchUp.InvertControls:

                switchUpText.text = "Controls are inverted";
                currentSwitchUp = SwitchUp.InvertControls;

                if (!isInvertControls)
                isInvertControls = InvertPlayerControls();

                if (isCameraRotated)
                    ResetUpsideDownCam();

                if (isPlayerModelInvisible)
                    ResetPlayerModelVisibility();

                if (takePoints)
                    ResetMinusPoints();

                if (isFOVChanged)
                    ResetCameraFOV();

                if (hasPlatform)
                    ResetPlatform();

                break;


            case SwitchUp.PlayerModelInvisible:

                switchUpText.text = "Player model is invisible. Spooky eyes!";
                MakePlayerModelInvisible();

                if (isCameraRotated)
                    ResetUpsideDownCam();

                if (isInvertControls)
                    ResetInvertControls();

                if (takePoints)
                    ResetMinusPoints();

                if (isFOVChanged)
                    ResetCameraFOV();

                if (hasPlatform)
                    ResetPlatform();

                break;

            case SwitchUp.MinusObjectScore:

                switchUpText.text = "Points are now minus, or are points worth double? I can't remember";
                MinusPoints();

                if (isCameraRotated)
                    ResetUpsideDownCam();

                if (isInvertControls)
                    ResetInvertControls();

                if (isPlayerModelInvisible)
                    ResetPlayerModelVisibility();

                if (isFOVChanged)
                    ResetCameraFOV();

                if (hasPlatform)
                    ResetPlatform();

                break;

            case SwitchUp.SmallCameraFOV:

                switchUpText.text = "Hope you can see well";
                LimitCameraFOV();

                if (isCameraRotated)
                    ResetUpsideDownCam();

                if (isInvertControls)
                    ResetInvertControls();

                if (isPlayerModelInvisible)
                    ResetPlayerModelVisibility();

                if (takePoints)
                    ResetMinusPoints();

                if (hasPlatform)
                    ResetPlatform();

                break;

            case SwitchUp.PlatformDeath:

                switchUpText.text = "Wait! Don't touch the red platform";
                ChangePlatform();

                if (isCameraRotated)
                    ResetUpsideDownCam();

                if (isInvertControls)
                    ResetInvertControls();

                if (isPlayerModelInvisible)
                    ResetPlayerModelVisibility();

                if (takePoints)
                    ResetMinusPoints();

                if (isFOVChanged)
                    ResetCameraFOV();

                break;

            default:
                break;
        }

        if(!isCameraAtDesiredRot && isCameraRotating)
        {
            StartCoroutine(FlipCamera(rotatedRot.eulerAngles));
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            PickRandomPlatform();
        }
    }

    private void ResetAll()
    {
        if (isCameraRotated)
            ResetUpsideDownCam();

        if (isInvertControls)
            ResetInvertControls();

        if (isPlayerModelInvisible)
            ResetPlayerModelVisibility();

        if (takePoints)
            ResetMinusPoints();

        if (isFOVChanged)
            ResetCameraFOV();

        if (hasPlatform)
            ResetPlatform();
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
}
