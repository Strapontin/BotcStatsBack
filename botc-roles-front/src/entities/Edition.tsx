import { Alignment } from "./enums/alignment";
import { CharacterType } from "./enums/characterType";

export type Edition = {
  roleId: number;
  name: string;
  type: CharacterType;
  defaultAlignment: Alignment;
};
