import { Role } from "./Role";

export type Player = {
  id: string;
  name: string;
  nbGamesPlayed: number;
  nbGamesWon: number;
  nbGamesLost: number;
  nbGamesGood: number;
  nbGamesEvil: number;
  timesPlayedRole: Role[];
};
