export type Game = {
  id: number;
  module: string;
  storyTeller: string;
  datePlayed: Date;
  notes: string;
  winningAlignment: string;
  playerRoleGame: PlayerRoleGame[];
};

export type PlayerRoleGame = {
  playerName: string;
  role: { name: string; category: string };
  finalAlignment: string;
};
