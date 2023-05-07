import { Role } from "./Role";

export type Player = {
  id: string;
  name: string;
  nbGamesPlayed: number;
  timesPlayedRole: Role[];
  // timesPlayedModule: IDictionary[];
};
