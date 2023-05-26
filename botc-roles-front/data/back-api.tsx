import { Alignment } from "@/entities/enums/alignment";
import {
  getAllGames as queryAllGames,
  getGameById as queryGameById,
  createNewGame as queryCreateNewGame,
  updateGame as queryUpdateGame,
} from "./back-api/back-api-game";
import {
  getAllPlayers as queryAllPlayers,
  getPlayerById as queryPlayerById,
  createNewPlayer as queryCreateNewPlayer,
} from "./back-api/back-api-player";
import {
  getAllRoles as queryAllRoles,
  getRoleById as queryRoleById,
  createNewRole as queryCreateNewRole,
} from "./back-api/back-api-role";
import {
  getAllEditions as queryAllEditions,
  getEditionById as queryEditionById,
  createNewEdition as queryCreateNewEdition,
  updateEdition as queryUpdateEdition,
} from "./back-api/back-api-edition";
import { CharacterType } from "@/entities/enums/characterType";
import { PlayerRole } from "@/entities/PlayerRole";
import { Role } from "@/entities/Role";

const apiUrl = "http://192.168.1.48:7099";

/* Games */

export async function getAllGames() {
  return queryAllGames(apiUrl);
}

export async function getGameById(id: number) {
  return queryGameById(apiUrl, id);
}

export async function createNewGame(
  editionId: number,
  storyTellerId: number,
  datePlayed: string,
  notes: string,
  winningAlignment: Alignment,
  playersRoles: PlayerRole[]
) {
  const playersIdRolesId = playersRoles.map((pr) => ({
    playerId: pr.player.id,
    roleId: pr.role.id,
  }));

  return queryCreateNewGame(
    apiUrl,
    editionId,
    storyTellerId,
    datePlayed,
    notes,
    winningAlignment,
    playersIdRolesId
  );
}

export async function updateGame(
  gameId: number,
  editionId: number,
  storyTellerId: number,
  datePlayed: string,
  notes: string,
  winningAlignment: Alignment,
  playersRoles: PlayerRole[]
) {
  const playersIdRolesId = playersRoles.map((pr) => ({
    playerId: pr.player.id,
    roleId: pr.role.id,
  }));

  return queryUpdateGame(
    apiUrl,
    gameId,
    editionId,
    storyTellerId,
    datePlayed,
    notes,
    winningAlignment,
    playersIdRolesId
  );
}

/* Players */

export async function getAllPlayers() {
  return queryAllPlayers(apiUrl);
}

export async function getPlayerById(playerId: number) {
  return queryPlayerById(apiUrl, playerId);
}

export async function createNewPlayer(
  playerName: string,
  pseudo: string
): Promise<boolean> {
  return queryCreateNewPlayer(apiUrl, playerName, pseudo);
}

/* Roles */

export async function getAllRoles() {
  var result = await queryAllRoles(apiUrl);

  // switch (orderBy) {
  //   case RoleOrderBy.Name | RoleOrderBy.CharacterType:
  //     return result.sort((a, b) => {
  //       if (a.characterType === b.characterType) {
  //         return toLowerRemoveDiacritics(a.name) <
  //           toLowerRemoveDiacritics(b.name)
  //           ? -1
  //           : 1;
  //       }
  //       return a.characterType < b.characterType ? -1 : 1;
  //     });

  //   default:
  //   case RoleOrderBy.None:
  //     return result;
  //   }
  return result;
}

export async function getRoleById(roleId: number) {
  return queryRoleById(apiUrl, roleId);
}

export async function createNewRole(
  roleName: string,
  characterType: CharacterType,
  alignment: Alignment
) {
  return queryCreateNewRole(apiUrl, roleName, characterType, alignment);
}

/* Edition */

export async function getAllEditions() {
  return queryAllEditions(apiUrl);
}

export async function getEditionById(editionId: number) {
  return queryEditionById(apiUrl, editionId);
}

export async function createNewEdition(editionName: string, rolesId: number[]) {
  return queryCreateNewEdition(apiUrl, editionName, rolesId);
}

export async function updateEdition(
  editionId: number,
  name: string,
  roles: Role[]
) {
  const rolesId = roles.map((r) => r.id);

  return queryUpdateEdition(apiUrl, editionId, name, rolesId);
}
