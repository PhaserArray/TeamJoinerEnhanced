using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PhaserArray.TeamJoinerEnhanced
{
	public class TeamJoinerEnhancedConfiguration : IRocketPluginConfiguration
	{
		public bool WipePermissionsGroups;

		public string NoTeamPopupUrl;

		[XmlArrayItem(ElementName = "IgnoredPermissionsGroupId")]
		public List<string> IgnoredPermissionsGroupIds;
		[XmlArrayItem(ElementName = "Team")]
		public List<TeamJoinerEnhancedConfigurationTeam> Teams;

		public void LoadDefaults()
		{
			WipePermissionsGroups = true;

			NoTeamPopupUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=1928426601";

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
					103582791465718191u
					),
				new TeamJoinerEnhancedConfigurationTeam(
					"axis",
					103582791465717990u
					)
			};
		}

		public struct TeamJoinerEnhancedConfigurationTeam
		{
			public string PermissionGroupId;
			public ulong SteamGroupId;

			public TeamJoinerEnhancedConfigurationTeam(string permissionGroupId, ulong steamGroupId)
			{
				PermissionGroupId = permissionGroupId;
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