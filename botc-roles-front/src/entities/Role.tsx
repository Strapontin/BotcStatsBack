import { Alignment } from "./enums/alignment";
import { CharacterType } from "./enums/characterType";

export type Role = {
  id: number;
  name: string;
  characterType: CharacterType;
  alignment: Alignment;

  timesPlayedByPlayer: number;
  timesWonByPlayer: number;
  timesLostByPlayer: number;

  timesPlayedTotal: number;
  timesWonTotal: number;
};
