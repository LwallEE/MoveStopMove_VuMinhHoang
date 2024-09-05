using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using UnityEngine;

public class UICanvasWorld : Singleton<UICanvasWorld>
{
   public Canvas MainCanvas { get; private set; }
   protected override void Awake()
   {
      base.Awake();
      MainCanvas = GetComponent<Canvas>();
   }
}
