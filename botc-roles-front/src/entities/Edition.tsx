import { Role } from "./Role";

export type Edition = {
  id: number;
  name: string;
  roles: Role[];
  timesPlayed: number;
  timesGoodWon: number;
};
