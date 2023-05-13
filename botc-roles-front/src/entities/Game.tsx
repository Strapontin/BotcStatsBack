import { Edition } from "./Edition";
import { Player } from "./Player";
import { PlayerRole } from "./PlayerRole";
import { Alignment } from "./enums/alignment";

export type Game = {
  id: number;
  edition: Edition;
  storyTeller: Player;
  creationDate: Date;
  notes: string;
  winningAlignment: Alignment;

  playerRoles: PlayerRole[];
};
