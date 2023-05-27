import { Player } from "@/entities/Player";

export async function getAllPlayers(apiUrl: string) {
  const response = await fetch(`${apiUrl}/Players`);
  const data = await response.json();
  const players: Player[] = [];

  for (const key in data) {
    players.push(data[key]);
  }

  console.log("getAllPlayers");
  return players;
}

export async function getPlayerById(apiUrl: string, playerId: number) {
  if (playerId === undefined || playerId === null || isNaN(playerId)) return;

  const response = await fetch(`${apiUrl}/Players/${playerId}`);
  const player: Player = await response.json();

  console.log("getPlayerById");
  return player;
}

export async function createNewPlayer(
  apiUrl: string,
  player: Player
): Promise<boolean> {
  const response = await fetch(`${apiUrl}/Players`, {
    method: "POST",
    mode: "cors",
    cache: "no-cache",
    credentials: "same-origin",
    headers: {
      "Content-Type": "application/json",
    },
    redirect: "follow",
    referrerPolicy: "no-referrer",
    body: JSON.stringify({ playerName: player.name, pseudo: player.pseudo }),
  });

  console.log("createPlayer");

  if (!response.ok) {
    const res = await response.json();
    console.log(res.error);
    return false;
  }

  return true;
}
