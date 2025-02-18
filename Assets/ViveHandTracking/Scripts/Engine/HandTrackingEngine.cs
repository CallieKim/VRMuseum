using System.Collections;
using UnityEngine;

namespace ViveHandTracking {

public class EngineState {
  public GestureMode Mode = GestureMode.Skeleton;
  public GestureStatus Status = GestureStatus.NotStarted;
  public GestureFailure Error = GestureFailure.None;

  public GestureResult LeftHand = null;
  public GestureResult RightHand = null;
  public bool UpdatedInThisFrame = false;

  public void ClearState() {
    Status = GestureStatus.NotStarted;
    UpdatedInThisFrame = false;
    Error = GestureFailure.None;
    LeftHand = RightHand = null;
  }
}

public abstract class HandTrackingEngine: ScriptableObject {
  // holds hand state and result, shares same reference to GestureProvider
  protected internal EngineState State = null;

  // if supported for current platform, called first
  public abstract bool IsSupported();

  // setup engine once, e.g. grant camera permission, called before StartDetection
  // if setup failed, set status to error
  public abstract IEnumerator Setup();

  // start detection with given option, change state for status and error as return value
  public abstract IEnumerator StartDetection(GestureOption option);

  // update detection result, change state as return value
  public abstract void UpdateResult();

  // stop detection
  public abstract void StopDetection();

  // description used in the editor settings windows
  public virtual string Description() {
    return "";
  }
}

}
