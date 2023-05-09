import {
  getAllGames as queryAllGames,
  getGameById as queryGameById,
} from "./back-api/back-api-game";
import {
  getAllPlayers as queryAllPlayers,
  getPlayerById as queryPlayerById,
  createPlayer as querycreatePlayer,
} from "./back-api/back-api-player";

const apiUrl = "http://192.168.1.48:7099";

export async function getAllGames() {
  return queryAllGames(apiUrl);
}

export async function getGameById(id: number) {
  return queryGameById(apiUrl, id);
}

export async function getAllPlayers() {
  return queryAllPlayers(apiUrl);
}

export async function getPlayerById(playerId: number) {
  return queryPlayerById(apiUrl, playerId);
}

export async function createPlayer(
  playerName: string,
  pseudo: string
): Promise<boolean> {
  return querycreatePlayer(apiUrl, playerName, pseudo);
}
