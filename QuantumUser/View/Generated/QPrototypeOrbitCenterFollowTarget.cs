// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial
// declarations in another file.
// </auto-generated>
#pragma warning disable 0109
#pragma warning disable 1591


namespace Quantum {
  using UnityEngine;
  
  [UnityEngine.DisallowMultipleComponent()]
  public unsafe partial class QPrototypeOrbitCenterFollowTarget : QuantumUnityComponentPrototype<Quantum.Prototypes.OrbitCenterFollowTargetPrototype>, IQuantumUnityPrototypeWrapperForComponent<Quantum.OrbitCenterFollowTarget> {
    partial void CreatePrototypeUser(Quantum.QuantumEntityPrototypeConverter converter, ref Quantum.Prototypes.OrbitCenterFollowTargetPrototype prototype);
    [DrawInline()]
    [ReadOnly(InEditMode = false)]
    public Quantum.Prototypes.Unity.OrbitCenterFollowTargetPrototype Prototype;
    public override System.Type ComponentType {
      get {
        return typeof(Quantum.OrbitCenterFollowTarget);
      }
    }
    public override ComponentPrototype CreatePrototype(Quantum.QuantumEntityPrototypeConverter converter) {
      Quantum.Prototypes.OrbitCenterFollowTargetPrototype result;
      converter.Convert(Prototype, out result);
      CreatePrototypeUser(converter, ref result);
      return result;
    }
  }
}
#pragma warning restore 0109
#pragma warning restore 1591
