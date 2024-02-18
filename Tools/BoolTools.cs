public static class BoolTools
{
	/// <summary>
	/// Returns true/false from int (1/0)
	/// </summary>
	/// <param name="integer">1=true 0=false</param>
	/// <returns>ints bool value</returns>
    public static bool IntToBool(int integer) {
		if (integer > 0) return true;
		else return false;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="boolToInt"></param>
	/// <returns>1=true 0=false</returns>
	public static int BoolToInt(bool boolToInt) {
		if (boolToInt) return 1; else return 0;
	}
}
