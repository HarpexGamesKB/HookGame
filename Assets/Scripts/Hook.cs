using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hook : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private SpringJoint joint;
    [SerializeField] private Collider Collider;
    [SerializeField] private Collider Trigger;
    [SerializeField] private float speed;

    [SerializeField] private RopeRenderer ropeRenderer;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Text fuelText;

    [SerializeField] private float _length;
    public float radius;
    public float shotTime;
    public float maxDistance = 6;
    public float distanceToStartPoint=1f;
    public int fuel = 5;
    public int maxFuel = 5;

    public int maxCapacity = 8;
    public int currentCapacity = 0;
    [SerializeField] private List<Collider> colliders;
    [SerializeField] private Transform radiusCenter;
    public Transform HookBody;

    private Quaternion startRotation;
    private Vector3 startPosition;

    private Vector3 touchPosition;

    [SerializeField] private bool getBack;
    [SerializeField] private float maxXpos = -4f;
    public bool CanShoot = true;
    public bool CanRotate = true;
    public bool IsReseted;
    private StressReceiver StressReceiver;
    private void Start()
    {
        StressReceiver = FindObjectOfType<StressReceiver>();
        startPosition = transform.position;
        startRotation = transform.rotation;
        FuelUpgrade(0);
        ropeRenderer.lenth = shotTime * 20f;

        fuelSlider.value = Mathf.Lerp(fuelSlider.value, fuel, 2f * Time.deltaTime);
        maxDistance = shotTime * 25f;
    }
    private void ResetHook()
    {
        if (Vector3.Distance(transform.position, startPosition) < 0.02f)
        {
            Trigger.enabled = false;
            IsReseted = true;
            //transform.SetPositionAndRotation(startPosition, startRotation);
            GetDownObjects();
            CanShoot = true;
            getBack = false;
            maxDistance = shotTime * 25f;
            distanceToStartPoint = 1f;
        }

    }
    private void Update()
    {
        fuelSlider.value = Mathf.Lerp(fuelSlider.value, fuel, 2f * Time.deltaTime);
        //just throwed
        if (CanShoot == false && getBack == false)
        {
            IsReseted = false;

            ropeRenderer.lenth = Mathf.Lerp(ropeRenderer.lenth, maxDistance, 10f * Time.deltaTime);

        }
        else if (CanShoot == false && getBack == true)//getting back
        {
            
            joint.maxDistance = Mathf.Lerp(joint.maxDistance, 0f, 1.2f * Time.deltaTime);
            ropeRenderer.lenth = Mathf.Lerp(ropeRenderer.lenth, 1f, 10f * Time.deltaTime);
            CollectInRadius();
            if (Vector3.Distance(transform.position, startPosition) < distanceToStartPoint)
            {
                MoveToStartPosition();
                if (!IsReseted)
                {
                    ResetHook();
                }
            }
        }
        else if (transform.rotation != startRotation)
        {
            MoveToStartPosition();
        }
        if (IsReseted)
        {
            ResetHook();
        }
    }
    private void MoveToStartPosition()
    {
        Rigidbody.isKinematic = true;
        Collider.enabled = false;

        transform.position = Vector3.MoveTowards(transform.position, startPosition, 7f * Time.deltaTime);
        if(CanRotate&& transform.position == startPosition)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, 10f * Time.deltaTime);
        }
        

    }
    public void FuelChange(int value)
    {
        fuel += value;
        fuelText.text = fuel.ToString();
    }
    public void FuelUpgrade(int value)
    {
        maxFuel += value;
        fuel = maxFuel;
        fuelSlider.maxValue = maxFuel;
        fuelSlider.value = fuel;
        fuelText.text = fuel.ToString();
    }
    public void ThrowHook(Vector3 targetPosition)
    {

        if (CanShoot)
        {
            if (fuel > 0)
            {
                Trigger.enabled = true;
                joint.maxDistance = maxDistance;
                CanShoot = false;
                transform.LookAt(targetPosition);
                touchPosition = targetPosition;
                Rigidbody.isKinematic = false;
                Rigidbody.AddForce(Rigidbody.mass * speed * transform.forward, ForceMode.Impulse);
                FuelChange(-1);
                StressReceiver.InduceStress(0.1f);
                Invoke(nameof(GetBackHook), shotTime);
            }
        }
    }
    public void GetBackHook()
    {
        Collider.enabled = true;
        CollectInRadius();
        getBack = true;
        StressReceiver.InduceStress(0.1f);
    }
    private void CollectInRadius()
    {
        Collider[] objectsInSphere = Physics.OverlapSphere(radiusCenter.position, radius);
        foreach (Collider obj in objectsInSphere)
        {
            if (currentCapacity >= maxCapacity)
            {
                break;
            }

            Brick brick = obj.GetComponent<Brick>();

            if (brick)
            {
                brick.gameObject.layer = 6;

                if (brick.Rotated)
                {
                    continue;
                }
                else
                {

                    if (HookBody.localScale.x < brick.Power)
                    {
                        continue;
                    }
                    else
                    {
                        colliders.Add(obj);
                        currentCapacity += brick.Mass;
                        brick.Freeze();
                        obj.transform.parent = transform;
                        brick.RotateRandom();
                    }
                }
            }
        }
    }

    private void GetDownObjects()
    {
        foreach (Collider obj in colliders)
        {
            if (obj == null) continue;
            obj.transform.parent = null;
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0f);
            obj.GetComponent<Brick>().Unfreeze();
            obj.GetComponent<Brick>().UnRotated();
        }
        currentCapacity = 0;
        colliders.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody contactBody = other.gameObject.GetComponent<Rigidbody>();

        if (contactBody)
        {
            Brick brick = other.GetComponent<Brick>();
            if (brick && brick.Freezed == false)
            {

                if (HookBody.localScale.x > brick.Power)
                {
                    contactBody.isKinematic = false;
                    //brick.CreateOnContactEffect();
                }
            }
        }
        else
        {
            Border border = other.gameObject.GetComponent<Border>();
            if (border)
            {
                GetBackHook();
                distanceToStartPoint = 999f;
            }
        }

    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(radiusCenter.position, radius);
    }
#endif
}
