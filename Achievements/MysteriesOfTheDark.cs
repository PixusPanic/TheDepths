using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace TheDepths.Achievements
{
	public class MysteriesOfTheDark : ModAchievement
	{
		public CustomFlagCondition WalkedIntoTheDepths { get; private set; }

		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Explorer);

			WalkedIntoTheDepths = AddCondition();
		}

		public override Position GetDefaultPosition() => new After("MINER_FOR_FIRE");

		public override Position GetAdvisorPosition() => new After("ITS_GETTING_HOT_IN_HERE");
	}
}
