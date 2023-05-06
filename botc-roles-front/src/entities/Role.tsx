import { Alignment } from "./enums/alignment";
import { CharacterType } from "./enums/characterType";

export type Role = {
  roleId: number;
  name: string;
  defaultAlignment: Alignment;
  characterType: CharacterType;
};
