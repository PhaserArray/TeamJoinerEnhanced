using Rocket.API;
using Steamworks;
using System.Collections.Generic;

namespace PhaserArray.TeamJoinerEnhanced
{
	public class TeamJoinerEnhancedConfiguration : IRocketPluginConfiguration
	{
		public bool WipePermissionsGroups;

		public string NoTeamMessage;

		public string NoTeamPopupMessage;
		public string NoTeamPopupUrl;

		public string PermissionsWipedMessage;

		public List<string> IgnoredPermissionsGroupIds;
		public List<TeamJoinerEnhancedConfigurationTeam> Teams;

		public void LoadDefaults()
		{
			WipePermissionsGroups = true;

			NoTeamMessage =
				"You are not currently in a team. Please ensure you have joined one of the Steam groups and set it as your active group in the main menu under survivors and then group.";

			NoTeamPopupMessage = "You have not joined a team. Click agree to open a steam guide on how to do that!";
			NoTeamPopupUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=1928426601";

			PermissionsWipedMessage =
				"You were previously on a different team. Permissions do not carry over between sides, the following permission group(s) were removed: {0}";

			IgnoredPermissionsGroupIds = new List<string>
			{
				"mod",
				"moderator",
				"admin",
				"administrator",
				"default"
			};

			Teams = new List<TeamJoinerEnhancedConfigurationTeam>
			{
				new TeamJoinerEnhancedConfigurationTeam(
					"allies",
					"You have joined the allies and have been given the appropriate permissions group.",
					(CSteamID) 103582791465718191u
					),
				new TeamJoinerEnhancedConfigurationTeam(
					"axis",
					"You have joined the axis and have been given the appropriate permissions group.",
					(CSteamID) 103582791465717990u
					)
			};
		}

		public struct TeamJoinerEnhancedConfigurationTeam
		{
			public string PermissionGroupId;
			public string WelcomeMessage;
			public CSteamID SteamGroupId;

			public TeamJoinerEnhancedConfigurationTeam(string permissionGroupId, string welcomeMessage, CSteamID steamGroupId)
			{
				PermissionGroupId = permissionGroupId;
				WelcomeMessage = welcomeMessage;
				SteamGroupId = steamGroupId;
			}

			public static bool operator ==(TeamJoinerEnhancedConfigurationTeam a,
				TeamJoinerEnhancedConfigurationTeam b)
			{
				return a.SteamGroupId == b.SteamGroupId;
			}

			public static bool operator !=(TeamJoinerEnhancedConfigurationTeam a, TeamJoinerEnhancedConfigurationTeam b)
			{
				return a.SteamGroupId != b.SteamGroupId;
			}
		}
	}
}