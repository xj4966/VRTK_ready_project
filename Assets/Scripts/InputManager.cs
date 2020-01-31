using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Prefabs.CameraRig.UnityXRCameraRig.Input;
using UnityEngine.SpatialTracking;
using Zinnia.Data.Type.Transformation.Conversion;
using Zinnia.Action;
using Zinnia.Data.Type;
using UnityEngine.Events;

[RequireComponent(typeof(BooleanAction))]
public class InputManager : MonoBehaviour
{

  [Serializable]
  public enum hands { Right, Left };
  public hands handedness;

  public VRTK.Prefabs.Locomotion.Movement.AxesToVector3.AxesToVector3Facade LocomotionAxisCalculator;

  [Serializable]
  public class InputMapping
  {
    public AxisType axisType;
    public InputType inputType;

    public string name;

    UnityAxis1DAction axis1D;
    UnityAxis2DAction axis2D;
    UnityButtonAction button;

    public FloatRange activationRange;


    [Tooltip("if not a 1d axis input will never be invokeed")]
    public UnityEventVoid Activated = new UnityEventVoid();
    [Tooltip("if not a 1d axis input will never be invokeed")]
    public UnityEventVoid Deactivated = new UnityEventVoid();
    [Tooltip("if not a 2d axis input will never be invokeed")]
    public UnityEventAxis2D Moved2D = new UnityEventAxis2D();

    public void Setup(hands handedness, GameObject actionContainer)
    {

      switch (axisType)
      {
        case AxisType.Button:
          break;
        case AxisType.Axis1D:
          axis1D = actionContainer.AddComponent<UnityAxis1DAction>();
          axis1D.AxisName = Axis1DTranslation(handedness);
          axis1D.ValueChanged.AddListener(Axis1DUpdate);
          break;
        case AxisType.Axis2D:
          axis2D = actionContainer.AddComponent<UnityAxis2DAction>();
          axis2D = Axis2DTranslationVRTK(axis2D, handedness);
          axis2D.ValueChanged.AddListener(Axis2DUpdate);
          break;
      }
    }

    public void Axis1DUpdate(float data)
    {
      if (data >= activationRange.minimum && data <= activationRange.maximum)
      {
        Activated.Invoke();
      }
      else
      {
        Deactivated.Invoke();
      }
    }

    public void Axis2DUpdate(Vector2 data)
    {
      Moved2D.Invoke(data);
    }

    string Axis1DTranslation(hands handedness)
    {
      switch (inputType)
      {
        case InputType.Hand:
          if (handedness == hands.Right)
            return "VRTK_Axis12_RightGrip";
          return "VRTK_Axis11_LeftGrip";

        case InputType.Trigger:
          if (handedness == hands.Right)
            return "VRTK_Axis10_RightTrigger";
          return "VRTK_Axis9_LeftTrigger";

        default:
          return "";
      }
    }

    UnityAxis2DAction Axis2DTranslationVRTK(UnityAxis2DAction axis, hands handedness)
    {
      UnityAxis2DAction tempAxis = axis;

      if (handedness == hands.Right)
      {
        axis.XAxisName = "VRTK_Axis4_RightHorizontal";
        axis.YAxisName = "VRTK_Axis5_RightVertical";
      }
      else
      {
        axis.XAxisName = "VRTK_Axis1_LeftHorizontal";
        axis.YAxisName = "VRTK_Axis2_LeftVertical";
      }
      return axis;
    }
  }

  #region UnityEvents

  [Serializable]
  public class UnityEventFloat : UnityEvent<float> { }
  [Serializable]
  public class UnityEventAxis2D : UnityEvent<Vector2> { }
  [Serializable]
  public class UnityEventVoid : UnityEvent { }

  #endregion

  #region AxisOptions

  [Serializable]
  public enum AxisType { Button, Axis1D, Axis2D }
  [Serializable]
  public enum InputType { Trigger, Thumb2D, Hand }

  #endregion

  public InputMapping[] inputMappings;
  BooleanAction grabAction;

  private void Start()
  {
    grabAction = GetComponent<BooleanAction>();

    foreach (var InputMap in inputMappings)
    {
      InputMap.Setup(handedness, gameObject);

      if (InputMap.name.Contains("Grab"))
      {
        InputMap.Activated.AddListener(GrabActivated);
        InputMap.Deactivated.AddListener(GrabDeactivated);
      }
      if (InputMap.name.Contains("Move"))
      {
        InputMap.Moved2D.AddListener(UpdateMove);
      }
      if (InputMap.name.Contains("Rotate"))
      {
        InputMap.Moved2D.AddListener(UpdateRot);
      }
    }
  }

  public void GrabActivated()
  {
    grabAction.Receive(true);
  }
  public void GrabDeactivated()
  {
    grabAction.Receive(false);
  }

  public void UpdateMove(Vector2 data)
  {
    LocomotionAxisCalculator.LateralAxis.Receive(data.x);
    LocomotionAxisCalculator.LongitudinalAxis.Receive(data.y);
  }
  public void UpdateRot(Vector2 data)
  {
    LocomotionAxisCalculator.LateralAxis.Receive(data.x);
  }
}
