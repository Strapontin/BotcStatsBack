import { Fragment, useEffect, useState } from "react";
import { getAllPlayers } from "../../../data/dummy-backend";
import { Player } from "@/entities/Player";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import { Link } from "@nextui-org/react";

export default function GamesPlayedByPlayerPage() {
  const [players, setPlayers] = useState<Player[]>([]);

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
        <Title>Nombre de parties/joueur</Title>
        <p>loading...</p>
      </Fragment>
    );
  }

  return (
    <Fragment>
      <Title>Nombre de parties/joueur</Title>
      <Container>
        {players.map((player: any) => (
          <Link key={player.id} href={`/details/${player.id}`} color="text">
            <ListItem
              key={player.id}
              name={player.id}
              value={player.gamesPlayed}
            ></ListItem>
          </Link>
        ))}
      </Container>
    </Fragment>
  );
}
