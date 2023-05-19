import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import { Button, Container, Input, Spacer } from "@nextui-org/react";
import { createNewGame, getAllGames } from "../../../../data/back-api";
import { Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, XOctagon } from "react-feather";
import { Role } from "@/entities/Role";
import { toLowerRemoveDiacritics } from "@/helper/string";
import RolesSelector from "@/components/roles-selector/RolesSelector";
import DropdownAlignment from "@/components/dropdown-alignment/DropdownAlignment";
import EditionSelector from "@/components/edition-selector/EditionSelector";
import { Edition, getNewEmptyEdition } from "@/entities/Edition";
import { Player, getNewEmptyPlayer } from "@/entities/Player";
import PlayerSelector from "@/components/player-selector/PlayerSelector";

export default function CreateGame() {
  const [inputKey, setInputKey] = useState(0);
  const [edition, setEdition] = useState<Edition>(getNewEmptyEdition());
  const [player, setPlayer] = useState<Player>(getNewEmptyPlayer());
  const [alignment, setAlignment] = useState();
  const [message, setMessage] = useState(<Fragment />);
  const [selectedRoles, setSelectedRoles] = useState<Role[]>([]);

  const title = <Title>Création d{"'"}une nouvelle partie</Title>;

  async function createGame() {
    // if (
    //   await createNewGame(
    //     gameName,
    //     selectedRoles.map((sr) => sr.id)
    //   )
    // ) {
    //   games.push(gameName);
    //   setGameName("");
    //   setGames(games);
    //   updateMessage(false, `Module "${gameName}" enregistré correctement.`);
    // setInputKey(inputKey + 2);
    // setSelectedRoles([]);
    // } else {
    //   //Erreur
    //   updateMessage(
    //     true,
    //     "Une erreur est survenue lors de l'enregistrement du module."
    //   );
    // }
  }

  function updateMessage(isError: boolean, message: string) {
    if (isError) {
      setMessage(
        <Text span className={classes.red}>
          <XOctagon className={classes.icon} />
          {message}
        </Text>
      );
    } else {
      setMessage(
        <Text span className={classes.green}>
          <Check className={classes.icon} />
          {message}
        </Text>
      );
    }
  }

  function canCreateGame() {
    return false;
    // // Can create a game when
    // //  - the name is set
    // //  - the name is unique
    // return (
    //   gameName !== "" &&
    //   games.filter(
    //     (p) =>
    //       toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(gameName)
    //   ).length === 0
    // );
  }

  function checkError() {
    setMessage(<Fragment />);

    // if (gameName.trim() === "") {
    //   updateMessage(true, "Le nom du module ne doit pas être vide.");
    //   return;
    // }
  }

  return (
    <Fragment>
      {title}
      <Spacer y={2} />
      {message}
      <Spacer y={2} />
      <Container fluid css={{ display: "flex", flexDirection: "column" }}>
        {/* <Input
          key={inputKey}
          clearable
          bordered
          labelPlaceholder="Nom du module"
          aria-label="Nom du module"
          // value={gameName}
          // onChange={(event) => gameNameChanged(event.target.value)}
        /> */}
        <EditionSelector
          selectedEdition={edition}
          setSelectedEdition={setEdition}
        />
        <Spacer y={1.75} />
        {/* <Input
          key={inputKey + 1}
          clearable
          bordered
          labelPlaceholder="Nom du conteur"
          aria-label="Nom du conteur"
          // value={gameName}
          // onChange={(event) => gameNameChanged(event.target.value)}
        /> */}
        <PlayerSelector
          selectedPlayer={player}
          setSelectedPlayer={setPlayer}
        />
        <Spacer y={1.75} />
        <Input
          key={inputKey + 2}
          clearable
          bordered
          labelPlaceholder="Date à laquelle la partie a été jouée"
          aria-label="Date à laquelle la partie a été jouée"
          // value={gameName}
          // onChange={(event) => gameNameChanged(event.target.value)}
        />
        <Spacer y={1.75} />
        <Input
          key={inputKey + 3}
          clearable
          bordered
          labelPlaceholder="Notes"
          aria-label="Notes"
          // value={gameName}
          // onChange={(event) => gameNameChanged(event.target.value)}
        />
        <Spacer y={1.75} />
        <DropdownAlignment
          key={inputKey + 4}
          text="Alignement gagnant"
          setAlignment={setAlignment}
        />
        <Spacer y={3} />
        <RolesSelector
          selectedRoles={selectedRoles}
          setSelectedRoles={setSelectedRoles}
        />
        <Spacer y={3} />
      </Container>

      <Button
        shadow
        ghost
        color="success"
        onPress={createGame}
        disabled={!canCreateGame()}
      >
        Créer un module
      </Button>
      <Spacer y={3} />
    </Fragment>
  );
}
