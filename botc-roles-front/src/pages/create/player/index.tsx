import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import {
  Button,
  Container,
  Input,
  PressEvent,
  Spacer,
} from "@nextui-org/react";
import { createNewPlayer, getAllPlayers } from "../../../../data/back-api/back-api";
import { Text } from "@nextui-org/react";
import { XOctagon, Check } from "react-feather";
import classes from "../index.module.css";
import { toLowerRemoveDiacritics } from "@/helper/string";

export default function CreatePlayer() {
  const [inputKey, setInputKey] = useState(0);
  const [prenom, setPrenom] = useState("");
  const [pseudo, setPseudo] = useState("");
  const [message, setMessage] = useState(<Fragment />);

  const [players, setPlayers] = useState<{ name: string; pseudo: string }[]>(
    []
  );
  useEffect(() => {
    async function initPlayers() {
      const tempPlayers = (await getAllPlayers()).map((player) => {
        return {
          name: player.name,
          pseudo: player.pseudo,
        };
      });
      setPlayers(tempPlayers);
    }
    initPlayers();
  }, []);

  const title = <Title>Création d{"'"}un nouveau joueur</Title>;

  async function createUser(e: PressEvent) {
    if (await createNewPlayer(prenom, pseudo)) {
      var newPlayer: { name: string; pseudo: string } = {
        name: prenom,
        pseudo: pseudo,
      };

      players.push(newPlayer);
      setPrenom("");
      setPseudo("");
      setPlayers(players);

      const pseudoMsg = pseudo ? " (" + pseudo + ")" : "";

      updateMessage(
        false,
        `Joueur "${prenom}${pseudoMsg}" enregistré correctement.`
      );
      setInputKey(inputKey + 2);
    } else {
      //Erreur
      updateMessage(
        true,
        "Une erreur est survenue lors de l'enregistrement du joueur."
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

  function canCreatePlayer() {
    // Can create a player when
    //  - a name is set
    //  - {pseudo, name} is unique
    return (
      prenom !== "" &&
      players.filter((p) => p.pseudo === pseudo && p.name === prenom).length ===
        0
    );
  }

  function prenomChanged(value: string) {
    setPrenom(value);

    checkError(value, pseudo);
  }

  function pseudoChanged(value: string) {
    setPseudo(value);

    checkError(prenom, value);
  }

  function checkError(newName: string, newPseudo: string) {
    setMessage(<Fragment />);

    if (newName === "") {
      updateMessage(true, "Le prénom ne doit pas être vide.");
      return;
    }

    const playersWithSameNameAndPseudo = players.filter(
      (p) =>
        toLowerRemoveDiacritics(p.pseudo) ===
          toLowerRemoveDiacritics(newPseudo) &&
        toLowerRemoveDiacritics(p.name) == toLowerRemoveDiacritics(newName)
    );
    if (playersWithSameNameAndPseudo.length !== 0) {
      updateMessage(
        true,
        `Le joueur '${playersWithSameNameAndPseudo[0].name}' avec le pseudo '${playersWithSameNameAndPseudo[0].pseudo}' existe déjà.`
      );
      return;
    }
  }

  return (
    <Fragment>
      {title}
      <Spacer y={2} />
      {message}
      <Spacer y={2} />
      <Container fluid css={{ display: "flex", flexDirection: "column" }}>
        <Input
          key={inputKey}
          clearable
          bordered
          labelPlaceholder="Prénom"
          aria-label="Prénom"
          onChange={(event) => prenomChanged(event.target.value)}
        />
        <Spacer y={1.75} />
        <Input
          key={inputKey + 1}
          clearable
          bordered
          labelPlaceholder="Pseudo"
          aria-label="Pseudo"
          onChange={(event) => pseudoChanged(event.target.value)}
        />
        <Spacer y={3} />
      </Container>

      <Button
        shadow
        ghost
        color="success"
        onPress={createUser}
        disabled={!canCreatePlayer()}
      >
        Créer un joueur
      </Button>
      <Spacer y={3} />
    </Fragment>
  );
}
