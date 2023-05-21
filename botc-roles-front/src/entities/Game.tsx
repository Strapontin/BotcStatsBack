import { Edition, getNewEmptyEdition } from "./Edition";
import { Player, getNewEmptyPlayer } from "./Player";
import { PlayerRole } from "./PlayerRole";
import { Alignment } from "./enums/alignment";

export type Game = {
  id: number;
  edition: Edition;
  storyTeller: Player;
  datePlayed: Date;
  notes: string;
  winningAlignment: Alignment;

  playerRoles: PlayerRole[];
};

export function getNewEmptyGame() {
  const game: Game = {
    id: -1,
    edition: getNewEmptyEdition(),
    storyTeller: getNewEmptyPlayer(),
    datePlayed: new Date(),
    notes: "",
    winningAlignment: Alignment.None,
    playerRoles: [],
  };
  return game;
}
