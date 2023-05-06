import { Module } from "./Module";
import { Player } from "./Player";
import { PlayerRoleGame } from "./PlayerRoleGame";
import { Alignment } from "./enums/alignment";

export type Game = {
  gameId: number;
  moduleId: number;
  module: Module;
  storyTellerId: string;
  storyTeller: Player;
  creationDate: Date;
  notes: string;
  winningAlignment: Alignment; // TODO
  playerRoleGames: PlayerRoleGame[];
};
