﻿using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rocket.API.Collections;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace PhaserArray.TeamJoinerEnhanced
{
	public class TeamJoinerEnhanced : RocketPlugin<TeamJoinerEnhancedConfiguration>
	{
		public const string Version = "v1.3";
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
			if (_config.Teams.All(team => (CSteamID)team.SteamGroupId != player.SteamGroupID))
			{
				Logger.Log(Translate("ConsoleNoTeam"));
				UnturnedChat.Say(Translate("NoTeamMessage", player), Color.yellow);
				StartCoroutine(SendLinkPopup(player));
				return;
			}

			var playerTeam =
				_config.Teams.Find(team => (CSteamID)team.SteamGroupId == player.SteamGroupID);
			var playerPermissionsGroups = R.Permissions.GetGroups(player, true);

			// The player only has the default permissions group, nothing to clear, just give them the appropriate permissions group.
			if (playerPermissionsGroups.Count <= 1)
			{
				R.Permissions.AddPlayerToGroup(playerTeam.PermissionGroupId, player);
				UnturnedChat.Say(player, Translate("TeamWelcomeMessage"), Color.blue);
				Logger.Log(Translate("ConsoleNewToTeam", player.DisplayName, playerTeam.PermissionGroupId));
			}
			else
			{
				
				// The player has a permissions group from a different team, so remove all non-ignored permissions and give them the appropriate permissions group.
				if (playerPermissionsGroups.Exists(group => _config.Teams.Any(team =>
					team != playerTeam &&
					(team.PermissionGroupId == group.Id || team.FriendlyPermissionGroupIds.Contains(group.Id)))))
				{
					if (!_config.WipePermissionGroups)
					{
						Logger.Log(Translate("ConsoleWrongTeamPermsWarning", player.DisplayName));
						UnturnedChat.Say(player, Translate("PermissionsFromOtherTeamWarning"), Color.red);
						return;
					}
					var removedPermissionsGroups = new List<string>();
					foreach (var group in playerPermissionsGroups.Where(group => !_config.IgnoredPermissionGroupIds.Contains(group.Id)))
					{
						R.Permissions.RemovePlayerFromGroup(group.Id, player);
						removedPermissionsGroups.Add(group.Id);
					}

					UnturnedChat.Say(player, Translate("PermissionsWipedMessage", string.Join(",", removedPermissionsGroups)), Color.red);

					R.Permissions.AddPlayerToGroup(playerTeam.PermissionGroupId, player);
					UnturnedChat.Say(player, Translate("TeamWelcomeMessage"), Color.blue);

					Logger.Log(Translate("ConsoleWrongTeamPermsWiped", player.DisplayName, playerTeam.PermissionGroupId, string.Join(",", removedPermissionsGroups)));
				}
				else
				{
					if (playerPermissionsGroups.Select(group => group.Id).Contains(playerTeam.PermissionGroupId))
					{
						return;
					}
					R.Permissions.AddPlayerToGroup(playerTeam.PermissionGroupId, player);
					UnturnedChat.Say(player, Translate("TeamWelcomeMessage"), Color.blue);
					Logger.Log(Translate("ConsoleNonTeamPerms", player.DisplayName, playerTeam.PermissionGroupId));
				}
			}
		}
		
		private IEnumerator SendLinkPopup(UnturnedPlayer player)
		{
			yield return new WaitForSeconds(2f);
			player.Player.sendBrowserRequest(Translate("NoTeamPopupMessage"), _config.NoTeamPopupUrl);
		}

		public override TranslationList DefaultTranslations => new TranslationList()
		{
			{"NoTeamMessage", "You are not currently in a team. Please ensure you have joined one of the Steam groups and set it as your active group in the main menu under survivors and then group."},
			{"NoTeamPopupMessage", "You have not joined a team! Click agree to open a steam guide on how to do that."},
			{"PermissionsWipedMessage", "You were previously on a different team. Permissions do not carry over between sides, the following permission group(s) were removed: {0}"},
			{"PermissionsFromOtherTeamWarning", "You appear to have been on a different team in the past. Please contact staff for new permissions, if you're attempting to swap sides!"},
			{"TeamWelcomeMessage", "You have joined the team and have been given the appropriate permissions group."},
			{"ConsoleNoTeam", "{0} joined without being in any relevant Steam groups."},
			{"ConsoleNewToTeam", "{0} joined without permissions and was given {1}."},
			{"ConsoleWrongTeamPermsWarning", "{0} joined with another team's permissions. WipePermissionGroups is set to false, so they were not removed and new permissions were not given."},
			{"ConsoleWrongTeamPermsWiped", "{0} joined with another team's permissions. They were given {1}, their removed permissions were: {2}."},
			{"ConsoleNonTeamPerms", "{0} joined with non-team permissions and was given {1}."}
		};
	}
}
