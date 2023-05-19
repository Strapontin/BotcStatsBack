import { Role } from "./Role";

export interface Player {
  id: number;
  name: string;
  pseudo: string;
  nbGamesPlayed?: number;
  nbGamesWon: number;
  nbGamesLost: number;
  nbGamesGood: number;
  nbGamesEvil: number;
  timesPlayedRole: Role[];
}

export function getNewEmptyPlayer() {
  const edition: Player = {
    id: -1,
    name: "",
    pseudo: "",
    nbGamesWon: 0,
    nbGamesLost: 0,
    nbGamesGood: 0,
    nbGamesEvil: 0,
    timesPlayedRole: [],
  };
  return edition;
}
