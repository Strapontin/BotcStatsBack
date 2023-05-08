import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import { Button, Input, PressEvent, Spacer } from "@nextui-org/react";
import { Player } from "@/entities/Player";
import { getAllPlayers } from "../../../../data/back-api";

export default function CreatePlayer() {
  const [prenom, setPrenom] = useState("");
  const [pseudo, setPseudo] = useState("");

  const [players, setPlayers] = useState<Player[]>([]);
  useEffect(() => {
    async function initPlayers() {
      const p = await getAllPlayers();
      setPlayers(p);
    }
    initPlayers();
  }, []);

  const title = <Title>Création d{"'"}un nouveau joueur</Title>;

  function createUser(e: PressEvent) {
    console.log(prenom);
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
      <Spacer y={3} />
      <Input
        fullWidth
        clearable
        bordered
        labelLeft="Prénom"
        aria-label="Prénom"
        value={prenom}
        onChange={(event) => setPrenom(event.target.value)}
      ></Input>
      <Spacer y={1.75} />
      <Input
        fullWidth
        clearable
        bordered
        labelLeft="Pseudo"
        aria-label="Pseudo"
        value={pseudo}
        onChange={(event) => setPseudo(event.target.value)}
        helperText="Obligatoire"
      ></Input>
      <Spacer y={3} />

      <Button
        auto
        shadow
        ghost
        color="success"
        onPress={createUser}
        disabled={!canCreatePlayer()}
      >
        Créer un joueur
      </Button>
    </Fragment>
  );
}
