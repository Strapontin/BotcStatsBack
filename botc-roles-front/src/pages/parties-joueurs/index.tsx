import { Fragment, useEffect, useState } from "react";
import { getAllPlayers } from "../../../data/dummy-backend";
import { Player } from "@/entities/Player";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Link from "next/link";
import Title from "@/components/ui/title";
import { useRouter } from "next/router";

export default function GamesPlayedByPlayerPage() {
  const [players, setPlayers] = useState<Player[]>([]);

  useEffect(() => {
    async function initPlayers() {
      const p = await getAllPlayers();
      setPlayers(p);
    }
    initPlayers();
  }, []);

  console.log("get in");

  if (players.length === 0) {
    return (
      <Fragment>
        <Title>Nombre de parties/joueurs</Title>
        <p>loading...</p>
      </Fragment>
    );
  }

  return (
    <Fragment>
      <Title>Nombre de parties/joueurs</Title>
      <Container>
        {players.map((player: any) => (
          <Link key={player.id} href={`/details/${player.id}`}>
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
