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
  return player;
}

export async function createPlayer(
  playerName: string,
  pseudo: string
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
    body: JSON.stringify({ playerName, pseudo }),
  });

  console.log("createPlayer");

  if (!response.ok) {
    const res = await response.json();
    console.log(res);
    return false;
  }

  return true;
  // console.log(response);

  // TODO : switch response.ok => notification (popover ? Toast ?) :
  //  - === true : message "Joueur 'prenom, pseudo' sauvegardé correctement"
  //    => Vider les champs pour si on veut ajouter un autre joueur
  //  - === false : message d'erreur
}
