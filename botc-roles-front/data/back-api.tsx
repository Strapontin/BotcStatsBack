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

  console.log("getAllPlayers");
  return players;
}

export async function getPlayerById(playerId: number) {
  if (playerId === undefined || playerId === null || isNaN(playerId)) return;

  const response = await fetch(`${apiUrl}/Players/${playerId}`);
  const player: Player = await response.json();

  console.log("getPlayerById");
  console.log(player);
  return player;
}

export async function getAllGames() {
  const response = await fetch(`${apiUrl}/Games`);
  const data = await response.json();
  const games: Game[] = [];

  for (const key in data) {
    games.push(data[key]);
  }

  console.log("getAllGames");
  return games;
}

export async function getGameById(id: number) {
  if (isNaN(id)) return;

  const response = await fetch(`${apiUrl}/Games/${id}`);
  const game = await response.json();

  console.log("getGameById");
  return game;
}
