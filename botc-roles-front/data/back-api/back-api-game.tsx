import { Game } from "@/entities/Game";
import { PlayerRole } from "@/entities/PlayerRole";
import { Alignment } from "@/entities/enums/alignment";

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

export async function createNewGame(
  apiUrl: string,
  editionId: number,
  storyTellerId: number,
  datePlayed: string,
  notes: string,
  winningAlignment: Alignment,
  playersIdRolesId: { playerId: number; roleId: number }[]
): Promise<boolean> {
  const response = await fetch(`${apiUrl}/Games`, {
    method: "POST",
    mode: "cors",
    cache: "no-cache",
    credentials: "same-origin",
    headers: {
      "Content-Type": "application/json",
    },
    redirect: "follow",
    referrerPolicy: "no-referrer",
    body: JSON.stringify({
      editionId,
      storyTellerId,
      datePlayed,
      notes,
      winningAlignment,
      playersIdRolesId,
    }),
  });

  console.log("createNewGame");

  if (!response.ok) {
    const res = await response.json();
    console.log(res);
    return false;
  }

  return true;
}

export async function editGame(
  apiUrl: string,
  gameId: number,
  editionId: number,
  storyTellerId: number,
  datePlayed: string,
  notes: string,
  winningAlignment: Alignment,
  playersIdRolesId: { playerId: number; roleId: number }[]
): Promise<boolean> {
  return false;
}
