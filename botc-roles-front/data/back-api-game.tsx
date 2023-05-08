import { Game } from "@/entities/Game";

const apiUrl = "https://localhost:7099";

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
