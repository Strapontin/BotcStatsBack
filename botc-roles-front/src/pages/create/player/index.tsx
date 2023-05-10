import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import {
  Button,
  Container,
  Input,
  PressEvent,
  Spacer,
} from "@nextui-org/react";
import { createPlayer, getAllPlayers } from "../../../../data/back-api";
import { Text } from "@nextui-org/react";
import { XOctagon, Check } from "react-feather";
import classes from "./index.module.css";

export default function CreatePlayer() {
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
    if (await createPlayer(prenom, pseudo)) {
      var newPlayer: { name: string; pseudo: string } = {
        name: prenom,
        pseudo: pseudo,
      };

      players.push(newPlayer);
      setPrenom("");
      setPseudo("");
      setPlayers(players);

      const pseudoMsg = pseudo ? " (" + pseudo + ")" : "";

      setMessage(
        <Text span className={classes.green}>
          <Check className={classes.icon} />
          Joueur {'"'}
          {prenom}
          {pseudoMsg}
          {'"'} enregistré correctement.
        </Text>
      );
    } else {
      //Erreur
      setMessage(
        <Text span className={classes.red}>
          <XOctagon className={classes.icon} />
          Une erreur est survenue lors de l{"'"}
          enregistrement du joueur.
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
      players.filter((p) => p.pseudo === pseudo && p.name == prenom).length ===
        0
    );
  }

  return (
    <Fragment>
      {title}
      <Spacer y={2} />
      {message}
      <Spacer y={2} />
      <Container fluid css={{ display: "flex", flexDirection: "column" }}>
        <Input
          clearable
          bordered
          labelPlaceholder="Prénom"
          aria-label="Prénom"
          value={prenom}
          onChange={(event) => setPrenom(event.target.value)}
        ></Input>
        <Spacer y={1.75} />
        <Input
          clearable
          bordered
          labelPlaceholder="Pseudo"
          aria-label="Pseudo"
          value={pseudo}
          onChange={(event) => setPseudo(event.target.value)}
        ></Input>
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
