import { Fragment, useEffect, useState } from "react";
import { getPlayerByName } from "../../../data/dummy-backend";
import { Player } from "@/entities/Player";
import { useRouter } from "next/router";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import { Text } from "@nextui-org/react";

export default function UserDetailsPage() {
  const playerName = useRouter().query.player;
  const [player, setPlayer] = useState<Player>();

  useEffect(() => {
    async function initPlayer() {
      const p = await getPlayerByName(playerName);
      setPlayer(p);
    }
    initPlayer();
  }, [playerName]);

  if (!playerName) {
    return;
  }

  if (!player) {
    return (
      <Fragment>
        <Title>
          Détails{" "}
          <Text span color="grey">
            {playerName}
          </Text>
        </Title>
        <p>loading...</p>
      </Fragment>
    );
  }

  return (
    <Fragment>
      <Title>
        Détails{" "}
        <Text span color="grey">
          {playerName}
        </Text>
      </Title>
      <Container>
        <ListItem name="Parties jouées" value={player.gamesPlayed} />
        <ListItem name="Parties gagnées" value={player.wins} />
        <ListItem name="Parties perdues" value={player.loses} />
        <ListItem name="Parties maléfique" value={player.nbTimesEvil} />
        <ListItem name="Parties gentil" value={player.nbTimesGood} />
      </Container>
    </Fragment>
  );
}
