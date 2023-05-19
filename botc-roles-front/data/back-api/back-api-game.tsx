import { Game } from "@/entities/Game";

export async function getAllGames(apiUrl: string) {
  const response = await fetch(`${apiUrl}/Games`);
  const data = await response.json();
  const games: Game[] = [];

  for (const key in data) {
    games.push(data[key]);
  }

  console.log("getAllGames");
  return games;
}

export async function getGameById(apiUrl: string, id: number) {
  if (isNaN(id)) return;

  const response = await fetch(`${apiUrl}/Games/${id}`);
  const game = await response.json();

  console.log("getGameById");
  return game;
}

export async function createNewGame() {}
