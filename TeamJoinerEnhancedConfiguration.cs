using System.Collections.Generic;
using Rocket.API;
using Steamworks;

namespace PhaserArray.TeamJoinerEnhanced
{
	public class TeamJoinerEnhancedConfiguration : IRocketPluginConfiguration
	{
		public bool WipePermissionsGroups;

		public string NoTeamMessage;
		public string NoTeamMessageColor;

		public string NoTeamPopupMessage;
		public string NoTeamPopupUrl;

		public string PermissionsWipedMessage;
		public string PermissionsWipedColor;

		public List<string> IgnoredPermissionsGroups;
		public List<TeamJoinerEnhancedConfigurationTeam> Teams;

		public void LoadDefaults()
		{
			WipePermissionsGroups = true;

			NoTeamMessage =
				"You are not currently in a team. Please ensure you have joined one of the Steam groups and set it as your active group in the main menu under survivors and then group.";
			NoTeamMessageColor = "#ffeb04";

			NoTeamPopupMessage = "You have not joined a team. Click agree to open a steam guide on how to do that!";
			NoTeamPopupUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=1928426601";

			PermissionsWipedMessage =
				"You were previously on a different team. Permissions do not carry over between sides, the following permission group(s) were removed: {0}";
			PermissionsWipedColor = "#ff0000";

			IgnoredPermissionsGroups = new List<string>
			{
				"mod",
				"moderator",
				"admin",
				"administrator"
			};

			Teams = new List<TeamJoinerEnhancedConfigurationTeam>
			{
				new TeamJoinerEnhancedConfigurationTeam(
					"allies",
					"You have joined the allies and have been given the appropriate permissions group.",
					"#0000ff",
					(CSteamID) 103582791465718191u
					),
				new TeamJoinerEnhancedConfigurationTeam(
					"axis",
					"You have joined the axis and have been given the appropriate permissions group.",
					"#0000ff",
					(CSteamID) 103582791465717990u
					)
			};
		}

		public struct TeamJoinerEnhancedConfigurationTeam
		{
			public string PermissionGroup;
			public string WelcomeMessage;
			public string WelcomeMessageColor;
			public CSteamID SteamGroupId;

			public TeamJoinerEnhancedConfigurationTeam(string permissionGroup, string welcomeMessage, string welcomeMessageColor, CSteamID steamGroupId)
			{
				PermissionGroup = permissionGroup;
				WelcomeMessage = welcomeMessage;
				WelcomeMessageColor = welcomeMessageColor;
				SteamGroupId = steamGroupId;
			}
		}
	}
}
