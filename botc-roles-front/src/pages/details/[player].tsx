import { Fragment, useEffect, useState } from "react";
import { getPlayerByName } from "../../../data/dummy-backend";
import { Player } from "@/entities/Player";
import { useRouter } from "next/router";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";

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

  if (!player) {
    return (
      <Fragment>
        <Title>
          Détails <strong>{playerName}</strong>
        </Title>
        <p>loading...</p>
      </Fragment>
    );
  }

  return (
    <Fragment>
      <Title>
        Détails <strong>{playerName}</strong>
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
