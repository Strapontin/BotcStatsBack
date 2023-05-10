export enum CharacterType {
  Townsfolk = 0,
  Outsider = 1,
  Minion = 2,
  Demon = 3,
  Traveller = 4,
  Fabled = 5,
}

export function characterTypeList(): { key: number; value: string }[] {
  const characterTypes = Object.values(CharacterType)
    .filter((ct) => typeof ct !== "number")
    .map((key: any) => ({
      key: +CharacterType[key],
      value: translate(key),
    }));

  return characterTypes;
}

function translate(name: string): string {
  return name === "Townsfolk"
    ? "Villageois"
    : name === "Outsider"
    ? "Etranger"
    : name === "Minion"
    ? "Sbire"
    : name === "Demon"
    ? "Démon"
    : name === "Traveller"
    ? "Voyageur"
    : name === "Fabled"
    ? "Légendaire"
    : "";
}
