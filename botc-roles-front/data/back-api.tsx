import { Game } from "@/entities/Game";
import { Player } from "@/entities/Player";

const apiUrl = "https://localhost:7099";

export async function getAllPlayers() {
  const response = await fetch(`${apiUrl}/Players`);
  const data = await response.json();
  const players: Player[] = [];

  for (const key in data) {
    players.push(data[key]);
  }

  return players;
}

export async function getPlayerByName(playerName: string) {
  if (playerName === undefined || playerName === null || playerName === "")
    return;

  const response = await fetch(`${apiUrl}/Players/${playerName}`);
  const player = await response.json();

  return { player, allRolesPlayed: player.playerRoleGames };
}

export async function getAllGames() {
  const response = await fetch(`${apiUrl}/Games`);
  const data = await response.json();
  const games: Game[] = [];

  for (const key in data) {
    games.push(data[key]);
  }

  return games;
}

export async function getGameById(id: number) {
  if (isNaN(id)) return;

  const response = await fetch(`${apiUrl}/Games/${id}`);
  const game = await response.json();

  return game;
}
