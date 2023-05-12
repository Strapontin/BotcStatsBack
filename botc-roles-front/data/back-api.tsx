import { Alignment } from "@/entities/enums/alignment";
import {
  getAllGames as queryAllGames,
  getGameById as queryGameById,
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
import { CharacterType } from "@/entities/enums/characterType";

const apiUrl = "http://192.168.1.48:7099";

/* Games */

export async function getAllGames() {
  return queryAllGames(apiUrl);
}

export async function getGameById(id: number) {
  return queryGameById(apiUrl, id);
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
  return queryAllRoles(apiUrl);
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
