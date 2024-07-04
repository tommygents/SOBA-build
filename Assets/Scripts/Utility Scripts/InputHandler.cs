using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputHandler 
{
    public void OnPullStart(InputAction.CallbackContext _context);
    public void OnPullEnd(InputAction.CallbackContext _context);

    public void OnPressStart(InputAction.CallbackContext _context);
    public void OnPress(InputAction.CallbackContext _context);
    public void OnPressEnd(InputAction.CallbackContext _context);
    public void OnPull(InputAction.CallbackContext _context);
    public void OnSquat(InputAction.CallbackContext _context);
    public void OnSquatStart(InputAction.CallbackContext _context);
    public void OnSquatEnd(InputAction.CallbackContext _context);

}


public interface IRunHandler {
    public void OnRunStart(InputAction.CallbackContext _context);
    public void OnRunEnd(InputAction.CallbackContext _context);
    public void OnSprintStart(InputAction.CallbackContext _context);
    public void OnSprintEnd(InputAction.CallbackContext _context);
}