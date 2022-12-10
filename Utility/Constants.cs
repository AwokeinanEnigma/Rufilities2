using System;

namespace Rufilities.Utility
{
	internal static class Constants
	{
		// Note: this type is marked as 'beforefieldinit'.
		static Constants()
		{
			int[] array = new int[2];
			array[0] = int.MinValue;
			Constants.NEGATIVE_INTEGERS_WITH_ZERO = array;
			Constants.POSITIVE_INTEGERS = new int[]
			{
				1,
				int.MaxValue
			};
			Constants.NEGATIVE_INTEGERS = new int[]
			{
				int.MinValue,
				-1
			};
		}

		public static readonly string[] CHARACTERS = new string[]
		{
			"Travis",
			"Floyd",
			"Meryl",
			"Leo",
			"Zack",
			"Renee"
		};

		public static readonly string[] DIRECTIONS = new string[]
		{
			"East",
			"Northeast",
			"North",
			"Northwest",
			"West",
			"Southwest",
			"South",
			"Southeast"
		};

		public static readonly string[] DIRECTIONS_WITH_FACE = new string[]
		{
			"East",
			"Northeast",
			"North",
			"Northwest",
			"West",
			"Southwest",
			"South",
			"Southeast",
			"Face Player"
		};

		public static readonly string[] STATS = new string[]
		{
			"Current HP",
			"Current PP",
			"Max HP",
			"Max PP",
			"Offense",
			"Defense",
			"Speed",
			"Guts",
			"IQ",
			"Luck",
			"Current GP",
			"Level"
		};

		public static readonly string[] COMPARISONS = new string[]
		{
			"Equal (=)",
			"Not equal (≠)",
			"Less than or equal to (≤)",
			"Greater than or equal to (≥)",
			"Less than (<)",
			"Greater than (>)"
		};

		public static readonly string[] WEATHER = new string[]
		{
			"Clear",
			"Light Rain",
			"Normal Rain",
			"Heavy Rain",
			"Storm",
			"Light Snow",
			"Normal Snow",
			"Heavy Snow",
			"Blizzard",
			"Blowing Leaves",
			"Blowing Petals"
		};

		public static readonly string[] DISTORTION = new string[]
		{
			"None",
			"Underwater",
			"Heat",
			"OMGWTFBBQ"
		};

		public static readonly string[] TIMES = new string[]
		{
			"Day",
			"Night"
		};

		public static readonly string[] BATTLE_TYPES = new string[]
		{
			"Normal",
			"Player Advantage",
			"Enemy Advantage",
			"Boss"
		};

		public static readonly string[] TRANSITIONS = new string[]
		{
			"None",
			"Fade Through Black",
			"Fade Through White",
			"Iris"
		};

		public static readonly string[] FLYOVER_LOCATIONS = new string[]
		{
			"Center",
			"Top-Left",
			"Top",
			"Top-Right",
			"Left",
			"Right",
			"Bottom-Left",
			"Bottom",
			"Bottom-Right"
		};

		public static readonly string[] FONT = new string[]
		{
			"Normal",
			"Flyover",
			"Mr. Saturn"
		};

		public static readonly string[] SMOOTHING = new string[]
		{
			"Linear",
			"Smooth",
			"Exponential In",
			"Exponential Out"
		};

		public static readonly string[] MOVE_MODES = new string[]
		{
			"None",
			"Random Turn",
			"Face Player",
			"Random Movement",
			"Path (constraint is path name)",
			"Area (constraint is area name)"
		};

		public static readonly string[] PSI_LEVELS = new string[]
		{
			"α",
			"β",
			"γ",
			"Ω",
			"Σ",
			"π",
			"λ",
			"××"
		};

		public static readonly Tuple<string, string>[] CONTROL_CODES = new Tuple<string, string>[]
		{
			new Tuple<string, string>("Pause", "[p:10]"),
			new Tuple<string, string>("Wait for Button", "[b]"),
			new Tuple<string, string>("Insert Graphic", "[g:subsprite]"),
			new Tuple<string, string>("Text Color", "[c:FFFF00FF]"),
			new Tuple<string, string>("Text Sound", "[ts:1]"),
			new Tuple<string, string>("Trigger", "[t:0,0]")
		};

		public static readonly int[] POSITIVE_INTEGERS_WITH_ZERO = new int[]
		{
			0,
			int.MaxValue
		};

		public static readonly int[] NEGATIVE_INTEGERS_WITH_ZERO;

		public static readonly int[] POSITIVE_INTEGERS;

		public static readonly int[] NEGATIVE_INTEGERS;
	}
}
