import { Fragment, useEffect, useState } from "react";
import { Player } from "@/entities/Player";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import { Link, Loading, Spacer } from "@nextui-org/react";
import { getAllPlayers } from "../../../data/back-api";

export default function GamesPlayedByPlayerPage() {
  const [players, setPlayers] = useState<Player[]>([]);
  const title = "Nombre de parties/joueur";

  useEffect(() => {
    async function initPlayers() {
      const p = await getAllPlayers();
      setPlayers(p);
    }
    initPlayers();
  }, []);

  if (players.length === 0) {
    return (
      <Fragment>
        <Title>{title}</Title>
        <Spacer y={3} />
        <Loading />
      </Fragment>
    );
  }

  return (
    <Fragment>
      <Title>{title}</Title>
      <Container>
        {players.map((player) => (
          <Link key={player.name} href={`/players/${player.name}`} color="text">
            <ListItem name={player.name} value={player.nbGamesPlayed} />
          </Link>
        ))}
      </Container>
    </Fragment>
  );
}