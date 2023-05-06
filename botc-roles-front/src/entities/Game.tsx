import { Module } from "./Module";
import { Player } from "./Player";
import { PlayerRoleGame } from "./PlayerRoleGame";

export type Game = {
  gameId: number;
  moduleId: number;
  module: Module;
  storyTellerId: string;
  storyTeller: Player;
  creationDate: Date;
  notes: string;
  winningAlignment: string; // TODO
  playerRoleGames: PlayerRoleGame[];
};
