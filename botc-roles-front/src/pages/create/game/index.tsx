import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import { Button, Container, Input, Spacer, Textarea } from "@nextui-org/react";
import { createNewGame } from "../../../../data/back-api";
import { Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, XOctagon } from "react-feather";
import { Role } from "@/entities/Role";
import EditionSelector from "@/components/edition-selector/EditionSelector";
import { Edition, getNewEmptyEdition } from "@/entities/Edition";
import { Player, getNewEmptyPlayer } from "@/entities/Player";
import PlayerSelector from "@/components/player-selector/PlayerSelector";
import PlayerRolesSelector from "@/components/player-role-selector/PlayerRolesSelector";
import { PlayerRole } from "@/entities/PlayerRole";
import DropdownAlignment from "@/components/dropdown-alignment/DropdownAlignment";
import { Alignment } from "@/entities/enums/alignment";

export default function CreateGame() {
  const [inputKey, setInputKey] = useState(0);
  const [edition, setEdition] = useState<Edition>(getNewEmptyEdition());
  const [storyTeller, setStoryTeller] = useState<Player>(getNewEmptyPlayer());
  const [datePlayed, setDatePlayed] = useState("");
  const [notes, setNotes] = useState("");
  const [winningAlignment, setWinningAlignment] = useState<Alignment>();
  const [message, setMessage] = useState(<Fragment />);
  const [selectedPlayerRoles, setSelectedPlayerRoles] = useState<PlayerRole[]>(
    []
  );
  const [rolesInSelectedEdition, setRolesInSelectedEdition] = useState<Role[]>(
    []
  );

  const title = <Title>Création d{"'"}une nouvelle partie</Title>;

  async function createGame() {
    if (winningAlignment === undefined) return;

    if (
      await createNewGame(
        edition.id,
        storyTeller.id,
        datePlayed,
        notes,
        winningAlignment,
        selectedPlayerRoles
      )
    ) {
      setEdition(getNewEmptyEdition());
      setStoryTeller(getNewEmptyPlayer());
      setDatePlayed("");
      setNotes("");
      setWinningAlignment(undefined);
      setSelectedPlayerRoles([]);
      setInputKey(inputKey + 2);
      updateMessage(false, `La partie a été enregistrée correctement.`);
    } else {
      //Erreur
      updateMessage(
        true,
        "Une erreur est survenue lors de l'enregistrement du module."
      );
    }
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
    // Can create a game when
    //  - the edition is set
    //  - the storyTeller is set
    //  - the date is set
    //  - the winningAlignment is set
    return (
      edition.id !== -1 &&
      storyTeller.id !== -1 &&
      datePlayed !== "" &&
      winningAlignment !== undefined
    );
  }

  // function checkError() {
  //   setMessage(<Fragment />);

  //   if (edition.id === -1) {
  //     updateMessage(true, "Vous devez choisir un module.");
  //     return;
  //   }
  // }

  function editionSelected(edition: Edition) {
    setEdition(edition);
    setRolesInSelectedEdition(edition.roles);
  }

  return (
    <Fragment>
      {title}
      <Spacer y={2} />
      {message}
      <Spacer y={2} />
      <Container fluid css={{ display: "flex", flexDirection: "column" }}>
        <EditionSelector
          selectedEdition={edition}
          setSelectedEdition={(edition: Edition) => editionSelected(edition)}
        />
        <Spacer y={1.75} />
        <PlayerSelector
          selectedPlayer={storyTeller}
          setSelectedPlayer={setStoryTeller}
        />
        <Spacer y={0.6} />
        <Input
          css={{ textAlign: "left" }} // Usefull so the label isn't centered
          key={inputKey + 2}
          type="date"
          bordered
          label="Date à laquelle la partie a été jouée"
          aria-label="Date à laquelle la partie a été jouée"
          onChange={(event) => setDatePlayed(event.target.value)}
        />
        <Spacer y={1.75} />
        <Textarea
          key={inputKey + 3}
          bordered
          labelPlaceholder="Notes"
          aria-label="Notes"
          onChange={(event) => setNotes(event.target.value)}
        />
        <Spacer y={1.75} />
        <DropdownAlignment
          key={inputKey + 4}
          text="Alignement gagnant"
          setAlignment={setWinningAlignment}
        />
        <Spacer y={3} />
        <PlayerRolesSelector
          selectedPlayerRoles={selectedPlayerRoles}
          setSelectedPlayerRoles={setSelectedPlayerRoles}
          rolesInSelectedEdition={rolesInSelectedEdition}
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
