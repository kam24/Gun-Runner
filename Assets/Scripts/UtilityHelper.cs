public class UtilityHelper
{
	public static bool IsEqualLayers(int layer1, int layer2)
	{
		return (layer1 & (1 << layer2)) != 0;
	}
}