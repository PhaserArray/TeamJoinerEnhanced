using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace PhaserArray.TeamJoinerEnhanced
{
	public class TeamJoinerEnhanced : RocketPlugin<TeamJoinerEnhancedConfiguration>
	{
		public const string Version = "v1.0";
		private TeamJoinerEnhancedConfiguration _config;

		protected override void Load()
		{
			_config = Configuration.Instance;
			U.Events.OnPlayerConnected += OnPlayerConnected;
			Logger.Log($"Loaded TeamJoinerEnhanced {Version} by PhaserArray");
		}

		protected override void Unload()
		{
			U.Events.OnPlayerConnected -= OnPlayerConnected;
			Logger.Log($"Unloaded TeamJoinerEnhanced {Version} by PhaserArray");
		}

		private void OnPlayerConnected(UnturnedPlayer player)
		{
			// The player is not in any of the Steam groups.
			if (_config.Teams.All(team => player.SteamGroupID != team.SteamGroupId))
			{
				Logger.Log($"{player.DisplayName} joined without being in any relevant Steam groups.");
				UnturnedChat.Say(player, _config.NoTeamMessage, Color.yellow);
				StartCoroutine(SendLinkPopup(player));
				return;
			}

			var playerTeam =
				_config.Teams.Find(team => team.SteamGroupId == player.SteamGroupID);
			var playerPermissionsGroups = R.Permissions.GetGroups(player, true);

			// The player only has the default permissions group, nothing to clear, just give them the appropriate permissions group.
			if (playerPermissionsGroups.Count <= 1)
			{
				R.Permissions.AddPlayerToGroup(playerTeam.PermissionGroupId, player);
				UnturnedChat.Say(player, playerTeam.WelcomeMessage, Color.blue);
				Logger.Log($"{player.DisplayName} joined without permissions and was given {playerTeam.PermissionGroupId}.");
			}
			else
			{
				// The player has a permissions group from a different team, so remove all non-ignored permissions and give them the appropriate permissions group.
				if (playerPermissionsGroups.Any(group =>
					_config.Teams.Where(team => team != playerTeam).Select(team => team.PermissionGroupId).ToList()
						.Contains(group.Id)))
				{
					if (!_config.WipePermissionsGroups)
					{
						Logger.Log($"{player.DisplayName} joined with another team's permissions. WipePermissionsGroups is set to false, so they were not removed and new permissions were not given.");
						UnturnedChat.Say(player, _config.PermissionsFromOtherTeamWarning, Color.red);
						return;
					}
					var removedPermissionsGroups = new List<string>();
					foreach (var group in playerPermissionsGroups.Where(group => !_config.IgnoredPermissionsGroupIds.Contains(group.Id)))
					{
						R.Permissions.RemovePlayerFromGroup(group.Id, player);
						removedPermissionsGroups.Add(group.Id);
					}
					UnturnedChat.Say(player, string.Format(_config.PermissionsWipedMessage, string.Join(",", removedPermissionsGroups)), Color.red);

					R.Permissions.AddPlayerToGroup(playerTeam.PermissionGroupId, player);
					UnturnedChat.Say(player, playerTeam.WelcomeMessage, Color.blue);

					Logger.Log(
						$"{player.DisplayName} joined with another team's permissions. {string.Join(",", removedPermissionsGroups)} were removed and was given {playerTeam.PermissionGroupId}.");
				}
				else
				{
					if (playerPermissionsGroups.Select(group => group.Id).Contains(playerTeam.PermissionGroupId))
					{
						return;
					}
					R.Permissions.AddPlayerToGroup(playerTeam.PermissionGroupId, player);
					UnturnedChat.Say(player, playerTeam.WelcomeMessage, Color.blue);
					Logger.Log($"{player.DisplayName} joined with non-team permissions and was given {playerTeam.PermissionGroupId}.");
				}
			}
		}

		private IEnumerator SendLinkPopup(UnturnedPlayer player)
		{
			yield return new WaitForSeconds(2f);
			player.Player.sendBrowserRequest(_config.NoTeamPopupMessage, _config.NoTeamPopupUrl);
		}
	}
}
