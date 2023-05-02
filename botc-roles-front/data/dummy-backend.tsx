import { Game } from "@/entities/Game";
import { Player } from "@/entities/Player";

export async function getAllPlayers() {
  const response = await fetch(
    "https://nextjs-course-b67fb-default-rtdb.firebaseio.com/botc.json"
  );
  const data = await response.json();
  const players: Player[] = [];

  for (const key in data.players) {
    players.push({
      id: key,
      ...data.players[key],
    });
  }

  return players;
}

export async function getAllRolesPlayed() {
  const response = await fetch(
    "https://nextjs-course-b67fb-default-rtdb.firebaseio.com/botc.json"
  );
  const data = await response.json();
  const rolesPlayed = [];

  console.log(data);

  for (const key in data["roles-played"]) {
    rolesPlayed.push({
      id: key,
      ...data["roles-played"][key],
    });
  }

  return rolesPlayed;
}

export async function getPlayerByName(playerName: any) {
  const allPlayers = await getAllPlayers();
  const allRolesPlayed = await getAllRolesPlayed();

  const player = allPlayers.find((player) => player.id === playerName);

  return { player, allRolesPlayed };
}

export async function getAllGames() {
  const response = await fetch(
    "https://nextjs-course-b67fb-default-rtdb.firebaseio.com/botc.json"
  );
  const data = await response.json();
  const games: Game[] = [];

  for (const key in data.games) {
    games.push({
      id: key,
      ...data.games[key],
    });
  }

  return games;
}

export async function getGameById(id: number) {
  const allGames = await getAllGames();
  const game = allGames.find((game) => game.id === id);

  return game;
}

// const DUMMY_DATA = {
//   players: [
//     {
//       id: "Pras",
//       gamesPlayed: 666,
//       wins: 123,
//       loses: 1,
//       nbTimesGood: 2,
//       nbTimesEvil: 3,
//     },
//     {
//       id: "Strapontin",
//       gamesPlayed: 123,
//       wins: 123,
//       loses: -1,
//       nbTimesGood: 100,
//       nbTimesEvil: 23,
//     },
//     {
//       id: "Gil",
//       gamesPlayed: 79,
//       wins: 123,
//       loses: 1,
//       nbTimesGood: 2,
//       nbTimesEvil: 3,
//     },
//     {
//       id: "Pacha",
//       gamesPlayed: 78,
//       wins: 123,
//       loses: 1,
//       nbTimesGood: 2,
//       nbTimesEvil: 3,
//     },
//     {
//       id: "Mika",
//       gamesPlayed: 5,
//       wins: 123,
//       loses: 1,
//       nbTimesGood: 2,
//       nbTimesEvil: 3,
//     },
//     {
//       id: "J9",
//       gamesPlayed: 0,
//       wins: 0,
//       loses: 0,
//       nbTimesGood: 0,
//       nbTimesEvil: 0,
//     },
//     {
//       id: "J8",
//       gamesPlayed: 0,
//       wins: 0,
//       loses: 0,
//       nbTimesGood: 0,
//       nbTimesEvil: 0,
//     },
//     {
//       id: "J7",
//       gamesPlayed: 0,
//       wins: 0,
//       loses: 0,
//       nbTimesGood: 0,
//       nbTimesEvil: 0,
//     },
//     {
//       id: "J6",
//       gamesPlayed: 0,
//       wins: 0,
//       loses: 0,
//       nbTimesGood: 0,
//       nbTimesEvil: 0,
//     },
//     {
//       id: "J5",
//       gamesPlayed: 0,
//       wins: 0,
//       loses: 0,
//       nbTimesGood: 0,
//       nbTimesEvil: 0,
//     },
//     {
//       id: "J4",
//       gamesPlayed: 0,
//       wins: 0,
//       loses: 0,
//       nbTimesGood: 0,
//       nbTimesEvil: 0,
//     },
//     {
//       id: "J3",
//       gamesPlayed: 0,
//       wins: 0,
//       loses: 0,
//       nbTimesGood: 0,
//       nbTimesEvil: 0,
//     },
//     {
//       id: "J2",
//       gamesPlayed: 0,
//       wins: 0,
//       loses: 0,
//       nbTimesGood: 0,
//       nbTimesEvil: 0,
//     },
//     {
//       id: "J1",
//       gamesPlayed: 0,
//       wins: 0,
//       loses: 0,
//       nbTimesGood: 0,
//       nbTimesEvil: 0,
//     },
//   ],
// };
