using Photon.Deterministic;
using Quantum;

public static class BushExtensions
{
	public static void RevealFromBush(this EntityRef entity, Frame f, FP time)
	{
		f.Set(entity, new BushTemporaryReveal { Value = time });
	}

	public static bool IsVisibleFromBush(this EntityRef entity, Frame f)
	{
		return !f.Has<InBush>(entity) || f.Has<BushTemporaryReveal>(entity);
	}
}