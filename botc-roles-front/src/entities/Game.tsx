import { Module } from "./Module";
import { Player } from "./Player";
import { PlayerRole } from "./PlayerRole";
import { Alignment } from "./enums/alignment";

export type Game = {
  id: number;
  module: Module;
  storyTeller: Player;
  creationDate: Date;
  notes: string;
  winningAlignment: Alignment;

  playerRoles: PlayerRole[];
};
