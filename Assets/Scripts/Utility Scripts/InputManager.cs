using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Reflection;

public class InputManager : MonoBehaviour
{   
public static InputManager Instance { get; private set; }

public event Action<InputAction.CallbackContext> OnPressStart;
public event Action<InputAction.CallbackContext> OnPress;
public event Action<InputAction.CallbackContext> OnPressEnd;
public event Action<InputAction.CallbackContext> OnPull;
public event Action<InputAction.CallbackContext> OnPullStart;
public event Action<InputAction.CallbackContext> OnPullEnd;
public event Action<InputAction.CallbackContext> OnSquat;
public event Action<InputAction.CallbackContext> OnSquatStart;
public event Action<InputAction.CallbackContext> OnSquatEnd;
public event Action<InputAction.CallbackContext> OnRunStart;
public event Action<InputAction.CallbackContext> OnRunEnd;
public event Action<InputAction.CallbackContext> OnSprintStart;
public event Action<InputAction.CallbackContext> OnSprintEnd;



    public event Action OnPause;
public Vector2 moveVector = Vector2.zero;
private bool invertYaxis = false;
private ControlScheme controls;

void Awake()    

    {
        InGameLogger.Instance.Log("Initializing InputManager");
         //first, initialize the instance as a singleton
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }


 // then, initialize the control scheme
 controls = new ControlScheme();
 controls.gameplay.Enable();

 //subscribe to the events
 SubscribeToEvents();

    }

                                                                                            

    public Vector2 GetMoveVector()
    {
     Vector2 _moveVector = controls.gameplay.move.ReadValue<Vector2>();
     if (invertYaxis)
     {
        _moveVector.y = -_moveVector.y;
    }
    return _moveVector;
    }

    public void SetInvertYAxis(bool value)
    {
        invertYaxis = value;
    }

    private void SubscribeToEvents()
{
    controls.gameplay.lightpush.started += ctx => OnPressStart?.Invoke(ctx);
    controls.gameplay.lightpush.canceled += ctx => OnPressEnd?.Invoke(ctx);
    controls.gameplay.lightpush.performed += ctx => OnPress?.Invoke(ctx);

    controls.gameplay.heavypush.started += ctx => OnPressStart?.Invoke(ctx);
    controls.gameplay.heavypush.canceled += ctx => OnPressEnd?.Invoke(ctx);
    controls.gameplay.heavypush.performed += ctx => OnPress?.Invoke(ctx);

    controls.gameplay.lightpull.started += ctx => OnPullStart?.Invoke(ctx);
    controls.gameplay.lightpull.canceled += ctx => OnPullEnd?.Invoke(ctx);
    controls.gameplay.lightpull.performed += ctx => OnPull?.Invoke(ctx);

    controls.gameplay.heavypull.started += ctx => OnPullStart?.Invoke(ctx);
    controls.gameplay.heavypull.canceled += ctx => OnPullEnd?.Invoke(ctx);
    controls.gameplay.heavypull.performed += ctx => OnPull?.Invoke(ctx);

    controls.gameplay.Squat.started += ctx => OnSquatStart?.Invoke(ctx);
    controls.gameplay.Squat.canceled += ctx => OnSquatEnd?.Invoke(ctx);
    controls.gameplay.Squat.performed += ctx => OnSquat?.Invoke(ctx);

    controls.gameplay.Run.started += ctx => OnRunStart?.Invoke(ctx);
    controls.gameplay.Run.canceled += ctx => OnRunEnd?.Invoke(ctx);

    controls.gameplay.Sprint.started += ctx => OnSprintStart?.Invoke(ctx);
    controls.gameplay.Sprint.canceled += ctx => OnSprintEnd?.Invoke(ctx);

    
}

}
