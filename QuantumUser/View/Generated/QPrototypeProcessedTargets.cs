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
  public unsafe partial class QPrototypeProcessedTargets : QuantumUnityComponentPrototype<Quantum.Prototypes.ProcessedTargetsPrototype>, IQuantumUnityPrototypeWrapperForComponent<Quantum.ProcessedTargets> {
    partial void CreatePrototypeUser(Quantum.QuantumEntityPrototypeConverter converter, ref Quantum.Prototypes.ProcessedTargetsPrototype prototype);
    [DrawInline()]
    [ReadOnly(InEditMode = false)]
    public Quantum.Prototypes.Unity.ProcessedTargetsPrototype Prototype;
    public override System.Type ComponentType {
      get {
        return typeof(Quantum.ProcessedTargets);
      }
    }
    public override ComponentPrototype CreatePrototype(Quantum.QuantumEntityPrototypeConverter converter) {
      Quantum.Prototypes.ProcessedTargetsPrototype result;
      converter.Convert(Prototype, out result);
      CreatePrototypeUser(converter, ref result);
      return result;
    }
  }
}
#pragma warning restore 0109
#pragma warning restore 1591
