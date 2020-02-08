using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PhaserArray.TeamJoinerEnhanced
{
	public class TeamJoinerEnhancedConfiguration : IRocketPluginConfiguration
	{
		public bool WipePermissionGroups;

		public string NoTeamPopupUrl;

		[XmlArrayItem(ElementName = "IgnoredPermissionGroupId")]
		public List<string> IgnoredPermissionGroupIds;
		[XmlArrayItem(ElementName = "Team")]
		public List<TeamJoinerEnhancedConfigurationTeam> Teams;

		public void LoadDefaults()
		{
			WipePermissionGroups = true;

			NoTeamPopupUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=1928426601";

			IgnoredPermissionGroupIds = new List<string>
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
					"apvt",
					103582791465718191u,
					new List<string>
					{
						"apvt",
						"apfc",
						"acpl",
						"asgt",
						"assgt",
						"a1sgt",
						"amsgt",
						"a2lt",
						"a1lt",
						"acpt",
						"amaj",
						"altc",
						"acol",
						"abg"
					}
				),
				new TeamJoinerEnhancedConfigurationTeam(
					"gpvt",
					103582791465717990u,
					new List<string>
					{
						"gpvt",
						"gpfc",
						"gcpl",
						"gsgt",
						"gssgt",
						"g1sgt",
						"gmsgt",
						"g2lt",
						"g1lt",
						"gcpt",
						"gmaj",
						"gltc",
						"gcol",
						"gbg"
					}
				)
			};
		}

		public struct TeamJoinerEnhancedConfigurationTeam
		{
			public string GivenPermissionGroupId;
			public ulong SteamGroupId;

			[XmlArrayItem(ElementName = "FriendlyPermissionGroupId")]
			public List<string> FriendlyPermissionGroupIds;

			public TeamJoinerEnhancedConfigurationTeam(string givenPermissionGroupId, ulong steamGroupId, List<string> friendlyPermissionGroupIds)
			{
				GivenPermissionGroupId = givenPermissionGroupId;
				SteamGroupId = steamGroupId;
				FriendlyPermissionGroupIds = friendlyPermissionGroupIds;
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