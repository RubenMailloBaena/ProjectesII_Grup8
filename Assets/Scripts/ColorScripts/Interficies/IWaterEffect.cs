using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaterEffect : IColorEffect
{
    void InitializeEffect(GameObject target);
    void ApplyEffect(GameObject target);
    void RemoveEffect(GameObject target);
}
