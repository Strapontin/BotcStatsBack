import { Fragment, useEffect, useState } from "react";
import { getAllPlayers } from "../../../data/dummy-backend";
import { Player } from "@/entities/Player";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import { Link, Loading, Spacer } from "@nextui-org/react";

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
          <Link key={player.id} href={`/players/${player.id}`} color="text">
            <ListItem name={player.id} value={player.gamesPlayed} />
          </Link>
        ))}
      </Container>
    </Fragment>
  );
}
